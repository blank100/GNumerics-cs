using System;
using Gal.Core;
using Xunit;

namespace Fixed64Test;

public class Fixed40_24Tests_gemini {
    // 辅助方法：将 double 转换为原始 long 值，方便精准断言
    private long ToRaw(double value) => (long)(value * (1L << 24));

    [Theory]
    [InlineData(1.0, 1.0)] // 整数
    [InlineData(1.5, 1.0)] // 正小数
    [InlineData(0.99, 0.0)] // 小于1的正数
    [InlineData(0.01, 0.0)] // 接近0的正数
    [InlineData(0.0, 0.0)] // 零
    public void Floor_PositiveValues_ShouldWork(double input, double expected) {
        var val = (Fixed40_24)input;
        var res = Fixed40_24.Floor(val);

        Assert.Equal(ToRaw(expected), res.Raw);
    }

    [Theory]
    [InlineData(-1.0, -1.0)] // 负整数
    [InlineData(-1.1, -2.0)] // 负小数：Floor(与运算) vs Floor1(位移)
    [InlineData(-1.9, -2.0)] // 负小数
    [InlineData(-0.5, -1.0)] // 负小数：接近0
    public void Floor_NegativeValues_BehaviorDifference(double input, double expectedFloor) {
        var val = (Fixed40_24)input;

        var t = Math.Floor(input);

        var res = Fixed40_24.Floor(val);

        Assert.Equal(ToRaw(expectedFloor), res.Raw);
    }

    [Theory]
    [InlineData(1.1, 2.0)]
    [InlineData(1.9, 2.0)]
    [InlineData(1.0, 1.0)]
    [InlineData(-1.1, -1.0)]
    [InlineData(-1.9, -1.0)]
    [InlineData(0.0, 0.0)]
    public void Ceiling_ShouldWork(double input, double expected) {
        var val = (Fixed40_24)input;
        var res = Fixed40_24.Ceiling(val);
        Assert.Equal(ToRaw(expected), res.Raw);
    }

    [Theory]
    [InlineData(2.5, 2.0)] // 四舍六入五取偶：2.5 靠近偶数 2
    [InlineData(3.5, 4.0)] // 四舍六入五取偶：3.5 靠近偶数 4
    [InlineData(1.1, 1.0)]
    [InlineData(1.9, 2.0)]
    public void Round_ShouldFollowBankersRounding(double input, double expected) {
        var val = (Fixed40_24)input;
        var res = Fixed40_24.Round(val);
        Assert.Equal(ToRaw(expected), res.Raw);
    }

    [Fact]
    public void MaxValue_Floor_ShouldWork() {
        var val = Fixed40_24.MaxValue;
        var res = Fixed40_24.Floor(val);

        // 结果的小数部分应该被清零
        Assert.Equal(0u, (ulong)res.Raw & Fixed40_24.FRACTIONAL_PART_MASK);
    }

    [Fact]
    public void MinValue_Floor_ShouldWork() {
        var val = Fixed40_24.MinValue;
        // MinValue 是负数
        var res = Fixed40_24.Floor(val);

        Assert.Equal(0u, (ulong)res.Raw & Fixed40_24.FRACTIONAL_PART_MASK);
    }

    [Fact]
    public void Precision_Test() {
        // 验证最小精度 Epsilon
        Assert.True(Fixed40_24.Precision.Raw > 0);
        Assert.Equal(1, Fixed40_24.Precision.Raw);
    }
}
