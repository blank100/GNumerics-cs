using System.Runtime.InteropServices;

namespace Gal.Core
{
	/// <summary>
	/// 2D rotation (8 bytes)
	/// This is similar to using a complex number for rotation
	/// </summary>
#if USE_FIXED64
	[StructLayout(LayoutKind.Explicit, Size = 16)]
#else
	[StructLayout(LayoutKind.Explicit, Size = 8)]
#endif
	public struct Complex
	{
		/// <summary>
		/// 实部(旋转的cos值)
		/// </summary>
		[FieldOffset(0)]
		public Single r;

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

		/// <summary>
		/// 获取单位向量，如果长度为0，则返回零向量
		/// </summary>
		public Complex Normalize {
			get {
				var mag = (Single)Math.Sqrt(i * i + r * r);
				var invMag = mag > 0 ? 1 / mag : 0;
				return new(r * invMag, i * invMag);
			}
		}

		public Complex(Single r, Single i) {
			this.r = r;
			this.i = i;
		}

		public static implicit operator Complex((Single c, Single s) tuple) => new(tuple.c, tuple.s);

		public override string ToString() => $"{r},{i}";

		/// Get the angle  radians  the range [-pi, pi]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly Single GetAngle() => (Single)Math.Atan2(i, r);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Complex FromAngle(Single angle) => angle == 0 ? Identity : new((Single)Math.Cos(angle), (Single)Math.Sin(angle));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 operator -(Complex a) => new(-a.r, -a.i);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Complex operator *(Complex left, Complex right) => Multiply(left, right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 operator *(Complex left, Vec2 right) => Multiply(left, right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Complex operator /(Complex left, Complex right) => Divide(left, right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 operator /(Complex left, Vec2 right) => Divide(left, right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Complex Multiply(Complex left, Complex right) => new(left.r * right.r - left.i * right.i, left.i * right.r + left.r * right.i);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Complex Divide(Complex left, Complex right) => new(right.r * left.r + right.i * left.i, right.r * left.i - right.i * left.r);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 Multiply(Complex complex, Vec2 vec) => new(complex.r * vec.x - complex.i * vec.y, complex.r * vec.y + complex.i * vec.x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 Divide(Complex complex, Vec2 vec) => new(vec.x * complex.r + vec.y * complex.i, vec.y * complex.r - vec.x * complex.i);

		/// Is this rotation normalized?
		public readonly bool IsNormalized() {
			// larger tolerance due to failure on mingw 32-bit
			var qq = i * i + r * r;
			return qq > (Single)1.0f - (Single)0.0006f && qq < (Single)1.0f + (Single)0.0006f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Complex Lerp(Single angleA, Single angleB, Single t) {
			var angle = angleA + (angleB - angleA) * t;
			return new((Single)Math.Cos(angle), (Single)Math.Sin(angle));
		}
	}
}
