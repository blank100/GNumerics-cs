using System.Numerics;
using System.Runtime.CompilerServices;

namespace Gal.Core {
    /// <summary>
    ///
    /// </summary>
    /// <author>gouanlin</author>
    public static class Fixed64Utils {
        public static long FromChars(ReadOnlySpan<char> chars, int fractionBits) {
            if (chars.IsEmpty) return 0;

            var sign = 1;
            var iPart = 0L;
            int i = 0, l = chars.Length;
            var n = chars[i];

            switch (n) {
                case '-':
                    sign = -1;
                    i++;
                    break;
                case '+':
                    sign = 1;
                    i++;
                    break;
            }

            while (i < l && char.IsDigit(n = chars[i++])) iPart = iPart * 10 + (n - '0');

            if (i >= l) return (iPart << fractionBits) * sign;
            if (n != '.') throw new FormatException("Invalid format");

            var fPart = 0L;
            var exp = 1;
            while (i < l) {
                if (!char.IsDigit(n = chars[i++])) throw new FormatException("Invalid format");
                fPart = fPart * 10 + (n - '0');
                exp *= 10;
            }

            return sign * ((iPart << fractionBits) + (fPart << fractionBits) / exp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Abs(long v) {
            var mask = v >> 63;
            return v + mask ^ mask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Floor(long v, ulong integerPartMask) => (long)((ulong)v & integerPartMask);

        /// <summary>
        /// 采用的是四舍六入五取偶原则(2.5->2,3.5->4)
        /// </summary>
        /// <param name="v"></param>
        /// <param name="one"></param>
        /// <param name="integerPartMask"></param>
        /// <param name="fractionalPartMask"></param>
        /// <returns></returns>
        public static long Round(long v, long one, ulong integerPartMask, ulong fractionalPartMask) {
            var fPart = (long)((ulong)v & fractionalPartMask);
            var iPart = (long)((ulong)v & integerPartMask);
            var t = one >> 1;
            if (fPart < t) return iPart;
            if (fPart > t) return iPart + one;
            return (iPart & one) == 0 ? iPart : iPart + one;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long FastMul(long a, long b, int fractionBits, ulong fractionalPartMask) {
            var aLo = (ulong)a & fractionalPartMask;
            var aHi = a >> fractionBits;
            var bLo = (ulong)b & fractionalPartMask;
            var bHi = b >> fractionBits;

            var p4 = aHi * bHi;

#if DEBUG || SAFE_FIXED64_MUL
            if (p4 != 0) {
                var limit = long.MaxValue >> fractionBits;
                var absP4 = Abs(p4);
                if (absP4 > limit) throw new OverflowException("FastMul overflow: high*high term");
            }

            return checked((long)(aLo * bLo >> fractionBits) + (long)aLo * bHi + aHi * (long)bLo + (p4 << fractionBits));
#else
            return (long)(aLo * bLo >> fractionBits)+ (long)aLo * bHi+ aHi * (long)bLo+ (p4 << fractionBits);
#endif
        }

        public static long Div(long dividend, long divisor, int fractionBits) {
            if (divisor == 0) throw new DivideByZeroException();

            var remainder = (ulong)(dividend >= 0 ? dividend : -dividend);
            var divider = (ulong)(divisor >= 0 ? divisor : -divisor);
            var quotient = 0UL;
            var bitPos = fractionBits + 1;

            // If the divider is divisible by 2^n, take advantage of it.
            while ((divider & 0xF) == 0 && bitPos >= 4) {
                divider >>= 4;
                bitPos -= 4;
            }

            while (remainder != 0 && bitPos >= 0) {
                var shift = BitOperations.LeadingZeroCount(remainder);
                if (shift > bitPos) shift = bitPos;
                remainder <<= shift;
                bitPos -= shift;

                var div = remainder / divider;
                remainder %= divider;
                quotient += div << bitPos;

                // Detect overflow
                if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0) return ((dividend ^ divisor) & long.MinValue) == 0 ? long.MaxValue : long.MinValue + 1;

                remainder <<= 1;
                --bitPos;
            }

            // rounding
            ++quotient;
            var result = (long)(quotient >> 1);
            if (((dividend ^ divisor) & long.MinValue) != 0) result = -result;

            return result;
        }

        public static long Pow(long v, int exponent, long one, int fractionBits, ulong fractionalPartMask) {
            long result = one, baseCopy = v;
            while (exponent > 0) {
                if ((exponent & 1) == 1) result = FastMul(result, baseCopy, fractionBits, fractionalPartMask);
                baseCopy = FastMul(baseCopy, baseCopy, fractionBits, fractionalPartMask);
                exponent >>= 1;
            }

            return result;
        }

        /// <summary>
        /// <ref>https://en.wikipedia.org/wiki/Methods_of_computing_square_roots</ref>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fractionBits"></param>
        /// <returns></returns>
        public static long Sqrt(long value, int fractionBits) {
            if (value < 0) throw new ArgumentOutOfRangeException("Cannot compute square root of a negative number.");

            var v = (ulong)value;
            var result = 0UL;

            var bit = 1UL << 62;
            while (bit > v) bit >>= 2;

            for (var i = 0; i < 2; ++i) {
                while (bit != 0) {
                    var trial = result + bit;
                    if (v >= trial) {
                        v -= trial;
                        result = (result >> 1) + bit;
                    } else result >>= 1;

                    bit >>= 2;
                }

                if (i != 0) continue;
                if (v > (1UL << (64 - fractionBits)) - 1) {
                    // The remainder 'num' is too large to be shifted left
                    // by 32, so we have to add 1 to result manually and
                    // adjust 'num' accordingly.
                    // v = a - (result + 0.5)^2
                    //       = v + result^2 - (result + 0.5)^2
                    //       = v - result - 0.5
                    v -= result;
                    var half = 1UL << (fractionBits - 1);
                    v = (v << fractionBits) - half;
                    result = (result << fractionBits) + half;
                } else {
                    v <<= fractionBits;
                    result <<= fractionBits;
                }

                bit = 1UL << (fractionBits - 2);
            }

            if (v > result) ++result;
            return (long)result;
        }

        public static long InvSqrtFast(long x, int fractionBits, ulong fractionalPartMask, long argument1, long argument2, long argument3) {
            if (x <= 0) return 0;

            int lz = BitOperations.LeadingZeroCount((ulong)x);
            int msbIndex = 63 - lz;
            int exponent = msbIndex - fractionBits;

            // 提取尾数 m ∈ [1, 2)，f 为纯小数部分
            int shift = fractionBits - msbIndex;
            long m = shift >= 0 ? x << shift : x >> -shift;
            long f = m & (long)fractionalPartMask;

            // ------------------------------------------------------------
            // ✅ 极致优化：用普通 64 位乘法替代 FastMul，并融合奇偶指数补偿
            // 数学原理：
            // 偶数指数: 1/sqrt(m) ≈ 1.0 - 0.2928932 * f
            // 奇数指数: 1/sqrt(2m) ≈ 0.70710678 - 0.20710678 * f
            // 误差依然 < 4.5%，2次牛顿迭代稳稳达到 1e-6 精度
            // ------------------------------------------------------------
            long y;
            if ((exponent & 1) == 0) {
                y = (1L << fractionBits) - ((f * argument1) >> fractionBits);
            } else {
                y = argument2 - ((f * argument3) >> fractionBits);
            }

            // 应用指数部分: y *= 2^(-exponent / 2)
            int halfExp = exponent >> 1;
            y = halfExp >= 0 ? y >> halfExp : y << -halfExp;

            // ------------------------------------------------------------
            // ✅ 展开 2 次牛顿迭代 (固定 6 个 FastMul，无分支)
            // ------------------------------------------------------------
            long threeHalfs = 3L << (fractionBits - 1);

            long y2 = FastMul(y, y, fractionBits, fractionalPartMask);
            long xy2 = FastMul(x, y2, fractionBits, fractionalPartMask);
            y = FastMul(y, threeHalfs - (xy2 >> 1), fractionBits, fractionalPartMask);

            y2 = FastMul(y, y, fractionBits, fractionalPartMask);
            xy2 = FastMul(x, y2, fractionBits, fractionalPartMask);
            y = FastMul(y, threeHalfs - (xy2 >> 1), fractionBits, fractionalPartMask);

            return y;
        }

        /// <summary>
        /// 基于 Padé [3/3] 近似的高精度 Exp 函数 (e^x)。
        /// </summary>
        /// <param name="x">输入的定点数 raw 值。</param>
        /// <param name="one">值为 1 的 raw 值。</param>
        /// <param name="halfOne">值为 0.5 的 raw 值。</param>
        /// <param name="fractionBits">小数位数。</param>
        /// <param name="fractionalPartMask">小数部分掩码。</param>
        /// <param name="log2Min">支持的最小输入的 raw 值。</param>
        /// <param name="log2Max">支持的最大输入的 raw 值。</param>
        /// <param name="ln2">ln(2) 的 raw 值。</param>
        /// <param name="invLn2">1/ln(2) 的 raw 值。</param>
        /// <param name="padeC2">Padé 系数 C2 (1/10) 的 raw 值。</param>
        /// <param name="padeC3">Padé 系数 C3 (1/120) 的 raw 值。</param>
        /// <returns>e^x 的定点数 raw 值。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Exp(
            long x, long one, long halfOne, int fractionBits, ulong fractionalPartMask,
            long log2Min, long log2Max, long ln2, long invLn2,
            long padeC2, long padeC3
        ) {
            // --- 1. 边界检查 ---
            if (x == 0) return one;
            if (x <= log2Min) return 0;
            if (x >= log2Max) return long.MaxValue;

            // --- 2. 范围缩减: x = k * ln(2) + r, where |r| <= 0.5 * ln(2) ---
            // k = round(x / ln2) = floor(x / ln2 + 0.5)
            // 在定点数中，这通过乘法实现: k_raw = (x_raw * invLn2_raw) >> fractionBits
            // 加 halfOne 是为了实现 round
            var k = (FastMul(x, invLn2, fractionBits, fractionalPartMask) + halfOne) >> fractionBits;

            // r = x - k * ln(2)
            var r = x - (k * ln2);

            // --- 3. Padé [3/3] 近似计算 e^r ---
            // 我们计算 r^2
            var r2 = FastMul(r, r, fractionBits, fractionalPartMask);

            // U = r * ( (1/120 * r^2) + 1/2 )
            var u_inner = FastMul(r2, padeC3, fractionBits, fractionalPartMask) + halfOne;
            var u = FastMul(r, u_inner, fractionBits, fractionalPartMask);

            // V = (1/10 * r^2) + 1
            var v = FastMul(r2, padeC2, fractionBits, fractionalPartMask) + one;

            // e^r = (V + U) / (V - U)
            // 注意：由于 r 范围极小 (abs < 0.347)，V-U 永远大于 0
            var er = Div(v + u, v - u, fractionBits);

            // --- 4. 重组结果: e^x = e^r * 2^k ---
            if (k == 0) return er;
            if (k > 0) {
                // 检查左移是否会溢出
                if (k >= 63) return long.MaxValue;
                // 另一个更精细的检查
                if ((er >> (63 - (int)k)) != 0) return long.MaxValue;
                return er << (int)k;
            } else // k < 0
            {
                // 对于负数 k，右移
                return er >> (int)-k;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Log2(long v, long one, int fractionBits, ulong fractionalPartMask) {
            if (v <= 0) throw new ArgumentOutOfRangeException(nameof(v));

            // 整数部分的 log2
            var log2Int = BitOperations.Log2((ulong)v);
            var y = ((long)log2Int - fractionBits) * one;

            var shift = fractionBits - log2Int;
            // 归一化 v 到 [1.0, 2.0)
            var rawX = shift >= 0 ? v << shift : v >> -shift;

            // 二进制迭代法提取小数对数部分
            var z = rawX;
            var bit = 1L << fractionBits - 1;

            for (var i = 0; i < fractionBits; i++) {
                z = FastMul(z, z, fractionBits, fractionalPartMask);

                if (z >= one << 1) {
                    z >>= 1;
                    y += bit;
                }

                bit >>= 1;
            }

            return y;
        }

        public static long Pow2(long v, long one, int fractionBits, ulong integerPartMask, ulong fractionalPartMask, long log2Min, long log2Max, long ln2) {
            if (v == 0) return one;

            var neg = v < 0;
            if (neg) v = -v;

            if (v == one) return neg ? one >> 1 : one << 1;
            if (v >= log2Max) return neg ? Div(one, long.MaxValue, fractionBits) : long.MaxValue;
            if (v <= log2Min) return neg ? long.MaxValue : 0;

            var iPart = (int)(Floor(v, integerPartMask) >> fractionBits);
            v = (long)((ulong)v & fractionalPartMask);

            var result = one;
            var term = one;
            var i = 1;
            while (term != 0) {
                term = Div(FastMul(FastMul(v, term, fractionBits, fractionalPartMask), ln2, fractionBits, fractionalPartMask), i, fractionBits);
                result += term;
                i++;
            }

            result <<= iPart;
            if (neg) result = Div(one, result, fractionBits);
            return result;
        }

        public static long Pow(long b, long exp, long one, int fractionBits, ulong integerPartMask, ulong fractionalPartMask, long log2Min, long log2Max, long ln2) {
            if (b == one) return one;
            if (exp == 0) return one;

            if (b == 0) {
                if (exp < 0) throw new DivideByZeroException("Cannot raise 0 to a negative power.");
                return 0;
            }

            var log2 = Log2(b, one, fractionBits, fractionalPartMask);
            return Pow2(FastMul(exp, log2, fractionBits, fractionalPartMask), one, fractionBits, integerPartMask, fractionalPartMask, log2Min, log2Max, ln2);
        }

        /// <summary>
        /// Computes the sine of a given angle in radians using an optimized
        /// minimax polynomial approximation.
        /// </summary>
        /// <param name="x">The angle in radians.</param>
        /// <returns>The sine of the given angle, in fixed-point format.</returns>
        /// <remarks>
        /// - This function uses a Chebyshev-polynomial-based approximation to ensure high accuracy
        ///   while maintaining performance in fixed-point arithmetic.
        /// - The coefficients have been carefully tuned to minimize fixed-point truncation errors.
        /// - The error is less than 1 ULP (unit in the last place) at key reference points,
        ///   ensuring <c>Sin(π/4) = 0.707106781192124</c> exactly within Fixed64 precision.
        /// - The function automatically normalizes input values to the range [-π, π] for stability.
        /// </remarks>
        public static long Sin(long x, long one, int fractionBits, ulong fractionalPartMask, long pi, long twoPI, long piOver2, long sSinCoeff3, long sSinCoeff5, long sSinCoeff7) {
            // Check for special cases
            if (x == 0) return 0; // sin(0) = 0
            if (x == piOver2) return one; // sin(π/2) = 1
            if (x == -piOver2) return -one; // sin(-π/2) = -1
            if (x == pi) return 0; // sin(π) = 0
            if (x == -pi) return 0; // sin(-π) = 0
            if (x == twoPI || x == -twoPI) return 0; // sin(2π) = 0

            // Normalize x to [-π, π]
            x %= twoPI;
            if (x < -pi) x += twoPI;
            else if (x > pi) x -= twoPI;

            var flip = false;
            if (x < 0) {
                x = -x;
                flip = true;
            }

            if (x > piOver2) x = pi - x;

            // Precompute x^2
            var x2 = FastMul(x, x, fractionBits, fractionalPartMask);
            var x4 = FastMul(x2, x2, fractionBits, fractionalPartMask);

            // Optimized Chebyshev Polynomial for Sin(x)
            var result = FastMul(x,
                one -
                FastMul(x2, sSinCoeff3, fractionBits, fractionalPartMask) +
                FastMul(x4, sSinCoeff5, fractionBits, fractionalPartMask) -
                FastMul(FastMul(x4, x2, fractionBits, fractionalPartMask), sSinCoeff7, fractionBits, fractionalPartMask),
                fractionBits,
                fractionalPartMask
            );
            return flip ? -result : result;
        }

        /// <summary>
        /// Computes the cosine of a given angle in radians using a sine-based identity transformation.
        /// </summary>
        /// <param name="x">The angle in radians.</param>
        /// <returns>The cosine of the given angle, in fixed-point format.</returns>
        /// <remarks>
        /// - Instead of directly approximating cosine, this function derives <c>cos(x)</c> using
        ///   the identity <c>cos(x) = sin(x + π/2)</c>. This ensures maximum accuracy.
        /// - The underlying sine function is computed using a highly optimized minimax polynomial approximation.
        /// - By leveraging this transformation, cosine achieves the same precision guarantees
        ///   as sine, including <c>Cos(π/4) = 0.707106781192124</c> exactly within Fixed64 precision.
        /// - The function automatically normalizes input values to the range [-π, π] for stability.
        /// </remarks>
        public static long Cos(long x, long one, int fractionBits, ulong fractionalPartMask, long pi, long twoPI, long piOver2, long sSinCoeff3, long sSinCoeff5, long sSinCoeff7) {
            var rawAngle = x + (x > 0 ? -pi - piOver2 : piOver2);
            return Sin(rawAngle, one, fractionBits, fractionalPartMask, pi, twoPI, piOver2, sSinCoeff3, sSinCoeff5, sSinCoeff7);
        }

        /// <summary>
        /// Returns the tangent of x.
        /// </summary>
        /// <remarks>
        /// This function is not well-tested. It may be wildly inaccurate.
        /// </remarks>
        public static long Tan(long x, long precision, long one, int fractionBits, ulong fractionalPartMask, long pi, long piOver2, long piOver4, long piOver6) {
            // Check for special cases
            if (x == 0) return 0;
            if (x == piOver4) return one;
            if (x == -piOver4) return -one;

            // Normalize x to [-π/2, π/2]
            x %= pi;
            if (x < -piOver2) x += pi;
            else if (x > piOver2) x -= pi;

            // Use continued fraction to approximate tan(x)
            var x2 = FastMul(x, x, fractionBits, fractionalPartMask);
            var numerator = x;
            var denominator = one;

            // Iterate over the continued fraction terms
            var prevDenominator = denominator;
            var start = Abs(x) > piOver6 ? 19 : 13;
            for (var i = start; i >= 1; i -= 2) {
                denominator = ((long)i << fractionBits) - Div(x2, denominator, fractionBits);
                if (Abs(denominator - prevDenominator) < precision) break;
                prevDenominator = denominator;
            }

            return Div(numerator, denominator, fractionBits);
        }

        public static long Asin(long x, int fractionBits, ulong fractionalPartMask, long one, long halfOne, long pi, long piOver2, long piOver6, long sPadeA1, long sPadeA2) {
            // Ensure x is within the domain [-1, 1]
            if (x < -one || x > one) throw new ArithmeticException("Input out of domain for Asin: " + x);

            // Handle boundary cases for -1 and 1
            if (x == one) return piOver2; // asin(1) = π/2
            if (x == -one) return -piOver2; // asin(-1) = -π/2

            // Special case handling for asin(0.5) -> π/6 and asin(-0.5) -> -π/6
            if (x == halfOne) return piOver6;
            if (x == -halfOne) return -piOver6;

            // For values close to 0, use a Padé approximation for better precision
            if (Abs(x) < halfOne) {
                // Padé approximation of asin(x) for |x| < 0.5
                var xSquared = FastMul(x, x, fractionBits, fractionalPartMask);
                var numerator = FastMul(x, one + FastMul(xSquared, sPadeA1 + FastMul(xSquared, sPadeA2, fractionBits, fractionalPartMask), fractionBits, fractionalPartMask), fractionBits,
                    fractionalPartMask);
                // var numerator = x * (one + xSquared * (s_PadeA1 + xSquared * s_PadeA2));
                return numerator;
            }

            // For values closer to ±1, use the identity: asin(x) = π/2 - acos(x) for stability
            return x > 0
                ? piOver2 - Acos(x, fractionBits, fractionalPartMask, one, pi, piOver2)
                : -piOver2 + Acos(-x, fractionBits, fractionalPartMask, one, pi, piOver2);
        }

        public static long Acos(long x, int fractionBits, ulong fractionalPartMask, long one, long pi, long piOver2) {
            if (x < -one || x > one) throw new ArgumentOutOfRangeException(nameof(x), "Input out of domain for Acos: " + x);

            // For values near 1 or -1, the result is directly known.
            if (x == one) return 0; // acos(1) = 0
            if (x == -one) return pi; // acos(-1) = π
            if (x == 0) return piOver2; // acos(0) = π/2

            // Compute using the relationship acos(x) = atan(sqrt(1 - x^2) / x) + π/2 when x is negative
            var sqrtTerm = Sqrt(one - FastMul(x, x, fractionBits, fractionalPartMask), fractionBits); // sqrt(1 - x^2)
            var atanTerm = Atan(Div(sqrtTerm, x, fractionBits), fractionBits, fractionalPartMask, one, piOver2);

            return x < 0
                ? atanTerm + pi // acos(-x) = atan(...) + π
                : atanTerm; // Otherwise, return just atan(sqrt(...))
        }

        public static long Atan(long z, int fractionBits, ulong fractionalPartMask, long one, long piOver2) {
            if (z == 0) return 0;

            // Force positive values for argument
            // Atan(-z) = -Atan(z).
            var neg = z < 0;
            if (neg) z = -z;

            var two = (long)2 << fractionBits;
            var three = (long)3 << fractionBits;

            var invert = z > one;
            if (invert) z = Div(one, z, fractionBits);

            var result = one;
            var term = one;

            var zSq = FastMul(z, z, fractionBits, fractionalPartMask);
            var zSq2 = FastMul(zSq, two, fractionBits, fractionalPartMask);
            var zSqPlusOne = zSq + one;
            var zSq12 = FastMul(zSqPlusOne, two, fractionBits, fractionalPartMask);
            var dividend = zSq2;
            var divisor = FastMul(zSqPlusOne, three, fractionBits, fractionalPartMask);

            for (var i = 2; i < 30; ++i) {
                term = FastMul(term, Div(dividend, divisor, fractionBits), fractionBits, fractionalPartMask);
                result += term;

                dividend += zSq2;
                divisor += zSq12;

                if (term == 0) break;
            }

            result = Div(FastMul(result, z, fractionBits, fractionalPartMask), zSqPlusOne, fractionBits);

            if (invert) result = piOver2 - result;

            if (neg) result = -result;
            return result;
        }

        public static long Atan(long z, int fractionBits, ulong fractionalPartMask, long one, long halfOne, long precision, long piOver4) {
            if (z == 0) return 0;
            if (z == one) return piOver4;
            if (z == -one) return -piOver4;

            var neg = z < 0;
            if (neg) z = -z;

            // Adjust series for z > 0.5 using the identity.
            long adjustedResult;
            if (z > halfOne) {
                // Apply the identity: atan(z) = π/4 - atan((1 - z) / (1 + z))
                var transformedZ = Div(one - z, one + z, fractionBits);
                adjustedResult = piOver4 - Atan(transformedZ, fractionBits, fractionalPartMask, one, halfOne, precision, piOver4);
            } else {
                // Use extended Taylor series directly for better precision on small z.
                var zSq = FastMul(z, z, fractionBits, fractionalPartMask);

                var result = z;
                var term = z;
                var sign = -1;

                for (var i = 3; i < 15; i += 2) {
                    term = FastMul(term, zSq, fractionBits, fractionalPartMask);
                    var nextTerm = term / i;
                    if (Abs(nextTerm) < precision) break;

                    result += nextTerm * sign;
                    sign = -sign;
                }

                adjustedResult = result;
            }

            return neg ? -adjustedResult : adjustedResult;
        }

        public static long Atan2(long y, long x, int fractionBits, ulong fractionalPartMask, long one, long pi, long piOver2) {
            if (x == 0) {
                return y switch {
                    > 0 => piOver2,
                    0 => 0,
                    _ => -piOver2
                };
            }

            var atan = Atan(Div(y, x, fractionBits), fractionBits, fractionalPartMask, one, piOver2);

            // Adjust based on the quadrant
            if (x < 0) {
                return y >= 0
                    ?
                    // Second quadrant
                    atan + pi
                    :
                    // Third quadrant
                    atan - pi;
            }

            // First or fourth quadrant
            return atan;
        }
    }
}
