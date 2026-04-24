using System;
using Xunit;
using Gal.Core;

namespace Gal.Core.Tests
{
    public class Fixed64InvSqrtTests
    {
        // 允许误差（根据你当前 InvSqrtFast 精度可调整）
        private const double Tolerance = 1e-4;

        private static void AssertApproximatelyEqual(double expected, double actual, double tolerance = Tolerance)
        {
            Assert.True(
                Math.Abs(expected - actual) <= tolerance,
                $"Expected: {expected}, Actual: {actual}, Diff: {Math.Abs(expected - actual)}"
            );
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.0)]
        [InlineData(3.0)]
        [InlineData(4.0)]
        [InlineData(0.5)]
        [InlineData(10.0)]
        [InlineData(100.0)]
        public void InvSqrt_NormalValues_ShouldBeAccurate(double value)
        {
            var fixedValue = (Fixed64)value;

            var result = Fixed64.InvSqrt(fixedValue);

            double actual = (double)result;
            double expected = 1.0 / Math.Sqrt(value);

            AssertApproximatelyEqual(expected, actual);
        }

        [Fact]
        public void InvSqrt_One_ShouldReturnOne()
        {
            var result = Fixed64.InvSqrt(Fixed64.One);

            AssertApproximatelyEqual(1.0, (double)result);
        }

        [Fact]
        public void InvSqrt_Four_ShouldReturnHalf()
        {
            var result = Fixed64.InvSqrt((Fixed64)4);

            AssertApproximatelyEqual(0.5, (double)result);
        }

        [Fact]
        public void InvSqrt_SmallValue_ShouldBeAccurate()
        {
            double value = 0.01;

            var fixedValue = (Fixed64)value;
            var result = Fixed64.InvSqrt(fixedValue);

            double expected = 1.0 / Math.Sqrt(value);
            double actual = (double)result;

            AssertApproximatelyEqual(expected, actual);
        }

        [Fact]
        public void InvSqrt_LargeValue_ShouldBeAccurate()
        {
            double value = 100000.0;

            var fixedValue = (Fixed64)value;
            var result = Fixed64.InvSqrt(fixedValue);

            double expected = 1.0 / Math.Sqrt(value);
            double actual = (double)result;

            AssertApproximatelyEqual(expected, actual);
        }

        [Fact]
        public void InvSqrt_ShouldSatisfyInverseProperty()
        {
            double value = 5.0;

            var fixedValue = (Fixed64)value;
            var invSqrt = Fixed64.InvSqrt(fixedValue);

            // invSqrt^2 * value ≈ 1
            var check = invSqrt * invSqrt * fixedValue;

            AssertApproximatelyEqual(1.0, (double)check, 1e-3);
        }

        [Fact]
        public void InvSqrt_ShouldBeMonotonicDecreasing()
        {
            var a = Fixed64.InvSqrt((Fixed64)1);
            var b = Fixed64.InvSqrt((Fixed64)2);
            var c = Fixed64.InvSqrt((Fixed64)3);

            Assert.True(a > b);
            Assert.True(b > c);
        }

        [Fact]
        public void InvSqrt_Zero_ShouldReturnZero()
        {
            var result = Fixed64.InvSqrt(Fixed64.Zero);

            Assert.Equal(Fixed64.Zero, result);
        }

        [Fact]
        public void InvSqrt_Negative_ShouldReturnZeroOrHandleGracefully()
        {
            var result = Fixed64.InvSqrt((Fixed64)(-1));

            // 你当前实现是直接调用 InvSqrtFast
            // 如果内部没做保护，这里可以改成 Assert.Throws
            Assert.True(result == Fixed64.Zero || result > Fixed64.Zero);
        }
    }
}
