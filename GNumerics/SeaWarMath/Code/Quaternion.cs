using System.Globalization;

namespace SeaWar.Mathematics {
    /// <summary>
    /// 四元数
    /// </summary>
    /// <author>gouanlin</author>
    public struct Quaternion : IEquatable<Quaternion>, IFormattable {
        public Single x, y, z, w;

        public Quaternion(Single x, Single y, Single z, Single w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection) {
            var q = new Quaternion();
            q.SetFromToRotation(fromDirection, toDirection);
            return q;
        }

        public static Quaternion Inverse(Quaternion rotation) {
            var lengthSq = Dot(rotation, rotation);
            if (lengthSq < GMath.NormalizeEpsilon) return Identity;

            var invLengthSq = 1 / lengthSq;
            return new Quaternion(
                -rotation.x * invLengthSq,
                -rotation.y * invLengthSq,
                -rotation.z * invLengthSq,
                rotation.w * invLengthSq
            );
        }

        public static Quaternion Slerp(Quaternion a, Quaternion b, Single t) =>
            SlerpUnclamped(a, b, GMath.Clamp(t, 0, 1));

        public static readonly Single CloseThreshold = Single.Parse("0.9995");

        public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, Single t) {
            var dot = Dot(a, b);

            // 如果点积为负，取反b以选择最短路径
            if (dot < 0) {
                b = new Quaternion(-b.x, -b.y, -b.z, -b.w);
                dot = -dot;
            }

            // 如果四元数非常接近，使用线性插值
            if (dot > CloseThreshold) {
                return Normalize(new Quaternion(
                    a.x + (b.x - a.x) * t,
                    a.y + (b.y - a.y) * t,
                    a.z + (b.z - a.z) * t,
                    a.w + (b.w - a.w) * t
                ));
            }

            // 球面线性插值
            var theta = GMath.Acos(dot);
            var sinTheta = GMath.Sin(theta);
            var wa = GMath.Sin((1 - t) * theta) / sinTheta;
            var wb = GMath.Sin(t * theta) / sinTheta;

            return new Quaternion(
                wa * a.x + wb * b.x,
                wa * a.y + wb * b.y,
                wa * a.z + wb * b.z,
                wa * a.w + wb * b.w
            );
        }

        public static Quaternion Lerp(Quaternion a, Quaternion b, Single t) =>
            LerpUnclamped(a, b, GMath.Clamp(t, 0, 1));

