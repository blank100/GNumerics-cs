using Gal.Core;
using Xunit;

namespace Fixed64Test
{
	public class FixedMathTests
    {
#region Test: Clamp01 Method

        [Fact]
        public void Clamp01_ValueLessThanZero_ReturnsZero()
        {
            var result = Fixed64.Clamp01(-1);
            Assert.Equal(Fixed64.Zero, result);
        }

        [Fact]
        public void Clamp01_ValueGreaterThanOne_ReturnsOne()
        {
            var result = Fixed64.Clamp01(2);
            Assert.Equal(Fixed64.One, result);
        }

        [Fact]
        public void Clamp01_ValueInRange_ReturnsValue()
        {
            var result = Fixed64.Clamp01((Fixed64)0.5f);
            Assert.Equal((Fixed64)0.5f, result);
        }

        #endregion

        #region Test: FastAbs Method

        [Fact]
        public void FastAbs_PositiveValue_ReturnsSameValue()
        {
            var result = Fixed64.Abs(10);
            Assert.Equal(10, result);
        }

        [Fact]
        public void FastAbs_NegativeValue_ReturnsPositiveValue()
        {
            var result = Fixed64.Abs(-10);
            Assert.Equal(10, result);
        }

        #endregion

        #region Test: Ceiling Method

        [Fact]
        public void Ceiling_WithFraction_ReturnsNextInteger()
        {
            var result = Fixed64.Ceiling((Fixed64)1.5);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Ceiling_ExactInteger_ReturnsSameInteger()
        {
            var result = Fixed64.Ceiling((Fixed64)3.0);
            Assert.Equal(3, result);
        }

        #endregion

        #region Test: Max Method

        [Fact]
        public void Max_FirstValueLarger_ReturnsFirstValue()
        {
            var result = Fixed64.Max(5, 3);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Max_SecondValueLarger_ReturnsSecondValue()
        {
            var result = Fixed64.Max(3, 5);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Max_EqualValues_ReturnsEitherValue()
        {
            var result = Fixed64.Max(5, 5);
            Assert.Equal(5, result);
        }

        #endregion

        #region Test: Min Method

        [Fact]
        public void Min_FirstValueSmaller_ReturnsFirstValue()
        {
            var result = Fixed64.Min(3, 5);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Min_SecondValueSmaller_ReturnsSecondValue()
        {
            var result = Fixed64.Min(5, 3);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Min_EqualValues_ReturnsEitherValue()
        {
            var result = Fixed64.Min(5, 5);
            Assert.Equal(5, result);
        }

        #endregion

        #region Test: Round Method (Without Decimal Places)

        [Fact]
        public void Round_ToEven_RoundsToNearestEven()
        {
            var result = Fixed64.Round((Fixed64)2.5);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Round_ToEven_NegativeNumber_RoundsToNearestEven()
        {
            var result = Fixed64.Round((Fixed64)(-2.5));
            Assert.Equal(-2, result);
        }

        #endregion

#region Test: Add Method

        [Fact]
        public void FastAdd_AddsTwoPositiveValues()
        {
            Fixed64 a = 2;
            Fixed64 b = 3;
            var result = a + b;
            Assert.Equal(5, result);
        }

        [Fact]
        public void FastAdd_AddsNegativeAndPositiveValue() {
            Fixed64 a = -2;
            Fixed64 b = 3;
            var result = a + b;
            Assert.Equal(1, result);
        }

        [Fact]
        public void FastAdd_AddsTwoNegativeValues() {
            Fixed64 a = -5;
            Fixed64 b = -3;
            var result = a + b;
            Assert.Equal(-8, result);
        }

        #endregion

        #region Test: Sub Method

        [Fact]
        public void FastSub_SubtractsTwoPositiveValues()
        {
            Fixed64 a = 5;
            Fixed64 b = 3;
            var result = a - b;
            Assert.Equal(2, result);
        }

        [Fact]
        public void FastSub_SubtractsNegativeFromPositive() {
            Fixed64 a = 5;
            Fixed64 b = -3;
            var result = a - b;
            Assert.Equal(8, result);
        }

        [Fact]
        public void FastSub_SubtractsPositiveFromNegative() {
            Fixed64 a = -5;
            Fixed64 b = 3;
            var result = a - b;
            Assert.Equal(-8, result);
        }

        #endregion

        #region Test: FastMul Method

        [Fact]
        public void FastMul_MultipliesTwoPositiveValues() {
            Fixed64 a = 2;
            Fixed64 b = 3;
            var result = a * b;
            Assert.Equal(6, result);
        }

        [Fact]
        public void FastMul_MultipliesPositiveAndNegativeValue()
        {
            Fixed64 a = 2;
            Fixed64 b = -3;
            var result = a*b;
            Assert.Equal(-6, result);
        }

        [Fact]
        public void FastMul_MultipliesWithZero()
        {
            Fixed64 a = 0;
            Fixed64 b = 3;
            var result = a*b;
            Assert.Equal(Fixed64.Zero, result);
        }

        #endregion

#region Test: Clamp Method
        [Fact]
        public void Clamp_ValueWithinRange_ReturnsValue()
        {
            var result = Fixed64.Clamp(3, 2, 5);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Clamp_ValueBelowMin_ReturnsMin()
        {
            var result = Fixed64.Clamp(1, 2, 5);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Clamp_ValueAboveMax_ReturnsMax()
        {
            var result = Fixed64.Clamp(6, 2, 5);
            Assert.Equal(5, result);
        }

        #endregion
    }
}
