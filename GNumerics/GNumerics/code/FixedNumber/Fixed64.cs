using NumericType = Gal.Core.Fixed64;

namespace Gal.Core {
    /// <summary>
    /// 64位定点数
    /// </summary>
    /// <author>gouanlin</author>
    [Serializable]
    public readonly partial struct Fixed64 : IEquatable<Fixed64>, IComparable<Fixed64>, IFormattable {
        public const int FRACTION_BITS = 32;

        private const long OneRawValue = 1L << FRACTION_BITS;
        private const long TwoRawValue = OneRawValue << 1;
        private const long HalfOneRawValue = OneRawValue >> 1;

        /// 整数部分掩码
        public const ulong INTEGER_PART_MASK = ulong.MaxValue >> FRACTION_BITS << FRACTION_BITS;

        /// 小数部分掩码
        public const ulong FRACTIONAL_PART_MASK = ~INTEGER_PART_MASK;

        /// 最小单位
        public static readonly NumericType Precision = new(1);

        /// 机器epsilon（用于极限判断）
        public static readonly NumericType Epsilon = new(1 << 6);

        /// 常规几何判断
        public static readonly NumericType Tolerance = new(1L << 8);

        /// 更宽松（比如接触稳定）
        public static readonly NumericType LooseTolerance = new(1L << 10);

        /// 非常严格（例如归一化）
        public static readonly NumericType TightTolerance = new(1L << 6);

        /// 支持的小数位数
        public static readonly int FractionalDigits = (int)Math.Log10(OneRawValue);

        public static readonly string StringFormat = "0." + new string('#', FractionalDigits);

        public static readonly NumericType Zero = new(0L);
        public static readonly NumericType One = new(OneRawValue);
        public static readonly NumericType Two = new(TwoRawValue);
        public static readonly NumericType Dot5 = new(HalfOneRawValue);

        public static readonly NumericType MaxValue = new(long.MaxValue);
        public static readonly NumericType MinValue = new(long.MinValue);

        /// 3.14159265358979323846
        public static readonly NumericType PI = new(13493037704); //3.1415926534682512

        /// PI * 2
        public static readonly NumericType TwoPI = new(PI._raw << 1);

        /// PI / 2
        public static readonly NumericType PIOver2 = new(PI._raw >> 1);

        /// PI / 3
        public static readonly NumericType PIOver3 = new(PI._raw / 3);

        /// PI / 4
        public static readonly NumericType PIOver4 = new(PI._raw >> 2);

        /// PI / 6
        public static readonly NumericType PIOver6 = new(PI._raw / 6);

        /// Natural logarithm of 2<br/>
        /// 0.6931471805599453D
        public static readonly NumericType LN2 = new(2977044471); //0.6931471803691238

        // 1/LN2
        public static readonly NumericType InvLn2 = 1 / LN2;

        public static readonly NumericType InvLog2_10 = 1 / Log2(10);

        /// Asin Padé approximations<br/>
        /// 1 / 6 = 0.183320102085006D
        public static readonly NumericType SPadeA1 = new(787353843); //0.18332010204903781

        /// Asin Padé approximations<br/>
        /// 0.021880409899862D
        public static readonly NumericType SPadeA2 = new(93975644); //0.02188040968030691

        private const long _log2MinRawValue = -64L * OneRawValue;
        public static readonly NumericType Log2Min = new(_log2MinRawValue);

        private const long _log2MaxRawValue = 63L * OneRawValue;
        public static readonly NumericType Log2Max = new(_log2MaxRawValue);

        /// <summary>
        /// Degrees to radians conversion factor
        /// <code>π / 180 = 0.01745329251994329576D</code>
        /// </summary>
        public static readonly NumericType Deg2Rad = new(74961320); //0.01745329238474369

        /// <summary>
        /// Radians to degrees conversion factor
        /// <code>180 / π = 57.2957795130823208767D</code>
        /// </summary>
        public static readonly NumericType Rad2Deg = new(246083499207); //57.295779512962326

        // Carefully optimized polynomial coefficients for sin(x), ensuring maximum precision in NumericType math.
        /// 0.16666667605750262737274169921875D; // 1/3!
        public static readonly NumericType SSinCoeff3 = new(715827923); //0.16666667605750263

        /// 0.0083328341133892536163330078125D; // 1/5!
        public static readonly NumericType SSinCoeff5 = new(35789250); //0.008332834113389254

        /// 0.00019588856957852840423583984375D; // 1/7!
        public static readonly NumericType SSinCoeff7 = new(841335); //0.0001958885695785284

        // --- Padé 近似系数 ---
        // C1 = 1/2 = 0.5
        // private static readonly NumericType PadeC1 = HalfOne;

        // C2 = 1/10 = 0.1
        private static readonly NumericType PadeC2 = new(429496729);

        // C3 = 1/120 = 0.00833333...
        private static readonly NumericType PadeC3 = new(35791394);

        // 0.2928932
        private static readonly long _invSqrtFastArg1 = 1257149253L;

        // 0.70710678
        private static readonly long _invSqrtFastArg2 = 3037000500L;

        // 0.20710678
        private static readonly long _invSqrtFastArg3 = 889216706L;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType FromRaw(long rawValue) => new(rawValue);

        public static NumericType Parse(ReadOnlySpan<char> chars) => new(Fixed64Utils.FromChars(chars, FRACTION_BITS));

        private readonly long _raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Fixed64(long v) => _raw = v;

        public long Raw {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _raw;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(float v) => new((long)((double)v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(NumericType v) => (float)((double)v._raw / OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NumericType(int v) => new((long)v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(NumericType v) => (int)(v._raw >> FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NumericType(uint v) => new((long)v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(NumericType v) => (uint)(v._raw >> FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(long v) => new(v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(NumericType v) => v._raw >> FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(double v) => new((long)(v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(NumericType v) => (double)v._raw / OneRawValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(decimal v) => new((long)(v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator decimal(NumericType v) => (decimal)v._raw / OneRawValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(string v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(ReadOnlySpan<char> v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(Span<char> v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(ReadOnlyMemory<char> v) => Parse(v.Span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator NumericType(Memory<char> v) => Parse(v.Span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator +(NumericType a) => new(a._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator -(NumericType a) => new(-a._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator +(NumericType a, NumericType b) => new(checked(a._raw + b._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator +(NumericType a, int b) => new(checked(a._raw + ((long)b << FRACTION_BITS)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator +(int a, NumericType b) => new(checked(((long)a << FRACTION_BITS) + b._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator -(NumericType a, NumericType b) => new(a._raw - b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator -(NumericType a, int b) => new(a._raw - ((long)b << FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator -(int a, NumericType b) => new(((long)a << FRACTION_BITS) - b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator ++(NumericType a) => new(a._raw + OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator --(NumericType a) => new(a._raw - OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator *(NumericType a, NumericType b) => new(Fixed64Utils.FastMul(a._raw, b._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType FastMul(NumericType a, NumericType b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator *(NumericType a, int b) => new(Fixed64Utils.FastMul(a._raw, (long)b << FRACTION_BITS, FRACTION_BITS, FRACTIONAL_PART_MASK));

        /// 有越界风险,当值很小时可以使用
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType FastMul(NumericType a, int b) => new(a._raw * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator *(int a, NumericType b) => new(Fixed64Utils.FastMul((long)a << FRACTION_BITS, b._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        /// 有越界风险,当值很小时可以使用
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType FastMul(int a, NumericType b) => new(a * b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator /(NumericType a, NumericType b) => new(Fixed64Utils.Div(a._raw, b._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator /(NumericType a, int b) => new(a._raw / b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator /(int a, NumericType b) => new(Fixed64Utils.Div((long)a << FRACTION_BITS, b._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator %(NumericType a, NumericType b) => new(a._raw % b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator <<(NumericType a, int c) => new(a._raw << c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType operator >> (NumericType a, int c) => new(a._raw >> c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(NumericType a, NumericType b) => a._raw == b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(NumericType a, int b) => a._raw == (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int a, NumericType b) => (long)a << FRACTION_BITS == b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(NumericType a, NumericType b) => a._raw != b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(NumericType a, int b) => a._raw != (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int a, NumericType b) => (long)a << FRACTION_BITS != b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(NumericType a, NumericType b) => a._raw > b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(NumericType a, int b) => a._raw > (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(int a, NumericType b) => (long)a << FRACTION_BITS > b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(NumericType a, NumericType b) => a._raw < b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(NumericType a, int b) => a._raw < (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(int a, NumericType b) => (long)a << FRACTION_BITS < b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(NumericType a, NumericType b) => a._raw >= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(NumericType a, int b) => a._raw >= (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(int a, NumericType b) => (long)a << FRACTION_BITS >= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(NumericType a, NumericType b) => a._raw <= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(NumericType a, int b) => a._raw <= (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(int a, NumericType b) => (long)a << FRACTION_BITS <= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(NumericType other) => _raw == other._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _raw.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(NumericType other) => _raw.CompareTo(other._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is NumericType other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetRawValue(NumericType value) => value._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType SetRawValue(long value) => new(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Truncate(NumericType value) {
            unchecked {
                var raw = value._raw;
                // 如果 raw < 0，offset = FRACTIONAL_PART_MASK；否则 offset = 0
                var offset = (raw >> 63) & (long)FRACTIONAL_PART_MASK;
                // 对于负数，加上偏移量后再屏蔽，能实现向零取整
                return new((raw + offset) & (long)INTEGER_PART_MASK);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Floor(NumericType value) => new((long)((ulong)value._raw & INTEGER_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Ceiling(NumericType value) => ((ulong)value._raw & FRACTIONAL_PART_MASK) != 0 ? Floor(value) + One : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Abs(NumericType value) => new(Fixed64Utils.Abs(value._raw));

        /// <summary>
        /// 采用的是四舍六入五取偶原则(2.5->2,3.5->4)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Round(NumericType value) => new(Fixed64Utils.Round(value._raw, OneRawValue, INTEGER_PART_MASK, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Clamp(NumericType value, NumericType min, NumericType max) => Max(min, Min(max, value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Clamp01(NumericType value) => Max(Zero, Min(One, value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Min(NumericType a, NumericType b)
        {
            var x = a._raw;
            var y = b._raw;
            var mask = (x - y) >> 63;
            return new(y ^ ((x ^ y) & mask));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Max(NumericType a, NumericType b)
        {
            var x = a._raw;
            var y = b._raw;
            var mask = (x - y) >> 63;
            return new(x ^ ((x ^ y) & mask));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(NumericType value) => value._raw < 0 ? -1 : value._raw > 0 ? 1 : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType CopySign(NumericType x, NumericType y) {
            var signMask = 1L << 63;
            return FromRaw((x.Raw & ~signMask) | (y.Raw & signMask));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Sqrt(NumericType value) => new(Fixed64Utils.Sqrt(value._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType FastSqrt(NumericType x) {
            if (x <= 0) return 0;

            var raw = x._raw;
            var inv = Fixed64Utils.InvSqrtFast(raw, FRACTION_BITS, FRACTIONAL_PART_MASK, _invSqrtFastArg1, _invSqrtFastArg2, _invSqrtFastArg3);
            return new(Fixed64Utils.FastMul(raw, inv, FRACTION_BITS, FRACTIONAL_PART_MASK));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType InvSqrt(NumericType value) =>
            new(Fixed64Utils.InvSqrtFast(value._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, _invSqrtFastArg1, _invSqrtFastArg2, _invSqrtFastArg3));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Pow(NumericType value, int exponent) =>
            new(Fixed64Utils.Pow(value._raw, exponent, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Pow(NumericType b, NumericType exp) =>
            new(Fixed64Utils.Pow(b.Raw, exp.Raw, OneRawValue, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, _log2MinRawValue, _log2MaxRawValue, LN2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Pow2(NumericType x) =>
            new(Fixed64Utils.Pow2(x._raw, OneRawValue, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, _log2MinRawValue, _log2MaxRawValue, LN2._raw));

        /// <summary>
        /// 基于 Padé [3/3] 近似的高精度 Exp 函数
        /// </summary>
        public static NumericType Exp(NumericType x) =>
            new(Fixed64Utils.Exp(
                x._raw,
                OneRawValue,
                HalfOneRawValue,
                FRACTION_BITS,
                FRACTIONAL_PART_MASK,
                _log2MinRawValue,
                _log2MaxRawValue,
                LN2._raw,
                InvLn2._raw,
                PadeC2._raw,
                PadeC3._raw
            ));

        /// C# 和 unity 中, ln = log
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Log(NumericType x)
            // 使用换底公式: ln(x) = log2(x) * ln(2)
            => new(Fixed64Utils.FastMul(Log2(x)._raw, LN2._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Log2(NumericType x) => new(Fixed64Utils.Log2(x._raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Ln(NumericType x) =>
            new(Fixed64Utils.FastMul(Log2(x)._raw, LN2._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Log10(NumericType x)
            // 使用换底公式: log10(x) = log2(x) * (1 / log2(10))
            => new(Fixed64Utils.FastMul(Log2(x)._raw, InvLog2_10._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Sin(NumericType x) =>
            new(Fixed64Utils.Sin(x.Raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI.Raw, TwoPI.Raw, PIOver2.Raw, SSinCoeff3.Raw, SSinCoeff5.Raw, SSinCoeff7.Raw));
        // new(Fixed64Utils.Sin(x._Raw, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, pi._Raw, twoPI._Raw, piOver2._Raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Cos(NumericType x) =>
            new(Fixed64Utils.Cos(x.Raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI._raw, TwoPI._raw, PIOver2._raw, SSinCoeff3._raw, SSinCoeff5._raw, SSinCoeff7._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Tan(NumericType x) =>
            // new(Fixed64Utils.Tan(x._Raw, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, pi._Raw, piOver2._Raw));
            new(Fixed64Utils.Tan(x._raw, Precision._raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI._raw, PIOver2._raw, PIOver4._raw, PIOver6._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Asin(NumericType x) =>
            new(Fixed64Utils.Asin(x._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, HalfOneRawValue, PI._raw, PIOver2._raw, PIOver6._raw, SPadeA1._raw, SPadeA2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Acos(NumericType x) => new(Fixed64Utils.Acos(x._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, PI._raw, PIOver2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Atan(NumericType z) =>
            // new(Fixed64Utils.Atan(z._Raw, FRACTION_BITS, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, piOver2._Raw));
            new(Fixed64Utils.Atan(z._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, HalfOneRawValue, Precision._raw, PIOver4._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NumericType Atan2(NumericType y, NumericType x) =>
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

        public class Comparer : IComparer<NumericType> {
            public static readonly Comparer Instance = new();

            private Comparer() {
            }

            int IComparer<NumericType>.Compare(NumericType x, NumericType y) => x._raw.CompareTo(y._raw);
        }

        public class EqualityComparer : IEqualityComparer<NumericType> {
            public static readonly EqualityComparer Instance = new();

            private EqualityComparer() {
            }

            bool IEqualityComparer<NumericType>.Equals(NumericType x, NumericType y) => x._raw == y._raw;

            int IEqualityComparer<NumericType>.GetHashCode(NumericType num) => num._raw.GetHashCode();
        }
    }
}
