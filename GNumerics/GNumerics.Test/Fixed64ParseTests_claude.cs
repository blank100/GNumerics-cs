using Xunit;
using Gal.Core;
using System;

namespace Gal.Core.Tests
{
    public class Fixed64ParseTests_claude
    {
        private const double EPSILON = 1e-9;

        #region 基础测试

        [Theory]
        [InlineData("0", 0.0)]
        [InlineData("1", 1.0)]
        [InlineData("-1", -1.0)]
        [InlineData("123", 123.0)]
        [InlineData("-456", -456.0)]
        public void Parse_整数_应返回正确值(string input, double expected)
        {
            var result = Fixed64.Parse(input);
            Assert.Equal(expected, (double)result, 9);
        }

        [Theory]
        [InlineData("0.5", 0.5)]
        [InlineData("0.25", 0.25)]
        [InlineData("0.125", 0.125)]
        [InlineData("-0.5", -0.5)]
        [InlineData("1.5", 1.5)]
        [InlineData("-1.5", -1.5)]
        [InlineData("123.456", 123.456)]
        [InlineData("-123.456", -123.456)]
        public void Parse_小数_应返回正确值(string input, double expected)
        {
            var result = Fixed64.Parse(input);
            Assert.Equal(expected, (double)result, 6);
        }

        [Theory]
        [InlineData("+0", 0.0)]
        [InlineData("+1", 1.0)]
        [InlineData("+123.456", 123.456)]
        public void Parse_正号前缀_应正确解析(string input, double expected)
        {
            var result = Fixed64.Parse(input);
            Assert.Equal(expected, (double)result, 6);
        }

        #endregion

        #region 边界值测试

        [Theory]
        [InlineData("2147483647", 2147483647.0)] // int.MaxValue
        [InlineData("-2147483648", -2147483648.0)] // int.MinValue
        [InlineData("2147483647.999999", 2147483647.999999)]
        [InlineData("-2147483647.999999", -2147483647.999999)]
        public void Parse_接近最大最小值_应正确解析(string input, double expected)
        {
            var result = Fixed64.Parse(input);
            Assert.Equal(expected, (double)result, 6);
        }

        [Fact]
        public void Parse_最大整数部分_应正确解析()
        {
            // Fixed64 的整数部分最大值 (32位，因为有32位小数)
            var maxIntPart = (1L << 31) - 1; // 2147483647
            var input = maxIntPart.ToString();
            var result = Fixed64.Parse(input);
            Assert.Equal((double)maxIntPart, (double)result, 0);
        }

        [Fact]
        public void Parse_最小整数部分_应正确解析()
        {
            var minIntPart = -(1L << 31); // -2147483648
            var input = minIntPart.ToString();
            var result = Fixed64.Parse(input);
            Assert.Equal((double)minIntPart, (double)result, 0);
        }

        [Theory]
        [InlineData("2147483647.5")]
        [InlineData("-2147483647.5")]
        public void Parse_接近边界的小数_应正确解析(string input)
        {
            var result = Fixed64.Parse(input);
            var expected = double.Parse(input);
            Assert.Equal(expected, (double)result, 6);
        }

        #endregion

        #region 溢出测试

        [Theory]
        [InlineData("2147483648")] // 超过最大整数部分
        [InlineData("-2147483649")] // 超过最小整数部分
        [InlineData("9999999999")]
        [InlineData("-9999999999")]
        public void Parse_整数部分溢出_应抛出异常(string input)
        {
            Assert.ThrowsAny<Exception>(() => Fixed64.Parse(input));
        }

        // [Theory]
        // [InlineData("2147483648.0")] // 整数部分刚好溢出
        // [InlineData("2147483647.9999999999999999")] // 大量小数位
        // public void Parse_边界溢出_应抛出异常(string input)
        // {
        //     Assert.ThrowsAny<Exception>(() => Fixed64.Parse(input));
        // }

        #endregion

        #region 精度测试

        [Theory]
        [InlineData("0.0000000001")] // 超出精度范围，应舍入为0
        [InlineData("0.00000001")]
        [InlineData("0.000000001")]
        public void Parse_超高精度小数_应正确处理(string input)
        {
            var result = Fixed64.Parse(input);
            // Fixed64 的精度约为 2^-32 ≈ 2.3e-10
            Assert.True(Fixed64.Abs(result) < (Fixed64)0.001);
        }

        [Theory]
        [InlineData("1.23456789", 1.23456789)]
        [InlineData("0.123456789", 0.123456789)]
        [InlineData("0.987654321", 0.987654321)]
        public void Parse_常规精度小数_应保持精度(string input, double expected)
        {
            var result = Fixed64.Parse(input);
            Assert.Equal(expected, (double)result, 6);
        }

        [Fact]
        public void Parse_大量小数位_应正确截断()
        {
            var input = "1." + new string('9', 50); // 1.999...999 (50个9)
            var result = Fixed64.Parse(input);
            Assert.True(result > Fixed64.One);
            Assert.True(result < Fixed64.Two);
        }

        #endregion

        #region 格式错误测试

        // [Theory]
        // [InlineData("")] // 空字符串
        // [InlineData(" ")] // 只有空格
        // [InlineData("+")] // 只有符号
        // [InlineData("-")] // 只有符号
        // [InlineData(".")] // 只有小数点
        // [InlineData("+.")] // 符号+小数点
        // [InlineData("-.")] // 符号+小数点
        // public void Parse_无效格式_应抛出FormatException(string input)
        // {
        //     Assert.Throws<FormatException>(() => Fixed64.Parse(input));
        // }

