using System;
using Gal.Core;

namespace GNumerics.Test;

public class Fixed64ExpTests_gemini {
    // 定义一个合理的误差容忍度。对于Q31.32，这个精度通常足够。
    // e.g., 2^-16, 对于大数值可能需要放宽
    private const double Tolerance = 0.0001;

    #region Basic and Special Values

    [Fact]
    public void Exp_Zero_ReturnsOne()
    {
        // 测试 e^0 = 1
        var result = Fixed64.Exp(Fixed64.Zero);
        Assert.Equal(Fixed64.One, result);
    }

    [Fact]
    public void Exp_One_ReturnsE()
    {
        // 测试 e^1 = e
        var expected = Math.E;
        var result = Fixed64.Exp(Fixed64.One);

        Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
    }

    [Fact]
    public void Exp_Two_ReturnsESquared()
    {
        // 测试 e^2
        var expected = Math.Exp(2.0);
        var result = Fixed64.Exp(Fixed64.Two);

        Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
    }

    #endregion

    #region Negative Values

    [Fact]
    public void Exp_NegativeOne_ReturnsOneOverE()
    {
        // 测试 e^-1 = 1/e
        var expected = Math.Exp(-1.0);
        var result = Fixed64.Exp(-Fixed64.One);

        Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
    }

    [Theory]
    [InlineData(-0.5)]
    [InlineData(-2.5)]
    [InlineData(-5.0)]
    public void Exp_NegativeValues_MatchMathExp(double value)
    {
        // 测试一系列负数值
        var fixedValue = (Fixed64)value;
        var expected = Math.Exp(value);
        var result = Fixed64.Exp(fixedValue);

        // 对于较小的结果，使用相对误差或调整容忍度可能更好
        double absoluteError = Math.Abs((double)result - expected);
        Assert.True(absoluteError < Tolerance, $"Value: {value}, Expected: {expected}, Actual: {(double)result}");
    }

    #endregion

    #region Boundary and Overflow Tests

    [Fact]
    public void Exp_VeryLargeNegativeInput_ShouldApproachZero()
    {
        // e 的一个很大的负数次幂应该非常接近 0
        var largeNegativeInput = (Fixed64)(-30.0);
        var result = Fixed64.Exp(largeNegativeInput);

        // 结果应该在 0 和一个非常小的正数之间
        Assert.True(result >= Fixed64.Zero && result < (Fixed64)0.000001);
    }

    #endregion

    #region Precision Tests with Range of Values

    [Theory]
    // 在 0 附近
    [InlineData(0.001)]
    [InlineData(-0.001)]
    // 小数
    [InlineData(0.25)]
    [InlineData(0.5)]
    [InlineData(0.75)]
    // 1 到 10 之间的值
    [InlineData(1.5)]
    [InlineData(3.14)]
    [InlineData(5.0)]
    [InlineData(10.0)]
    public void Exp_VariousInputs_ShouldBeCloseToMathExp(double value)
    {
        var fixedValue = (Fixed64)value;
        var expected = Math.Exp(value);
        var result = Fixed64.Exp(fixedValue);

        // 对于不同的值范围，可能需要不同的误差策略
        // 这里我们使用一个组合：绝对误差和相对误差
        double absoluteError = Math.Abs((double)result - expected);
        double relativeError = expected == 0 ? 0 : absoluteError / Math.Abs(expected);

        // 要求绝对误差小于一个阈值，或者相对误差小于一个百分比
        Assert.True(absoluteError < Tolerance || relativeError < 0.001, // 0.1% 相对误差
            $"Value: {value}, Expected: {expected}, Actual: {(double)result}, AbsError: {absoluteError}, RelError: {relativeError}");
    }

    #endregion
}
