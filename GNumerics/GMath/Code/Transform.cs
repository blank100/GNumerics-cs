using System.Runtime.InteropServices;


namespace Gal.Core
{
	/// <summary>
	/// A 2D rigid transform (16 bytes)
	/// 位移描述，含坐标和角度(弧度制)
	/// </summary>
#if USE_FIXED64
	[StructLayout(LayoutKind.Explicit, Size = 32)]
#else
	[StructLayout(LayoutKind.Explicit, Size = 16)]
#endif
	public struct Transform
	{
		[FieldOffset(0)]
		public Vec2 p;

#if USE_FIXED64
		[FieldOffset(16)]
#else
		[FieldOffset(8)]
#endif
		public Complex q;

		public Transform(Vec2 p, Complex q) {
			this.p = p;
			this.q = q;
		}

		public static implicit operator Transform((Vec2 p, Complex q) tuple) => new(tuple.p, tuple.q);

		public static readonly Transform Identity = new(new(0, 0), new(1, 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Transform operator *(in Transform left, in Transform right) => Multiply(in left, in right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Transform operator /(in Transform left, in Transform right) => Divide(in left, in right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 operator *(in Transform t, in Vec2 p) => Multiply(in t, in p);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 operator /(in Transform t, in Vec2 p) => Divide(in t, in p);

		public static Transform Multiply(in Transform left, in Transform right) => new(
			Complex.Multiply(right.q, left.p) + right.p,
			Complex.Multiply(left.q, right.q)
		);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Transform Divide(in Transform left, in Transform right) {
			var tp = right.p - left.p;
			return new(
				Complex.Divide(left.q, tp),
				Complex.Divide(left.q, right.q)
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 Multiply(in Transform t, in Vec2 p) {
			// Opt: var result = Rot.Multiply(left, right.Q) + right.P;
			return new(
				(p.x * t.q.r - p.y * t.q.i) + t.p.x,
				(p.y * t.q.r + p.x * t.q.i) + t.p.y
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec2 Divide(in Transform t, in Vec2 p) {
			// Opt: var result = Rot.Divide(left - right.P, right);
			var px = p.x - t.p.x;
			var py = p.y - t.p.y;
			return new(
				(px * t.q.r + py * t.q.i),
				(py * t.q.r - px * t.q.i)
			);
		}

		public static Transform Lerp(in Transform a, in Transform b, Single t) {
			return new(
				Vec2.Lerp(a.p, b.p, t),
				Complex.Lerp(a.q.GetAngle(), b.q.GetAngle(), t)
			);
		}


		public override string ToString() => $"{p}({q})";
	}
}