        [Theory]
        [InlineData("abc")]
        [InlineData("12.34.56")] // 多个小数点
        [InlineData("12a34")]
        [InlineData("12.34e5")] // 科学计数法
        [InlineData("0x10")] // 十六进制
        [InlineData("12,345")] // 包含逗号
        [InlineData("12 34")] // 包含空格
        public void Parse_包含非法字符_应抛出FormatException(string input)
        {
            Assert.Throws<FormatException>(() => Fixed64.Parse(input));
        }

        #endregion

        #region 特殊数值测试

        [Theory]
        [InlineData("3.14159265358979")]
        [InlineData("2.71828182845905")]
        [InlineData("1.41421356237309")] // sqrt(2)
        [InlineData("0.57721566490153")] // Euler-Mascheroni constant
        public void Parse_数学常数_应正确解析(string input)
        {
            var result = Fixed64.Parse(input);
            var expected = double.Parse(input);
            Assert.Equal(expected, (double)result, 6);
        }

        [Theory]
        [InlineData("0.0")]
        [InlineData("0.00")]
        [InlineData("00.00")]
        [InlineData("-0.0")]
        [InlineData("+0.0")]
        public void Parse_零的各种表示_应返回零(string input)
        {
            var result = Fixed64.Parse(input);
            Assert.Equal(Fixed64.Zero, result);
        }

        [Theory]
        [InlineData("1.0", "1")]
        [InlineData("1.00", "1.0")]
        [InlineData("01.0", "1")]
        [InlineData("001", "1")]
        public void Parse_等价表示_应返回相同值(string input1, string input2)
        {
            var result1 = Fixed64.Parse(input1);
            var result2 = Fixed64.Parse(input2);
            Assert.Equal(result1, result2);
        }

        #endregion

        #region 随机数值测试

        [Fact]
        public void Parse_随机正数_应与double转换一致()
        {
            var random = new Random(12345);
            for (int i = 0; i < 1000; i++)
            {
                var intPart = random.Next(0, 1000000);
                var fracPart = random.NextDouble();
                var value = intPart + fracPart;
                var input = value.ToString("F6");

                var result = Fixed64.Parse(input);
                Assert.Equal(value, (double)result, 5);
            }
        }

        [Fact]
        public void Parse_随机负数_应与double转换一致()
        {
            var random = new Random(54321);
            for (int i = 0; i < 1000; i++)
            {
                var intPart = random.Next(-1000000, 0);
                var fracPart = random.NextDouble();
                var value = intPart - fracPart;
                var input = value.ToString("F6");

                var result = Fixed64.Parse(input);
                Assert.Equal(value, (double)result, 5);
            }
        }

        [Fact]
        public void Parse_随机小数_应正确处理()
        {
            var random = new Random(99999);
            for (int i = 0; i < 1000; i++)
            {
                var value = (random.NextDouble() - 0.5) * 2.0; // -1.0 到 1.0
                var input = value.ToString("F9");

                var result = Fixed64.Parse(input);
                Assert.Equal(value, (double)result, 6);
            }
        }

        #endregion

        #region 往返测试 (Round-trip)

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.0)]
        [InlineData(-1.0)]
        [InlineData(123.456)]
        [InlineData(-123.456)]
        [InlineData(0.00390625)] // 2^-8
        public void Parse_ToString往返_应保持值(double value)
        {
            var fixed64 = (Fixed64)value;
            var str = fixed64.ToString();
            var parsed = Fixed64.Parse(str);
            Assert.Equal(fixed64, parsed);
        }

        #endregion

        #region ReadOnlySpan测试

        [Fact]
        public void Parse_ReadOnlySpan_应正确解析()
        {
            ReadOnlySpan<char> span = "123.456".AsSpan();
            var result = Fixed64.Parse(span);
            Assert.Equal(123.456, (double)result, 6);
        }

        [Fact]
        public void Parse_Span_应正确解析()
        {
            Span<char> span = stackalloc char[] { '1', '2', '3', '.', '4', '5', '6' };
            var result = Fixed64.Parse(span);
            Assert.Equal(123.456, (double)result, 6);
        }

        #endregion

        #region 性能基准测试

        [Fact]
        public void Parse_大量数据_应快速处理()
        {
            var testData = new string[10000];
            var random = new Random(42);
            for (int i = 0; i < testData.Length; i++)
            {
                var value = (random.NextDouble() - 0.5) * 1000000;
                testData[i] = value.ToString("F6");
            }

            var startTime = DateTime.UtcNow;
            foreach (var data in testData)
            {
                var _ = Fixed64.Parse(data);
            }
            var elapsed = DateTime.UtcNow - startTime;

            // 应该在合理时间内完成（例如小于1秒）
            Assert.True(elapsed.TotalSeconds < 1.0, $"解析耗时: {elapsed.TotalMilliseconds}ms");
        }

        #endregion

        #region 边界情况压力测试

        [Fact]
        public void Parse_最大安全值边界_应正确处理()
        {
            // 测试接近但不超过最大值的情况
            var testValues = new[]
            {
                "2147483646",
                "2147483646.9",
                "2147483646.99",
                "2147483646.999",
                "2147483646.9999",
                "2147483646.99999",
            };

            foreach (var input in testValues)
            {
                var result = Fixed64.Parse(input);
                var expected = double.Parse(input);
                Assert.Equal(expected, (double)result, 4);
            }
        }

        [Fact]
        public void Parse_最小安全值边界_应正确处理()
        {
            var testValues = new[]
            {
                "-2147483647",
                "-2147483647.9",
                "-2147483647.99",
                "-2147483647.999",
            };

            foreach (var input in testValues)
            {
                var result = Fixed64.Parse(input);
                var expected = double.Parse(input);
                Assert.Equal(expected, (double)result, 4);
            }
        }

        #endregion
    }
}