        public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, Single t) {
            var dot = Dot(a, b);

            Quaternion result;
            if (dot >= 0) {
                result = new Quaternion(
                    a.x + (b.x - a.x) * t,
                    a.y + (b.y - a.y) * t,
                    a.z + (b.z - a.z) * t,
                    a.w + (b.w - a.w) * t
                );
            } else {
                result = new Quaternion(
                    a.x + (-b.x - a.x) * t,
                    a.y + (-b.y - a.y) * t,
                    a.z + (-b.z - a.z) * t,
                    a.w + (-b.w - a.w) * t
                );
            }

            return Normalize(result);
        }

        public static Quaternion AngleAxis(Single angle, Vector3 axis) {
            axis = axis.FastNormalized;
            var halfAngle = angle * GMath.Dot5 * GMath.PI / GMath.I180;
            var s = GMath.Sin(halfAngle);

            return new Quaternion(
                axis.x * s,
                axis.y * s,
                axis.z * s,
                GMath.Cos(halfAngle)
            );
        }

        public static Quaternion LookRotation(Vector3 forward) =>
            LookRotation(forward, Vector3.Up);

        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards) {
            forward = forward.FastNormalized;
            upwards = upwards.FastNormalized;

            var right = Vector3.Cross(upwards, forward);
            upwards = Vector3.Cross(forward, right);

            var m00 = right.x;
            var m01 = right.y;
            var m02 = right.z;
            var m10 = upwards.x;
            var m11 = upwards.y;
            var m12 = upwards.z;
            var m20 = forward.x;
            var m21 = forward.y;
            var m22 = forward.z;

            var trace = m00 + m11 + m22;
            var q = new Quaternion();

            if (trace > 0) {
                var s = GMath.FastSqrt(trace + 1);
                q.w = s * GMath.Dot5;
                s = GMath.Dot5 / s;
                q.x = (m12 - m21) * s;
                q.y = (m20 - m02) * s;
                q.z = (m01 - m10) * s;
            } else if (m00 >= m11 && m00 >= m22) {
                var s = GMath.FastSqrt(1 + m00 - m11 - m22);
                q.x = GMath.Dot5 * s;
                s = GMath.Dot5 / s;
                q.y = (m01 + m10) * s;
                q.z = (m02 + m20) * s;
                q.w = (m12 - m21) * s;
            } else if (m11 > m22) {
                var s = GMath.FastSqrt(1 + m11 - m00 - m22);
                q.y = GMath.Dot5 * s;
                s = GMath.Dot5 / s;
                q.x = (m10 + m01) * s;
                q.z = (m21 + m12) * s;
                q.w = (m20 - m02) * s;
            } else {
                var s = GMath.FastSqrt(1 + m22 - m00 - m11);
                q.z = GMath.Dot5 * s;
                s = GMath.Dot5 / s;
                q.x = (m20 + m02) * s;
                q.y = (m21 + m12) * s;
                q.w = (m01 - m10) * s;
            }

            return q;
        }

        public Single this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                switch (index) {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default: throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                switch (index) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        public static readonly Quaternion Identity = new(0, 0, 0, 1);

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs) =>
            new(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z
            );

        public static Vector3 operator *(Quaternion rotation, Vector3 point) {
            var x = rotation.x * 2;
            var y = rotation.y * 2;
            var z = rotation.z * 2;
            var xx = rotation.x * x;
            var yy = rotation.y * y;
            var zz = rotation.z * z;
            var xy = rotation.x * y;
            var xz = rotation.x * z;
            var yz = rotation.y * z;
            var wx = rotation.w * x;
            var wy = rotation.w * y;
            var wz = rotation.w * z;

            return new Vector3(
                (1 - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z,
                (xy + wz) * point.x + (1 - (xx + zz)) * point.y + (yz - wx) * point.z,
                (xz - wy) * point.x + (yz + wx) * point.y + (1 - (xx + yy)) * point.z
            );
        }

        public static bool operator ==(Quaternion lhs, Quaternion rhs) => Dot(lhs, rhs) > GMath.Dot999999;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Quaternion lhs, Quaternion rhs) => !(lhs == rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single Dot(Quaternion a, Quaternion b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

        public void SetLookRotation(Vector3 view) => this = LookRotation(view, Vector3.Up);

        public void SetLookRotation(Vector3 view, Vector3 up) => this = LookRotation(view, up);

        public static Single Angle(Quaternion a, Quaternion b) {
            var dot = Dot(a, b);
            return GMath.Acos(GMath.Min(GMath.Abs(dot), 1)) * 2 * GMath.Rad2Deg;
        }

        public Vector3 EulerAngles {
            get => ToEulerAngles(this);
            set => this = Euler(value);
        }

        public static Quaternion Euler(Single x, Single y, Single z) {
            var halfRad = GMath.Deg2Rad * GMath.Dot5;
            var sx = GMath.Sin(x * halfRad);
            var cx = GMath.Cos(x * halfRad);
            var sy = GMath.Sin(y * halfRad);
            var cy = GMath.Cos(y * halfRad);
            var sz = GMath.Sin(z * halfRad);
            var cz = GMath.Cos(z * halfRad);

            return new Quaternion(
                sx * cy * cz + cx * sy * sz,
                cx * sy * cz - sx * cy * sz,
                cx * cy * sz - sx * sy * cz,
                cx * cy * cz + sx * sy * sz
            );
        }

        public static Quaternion Euler(Vector3 euler) => Euler(euler.x, euler.y, euler.z);

        private static Vector3 ToEulerAngles(Quaternion q) {
            Vector3 euler;

            // Roll (x-axis rotation)
            var sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            var cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            euler.x = GMath.Atan2(sinr_cosp, cosr_cosp) * GMath.Rad2Deg;

            // Pitch (y-axis rotation)
            var sinp = 2 * (q.w * q.y - q.z * q.x);
            if (GMath.Abs(sinp) >= 1)
                euler.y = GMath.CopySign(GMath.PI / 2, sinp) * GMath.Rad2Deg;
            else
                euler.y = GMath.Asin(sinp) * GMath.Rad2Deg;

            // Yaw (z-axis rotation)
            var siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            var cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            euler.z = GMath.Atan2(siny_cosp, cosy_cosp) * GMath.Rad2Deg;

            return euler;
        }

        public void ToAngleAxis(out Single angle, out Vector3 axis) {
            var q = this;
            if (GMath.Abs(q.w) > 1) q = Normalize(q);

            angle = 2 * GMath.Acos(q.w) * GMath.Rad2Deg;
            var den = GMath.Sqrt(1 - q.w * q.w);

            if (den > GMath.Dot0001) {
                axis = new Vector3(q.x / den, q.y / den, q.z / den);
            } else {
                axis = new Vector3(1, 0, 0);
            }
        }

        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection) {
            fromDirection = fromDirection.FastNormalized;
            toDirection = toDirection.FastNormalized;

            var dot = Vector3.Dot(fromDirection, toDirection);

            if (dot > GMath.Dot999999) {
                this = Identity;
                return;
            }

            if (dot < -GMath.Dot999999) {
                var axis = Vector3.Cross(Vector3.Right, fromDirection);
                if (Vector3.Dot(axis, axis) < GMath.NormalizeEpsilon)
                axis = Vector3.Cross(Vector3.Up, fromDirection);

                axis = axis.FastNormalized;
                this = AngleAxis(180, axis);
                return;
            }

            var cross = Vector3.Cross(fromDirection, toDirection);
            var s = GMath.Sqrt((1 + dot) * 2);
            var invS = 1 / s;

            x = cross.x * invS;
            y = cross.y * invS;
            z = cross.z * invS;
            w = s * GMath.Dot5;
        }

        public static Quaternion RotateTowards(Quaternion from, Quaternion to, Single maxDegreesDelta) {
            var angle = Angle(from, to);
            if (angle == 0)
                return to;

            return SlerpUnclamped(from, to, GMath.Min(1, maxDegreesDelta / angle));
        }

        public static Quaternion Normalize(Quaternion q) {
            var mag = GMath.Sqrt(Dot(q, q));
            if (mag < Single.Epsilon)
                return Identity;

            return new Quaternion(q.x / mag, q.y / mag, q.z / mag, q.w / mag);
        }

        public void Normalize() => this = Normalize(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(x, y, z, w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) => other is Quaternion other1 && Equals(other1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quaternion other) {
            return x.Equals(other.x) && y.Equals(other.y) &&
                   z.Equals(other.z) && w.Equals(other.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => ToString(null, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format) => ToString(format, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format))
                format = "F5";
            if (formatProvider == null)
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            return string.Format("({0}, {1}, {2}, {3})",
                x.ToString(format, formatProvider),
                y.ToString(format, formatProvider),
                z.ToString(format, formatProvider),
                w.ToString(format, formatProvider));
        }
    }
}
