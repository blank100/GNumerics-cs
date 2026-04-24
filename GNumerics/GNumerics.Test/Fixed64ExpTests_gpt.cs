using System;
using Gal.Core;

namespace GNumerics.Test;

public class Fixed64ExpTests_gpt {
    private const double Tolerance = 1e-6;

    // ✅ 1️⃣ 基础值测试
    [Fact]
    public void Exp_BasicValues()
    {
        AssertClose(1.0, Fixed64.Exp(Fixed64.Zero));
        AssertClose(Math.E, Fixed64.Exp((Fixed64)1));
        AssertClose(Math.Exp(-1), Fixed64.Exp((Fixed64)(-1)));
    }

    // ✅ 2️⃣ 小数测试
    [Theory]
    [InlineData(0.1)]
    [InlineData(0.5)]
    [InlineData(1.5)]
    [InlineData(-0.1)]
    [InlineData(-0.5)]
    [InlineData(-1.5)]
    public void Exp_KnownValues(double x)
    {
        var expected = Math.Exp(x);
        var actual = (double)Fixed64.Exp((Fixed64)x);

        Assert.InRange(actual, expected - Tolerance, expected + Tolerance);
    }

    // ✅ 3️⃣ 单调性测试
    [Fact]
    public void Exp_IsMonotonic()
    {
        for (double x = -5; x < 5; x += 0.1)
        {
            var a = Fixed64.Exp((Fixed64)x);
            var b = Fixed64.Exp((Fixed64)(x + 0.1));

            Assert.True(a < b);
        }
    }

    // ✅ 4️⃣ exp(a+b) ≈ exp(a)*exp(b)
    [Theory]
    [InlineData(0.5, 0.3)]
    [InlineData(-0.5, 0.3)]
    [InlineData(1.2, -0.7)]
    public void Exp_AdditionIdentity(double a, double b)
    {
        var left = Fixed64.Exp((Fixed64)(a + b));
        var right = Fixed64.Exp((Fixed64)a) * Fixed64.Exp((Fixed64)b);

        var diff = Math.Abs((double)(left - right));
        Assert.True(diff < 1e-5);
    }

    // ✅ 5️⃣ exp(-x) = 1 / exp(x)
    [Theory]
    [InlineData(0.1)]
    [InlineData(0.5)]
    [InlineData(1.5)]
    public void Exp_Inverse(double x)
    {
        var expX = Fixed64.Exp((Fixed64)x);
        var expNegX = Fixed64.Exp((Fixed64)(-x));

        var inv = Fixed64.One / expX;

        var diff = Math.Abs((double)(expNegX - inv));
        Assert.True(diff < 1e-6);
    }

    // ✅ 6️⃣ 随机测试（安全范围内）
    [Fact]
    public void Exp_Random()
    {
        var rnd = new Random(1234);

        for (int i = 0; i < 1000; i++)
        {
            double x = rnd.NextDouble() * 10 - 5; // [-5,5]

            var expected = Math.Exp(x);
            var actual = (double)Fixed64.Exp((Fixed64)x);

            Assert.InRange(actual, expected - 1e-5, expected + 1e-5);
        }
    }

    // ✅ 7️⃣ 边界测试（接近溢出）
    [Fact]
    public void Exp_UpperBound()
    {
        double x = 20; // 安全上限附近
        var result = (double)Fixed64.Exp((Fixed64)x);

        Assert.True(result > 0);
    }

    // ✅ 8️⃣ 大负数测试
    [Fact]
    public void Exp_LargeNegative()
    {
        double x = -20;
        var result = (double)Fixed64.Exp((Fixed64)x);

        Assert.True(result > 0);
        Assert.True(result < 1e-7);
    }

    private void AssertClose(double expected, Fixed64 actual)
    {
        var value = (double)actual;
        Assert.InRange(value, expected - Tolerance, expected + Tolerance);
    }
}
