using System.Runtime.CompilerServices;

namespace Gal.Core {
    /// <summary>
    /// 64位定点数
    /// </summary>
    /// <author>gouanlin</author>
    [Serializable]
    public readonly partial struct Fixed40_24 : IEquatable<Fixed40_24>, IComparable<Fixed40_24>, IFormattable {
        public const int FRACTION_BITS = 24;

        private const long OneRawValue = 1L << FRACTION_BITS;
        private const long TwoRawValue = OneRawValue << 1;
        private const long HalfOneRawValue = OneRawValue >> 1;

        /// <summary>
        /// 整数部分掩码
        /// </summary>
        public const ulong INTEGER_PART_MASK = ulong.MaxValue >> FRACTION_BITS << FRACTION_BITS;

        /// <summary>
        /// 小数部分掩码
        /// </summary>
        public const ulong FRACTIONAL_PART_MASK = ~INTEGER_PART_MASK;

        public static readonly Fixed40_24 Precision = new(1L);
        public static readonly int FractionalDigits = (int)Math.Log10(OneRawValue) + 1;
        public static readonly string StringFormat = "0." + new string('#', FractionalDigits);
        public static readonly Fixed40_24 Zero = new(0L);
        public static readonly Fixed40_24 One = new(OneRawValue);
        public static readonly Fixed40_24 Two = new(TwoRawValue);
        public static readonly Fixed40_24 HalfOne = new(HalfOneRawValue);
        public static readonly Fixed40_24 MaxValue = new(long.MaxValue);
        public static readonly Fixed40_24 MinValue = new(long.MinValue);
        public static readonly Fixed40_24 Epsilon = new(1L << (FRACTION_BITS >> 1));
        public static readonly Fixed40_24 EpsilonSqr = Epsilon * Epsilon;

        /// 3.14159265358979323846
        public static readonly Fixed40_24 PI = new(52707179); //3.1415926814079285

        public static readonly Fixed40_24 TwoPI = PI * 2;
        public static readonly Fixed40_24 PIOver2 = PI / 2;
        public static readonly Fixed40_24 PIOver3 = PI / 3;
        public static readonly Fixed40_24 PIOver4 = PI / 4;
        public static readonly Fixed40_24 PIOver6 = PI / 6;

        /// Natural logarithm of 2<br/>
        /// 0.6931471805599453D
        public static readonly Fixed40_24 LN2 = new(11629080); //0.6931471824645996

        // 1/LN2
        public static readonly Fixed40_24 InvLn2 = 1 / LN2;

        /// Asin Padé approximations<br/>
        /// 0.183320102085006D
        public static readonly Fixed40_24 SPadeA1 = new(3075601); //0.18332010507583618

        /// Asin Padé approximations<br/>
        /// 0.021880409899862D
        public static readonly Fixed40_24 SPadeA2 = new(367092); //0.021880388259887695;

        private const long _log2MinRawValue = -64L * OneRawValue;
        public static readonly Fixed40_24 LOG2Min = new(_log2MinRawValue);

        private const long _log2MaxRawValue = 63L * OneRawValue;
        public static readonly Fixed40_24 LOG2Max = new(_log2MaxRawValue);

        /// <summary>
        /// Degrees to radians conversion factor
        /// <code>π / 180 = 0.01745329251994329576D</code>
        /// </summary>
        public static readonly Fixed40_24 Deg2Rad = new(292818); //0.017453312873840332

        /// <summary>
        /// Radians to degrees conversion factor
        /// <code>180 / π = 57.2957795130823208767D</code>
        /// </summary>
        public static readonly Fixed40_24 Rad2Deg = new(961263669); //57.29577952623367

        // Carefully optimized polynomial coefficients for sin(x), ensuring maximum precision in Fixed64 math.
        /// 0.16666667605750262737274169921875D; // 1/3!
        public static readonly Fixed40_24 SSinCoeff3 = new(2796203); //0.1666666865348816

        /// 0.0083328341133892536163330078125D; // 1/5!
        public static readonly Fixed40_24 SSinCoeff5 = new(139802); //0.00833284854888916;

        /// 0.00019588856957852840423583984375D; // 1/7!
        public static readonly Fixed40_24 SSinCoeff7 = new(3286); //0.0001958608627319336;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 FromRaw(long rawValue) => new(rawValue);

        public static Fixed40_24 Parse(ReadOnlySpan<char> chars) => new(Fixed64Utils.FromChars(chars, FRACTION_BITS));

        private readonly long _raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Fixed40_24(long v) => _raw = v;

        public long Raw {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _raw;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(float v) => new((long)((double)v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(Fixed40_24 v) => (float)((double)v._raw / OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Fixed40_24(int v) => new((long)v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(Fixed40_24 v) => (int)(v._raw >> FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Fixed40_24(uint v) => new((long)v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(Fixed40_24 v) => (uint)(v._raw >> FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(long v) => new(v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(Fixed40_24 v) => v._raw >> FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(double v) => new((long)(v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(Fixed40_24 v) => (double)v._raw / OneRawValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(decimal v) => new((long)(v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator decimal(Fixed40_24 v) => (decimal)v._raw / OneRawValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(string v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(ReadOnlySpan<char> v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(Span<char> v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(ReadOnlyMemory<char> v) => Parse(v.Span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed40_24(Memory<char> v) => Parse(v.Span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator +(Fixed40_24 a) => new(a._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator -(Fixed40_24 a) => new(-a._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator +(Fixed40_24 a, Fixed40_24 b) => new(checked(a._raw + b._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator +(Fixed40_24 a, int b) => new(checked(a._raw + ((long)b << FRACTION_BITS)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator +(int a, Fixed40_24 b) => new(checked(((long)a << FRACTION_BITS) + b._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator -(Fixed40_24 a, Fixed40_24 b) => new(a._raw - b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator -(Fixed40_24 a, int b) => new(a._raw - ((long)b << FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator -(int a, Fixed40_24 b) => new(((long)a << FRACTION_BITS) - b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator ++(Fixed40_24 a) => new(a._raw + OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator --(Fixed40_24 a) => new(a._raw - OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator *(Fixed40_24 a, Fixed40_24 b) => new(Fixed64Utils.FastMul(a._raw, b._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 FastMul(Fixed40_24 a, Fixed40_24 b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator *(Fixed40_24 a, int b) => new(Fixed64Utils.FastMul(a._raw, (long)b << FRACTION_BITS, FRACTION_BITS, FRACTIONAL_PART_MASK));

        public static Fixed40_24 FastMul(Fixed40_24 a, int b) => new(a._raw * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator *(int a, Fixed40_24 b) => new(Fixed64Utils.FastMul((long)a << FRACTION_BITS, b._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        public static Fixed40_24 FastMul(int a, Fixed40_24 b) => new(a * b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator /(Fixed40_24 a, Fixed40_24 b) => new(Fixed64Utils.Div(a._raw, b._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator /(Fixed40_24 a, int b) => new(a._raw / b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator /(int a, Fixed40_24 b) => new(Fixed64Utils.Div((long)a << FRACTION_BITS, b._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator %(Fixed40_24 a, Fixed40_24 b) => new(a._raw % b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator <<(Fixed40_24 a, int c) => new(a._raw << c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 operator >> (Fixed40_24 a, int c) => new(a._raw >> c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed40_24 a, Fixed40_24 b) => a._raw == b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed40_24 a, int b) => a._raw == (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int a, Fixed40_24 b) => (long)a << FRACTION_BITS == b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed40_24 a, Fixed40_24 b) => a._raw != b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed40_24 a, int b) => a._raw != (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int a, Fixed40_24 b) => (long)a << FRACTION_BITS != b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fixed40_24 a, Fixed40_24 b) => a._raw > b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fixed40_24 a, int b) => a._raw > (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(int a, Fixed40_24 b) => (long)a << FRACTION_BITS > b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Fixed40_24 a, Fixed40_24 b) => a._raw < b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Fixed40_24 a, int b) => a._raw < (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(int a, Fixed40_24 b) => (long)a << FRACTION_BITS < b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fixed40_24 a, Fixed40_24 b) => a._raw >= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fixed40_24 a, int b) => a._raw >= (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(int a, Fixed40_24 b) => (long)a << FRACTION_BITS >= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fixed40_24 a, Fixed40_24 b) => a._raw <= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fixed40_24 a, int b) => a._raw <= (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(int a, Fixed40_24 b) => (long)a << FRACTION_BITS <= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Fixed40_24 other) => _raw == other._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _raw.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Fixed40_24 other) => _raw.CompareTo(other._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Fixed40_24 other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetRawValue(Fixed40_24 value) => value._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 SetRawValue(long value) => new(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Truncate(Fixed40_24 value) {
            unchecked {
                long raw = value._raw;
                // 如果 raw < 0，offset = FRACTIONAL_PART_MASK；否则 offset = 0
                long offset = (raw >> 63) & (long)FRACTIONAL_PART_MASK;
                // 对于负数，加上偏移量后再屏蔽，能实现向零取整
                return new((raw + offset) & (long)INTEGER_PART_MASK);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Floor(Fixed40_24 value) => new((long)((ulong)value._raw & INTEGER_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Ceiling(Fixed40_24 value) => ((ulong)value._raw & FRACTIONAL_PART_MASK) != 0 ? Floor(value) + One : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Abs(Fixed40_24 value) => new(Fixed64Utils.Abs(value._raw));

        /// <summary>
        /// 采用的是四舍六入五取偶原则(2.5->2,3.5->4)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Round(Fixed40_24 value) => new(Fixed64Utils.Round(value._raw, OneRawValue, INTEGER_PART_MASK, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Clamp(Fixed40_24 value, Fixed40_24 min, Fixed40_24 max) => value._raw < min._raw ? min : value._raw > max._raw ? max : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Clamp01(Fixed40_24 value) => value._raw < Zero._raw ? Zero : value._raw > One._raw ? One : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Min(Fixed40_24 a, Fixed40_24 b) => a._raw <= b._raw ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Max(Fixed40_24 a, Fixed40_24 b) => a._raw >= b._raw ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(Fixed40_24 value) => value._raw < 0 ? -1 : value._raw > 0 ? 1 : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Sqrt(Fixed40_24 value) => new(Fixed64Utils.Sqrt(value._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Pow(Fixed40_24 value, int exponent) => new(Fixed64Utils.Pow(value._raw, exponent, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Pow(Fixed40_24 b, Fixed40_24 exp) =>
            new(Fixed64Utils.Pow(b.Raw, exp.Raw, OneRawValue, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, _log2MinRawValue, _log2MaxRawValue, LN2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Pow2(Fixed40_24 x) =>
            new(Fixed64Utils.Pow2(x._raw, OneRawValue, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, _log2MinRawValue, _log2MaxRawValue, LN2._raw));

        // --- Padé 近似系数 ---
// C1 = 1/2 = 0.5
        private static readonly Fixed40_24 PadeC1 = new(8388608);
// C2 = 1/10 = 0.1
        private static readonly Fixed40_24 PadeC2 = new(1677722); 
// C3 = 1/120 = 0.00833333...
        private static readonly Fixed40_24 PadeC3 = new(139810);

        /// <summary>
        /// 基于 Padé [3/3] 近似的高精度 Exp 函数
        /// </summary>
        public static Fixed40_24 Exp(Fixed40_24 x) {
            if (x._raw == 0) return One;
            if (x._raw <= _log2MinRawValue) return Zero;
            if (x._raw >= _log2MaxRawValue) return MaxValue;

            // 1. 范围缩减 (与之前一致)
            // k = round(x / ln2)
            long k = (Fixed64Utils.FastMul(x._raw, InvLn2._raw, FRACTION_BITS, FRACTIONAL_PART_MASK) + HalfOneRawValue) >> FRACTION_BITS;
            // r = x - k * ln2
            Fixed40_24 r = new(x._raw - (k * LN2._raw));

            // 2. Padé [3/3] 近似计算
            // 我们计算 r^2
            Fixed40_24 r2 = r * r;

            // U = r * ( (1/120 * r^2) + 1/2 )
            Fixed40_24 u = r * (r2 * PadeC3 + PadeC1);

            // V = (1/10 * r^2) + 1
            Fixed40_24 v = r2 * PadeC2 + One;

            // e^r = (V + U) / (V - U)
            // 注意：由于 r 范围极小 (abs < 0.346)，V-U 永远大于 0
            Fixed40_24 er = (v + u) / (v - u);

            // 3. 重组结果: er * 2^k
            if (k == 0) return er;
            if (k > 0) return new(er._raw << (int)k);
            return new(er._raw >> (int)-k);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Log2(Fixed40_24 x) => new(Fixed64Utils.Log2(x._raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Ln(Fixed40_24 x) => new(Fixed64Utils.FastMul(Log2(x)._raw, LN2._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Sin(Fixed40_24 x) =>
            new(Fixed64Utils.Sin(x.Raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI.Raw, TwoPI.Raw, PIOver2.Raw, SSinCoeff3.Raw, SSinCoeff5.Raw, SSinCoeff7.Raw));
        // new(Fixed64Utils.Sin(x._Raw, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, pi._Raw, twoPI._Raw, piOver2._Raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Cos(Fixed40_24 x) =>
            new(Fixed64Utils.Cos(x.Raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI._raw, TwoPI._raw, PIOver2._raw, SSinCoeff3._raw, SSinCoeff5._raw, SSinCoeff7._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Tan(Fixed40_24 x) =>
            // new(Fixed64Utils.Tan(x._Raw, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, pi._Raw, piOver2._Raw));
            new(Fixed64Utils.Tan(x._raw, Precision._raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI._raw, PIOver2._raw, PIOver4._raw, PIOver6._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Asin(Fixed40_24 x) =>
            new(Fixed64Utils.Asin(x._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, HalfOneRawValue, PI._raw, PIOver2._raw, PIOver6._raw, SPadeA1._raw, SPadeA2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Acos(Fixed40_24 x) => new(Fixed64Utils.Acos(x._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, PI._raw, PIOver2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Atan(Fixed40_24 z) =>
            // new(Fixed64Utils.Atan(z._Raw, FRACTION_BITS, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, piOver2._Raw));
            new(Fixed64Utils.Atan(z._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, HalfOneRawValue, Precision._raw, PIOver4._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed40_24 Atan2(Fixed40_24 y, Fixed40_24 x) =>
            new(Fixed64Utils.Atan2(y._raw, x._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, PI._raw, PIOver2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => ((double)this).ToString(StringFormat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format) => ((double)this).ToString(format);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider) => ((decimal)this).ToString(format, formatProvider);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string IFormattable.
            ToString(string format, IFormatProvider formatProvider) => ((double)this).ToString(format, formatProvider);

        public class Comparer : IComparer<Fixed40_24> {
            public static readonly Comparer Instance = new Comparer();

            private Comparer() {
            }

            int IComparer<Fixed40_24>.Compare(Fixed40_24 x, Fixed40_24 y) => x._raw.CompareTo(y._raw);
        }

        public class EqualityComparer : IEqualityComparer<Fixed40_24> {
            public static readonly EqualityComparer Instance = new();

            private EqualityComparer() {
            }

            bool IEqualityComparer<Fixed40_24>.Equals(Fixed40_24 x, Fixed40_24 y) => x._raw == y._raw;

            int IEqualityComparer<Fixed40_24>.GetHashCode(Fixed40_24 num) => num._raw.GetHashCode();
        }
    }
}
