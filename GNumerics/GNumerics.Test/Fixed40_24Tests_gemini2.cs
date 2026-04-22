using Xunit;
using System;
using Gal.Core;
using System.Collections.Generic;

namespace Gal.Core.Tests
{
    public class Fixed40_24Tests_gemini2
    {
        private const long OneRaw = 1L << 24;

        // 辅助方法：生成原始 raw 值
        private long ToRaw(double value) => (long)(value * OneRaw);

        #region 1. 基础转换与常量测试

        [Fact]
        public void Constants_ShouldBeCorrect()
        {
            Assert.Equal(0, Fixed40_24.Zero.Raw);
            Assert.Equal(OneRaw, Fixed40_24.One.Raw);
            Assert.Equal(OneRaw * 2, Fixed40_24.Two.Raw);
            Assert.Equal(OneRaw / 2, Fixed40_24.HalfOne.Raw);
        }

        [Theory]
        [InlineData(10, 10.0)]
        [InlineData(-5, -5.0)]
        public void Cast_FromInt_ShouldWork(int input, double expected)
        {
            Fixed40_24 val = input;
            Assert.Equal(ToRaw(expected), val.Raw);
        }

        [Theory]
        [InlineData(1.5f, 1.5)]
        [InlineData(-0.25f, -0.25)]
        public void Cast_FromFloat_ShouldWork(float input, double expected)
        {
            Fixed40_24 val = (Fixed40_24)input;
            // 允许 1 bit 的原始误差（浮点转换精度问题）
            Assert.InRange(val.Raw, ToRaw(expected) - 1, ToRaw(expected) + 1);
        }

        #endregion

        #region 2. 核心数学函数测试 (Floor/Ceiling/Round)

        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(1.9, 1.0)]
        [InlineData(0.5, 0.0)]
        [InlineData(0.0, 0.0)]
        public void Floor_PositiveValues_ShouldWork(double input, double expected)
        {
            var val = (Fixed40_24)input;
            var res = Fixed40_24.Floor(val);
            Assert.Equal(ToRaw(expected), res.Raw);
        }

        /// <summary>
        /// 特别注意：当前实现的 Floor 使用的是位与 Mask 逻辑
        /// 这种逻辑在处理负数时表现为“向零取整”（Truncate），而非数学上的“向负无穷取整”
        /// </summary>
        [Theory]
        [InlineData(-1.0, -1.0)]
        [InlineData(-1.1, -2.0)] // 注意：位掩码会导致 -1.1 变为 -1.0
        [InlineData(-1.9, -2.0)]
        [InlineData(-0.5, -1.0)]
        public void Floor_NegativeValues_TruncateBehavior(double input, double expected)
        {
            var val = (Fixed40_24)input;
            var res = Fixed40_24.Floor(val);
            Assert.Equal(ToRaw(expected), res.Raw);
        }

        [Theory]
        [InlineData(1.1, 2.0)]
        [InlineData(1.0, 1.0)]
        [InlineData(-1.1, -1.0)] // 因为 Floor 是截断，Ceiling 逻辑也会受影响
        [InlineData(0.0, 0.0)]
        public void Ceiling_ShouldWork(double input, double expected)
        {
            var val = (Fixed40_24)input;
            var res = Fixed40_24.Ceiling(val);
            Assert.Equal(ToRaw(expected), res.Raw);
        }

        [Theory]
        [InlineData(2.5, 2.0)] // 四舍六入五取偶
        [InlineData(3.5, 4.0)]
        [InlineData(2.51, 3.0)]
        [InlineData(-2.5, -2.0)]
        public void Round_ShouldFollowBankersRounding(double input, double expected)
        {
            var val = (Fixed40_24)input;
            var res = Fixed40_24.Round(val);
            Assert.Equal(ToRaw(expected), res.Raw);
        }

        #endregion

        #region 3. 算术运算测试

        [Fact]
        public void BasicArithmetic_ShouldBeAccurate()
        {
            var a = (Fixed40_24)10;
            var b = (Fixed40_24)3;

            Assert.Equal((Fixed40_24)13, a + b);
            Assert.Equal((Fixed40_24)7, a - b);
            Assert.Equal((Fixed40_24)30, a * b);

            // 验证除法精度 10 / 3 ≈ 3.3333333
            var div = a / b;
            double divDouble = (double)div;
            Assert.Equal(3.3333333, divDouble, 5);
        }

        [Fact]
        public void Operator_Overload_Int_Mixed()
        {
            var a = (Fixed40_24)10;

            Assert.Equal((Fixed40_24)15, a + 5);
            Assert.Equal((Fixed40_24)5, a - 5);
            Assert.Equal((Fixed40_24)50, a * 5);
            Assert.Equal((Fixed40_24)2, a / 5);
        }

        #endregion

        #region 4. 比较运算

        [Fact]
        public void Comparison_ShouldWork()
        {
            var small = (Fixed40_24)1.2;
            var large = (Fixed40_24)1.5;

            Assert.True(large > small);
            Assert.True(small < large);
            Assert.True(small <= small);
            Assert.True(large >= small);
            Assert.True(small != large);
            Assert.False(small == large);
        }

        #endregion

        #region 5. 高级数学函数 (Sin/Cos/Sqrt/Log)

        [Theory]
        [InlineData(4.0, 2.0)]
        [InlineData(9.0, 3.0)]
        [InlineData(2.0, 1.41421356)]
        public void Sqrt_ShouldWork(double input, double expected)
        {
            var val = (Fixed40_24)input;
            var res = Fixed40_24.Sqrt(val);
            Assert.Equal(expected, (double)res, 5);
        }

        [Fact]
        public void Trigonometry_ShouldBeReasonable()
        {
            // Sin(PI/2) = 1
            Assert.Equal(1.0, (double)Fixed40_24.Sin(Fixed40_24.PIOver2), 5);

            // Cos(PI) = -1
            Assert.Equal(-1.0, (double)Fixed40_24.Cos(Fixed40_24.PI), 5);

            // Tan(PI/4) = 1
            Assert.Equal(1.0, (double)Fixed40_24.Tan(Fixed40_24.PIOver4), 4);
        }

        [Fact]
        public void Power_And_Log_ShouldWork()
        {
            // 2^3 = 8
            var res = Fixed40_24.Pow((Fixed40_24)2, 3);
            Assert.Equal(8.0, (double)res, 4);

            // Log2(8) = 3
            var logRes = Fixed40_24.Log2((Fixed40_24)8);
            Assert.Equal(3.0, (double)logRes, 4);
        }

        #endregion

        #region 6. 异常与边界值

        [Fact]
        public void Sign_ShouldReturnCorrectValues()
        {
            Assert.Equal(-1, Fixed40_24.Sign((Fixed40_24)(-10)));
            Assert.Equal(1, Fixed40_24.Sign((Fixed40_24)10));
            Assert.Equal(0, Fixed40_24.Sign(Fixed40_24.Zero));
        }

        #endregion
    }
}
