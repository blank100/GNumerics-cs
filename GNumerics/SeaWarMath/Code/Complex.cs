using System.Runtime.InteropServices;

namespace SeaWarMath {
    /// <summary>
    /// 复数,可以用于表示2D旋转
    /// </summary>
    /// <author>gouanlin</author>
#if USE_FIXED64
    [StructLayout(LayoutKind.Explicit, Size = 16)]
#else
	[StructLayout(LayoutKind.Explicit, Size = 8)]
#endif
    public struct Complex {
        /// <summary>
        /// 实部(旋转的cos值)
        /// </summary>
        [FieldOffset(0)] public Single r;

        /// <summary>
        /// 虚部(旋转的sin值)
        /// </summary>
#if USE_FIXED64
        [FieldOffset(8)]
#else
		[FieldOffset(4)]
#endif
        public Single i;

        public static readonly Complex Identity = new(1, 0);

        public Complex(Single r, Single i) {
            this.r = r;
            this.i = i;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Single LengthSquared() => r * r + i * i;

        /// <summary>
        /// 获取单位向量，如果长度为0，则返回零向量
        /// </summary>
        public readonly Complex Normalize() {
            var lenSq = r * r + i * i;
            if (lenSq <= 0) return default;

            var inv = 1 / Math.Sqrt(lenSq);
            return new(r * inv, i * inv);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Complex FastRenormalize() {
            var lenSq = r * r + i * i;
            var k = (Single)1.5f - (Single)0.5f * lenSq; // 牛顿一步近似
            return new Complex(r * k, i * k);
        }

        /// Is this rotation normalized?
        public readonly bool IsNormalized() => Math.Abs(i * i + r * r - 1) < (Single)0.0006f;

        /// Get the angle radians the range [-pi, pi]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Single GetAngle() => Math.Atan2(i, r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex FromAngle(Single angle) => angle == 0 ? Identity : new(Math.Cos(angle), Math.Sin(angle));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Conjugate(Complex c) => new(c.r, -c.i);

        public static implicit operator Complex((Single c, Single s) tuple) => new(tuple.c, tuple.s);

        public override string ToString() => $"{r},{i}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(Complex a, Complex b) => Mul(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Complex a, Vector2 b) => Rotate(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Mul(Complex a, Complex b) =>
            new(
                a.r * b.r - a.i * b.i,
                a.i * b.r + a.r * b.i
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Rotate(Complex c, Vector2 v) =>
            new(
                c.r * v.x - c.i * v.y,
                c.r * v.y + c.i * v.x
            );

        // 逆旋转向量
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 InverseRotate(in Complex c, in Vector2 v) =>
            new(
                c.r * v.x + c.i * v.y,
                -c.i * v.x + c.r * v.y
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex LookRotation(in Vector2 forward) {
            var lenSq = forward.x * forward.x + forward.y * forward.y;
            if (lenSq <= 0) return Identity;

            var inv = 1 / Math.Sqrt(lenSq);
            return new Complex(forward.x * inv, forward.y * inv);
        }

        /// 快速构建小角度旋转,在10度(0.174幅度)以内比较合适
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex FromSmallAngle(Single angle) {
            // 泰勒近似（小角度）
            // cos ≈ 1 - a²/2
            // sin ≈ a
            var a2 = angle * angle;
            return new Complex(1 - (Single)0.5f * a2, angle).FastRenormalize();
        }

        /// 性能很低,谨慎调用
        public static Complex LerpAngle(Single a, Single b, Single t) {
            var delta = Math.Atan2(Math.Sin(b - a), Math.Cos(b - a));
            var angle = a + delta * t;
            return FromAngle(angle);
        }

        /// 球形线性插值 (匀速旋转)
        public static Complex Slerp(Complex a, Complex b, Single t) {
            if (t <= 0) return a;
            if (t >= 1) return b;

            var dot = a.r * b.r + a.i * b.i;
            // 确保最短路径
            var targetB = dot < 0 ? new Complex(-b.r, -b.i) : b;

            // 计算 A 到 B 的相对旋转角度
            var relative = Mul(targetB, Conjugate(a));
            var angle = relative.GetAngle() * t;

            // 叠加旋转
            // 性能优化：如果角度极小，使用快速构造
            var delta = (Math.Abs(angle) < (Single)0.174f) ? FromSmallAngle(angle) : FromAngle(angle);
            return Mul(a, delta);
        }

        /// 性能高,且视觉影响较小
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex NLerp(Complex a, Complex b, Single t) => InnerNLerp(a, b, t).Normalize();

        /// 步长较小时使用,它的性能非常高
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex FastNLerp(Complex a, Complex b, Single t) => InnerNLerp(a, b, t).FastRenormalize();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Complex InnerNLerp(Complex a, Complex b, Single t) {
            // 处理点积，确保总是沿最短弧插值
            // 如果点积为负，说明两个旋转方向相反（差 180 度以上），翻转其中一个
            var dot = a.r * b.r + a.i * b.i;
            Single factor = dot < 0 ? -1 : 1;

            return new Complex(
                a.r + (b.r * factor - a.r) * t,
                a.i + (b.i * factor - a.i) * t
            );
        }
    }
}
