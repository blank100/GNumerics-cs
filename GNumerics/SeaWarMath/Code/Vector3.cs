using System.Globalization;

namespace SeaWar.Mathematics {
    /// <summary>
    /// 3D向量
    /// </summary>
    /// <author>gouanlin</author>
    [Serializable]
    public struct Vector3 {
        public Single x, y, z;

        public Single this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get =>
                index switch {
                    0 => x,
                    1 => y,
                    2 => z,
                    _ => throw new IndexOutOfRangeException($"Invalid {nameof(Vector3)} index!")
                };
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                switch (index) {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException($"Invalid {nameof(Vector3)} index!");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(Single x, Single y, Single z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 a, Vector3 b, Single t) {
            t = Math.Clamp(t, 0, 1);
            return new(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, Single t) =>
            new(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);

        /// 如果调用的比较频繁,建议外部缓存两向量的距离后手动实现
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, Single maxDistanceDelta) {
            var tx = target.x - current.x;
            var ty = target.y - current.y;
            var tz = target.z - current.z;
            var d = tx * tx + ty * ty + tz * tz;
            if (d == 0 || maxDistanceDelta >= 0 && d <= maxDistanceDelta * maxDistanceDelta) return target;
            var length = Math.Sqrt(d);
            return new(
                current.x + tx / length * maxDistanceDelta,
                current.y + ty / length * maxDistanceDelta,
                current.z + tz / length * maxDistanceDelta
            );
        }

        public static Vector3 RotateTowards(
            Vector3 current,
            Vector3 target,
            Single maxRadiansDelta,
            Single maxMagnitudeDelta
        ) {
            var curMag = current.Magnitude;
            var tarMag = target.Magnitude;

            // --- 1. 处理零向量（必须提前）
            if (curMag < Math.NormalizeEpsilon || tarMag < Math.NormalizeEpsilon)
            {
                return MoveTowards(current, target, maxMagnitudeDelta);
            }

            // --- 2. 单位方向
            var from = current / curMag;
            var to = target / tarMag;

            var dot = Dot(from, to);
            dot = Math.Clamp(dot, -Math.One, Math.One);

            var angle = Math.Acos(dot);

            // --- 3. 限制旋转
            var t = (angle > Math.NormalizeEpsilon)
                ? Math.Min(Math.One, maxRadiansDelta / angle)
                : Math.One;

            // --- 4. Slerp方向
            var sinAngle = Math.Sin(angle);

            Vector3 newDir;
            if (sinAngle > Math.NormalizeEpsilon)
            {
                var coeff0 = Math.Sin((1 - t) * angle) / sinAngle;
                var coeff1 = Math.Sin(t * angle) / sinAngle;
                newDir = from * coeff0 + to * coeff1;
            }
            else
            {
                newDir = to;
            }

            // --- 5. 目标长度（不直接用scalar MoveTowards）
            var newMag = curMag;

            if (Math.Abs(tarMag - curMag) > Math.NormalizeEpsilon)
            {
                var delta = tarMag - curMag;
                var maxDelta = maxMagnitudeDelta;

                if (Math.Abs(delta) <= maxDelta)
                    newMag = tarMag;
                else
                    newMag = curMag + Math.Sign(delta) * maxDelta;
            }

            var rotated = newDir * newMag;

            // --- 6. 最关键：再用 Vector MoveTowards 兜底（统一语义）
            return MoveTowards(current, rotated, maxMagnitudeDelta);
        }

#if USE_FIXED64
        private static readonly Single _dot48 = Single.Parse("0.48");
        private static readonly Single _dot235 = Single.Parse("0.235");
#else
        private const Single _dot48 = 0.48;
        private const Single _dot235 = 0.235;
#endif

        public static Vector3 SmoothDamp(
            Vector3 current,
            Vector3 target,
            ref Vector3 currentVelocity,
            Single smoothTime,
            Single maxSpeed,
            Single deltaTime
        ) {
            smoothTime = Math.Max(Math.Dot0001, smoothTime);

            var omega = Math.Two / smoothTime;

            var x = omega * deltaTime;
            var exp = Math.One / (Math.One + x + _dot48 * x * x + _dot235 * x * x * x);

            var change = current - target;
            var maxChange = maxSpeed * smoothTime;

            // 限制最大移动范围
            var maxChangeSq = maxChange * maxChange;
            var sqrmag = change.SqrMagnitude;
            if (sqrmag > maxChangeSq) {
                var mag = Math.Sqrt(sqrmag);
                change = change / mag * maxChange;
            }

            var targetTemp = current - change;

            // 更新速度
            var temp = (currentVelocity + change * omega) * deltaTime;
            currentVelocity = (currentVelocity - temp * omega) * exp;

            var output = targetTemp + (change + temp) * exp;

            // 防止 overshoot
            var origMinusCurrent = target - current;
            var outMinusOrig = output - target;

            if (Dot(origMinusCurrent, outMinusOrig) > 0) {
                output = target;
                currentVelocity = (output - target) / deltaTime;
            }

            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Scale(Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);

        public Vector3 Normalized {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                var m = Math.Sqrt(x * x + y * y + z * z);
                return m > Math.NormalizeEpsilon ? this / m : Zero;
            }
        }

        public Vector3 FastNormalized {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                var lenSq = x * x + y * y + z * z;
                return lenSq > Math.NormalizeEpsilon ? this * Math.InvSqrt(lenSq) : Zero;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Dot(Vector3 lhs, Vector3 rhs) => lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs) =>
            new(
                lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.x * rhs.y - lhs.y * rhs.x
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Project(Vector3 vector, Vector3 onNormal) {
            var num1 = Dot(onNormal, onNormal);
            if (num1 < Math.NormalizeEpsilon) return Zero;
            var num2 = Dot(vector, onNormal);
            return new Vector3(
                onNormal.x * num2 / num1,
                onNormal.y * num2 / num1,
                onNormal.z * num2 / num1
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal) {
            var num1 = Dot(planeNormal, planeNormal);
            if (num1 < Math.NormalizeEpsilon) return vector;
            var num2 = Dot(vector, planeNormal);
            return new Vector3(
                vector.x - planeNormal.x * num2 / num1,
                vector.y - planeNormal.y * num2 / num1,
                vector.z - planeNormal.z * num2 / num1
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Angle(Vector3 from, Vector3 to) {
            var num = Math.Sqrt(from.SqrMagnitude * to.SqrMagnitude);
            return num < Math.LooseTolerance ? 0 : Math.Acos(Math.Clamp(Dot(from, to) / num, -1, 1)) * Math.Rad2Deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single FastAngle(Vector3 from, Vector3 to) {
            var denom = from.SqrMagnitude * to.SqrMagnitude;
            if (denom < Math.NormalizeEpsilon) return 0;
            var inv = Math.InvSqrt(denom);
            return Math.Acos(Math.Clamp(Dot(from, to) * inv, -1, 1)) * Math.Rad2Deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single SignedAngle(Vector3 from, Vector3 to, Vector3 axis) {
            var angle = Angle(from, to);
            var n1 = from.y * to.z - from.z * to.y;
            var n2 = from.z * to.x - from.x * to.z;
            var n3 = from.x * to.y - from.y * to.x;
            Single num5 = Math.Sign(axis.x * n1 + axis.y * n2 + axis.z * n3);
            return angle * num5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single FastSignedAngle(Vector3 from, Vector3 to, Vector3 axis) {
            var angle = FastAngle(from, to);
            var n1 = from.y * to.z - from.z * to.y;
            var n2 = from.z * to.x - from.x * to.z;
            var n3 = from.x * to.y - from.y * to.x;
            Single num5 = Math.Sign(axis.x * n1 + axis.y * n2 + axis.z * n3);
            return angle * num5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Distance(Vector3 a, Vector3 b) {
            var tx = a.x - b.x;
            var ty = a.y - b.y;
            var tz = a.z - b.z;
            return Math.Sqrt(tx * tx + ty * ty + tz * tz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single FastDistance(Vector3 a, Vector3 b) {
            var tx = a.x - b.x;
            var ty = a.y - b.y;
            var tz = a.z - b.z;
            return Math.FastSqrt(tx * tx + ty * ty + tz * tz);
        }

        public readonly Single Magnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Sqrt(x * x + y * y + z * z);
        }

        public readonly Single FastMagnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.FastSqrt(x * x + y * y + z * z);
        }

        public readonly Single SqrMagnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => x * x + y * y + z * z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ClampMagnitude(Vector3 vector, Single maxLength) {
            var lengthSq = vector.SqrMagnitude;
            if (lengthSq <= maxLength * maxLength) return vector;
            var length = Math.Sqrt(lengthSq);
            return new(vector.x / length * maxLength, vector.y / length * maxLength, vector.z / length * maxLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 FastClampMagnitude(Vector3 vector, Single maxLength) {
            var lengthSq = vector.SqrMagnitude;
            if (lengthSq <= maxLength * maxLength) return vector;
            var invLen = Math.InvSqrt(lengthSq);
            return new(vector.x * invLen * maxLength, vector.y * invLen * maxLength, vector.z * invLen * maxLength);
        }

        public override string ToString() => ToString(null, null);

        public string ToString(string format) => ToString(format, null);

        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format)) format = "F2";
            formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
            return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(x, y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) => other is Vector3 other1 && Equals(other1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3 other) => x == other.x && y == other.y && z == other.z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal) {
            var num = -2 * Dot(inNormal, inDirection);
            return new(
                num * inNormal.x + inDirection.x,
                num * inNormal.y + inDirection.y,
                num * inNormal.z + inDirection.z
            );
        }

        public readonly Vector3 GetNormalizedAndLength(out Single length) {
            length = Magnitude;
            return length > 0 ? new(x / length, y / length, z / length) : Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Min(Vector3 a, Vector3 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Max(Vector3 a, Vector3 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 a, Vector3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 a) => new(-a.x, -a.y, -a.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 a, Single d) => new(a.x * d, a.y * d, a.z * d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Single d, Vector3 a) => new(a.x * d, a.y * d, a.z * d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 a, Single d) => new(a.x / d, a.y / d, a.z / d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3 a, Vector3 b) => a.x == b.x && a.y == b.y && a.z == b.z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);

        public static readonly Vector3 Zero = new(0, 0, 0);
        public static readonly Vector3 One = new(1, 1, 1);
        public static readonly Vector3 Up = new(0, 1, 0);
        public static readonly Vector3 Down = new(0, -1, 0);
        public static readonly Vector3 Left = new(-1, 0, 0);
        public static readonly Vector3 Right = new(1, 0, 0);
        public static readonly Vector3 Forward = new(0, 0, 1);
        public static readonly Vector3 Back = new(0, 0, -1);
    }
}
