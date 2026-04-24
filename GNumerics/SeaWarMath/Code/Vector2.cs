using System.Globalization;

namespace SeaWar.Mathematics {
    /// <summary>
    /// 2D向量
    /// </summary>
    /// <author>gouanlin</author>
    [Serializable]
    public struct Vector2 : IEquatable<Vector2>, IFormattable {
        public Single x, y;

        public Single this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get =>
                index switch {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException("Invalid Vector2Fixed index!")
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
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2Fixed index!");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(Single x, Single y) {
            this.x = x;
            this.y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(Vector2 a, Vector2 b, Single t) {
            t = Math.Clamp(t, 0, 1);
            return new(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, Single t) => new(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);

        /// 如果调用的比较频繁,建议外部缓存两向量的距离后手动实现
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 MoveTowards(Vector2 current, Vector2 target, Single maxDistanceDelta) {
            var num1 = target.x - current.x;
            var num2 = target.y - current.y;
            var d = num1 * num1 + num2 * num2;
            if (d == 0 || maxDistanceDelta >= 0 && d <= maxDistanceDelta * maxDistanceDelta) return target;
            var num3 = Math.Sqrt(d);
            return new(current.x + num1 / num3 * maxDistanceDelta, current.y + num2 / num3 * maxDistanceDelta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Scale(Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

        public Vector2 Normalized {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                var m = Math.Sqrt(x * x + y * y);
                return m > Math.NormalizeEpsilon ? this / m : Zero;
            }
        }

        public Vector2 FastNormalized {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                var lenSq = x * x + y * y;
                return lenSq > Math.NormalizeEpsilon ? this * Math.InvSqrt(lenSq) : Zero;
            }
        }

        public override string ToString() => ToString(null, null);

        public string ToString(string format) => ToString(format, null);

        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format)) format = "F2";
            formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
            return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) => other is Vector2 other1 && Equals(other1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2 other) => x == other.x && y == other.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal) {
            var num = -2 * Dot(inNormal, inDirection);
            return new(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Perpendicular(Vector2 inDirection) => new(-inDirection.y, inDirection.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Dot(Vector2 lhs, Vector2 rhs) => lhs.x * rhs.x + lhs.y * rhs.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Cross(Vector2 lhs, Vector2 rhs) => lhs.x * rhs.y - lhs.y * rhs.x;

        public readonly Single Magnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.Sqrt(x * x + y * y);
        }

        public readonly Single FastMagnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Math.FastSqrt(x * x + y * y);
        }

        public Single SqrMagnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => x * x + y * y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Angle(Vector2 from, Vector2 to) {
            var num = Math.Sqrt(from.SqrMagnitude * to.SqrMagnitude);
            return num < Math.LooseTolerance ? 0 : Math.Acos(Math.Clamp(Dot(from, to) / num, -1, 1)) * Math.Rad2Deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single FastAngle(Vector2 from, Vector2 to) {
            var denom = from.SqrMagnitude * to.SqrMagnitude;
            if (denom < Math.NormalizeEpsilon) return 0;
            var inv = Math.InvSqrt(denom);
            return Math.Acos(Math.Clamp(Dot(from, to) * inv, -1, 1)) * Math.Rad2Deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single SignedAngle(Vector2 from, Vector2 to) => Angle(from, to) * Math.Sign(from.x * to.y - from.y * to.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single FastSignedAngle(Vector2 from, Vector2 to) => FastAngle(from, to) * Math.Sign(from.x * to.y - from.y * to.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Distance(Vector2 a, Vector2 b) {
            var num1 = a.x - b.x;
            var num2 = a.y - b.y;
            return Math.Sqrt(num1 * num1 + num2 * num2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single FastDistance(Vector2 a, Vector2 b) {
            var tx = a.x - b.x;
            var ty = a.y - b.y;
            return Math.FastSqrt(tx * tx + ty * ty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ClampMagnitude(Vector2 vector, Single maxLength) {
            var lengthSq = vector.SqrMagnitude;
            if (lengthSq <= maxLength * maxLength) return vector;
            var length = Math.Sqrt(lengthSq);
            return new(vector.x / length * maxLength, vector.y / length * maxLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 FastClampMagnitude(Vector2 vector, Single maxLength) {
            var lengthSq = vector.SqrMagnitude;
            if (lengthSq <= maxLength * maxLength) return vector;
            var invLen = Math.InvSqrt(lengthSq);
            return new(vector.x * invLen * maxLength, vector.y * invLen * maxLength);
        }

        public readonly Vector2 GetNormalizedAndLength(out Single length) {
            length = Magnitude;
            return length > 0 ? new(x / length, y / length) : Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Min(Vector2 lhs, Vector2 rhs) => new(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Max(Vector2 lhs, Vector2 rhs) => new(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));

#if USE_FIXED64
        private static readonly Single _dot48 = Single.Parse("0.48");
        private static readonly Single _dot235 = Single.Parse("0.235");
#else
        private const Single _dot48 = 0.48;
        private const Single _dot235 = 0.235;
#endif

        public static Vector2 SmoothDamp(
            Vector2 current,
            Vector2 target,
            ref Vector2 currentVelocity,
            Single smoothTime,
            Single maxSpeed,
            Single deltaTime
        ) {
            smoothTime = Math.Max(Math.Dot0001, smoothTime);

            var omega = Math.Two / smoothTime;

            var x = omega * deltaTime;
            var exp = Math.One / (Math.One + x + _dot48 * x * x + _dot235 * x * x * x);

            var changeX = current.x - target.x;
            var changeY = current.y - target.y;

            var originalTarget = target;

            // Clamp max speed
            var maxChange = maxSpeed * smoothTime;
            var maxChangeSq = maxChange * maxChange;
            var sqrmag = changeX * changeX + changeY * changeY;

            if (sqrmag > maxChangeSq) {
                var mag = Math.Sqrt(sqrmag);
                changeX = changeX / mag * maxChange;
                changeY = changeY / mag * maxChange;
            }

            target.x = current.x - changeX;
            target.y = current.y - changeY;

            var tempX = (currentVelocity.x + omega * changeX) * deltaTime;
            var tempY = (currentVelocity.y + omega * changeY) * deltaTime;

            currentVelocity.x = (currentVelocity.x - omega * tempX) * exp;
            currentVelocity.y = (currentVelocity.y - omega * tempY) * exp;

            var outputX = target.x + (changeX + tempX) * exp;
            var outputY = target.y + (changeY + tempY) * exp;

            // 防止 overshoot
            var origMinusCurrentX = originalTarget.x - current.x;
            var origMinusCurrentY = originalTarget.y - current.y;

            var outMinusOrigX = outputX - originalTarget.x;
            var outMinusOrigY = outputY - originalTarget.y;

            if (origMinusCurrentX * outMinusOrigX + origMinusCurrentY * outMinusOrigY > 0) {
                outputX = originalTarget.x;
                outputY = originalTarget.y;

                currentVelocity.x = (outputX - originalTarget.x) / deltaTime;
                currentVelocity.y = (outputY - originalTarget.y) / deltaTime;
            }

            return new Vector2(outputX, outputY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.x + b.x, a.y + b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.x - b.x, a.y - b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.x / b.x, a.y / b.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 a) => new(-a.x, -a.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 a, Single d) => new(a.x * d, a.y * d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Single d, Vector2 a) => new(a.x * d, a.y * d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 a, Single d) => new(a.x / d, a.y / d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2 lhs, Vector2 rhs) => lhs.x == rhs.x && lhs.y == rhs.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

        public static readonly Vector2 Zero = new(0, 0);
        public static readonly Vector2 One = new(1, 1);
        public static readonly Vector2 Up = new(0, 1);
        public static readonly Vector2 Down = new(0, -1);
        public static readonly Vector2 Left = new(-1, 0);
        public static readonly Vector2 Right = new(1, 0);
    }
}
