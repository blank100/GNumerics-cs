using System;
using Gal.Core;
using Xunit;

namespace Fixed64Test
{
	public class FixedTrigonometryTests
	{
		// private static readonly double Tolerance = (double)Fixed64.FromChars("0.0001");
		private static readonly double Tolerance = (double)(Fixed64.Tolerance);

#region Test: Pow Method

		[Fact]
		public void Pow_RaisesValueToPositiveExponent() {
			Fixed64 baseValue = 2;
			Fixed64 exponent = 3;
			var result = Fixed64.Pow(baseValue, exponent);
			Assert.Equal(8, result);// 2^3 = 8
		}

		[Fact]
		public void Pow_RaisesValueToZeroExponent_ReturnsOne() {
			Fixed64 baseValue = 5;
			Fixed64 exponent = Fixed64.Zero;
			var result = Fixed64.Pow(baseValue, exponent);
			Assert.Equal(Fixed64.One, result);// Any number raised to 0 is 1
		}

		[Fact]
		public void Pow_RaisesToNegativeExponent() {
			Fixed64 baseValue = 2;
			Fixed64 exponent = -1;
			var result = Fixed64.Pow(baseValue, exponent);
			Assert.Equal((Fixed64)0.5, result);// 2^-1 = 0.5
		}

		[Fact]
		public void Pow_ZeroToNegativeExponent_ThrowsException() {
			var baseValue = Fixed64.Zero;
			Fixed64 exponent = -1;
			Assert.Throws<DivideByZeroException>(() => Fixed64.Pow(baseValue, exponent));
		}

#endregion

#region Test: Sqrt Method

		[Fact]
		public void Sqrt_PositiveValue_ReturnsSquareRoot() {
			Fixed64 value = 4;
			var result = Fixed64.Sqrt(value);
			Assert.Equal(2, result);// sqrt(4) = 2
		}

		[Fact]
		public void Sqrt_Zero_ReturnsZero() {
			var value = Fixed64.Zero;
			var result = Fixed64.Sqrt(value);
			Assert.Equal(Fixed64.Zero, result);
		}

#endregion

#region Test: Log2 and Ln Methods

		[Fact]
		public void Value_ReturnsCorrectLog() {
			Fixed64 value = 8;
			var result = Fixed64.Log2(value);
			Assert.Equal(3, result);// log2(8) = 3
		}

		[Fact]
		public void Log2_Zero_ThrowsException() {
			var value = Fixed64.Zero;
			Assert.Throws<ArgumentOutOfRangeException>(() => Fixed64.Log2(value));
		}

