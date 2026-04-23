using System.Runtime.CompilerServices;

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

        /// <summary>
        /// 整数部分掩码
        /// </summary>
        public const ulong INTEGER_PART_MASK = ulong.MaxValue >> FRACTION_BITS << FRACTION_BITS;

        /// <summary>
        /// 小数部分掩码
        /// </summary>
        public const ulong FRACTIONAL_PART_MASK = ~INTEGER_PART_MASK;

        public static readonly Fixed64 Precision = new(1L);
        public static readonly int FractionalDigits = (int)Math.Log10(OneRawValue) + 1;
        public static readonly string StringFormat = "0." + new string('#', FractionalDigits);
        public static readonly Fixed64 Zero = new(0L);
        public static readonly Fixed64 One = new(OneRawValue);
        public static readonly Fixed64 Two = new(TwoRawValue);
        public static readonly Fixed64 HalfOne = new(HalfOneRawValue);
        public static readonly Fixed64 MaxValue = new(long.MaxValue);
        public static readonly Fixed64 MinValue = new(long.MinValue);
        public static readonly Fixed64 Epsilon = new(1L << (FRACTION_BITS >> 1));
        public static readonly Fixed64 EpsilonSqr = Epsilon * Epsilon;

        /// 3.14159265358979323846
        public static readonly Fixed64 PI = new(13493037704); //3.1415926534682512

        public static readonly Fixed64 TwoPI = PI * 2;
        public static readonly Fixed64 PIOver2 = PI / 2;
        public static readonly Fixed64 PIOver3 = PI / 3;
        public static readonly Fixed64 PIOver4 = PI / 4;
        public static readonly Fixed64 PIOver6 = PI / 6;

        /// Natural logarithm of 2<br/>
        /// 0.6931471805599453D
        public static readonly Fixed64 LN2 = new(2977044471); //0.6931471803691238

        /// Asin Padé approximations<br/>
        /// 0.183320102085006D
        public static readonly Fixed64 SPadeA1 = new(787353843); //0.18332010204903781

        /// Asin Padé approximations<br/>
        /// 0.021880409899862D
        public static readonly Fixed64 SPadeA2 = new(93975644); //0.02188040968030691

        private const long _log2MinRawValue = -64L * OneRawValue;
        public static readonly Fixed64 LOG2Min = new(_log2MinRawValue);

        private const long _log2MaxRawValue = 63L * OneRawValue;
        public static readonly Fixed64 LOG2Max = new(_log2MaxRawValue);

        /// <summary>
        /// Degrees to radians conversion factor
        /// <code>π / 180 = 0.01745329251994329576D</code>
        /// </summary>
        public static readonly Fixed64 Deg2Rad = new(74961320); //0.01745329238474369

        /// <summary>
        /// Radians to degrees conversion factor
        /// <code>180 / π = 57.2957795130823208767D</code>
        /// </summary>
        public static readonly Fixed64 Rad2Deg = new(246083499207); //57.295779512962326

        // Carefully optimized polynomial coefficients for sin(x), ensuring maximum precision in Fixed64 math.
        /// 0.16666667605750262737274169921875D; // 1/3!
        public static readonly Fixed64 SSinCoeff3 = new(715827923); //0.16666667605750263

        /// 0.0083328341133892536163330078125D; // 1/5!
        public static readonly Fixed64 SSinCoeff5 = new(35789250); //0.008332834113389254

        /// 0.00019588856957852840423583984375D; // 1/7!
        public static readonly Fixed64 SSinCoeff7 = new(841335); //0.0001958885695785284

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 FromRaw(long rawValue) => new(rawValue);

        public static Fixed64 Parse(ReadOnlySpan<char> chars) => new(Fixed64Utils.FromChars(chars, FRACTION_BITS));

        private readonly long _raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Fixed64(long v) => _raw = v;

        public long Raw {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _raw;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(float v) => new((long)((double)v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(Fixed64 v) => (float)((double)v._raw / OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Fixed64(int v) => new((long)v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(Fixed64 v) => (int)(v._raw >> FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Fixed64(uint v) => new((long)v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(Fixed64 v) => (uint)(v._raw >> FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(long v) => new(v << FRACTION_BITS);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(Fixed64 v) => v._raw >> FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(double v) => new((long)(v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(Fixed64 v) => (double)v._raw / OneRawValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(decimal v) => new((long)(v * OneRawValue));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator decimal(Fixed64 v) => (decimal)v._raw / OneRawValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(string v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(ReadOnlySpan<char> v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(Span<char> v) => Parse(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(ReadOnlyMemory<char> v) => Parse(v.Span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fixed64(Memory<char> v) => Parse(v.Span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator +(Fixed64 a) => new(a._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator -(Fixed64 a) => new(-a._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator +(Fixed64 a, Fixed64 b) => new(checked(a._raw + b._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator +(Fixed64 a, int b) => new(checked(a._raw + ((long)b << FRACTION_BITS)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator +(int a, Fixed64 b) => new(checked(((long)a << FRACTION_BITS) + b._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator -(Fixed64 a, Fixed64 b) => new(a._raw - b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator -(Fixed64 a, int b) => new(a._raw - ((long)b << FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator -(int a, Fixed64 b) => new(((long)a << FRACTION_BITS) - b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator ++(Fixed64 a) => new(a._raw + OneRawValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator --(Fixed64 a) => new(a._raw - OneRawValue);

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator *(Fixed64 a, Fixed64 b) => new(Fixed64Utils.FastMul(a._raw, b._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 FastMul(Fixed64 a, Fixed64 b) => a * b;

        public static Fixed64 operator *(Fixed64 a, int b) => new(Fixed64Utils.FastMul(a._raw, (long)b << FRACTION_BITS, FRACTION_BITS, FRACTIONAL_PART_MASK));

        public static Fixed64 FastMul(Fixed64 a, int b) => new(a._raw * b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator *(int a, Fixed64 b) => new(Fixed64Utils.FastMul((long)a << FRACTION_BITS, b._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        public static Fixed64 FastMul(int a, Fixed64 b) => new(a * b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator /(Fixed64 a, Fixed64 b) => new(Fixed64Utils.Div(a._raw, b._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator /(Fixed64 a, int b) => new(a._raw / b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator /(int a, Fixed64 b) => new(Fixed64Utils.Div((long)a << FRACTION_BITS, b._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator %(Fixed64 a, Fixed64 b) => new(a._raw % b._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator <<(Fixed64 a, int c) => new(a._raw << c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 operator >> (Fixed64 a, int c) => new(a._raw >> c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed64 a, Fixed64 b) => a._raw == b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Fixed64 a, int b) => a._raw == (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int a, Fixed64 b) => (long)a << FRACTION_BITS == b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed64 a, Fixed64 b) => a._raw != b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Fixed64 a, int b) => a._raw != (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int a, Fixed64 b) => (long)a << FRACTION_BITS != b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fixed64 a, Fixed64 b) => a._raw > b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Fixed64 a, int b) => a._raw > (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(int a, Fixed64 b) => (long)a << FRACTION_BITS > b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Fixed64 a, Fixed64 b) => a._raw < b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Fixed64 a, int b) => a._raw < (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(int a, Fixed64 b) => (long)a << FRACTION_BITS < b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fixed64 a, Fixed64 b) => a._raw >= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Fixed64 a, int b) => a._raw >= (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(int a, Fixed64 b) => (long)a << FRACTION_BITS >= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fixed64 a, Fixed64 b) => a._raw <= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Fixed64 a, int b) => a._raw <= (long)b << FRACTION_BITS;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(int a, Fixed64 b) => (long)a << FRACTION_BITS <= b._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Fixed64 other) => _raw == other._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _raw.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Fixed64 other) => _raw.CompareTo(other._raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Fixed64 other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetRawValue(Fixed64 value) => value._raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 SetRawValue(long value) => new(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Truncate(Fixed64 value) {
            if(value._raw>=0) return new((long)((ulong)value._raw & INTEGER_PART_MASK));
            return new(-(long)((ulong)-value._raw & INTEGER_PART_MASK));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Floor(Fixed64 value) => new((long)((ulong)value._raw & INTEGER_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Ceiling(Fixed64 value) => ((ulong)value._raw & FRACTIONAL_PART_MASK) != 0 ? Floor(value) + One : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Abs(Fixed64 value) => new(Fixed64Utils.Abs(value._raw));

        /// <summary>
        /// 采用的是四舍六入五取偶原则(2.5->2,3.5->4)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Round(Fixed64 value) => new(Fixed64Utils.Round(value._raw, OneRawValue, INTEGER_PART_MASK, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Clamp(Fixed64 value, Fixed64 min, Fixed64 max) => value._raw < min._raw ? min : value._raw > max._raw ? max : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Clamp01(Fixed64 value) => value._raw < Zero._raw ? Zero : value._raw > OneRawValue ? One : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Min(Fixed64 a, Fixed64 b) => a._raw <= b._raw ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Max(Fixed64 a, Fixed64 b) => a._raw >= b._raw ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(Fixed64 value) => value._raw < 0 ? -1 : value._raw > 0 ? 1 : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Sqrt(Fixed64 value) => new(Fixed64Utils.Sqrt(value._raw, FRACTION_BITS));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Pow(Fixed64 value, int exponent) => new(Fixed64Utils.Pow(value._raw, exponent, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Pow(Fixed64 b, Fixed64 exp) =>
            new(Fixed64Utils.Pow(b.Raw, exp.Raw, OneRawValue, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, _log2MinRawValue, _log2MaxRawValue, LN2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Pow2(Fixed64 x) =>
            new(Fixed64Utils.Pow2(x._raw, OneRawValue, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, _log2MinRawValue, _log2MaxRawValue, LN2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Log2(Fixed64 x) => new(Fixed64Utils.Log2(x._raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Ln(Fixed64 x) => new(Fixed64Utils.FastMul(Log2(x)._raw, LN2._raw, FRACTION_BITS, FRACTIONAL_PART_MASK));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Sin(Fixed64 x) =>
            new(Fixed64Utils.Sin(x.Raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI.Raw, TwoPI.Raw, PIOver2.Raw, SSinCoeff3.Raw, SSinCoeff5.Raw, SSinCoeff7.Raw));
        // new(Fixed64Utils.Sin(x._Raw, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, pi._Raw, twoPI._Raw, piOver2._Raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Cos(Fixed64 x) =>
            new(Fixed64Utils.Cos(x.Raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI._raw, TwoPI._raw, PIOver2._raw, SSinCoeff3._raw, SSinCoeff5._raw, SSinCoeff7._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Tan(Fixed64 x) =>
            // new(Fixed64Utils.Tan(x._Raw, FRACTION_BITS, INTEGER_PART_MASK, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, pi._Raw, piOver2._Raw));
            new(Fixed64Utils.Tan(x._raw, Precision._raw, OneRawValue, FRACTION_BITS, FRACTIONAL_PART_MASK, PI._raw, PIOver2._raw, PIOver4._raw, PIOver6._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Asin(Fixed64 x) =>
            new(Fixed64Utils.Asin(x._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, HalfOneRawValue, PI._raw, PIOver2._raw, PIOver6._raw, SPadeA1._raw, SPadeA2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Acos(Fixed64 x) => new(Fixed64Utils.Acos(x._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, PI._raw, PIOver2._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Atan(Fixed64 z) =>
            // new(Fixed64Utils.Atan(z._Raw, FRACTION_BITS, FRACTIONAL_PART_MASK, ONE_RAW_VALUE, piOver2._Raw));
            new(Fixed64Utils.Atan(z._raw, FRACTION_BITS, FRACTIONAL_PART_MASK, OneRawValue, HalfOneRawValue, Precision._raw, PIOver4._raw));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed64 Atan2(Fixed64 y, Fixed64 x) =>
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

        public class Comparer : IComparer<Fixed64> {
            public static readonly Comparer Instance = new();

            private Comparer() {
            }

            int IComparer<Fixed64>.Compare(Fixed64 x, Fixed64 y) => x._raw.CompareTo(y._raw);
        }

        public class EqualityComparer : IEqualityComparer<Fixed64> {
            public static readonly EqualityComparer Instance = new();

            private EqualityComparer() {
            }

            bool IEqualityComparer<Fixed64>.Equals(Fixed64 x, Fixed64 y) => x._raw == y._raw;

            int IEqualityComparer<Fixed64>.GetHashCode(Fixed64 num) => num._raw.GetHashCode();
        }
    }
}
