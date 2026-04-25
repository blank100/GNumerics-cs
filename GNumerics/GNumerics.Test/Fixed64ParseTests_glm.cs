using System;
using System.Collections.Generic;
using Gal.Core;
using Xunit;

namespace Gal.Core.Tests {
    public class Fixed64ParseTests_glm {
        // 由于 Fixed64 的 FRACTION_BITS = 32，其能表示的最大整数部分为 long.MaxValue >> 32 ≈ 2147483647
        private const long MaxIntegerPart = long.MaxValue >> Fixed64.FRACTION_BITS;

        [Fact]
        public void Parse_EmptySpan_ReturnsZero() {
            var span = ReadOnlySpan<char>.Empty;
            var result = Fixed64.Parse(span);
            Assert.Equal(0, result.Raw);
        }

        [Fact]
        public void Parse_ZeroString_ReturnsZero() {
            var result = Fixed64.Parse("0".AsSpan());
            Assert.Equal(Fixed64.Zero, result);
        }

        [Theory]
        [InlineData("+0", 0)]
        [InlineData("-0", 0)]
        [InlineData("1", 1)]
        [InlineData("-1", -1)]
        [InlineData("+1", 1)]
        [InlineData("123456", 123456)]
        [InlineData("-123456", -123456)]
        public void Parse_IntegerValues_ReturnsCorrectRaw(string input, int expectedInt) {
            var result = Fixed64.Parse(input.AsSpan());
            var expected = (Fixed64)expectedInt; // 隐式转换 int -> Fixed64
            Assert.Equal(expected.Raw, result.Raw);
        }

        [Theory]
        [InlineData("0.5", 0.5)]
        [InlineData("-0.5", -0.5)]
        [InlineData("0.25", 0.25)]
        [InlineData("0.75", 0.75)]
        [InlineData("123.456", 123.456)]
        [InlineData("-123.456", -123.456)]
        public void Parse_DecimalValues_MatchesDoubleConversion(string input, double expectedDouble) {
            var result = Fixed64.Parse(input.AsSpan());
            double actualDouble = (double)result;

            // Fixed64 精度约为 9 位十进制，这里允许 6 位误差足以通过常规测试
            Assert.Equal(expectedDouble, actualDouble, 6);
        }

        [Fact]
        public void Parse_SpecialFormats_HandledCorrectly() {
            // ".5" 等价于 "0.5"
            var result1 = Fixed64.Parse(".5".AsSpan());
            Assert.Equal((double)Fixed64.Dot5, (double)result1, 6);

            // "5." 抛出异常
            Assert.Throws<FormatException>(() => {
                Fixed64.Parse("5.".AsSpan());
            });

            // 只有符号
            Assert.Throws<FormatException>(() => {
                Fixed64.Parse("+".AsSpan());
            });

            Assert.Throws<FormatException>(() => {
                Fixed64.Parse("-".AsSpan());
            });
        }

        [Fact]
        public void Parse_NearMaxValue_ParsedCorrectly() {
            // 最大整数部分：2147483647
            string maxIntStr = MaxIntegerPart.ToString();
            var result = Fixed64.Parse(maxIntStr.AsSpan());
            Assert.Equal(MaxIntegerPart, result.Raw >> Fixed64.FRACTION_BITS);

            // 最大整数部分 + 最大小数部分 (接近 MaxValue)
            string nearMaxStr = MaxIntegerPart + ".999999999";
            var resultNearMax = Fixed64.Parse(nearMaxStr.AsSpan());
            Assert.True(resultNearMax.Raw <= long.MaxValue);
            Assert.True(resultNearMax.Raw > 0);
        }