		[Theory]
		[InlineData(1.0, 0.0)]
		[InlineData(2.0, 1.0)]
		[InlineData(4.0, 2.0)]
		[InlineData(0.5, -1.0)]
		[InlineData(8.0, 3.0)]
		public void Log2Pure_IntegerPowersOfTwo(double input, double expected) {
			var x = (Fixed64)input;
			var result = Fixed64.Log2(x);
			double actual = (double)result;
			Assert.InRange(actual, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Log2Pure_ValueOnePointOne() {
			var x = (Fixed64)1.1;
			var result = Fixed64.Log2(x);
			double actual = (double)result;
			double expected = Math.Log(1.1, 2);
			Assert.InRange(actual, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Ln_PositiveValue_ReturnsCorrectLog() {
			Fixed64 value = (Fixed64)Math.E;
			var result = Fixed64.Ln(value);
			Assert.InRange((double)result, 1 - Tolerance, 1 + Tolerance);
		}

		[Fact]
		public void Ln_NegativeValue_ThrowsException() {
			Fixed64 value = -1;
			Assert.Throws<ArgumentOutOfRangeException>(() => Fixed64.Ln(value));
		}

#endregion

#region Test: Rad2Deg and Deg2Rad Methods

		[Fact]
		public void Rad2Deg_ConvertsRadiansToDegrees() {
			var radians = Fixed64.PIOver2;// π/2 radians
			var result = radians * Fixed64.Rad2Deg;
			double expected = 90;
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Deg2Rad_ConvertsDegreesToRadians() {
			var degrees = (Fixed64)180;
			var result = degrees * Fixed64.Deg2Rad;
			var expected = (double)Fixed64.PI;// 180 degrees = π radians
			// var t = Math.PI / 180;
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

#endregion

#region Test: Sin and Cos Methods

		[Fact]
		public void Sin_ReturnsSineOfAngle() {
			var angle = Fixed64.PIOver2;// π/2 radians = 90 degrees
			var result = Fixed64.Sin(angle);
			Assert.Equal(Fixed64.One, result);// sin(90°) = 1
		}

		[Fact]
		public void Cos_ReturnsCosineOfAngle() {
			var angle = Fixed64.PI;// π radians = 180 degrees
			var result = Fixed64.Cos(angle);
			Assert.Equal(-Fixed64.One, result);// cos(180°) = -1
		}

#endregion

#region Test: Tan Method

		[Fact]
		public void Tan_ReturnsTangentOfAngle() {
			var angle = (Fixed64)35;// 45 degrees in radians
			var rad = Fixed64.FromRaw(2623646220); //angle * Fixed64.deg2Rad;
			var result = Fixed64.Tan(rad);
			var expected = Math.Tan((double)rad);
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

#endregion

#region Test: Asin Method

		[Fact]
		public void Asin_ReturnsArcsine() {
			var value = Fixed64.One;// sin(90°) = 1
			var result = Fixed64.Asin(value);
			Assert.Equal(Fixed64.PIOver2, result);// asin(1) = π/2
		}

		[Fact]
		public void Asin_ReturnsNegativePiOver2() {
			var value = -Fixed64.One;// sin(-90°) = -1
			var result = Fixed64.Asin(value);
			Assert.Equal(-Fixed64.PIOver2, result);// asin(-1) = -π/2
		}

		[Fact]
		public void Asin_ReturnsAsinOfHalf() {
			var value = (Fixed64)0.5;
			var result = Fixed64.Asin(value);
			var expected = (double)Fixed64.PIOver6;// asin(0.5) ≈ π/6 ≈ 0.5236 radians
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Asin_ReturnsAsinOfNegativeHalf() {
			var value = (Fixed64)(-0.5);
			var result = Fixed64.Asin(value);
			var expected = (double)-Fixed64.PIOver6;// asin(-0.5) ≈ -π/6 ≈ -0.5236 radians
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Asin_ThrowsForOutOfDomain() {
			Fixed64 value = 2;// Value greater than 1
			Assert.Throws<ArithmeticException>(() => Fixed64.Asin(value));
		}

		[Fact]
		public void Asin_ReturnsZeroForZero() {
			var value = Fixed64.Zero;
			var result = Fixed64.Asin(value);
			Assert.Equal(Fixed64.Zero, result);// asin(0) = 0
		}

#endregion

#region Test: Acos Method

		[Fact]
		public void Acos_ReturnsArccosine() {
			var value = Fixed64.Zero;// cos(90°) = 0
			var result = Fixed64.Acos(value);
			Assert.Equal(Fixed64.PIOver2, result);// acos(0) = π/2
		}

		[Fact]
		public void Acos_ReturnsZeroForOne() {
			var value = Fixed64.One;// cos(0°) = 1
			var result = Fixed64.Acos(value);
			Assert.Equal(Fixed64.Zero, result);// acos(1) = 0
		}

		[Fact]
		public void Acos_ReturnsPiForNegativeOne() {
			var value = -Fixed64.One;// cos(180°) = -1
			var result = Fixed64.Acos(value);
			Assert.Equal(Fixed64.PI, result);// acos(-1) = π
		}

		[Fact]
		public void Acos_ReturnsPiOver3ForHalf() {
			var value = (Fixed64)0.5;// cos(60°) = 0.5
			var result = Fixed64.Acos(value);
			var expected = (double)Fixed64.PIOver3;// acos(0.5) ≈ π/3 ≈ 1.0472 radians
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Acos_Returns2PiOver3ForNegativeHalf() {
			var value = (Fixed64)(-0.5);// cos(120°) = -0.5
			var result = Fixed64.Acos(value);
			var expected = (double)Fixed64.TwoPI / 3;// acos(-0.5) ≈ 2π/3 ≈ 2.0944 radians
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Acos_ThrowsOutOfRangeForGreaterThanOne() {
			var value = (Fixed64)1.1;
			Assert.Throws<ArgumentOutOfRangeException>(() => Fixed64.Acos(value));
		}

		[Fact]
		public void Acos_ThrowsOutOfRangeForLessThanNegativeOne() {
			var value = (Fixed64)(-1.1);
			Assert.Throws<ArgumentOutOfRangeException>(() => Fixed64.Acos(value));
		}

#endregion

#region Test: Atan

		[Fact]
		public void Atan_ReturnsArctangent() {
			var value = Fixed64.One;// tan(45°) = 1
			var result = Fixed64.Atan(value);
			Assert.InRange((double)result, (double)Fixed64.PIOver4 - Tolerance, (double)Fixed64.PIOver4 + Tolerance);
		}

		[Fact]
		public void Atan_Symmetry() {
			var value = (Fixed64)(0.5);
			var positiveResult = Fixed64.Atan(value);
			var negativeResult = Fixed64.Atan(-value);
			Assert.Equal(positiveResult, -negativeResult);
		}

		[Fact]
		public void Atan_NearZero() {
			var value = (Fixed64)(0.00001);
			var result = Fixed64.Atan(value);
			Assert.InRange((double)result, (double)value - Tolerance, (double)value + Tolerance);
		}

		[Fact]
		public void Atan_NearOne() {
			// var value = 0.9999M;
			// var result = value.Atan();

			var value = (Fixed64)(0.9999);
			var result = Fixed64.Atan(value);
			var expected = Math.Atan((double)value);
			// Assert.Equal(expected,(double)result);
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Atan_TanInverse() {
			var values = new[] { (Fixed64)(0.1), (Fixed64)(0.5), 1, 2 };
			foreach (var value in values) {
				// var atanResult = Fixed64.Atan(Fixed64.FromRaw(429496729));
				var atanResult = Fixed64.Atan(value);
				var tanResult = Fixed64.Tan(atanResult);
				Assert.InRange((double)tanResult, (double)value - (double)Fixed64.LooseTolerance * 10, (double)value + (double)Fixed64.LooseTolerance*10);
			}
		}

		[Fact]
		public void Atan_RangeLimits() {
			var values = new[] { (Fixed64)(-10), (Fixed64)(-1), 0, 1, 10 };
			foreach (var value in values) {
				var result = Fixed64.Atan(value);
				Assert.InRange((double)result, (double)-Fixed64.PIOver2, (double)Fixed64.PIOver2);
			}
		}

		[Fact]
		public void Atan_LargeInputs() {
			var values = new[] { (Fixed64)1000, -1000, 1000000, -1000000 };
			foreach (var value in values) {
				var result = Fixed64.Atan(value);
				Assert.InRange((double)result, (double)-Fixed64.PIOver2, (double)Fixed64.PIOver2);
			}
		}

#endregion

#region Test: Atan2 Method

		[Fact]
		public void Atan2_ReturnsArctangentOfQuotient() {
			var y = Fixed64.One;
			var x = Fixed64.One;
			var result = Fixed64.Atan2(y, x);
			var expected = Math.Atan2(1, 1);
			// Assert.Equal(expected:expected, (double)result);
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
			// Assert.Equal(Fixed64.piOver4, result);// atan2(1, 1) = π/4
		}

		[Fact]
		public void Atan2_Symmetry() {
			var y = Fixed64.One;
			var x = Fixed64.One;
			var positiveResult = Fixed64.Atan2(y, x);
			var negativeResult = Fixed64.Atan2(-y, -x);
			Assert.Equal(positiveResult - Fixed64.PI, negativeResult);
		}

		[Fact]
		public void Atan2_ZeroY() {
			var x = Fixed64.One;
			var result = Fixed64.Atan2(Fixed64.Zero, x);
			Assert.Equal(Fixed64.Zero, result);// atan2(0, x) should be 0 for positive x

			result = Fixed64.Atan2(Fixed64.Zero, -x);
			Assert.Equal(Fixed64.PI, result);// atan2(0, -x) should be π for negative x
		}

		[Fact]
		public void Atan2_ZeroX() {
			var y = Fixed64.One;
			var result = Fixed64.Atan2(y, Fixed64.Zero);
			Assert.Equal(Fixed64.PIOver2, result);// atan2(y, 0) should be π/2 for positive y

			result = Fixed64.Atan2(-y, Fixed64.Zero);
			Assert.Equal(-Fixed64.PIOver2, result);// atan2(-y, 0) should be -π/2 for negative y
		}

		[Fact]
		public void Atan2_Quadrants() {
			var result = Fixed64.Atan2(Fixed64.One, Fixed64.One);// Q1
			Assert.InRange((double)result, (double)Fixed64.Zero, (double)Fixed64.PIOver2);

			result = Fixed64.Atan2(Fixed64.One, -Fixed64.One);// Q2
			Assert.InRange((double)result, (double)Fixed64.PIOver2, (double)Fixed64.PI);

			result = Fixed64.Atan2(-Fixed64.One, -Fixed64.One);// Q3
			Assert.InRange((double)result, (double)-Fixed64.PI, (double)-Fixed64.PIOver2);

			result = Fixed64.Atan2(-Fixed64.One, Fixed64.One);// Q4
			Assert.InRange((double)result, (double)-Fixed64.PIOver2, (double)-Fixed64.Zero);
		}

		// [Fact]
		// public void Atan2_SpecificAngles() {
		// 	FixedMathTestHelper.AssertWithinRelativeTolerance(Fixed64.piOver4, Fixed64.Atan2(Fixed64.one, Fixed64.one));// 45 degrees
		// 	FixedMathTestHelper.AssertWithinRelativeTolerance(-Fixed64.piOver4, Fixed64.Atan2(-Fixed64.one, Fixed64.one));// -45 degrees
		// 	FixedMathTestHelper.AssertWithinRelativeTolerance(3 * Fixed64.piOver4, Fixed64.Atan2(Fixed64.one, -Fixed64.one));// 135 degrees
		// 	FixedMathTestHelper.AssertWithinRelativeTolerance(-3 * Fixed64.piOver4, Fixed64.Atan2(-Fixed64.one, -Fixed64.one));// -135 degrees
		// }
		//
		// [Fact]
		// public void Atan2_LargeValues() {
		// 	var largeValue = 1000000);
		// 	var result = Fixed64.Atan2(largeValue, largeValue);
		// 	FixedMathTestHelper.AssertWithinRelativeTolerance(Fixed64.piOver4, result);// atan2(1000000, 1000000) should be π/4
		//
		// 	result = Fixed64.Atan2(-largeValue, -largeValue);
		// 	FixedMathTestHelper.AssertWithinRelativeTolerance(-3 * Fixed64.piOver4, result);// atan2(-1000000, -1000000) should be -3π/4
		// }

#endregion
	}
}
