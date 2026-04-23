using System.Runtime.InteropServices;


namespace Gal.Core {
    /// <summary>
    /// 2D变换
    /// 位移描述，含坐标和角度(弧度制)
    /// </summary>
    /// <author>gouanlin</author>
#if USE_FIXED64
    [StructLayout(LayoutKind.Explicit, Size = 32)]
#else
	[StructLayout(LayoutKind.Explicit, Size = 16)]
#endif
    public struct Transform {
        [FieldOffset(0)] public Vec2 p;

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

        public static Transform Mul(in Transform a, in Transform b) => new(
            Complex.Rotate(a.q, b.p) + a.p,
            Complex.Mul(a.q, b.q)
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform Inverse(in Transform t) {
            var invQ = Complex.Conjugate(t.q);
            var invP = -Complex.Rotate(invQ, t.p);
            return new(invP, invQ);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2 TransformPoint(in Transform t, in Vec2 v) {
            var r = t.q.r;
            var i = t.q.i;

            return new(
                v.x * r - v.y * i + t.p.x,
                v.x * i + v.y * r + t.p.y
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2 InverseTransformPoint(in Transform t, in Vec2 v) {
            var px = v.x - t.p.x;
            var py = v.y - t.p.y;

            var r = t.q.r;
            var i = t.q.i;

            return new(
                px * r + py * i,
                py * r - px * i
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform operator *(in Transform left, in Transform right) => Mul(in left, in right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec2 operator *(in Transform t, in Vec2 p) => TransformPoint(in t, in p);

        public override string ToString() => $"{p}({q})";
    }
}