        [Fact]
        public void Parse_NearMinValue_ParsedCorrectly() {
            // 最小整数部分：-2147483648 (因为 long.MinValue >> 32 是 -2147483648)
            long minIntPart = long.MinValue >> Fixed64.FRACTION_BITS;
            string minIntStr = minIntPart.ToString();
            var result = Fixed64.Parse(minIntStr.AsSpan());
            Assert.Equal(minIntPart, result.Raw >> Fixed64.FRACTION_BITS);

            // 最小整数部分 + 最小小数部分 (接近 MinValue)
            string nearMinStr = minIntPart + ".000000000";
            var resultNearMin = Fixed64.Parse(nearMinStr.AsSpan());
            Assert.True(resultNearMin.Raw >= long.MinValue);
            Assert.True(resultNearMin.Raw < 0);
        }

        [Fact]
        public void Parse_IntegerPartOverflow_ThrowsOverflowException() {
            // 比最大允许整数大 1：2147483648
            string overflowStr = (MaxIntegerPart + 1).ToString();
            Assert.Throws<OverflowException>(() => Fixed64.Parse(overflowStr.AsSpan()));

            // 比最小允许整数小 1：-2147483649
            long minIntPart = long.MinValue >> Fixed64.FRACTION_BITS;
            string underflowStr = (minIntPart - 1).ToString();
            Assert.Throws<OverflowException>(() => Fixed64.Parse(underflowStr.AsSpan()));
        }

        [Fact]
        public void Parse_LongFractionalPart_TruncatesSafelyWithoutOverflow() {
            // 故意构造极长的小数部分，验证是否会抛出溢出异常，以及是否能安全截断
            string longFraction = "1." + new string('9', 50);
            var result = Fixed64.Parse(longFraction.AsSpan());

            // 理论上无限个 9 会向前进位变成 2，但在 Fixed64 精度截断下，最高应当逼近 2.0
            Assert.Equal(2.0, (double)result, 5);
            Assert.True(result.Raw < long.MaxValue); // 确保没有溢出 long
        }

        [Theory]
        [InlineData("12.34.56")]
        [InlineData("1.2.3")]
        public void Parse_MultipleDecimalPoints_ThrowsFormatException(string input) {
            Assert.Throws<FormatException>(() => Fixed64.Parse(input.AsSpan()));
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("12a34")]
        [InlineData("12.34a")]
        [InlineData("1@2")]
        [InlineData("--1")]
        [InlineData("++1")]
        [InlineData("1-2")]
        public void Parse_InvalidCharacters_ThrowsFormatException(string input) {
            Assert.Throws<FormatException>(() => Fixed64.Parse(input.AsSpan()));
        }

        [Fact]
        public void Parse_RandomValues_MatchesDoubleConversion() {
            var random = new Random(42); // 固定种子保证测试可重复
            var testValues = new List<double>();

            // 生成随机数
            for (int i = 0; i < 100; i++) {
                double val = (random.NextDouble() - 0.5) * (MaxIntegerPart * 2.0);
                testValues.Add(val);
            }

            // 添加一些极端小数
            testValues.Add(0.000000001);
            testValues.Add(-0.000000001);
            testValues.Add(0.999999999);
            testValues.Add(-0.999999999);

            foreach (var expectedDouble in testValues) {
                // 格式化保留 9 位小数，避免原生 double 转换时的浮点垃圾数据干扰
                string input = expectedDouble.ToString("F9");

                var parsedFixed = Fixed64.Parse(input.AsSpan());
                double actualDouble = (double)parsedFixed;

                // 验证误差在可接受范围内 (Fixed64 有约 9.6 位十进制精度)
                Assert.Equal(expectedDouble, actualDouble, 7);
            }
        }

        [Fact]
        public void Parse_PiConstant_MatchesPredefined() {
            string piStr = "3.141592653";
            var parsedPi = Fixed64.Parse(piStr.AsSpan());
            // 允许微小精度损失，比对预定义常量
            double diff = Math.Abs((double)(parsedPi - Fixed64.PI));
            Assert.True(diff < 0.0000001, $"Parsed PI differs from constant. Diff: {diff}");
        }
    }
}
