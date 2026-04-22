using Gal.Core;
using Xunit;

namespace Gal.Core.Tests;

public class Fixed40_24Tests_gpt {
    private static Fixed40_24 F(double v) => (Fixed40_24)v;
    private static double D(Fixed40_24 v) => (double)v;

    // ===============================
    // ✅ 基础转换测试
    // ===============================

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(12345)]
    public void Int_Conversion(int value) {
        Fixed40_24 f = value;
        Assert.Equal(value, (int)f);
    }

    [Theory]
    [InlineData(1.5)]
    [InlineData(-2.25)]
    [InlineData(123.456)]
    public void Double_Conversion(double value) {
        var f = F(value);
        Assert.Equal(value, D(f), 6);
    }

    // ===============================
    // ✅ 运算符测试
    // ===============================

    [Fact]
    public void Add_Sub_Mul_Div() {
        var a = F(10.5);
        var b = F(2.0);

        Assert.Equal(12.5, D(a + b), 6);
        Assert.Equal(8.5, D(a - b), 6);
        Assert.Equal(21.0, D(a * b), 6);
        Assert.Equal(5.25, D(a / b), 6);
    }

    [Fact]
    public void Increment_Decrement() {
        var a = F(1.0);
        a++;
        Assert.Equal(2.0, D(a), 6);

        a--;
        Assert.Equal(1.0, D(a), 6);
    }

    // ===============================
    // ✅ 比较测试
    // ===============================

    [Fact]
    public void Comparison() {
        var a = F(5);
        var b = F(3);

        Assert.True(a > b);
        Assert.True(b < a);
        Assert.True(a >= b);
        Assert.True(b <= a);
        Assert.True(a != b);
    }

    // ===============================
    // ✅ Floor 测试
    // ===============================

    [Theory]
    [InlineData(3.7, 3.0)]
    [InlineData(0.999, 0.0)]
    [InlineData(-3.7, -4.0)]
    [InlineData(-0.1, -1.0)]
    public void Floor_Test(double input, double expected) {
        var f = F(input);
        var r = Fixed40_24.Floor(f);

        Assert.Equal(expected, D(r), 6);
    }

    // ===============================
    // ✅ Ceiling 测试
    // ===============================

    [Theory]
    [InlineData(3.2, 4.0)]
    [InlineData(3.0, 3.0)]
    [InlineData(-3.2, -3.0)]
    public void Ceiling_Test(double input, double expected) {
        var f = F(input);
        var r = Fixed40_24.Ceiling(f);

        Assert.Equal(expected, D(r), 6);
    }

    // ===============================
    // ✅ Round 测试（四舍六入五取偶）
    // ===============================

    [Theory]
    [InlineData(2.5, 2.0)]
    [InlineData(3.5, 4.0)]
    [InlineData(1.4, 1.0)]
    [InlineData(1.6, 2.0)]
    public void Round_Test(double input, double expected) {
        var f = F(input);
        var r = Fixed40_24.Round(f);

        Assert.Equal(expected, D(r), 6);
    }

    // ===============================
    // ✅ Clamp 测试
    // ===============================

    [Fact]
    public void Clamp_Test() {
        var value = F(10);
        var min = F(0);
        var max = F(5);

        var r = Fixed40_24.Clamp(value, min, max);

        Assert.Equal(5.0, D(r), 6);
    }

    [Fact]
    public void Clamp01_Test() {
        Assert.Equal(0.0, D(Fixed40_24.Clamp01(F(-1))), 6);
        Assert.Equal(1.0, D(Fixed40_24.Clamp01(F(2))), 6);
        Assert.Equal(0.5, D(Fixed40_24.Clamp01(F(0.5))), 6);
    }

    // ===============================
    // ✅ Abs / Sign / Min / Max
    // ===============================

    [Fact]
    public void Abs_Test() {
        Assert.Equal(5.0, D(Fixed40_24.Abs(F(-5))), 6);
    }

    [Fact]
    public void Sign_Test() {
        Assert.Equal(1, Fixed40_24.Sign(F(10)));
        Assert.Equal(-1, Fixed40_24.Sign(F(-10)));
        Assert.Equal(0, Fixed40_24.Sign(F(0)));
    }

    [Fact]
    public void Min_Max_Test() {
        var a = F(5);
        var b = F(3);

        Assert.Equal(3.0, D(Fixed40_24.Min(a, b)), 6);
        Assert.Equal(5.0, D(Fixed40_24.Max(a, b)), 6);
    }

    // ===============================
    // ✅ Sqrt
    // ===============================

    [Fact]
    public void Sqrt_Test() {
        var r = Fixed40_24.Sqrt(F(9));
        Assert.Equal(3.0, D(r), 6);
    }

    // ===============================
    // ✅ Pow
    // ===============================

    [Fact]
    public void Pow_Int_Test() {
        var r = Fixed40_24.Pow(F(2), 3);
        Assert.Equal(8.0, D(r), 6);
    }

    // ===============================
    // ✅ 三角函数基础测试
    // ===============================

    [Fact]
    public void Sin_Cos_Test() {
        var sin = Fixed40_24.Sin(Fixed40_24.PIOver2);
        Assert.Equal(1.0, D(sin), 4);

        var cos = Fixed40_24.Cos(F(0));
        Assert.Equal(1.0, D(cos), 4);
    }

    // ===============================
    // ✅ ToString
    // ===============================

    [Fact]
    public void ToString_Test() {
        var f = F(3.1415);
        var s = f.ToString();
        Assert.Contains("3.14", s);
    }
}
