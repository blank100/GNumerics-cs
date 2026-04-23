using System;
using Xunit;
using Gal.Core;

namespace Gal.Tests
{
    public class Fixed40_24_Exp_Tests_gpt
    {
        private static Fixed40_24 F(double v) => (Fixed40_24)v;
        private static double D(Fixed40_24 v) => (double)v;

        private const double EPS = 1e-4;
        private const double WIDE_EPS = 1e-3;

        // ============================================================
        // ✅ 1. 基础值测试
        // ============================================================

        [Fact]
        public void Exp_Zero()
        {
            var result = Fixed40_24.Exp(Fixed40_24.Zero);
            Assert.InRange(Math.Abs(D(result) - 1.0), 0, EPS);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.0)]
        [InlineData(3.0)]
        [InlineData(-1.0)]
        [InlineData(-2.0)]
        public void Exp_Basic_Values(double x)
        {
            var fx = F(x);
            var result = Fixed40_24.Exp(fx);
            var expected = Math.Exp(x);

            Assert.InRange(Math.Abs(D(result) - expected), 0, WIDE_EPS);
        }

        // ============================================================
        // ✅ 2. 随机误差测试
        // ============================================================

        [Fact]
        public void Exp_Random_Test()
        {
            var rand = new Random(12345);

            for (int i = 0; i < 10000; i++)
            {
                double x = rand.NextDouble() * 10 - 5; // [-5,5]

                var fx = F(x);
                var result = Fixed40_24.Exp(fx);
                var expected = Math.Exp(x);

                Assert.InRange(Math.Abs(D(result) - expected), 0, WIDE_EPS);
            }
        }

        // ============================================================
        // ✅ 3. 单调性测试
        // ============================================================

        [Fact]
        public void Exp_Should_Be_Monotonic()
        {
            for (double x = -5; x < 5; x += 0.1)
            {
                var a = Fixed40_24.Exp(F(x));
                var b = Fixed40_24.Exp(F(x + 0.1));

                Assert.True(a < b);
            }
        }

        // ============================================================
        // ✅ 4. 反函数测试 Ln(Exp(x)) ≈ x
        // ============================================================

        [Fact]
        public void Ln_Exp_Inverse_Test()
        {
            for (double x = -4; x < 4; x += 0.2)
            {
                var fx = F(x);
                var result = Fixed40_24.Ln(Fixed40_24.Exp(fx));

                Assert.InRange(Math.Abs(D(result) - x), 0, WIDE_EPS);
            }
        }

        // ============================================================
        // ✅ 5. 乘法一致性测试
        // exp(a + b) ≈ exp(a) * exp(b)
        // ============================================================

        [Fact]
        public void Exp_Add_Property_Test()
        {
            var rand = new Random(6789);

            for (int i = 0; i < 5000; i++)
            {
                double a = rand.NextDouble() * 3 - 1.5;
                double b = rand.NextDouble() * 3 - 1.5;

                var fa = F(a);
                var fb = F(b);

                var left = Fixed40_24.Exp(fa + fb);
                var right = Fixed40_24.Exp(fa) * Fixed40_24.Exp(fb);

                Assert.InRange(Math.Abs(D(left) - D(right)), 0, 0.01);
            }
        }

        // ============================================================
        // ✅ 6. 极值测试
        // ============================================================

        [Fact]
        public void Exp_Large_Positive_Should_Saturate()
        {
            var big = F(100);
            var result = Fixed40_24.Exp(big);

            Assert.True(result == Fixed40_24.MaxValue || D(result) > 1e10);
        }

        [Fact]
        public void Exp_Large_Negative_Should_Approach_Zero()
        {
            var small = F(-100);
            var result = Fixed40_24.Exp(small);

            Assert.True(result == Fixed40_24.Zero || D(result) < 1e-5);
        }

        // ============================================================
        // ✅ 7. 精度边界测试
        // ============================================================

        [Fact]
        public void Exp_Small_Value_Precision()
        {
            var tiny = Fixed40_24.Precision;
            var result = Fixed40_24.Exp(tiny);

            Assert.InRange(Math.Abs(D(result) - Math.Exp(D(tiny))), 0, EPS);
        }

        // ============================================================
        // ✅ 8. 对称性测试
        // exp(-x) ≈ 1 / exp(x)
        // ============================================================

        [Fact]
        public void Exp_Negative_Inverse_Test()
        {
            for (double x = 0.1; x < 4; x += 0.2)
            {
                var fx = F(x);

                var positive = Fixed40_24.Exp(fx);
                var negative = Fixed40_24.Exp(-fx);

                var reciprocal = Fixed40_24.One / positive;

                Assert.InRange(Math.Abs(D(negative) - D(reciprocal)), 0, 0.01);
            }
        }
    }
}
