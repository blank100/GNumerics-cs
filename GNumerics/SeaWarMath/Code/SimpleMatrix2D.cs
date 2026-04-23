using System.Runtime.InteropServices;

namespace SeaWar.Mathematics {
    /// <summary>
    /// 简单的2D变换矩阵,它只包含位移和旋转,不包含缩放
    /// 位移描述，含坐标和角度(弧度制)
    /// </summary>
    /// <author>gouanlin</author>
#if USE_FIXED64
    [StructLayout(LayoutKind.Explicit, Size = 32)]
#else
	[StructLayout(LayoutKind.Explicit, Size = 16)]
#endif
    public struct SimpleMatrix2D {
        [FieldOffset(0)] public Vector2 p;

#if USE_FIXED64
        [FieldOffset(16)]
#else
		[FieldOffset(8)]
#endif
        public Complex q;

        public SimpleMatrix2D(Vector2 p, Complex q) {
            this.p = p;
            this.q = q;
        }

        public static implicit operator SimpleMatrix2D((Vector2 p, Complex q) tuple) => new(tuple.p, tuple.q);

        public static readonly SimpleMatrix2D Identity = new(new(0, 0), new(1, 0));

        public static SimpleMatrix2D Mul(in SimpleMatrix2D a, in SimpleMatrix2D b) => new(
            Complex.Rotate(a.q, b.p) + a.p,
            Complex.Mul(a.q, b.q)
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SimpleMatrix2D Inverse(in SimpleMatrix2D t) {
            var invQ = Complex.Conjugate(t.q);
            var invP = -Complex.Rotate(invQ, t.p);
            return new(invP, invQ);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 TransformPoint(in SimpleMatrix2D t, in Vector2 v) {
            var r = t.q.r;
            var i = t.q.i;

            return new(
                v.x * r - v.y * i + t.p.x,
                v.x * i + v.y * r + t.p.y
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 InverseTransformPoint(in SimpleMatrix2D t, in Vector2 v) {
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
        public static SimpleMatrix2D operator *(in SimpleMatrix2D left, in SimpleMatrix2D right) => Mul(in left, in right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(in SimpleMatrix2D t, in Vector2 p) => TransformPoint(in t, in p);

        public override string ToString() => $"{p}({q})";
    }
}
