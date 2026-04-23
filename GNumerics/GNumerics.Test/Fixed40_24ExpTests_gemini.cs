using Xunit;
using System;
using Gal.Core;

namespace Gal.Core.Tests
{
    public class Fixed40_24ExpTests_gemini
    {
        // 允许的误差范围。定点数近似通常在小数点后 4-5 位是准确的
        private const double PrecisionDelta = 0.0005;

        #region 1. 基础数学特性

        [Fact]
        public void Exp_Zero_ReturnsOne()
        {
            // e^0 = 1
            Assert.Equal(Fixed40_24.One, Fixed40_24.Exp(Fixed40_24.Zero));
        }

        [Fact]
        public void Exp_One_ReturnsE()
        {
            // e^1 ≈ 2.7182818
            double expected = Math.Exp(1.0);
            Fixed40_24 actual = Fixed40_24.Exp(Fixed40_24.One);

            Assert.Equal(expected, (double)actual, 5); // 验证到小数点后5位
        }

        #endregion

        #region 2. 各种范围对比测试 (与 Math.Exp 对比)

        [Theory]
        [InlineData(0.5)]   // 正小数
        [InlineData(2.0)]   // 正整数
        [InlineData(-0.5)]  // 负小数
        [InlineData(-1.0)]  // 负整数
        [InlineData(0.12345)]
        [InlineData(-2.718)]
        public void Exp_Comparison_With_MathExp(double input)
        {
            Fixed40_24 fInput = (Fixed40_24)input;
            double expected = Math.Exp(input);
            double actual = (double)Fixed40_24.Exp(fInput);

            // 检查绝对误差
            Assert.Equal(expected, actual, 4);
        }

        #endregion

        #region 3. 极值与边界测试

        [Fact]
        public void Exp_LargePositive_ApproachesLimit()
        {
            // 测试一个较大的值，但确保不会导致 Pow2 内部长期溢出
            // 你的库中 _log2MaxRawValue 约 63，x * 1.4427 < 63 => x < 43.6
            double input = 30.0;
            Fixed40_24 fInput = (Fixed40_24)input;

            double expected = Math.Exp(input);
            double actual = (double)Fixed40_24.Exp(fInput);

            // 对于非常大的数，检查相对误差（Error / Value）
            double relativeError = Math.Abs(expected - actual) / expected;
            Assert.True(relativeError < 0.001, $"Relative error too high: {relativeError}");
        }

        [Fact]
        public void Exp_LargeNegative_ApproachesZero()
        {
            // e^-20 应该是一个非常接近 0 的正数
            Fixed40_24 fInput = (Fixed40_24)(-20.0);
            Fixed40_24 result = Fixed40_24.Exp(fInput);

            Assert.True(result >= Fixed40_24.Zero);
            Assert.InRange((double)result, 0.0, 0.000001);
        }

        #endregion

        #region 4. 随机压力测试

        [Fact]
        public void Exp_RandomValues_StressTest()
        {
            Random rand = new Random(42);
            for (int i = 0; i < 1000; i++)
            {
                // 在 -10 到 10 之间随机测试，这个区间最常用且精度最敏感
                double val = (rand.NextDouble() * 20.0) - 10.0;
                Fixed40_24 fVal = (Fixed40_24)val;

                double expected = Math.Exp(val);
                double actual = (double)Fixed40_24.Exp(fVal);

                // 误差控制
                if (expected > 0.01)
                {
                    double relError = Math.Abs(expected - actual) / expected;
                    Assert.True(relError < 0.001, $"Failed at {val}, Expected {expected}, Actual {actual}");
                }
                else
                {
                    Assert.True(Math.Abs(expected - actual) < 0.0001);
                }
            }
        }

        #endregion
    }
}
