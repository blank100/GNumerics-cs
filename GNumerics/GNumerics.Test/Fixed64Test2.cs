using System;
using Gal.Core;
using Xunit;

namespace Fixed64Test
{
	public class Fixed64Test2
	{
#region Test: Basic Arithmetic Operations (+, -, *, /)

		[Fact]
		public void Add_Fixed64Values_ReturnsCorrectSum() {
			Fixed64 a = 2;
			Fixed64 b = 3;
			var result = a + b;
			Assert.Equal(5, result);
			Assert.Equal(5, (int)result);
		}

		[Fact]
		public void Subtract_Fixed64Values_ReturnsCorrectDifference() {
			Fixed64 a = 5;
			Fixed64 b = 3;
			var result = a - b;
			Assert.Equal(2, result);
			Assert.Equal(2, (int)result);
		}

		[Fact]
		public void Multiply_Fixed64Values_ReturnsCorrectProduct() {
			Fixed64 a = 2;
			Fixed64 b = 3;
			var result = a * b;
			Assert.Equal(6, result);
			Assert.Equal(6, (int)result);
		}

		[Fact]
		public void Divide_Fixed64Values_ReturnsCorrectQuotient() {
			Fixed64 a = 6;
			Fixed64 b = 2;
			var result = a / b;
			Assert.Equal(3, result);
			Assert.Equal(3, (int)result);
		}

		[Fact]
		public void Divide_ByZero_ThrowsException() {
			Fixed64 a = 6;
			Assert.Throws<DivideByZeroException>(() => {
				var result = a / Fixed64.Zero;
			});
		}

#endregion

#region Test: Comparison Operators (<, <=, >, >=, ==, !=)

		[Fact]
		public void GreaterThan_Fixed64Values_ReturnsTrue() {
			Fixed64 a = 5;
			Fixed64 b = 3;
			Assert.True(a > b);
		}

		[Fact]
		public void LessThanOrEqual_Fixed64Values_ReturnsTrue() {
			Fixed64 a = 3;
			Fixed64 b = 5;
			Assert.True(a <= b);
		}

		[Fact]
		public void Equality_Fixed64Values_ReturnsTrue() {
			Fixed64 a = 5;
			Fixed64 b = 5;
			Assert.True(a == b);
		}

		[Fact]
		public void NotEquality_Fixed64Values_ReturnsTrue() {
			Fixed64 a = 5;
			Fixed64 b = 3;
			Assert.True(a != b);
		}

#endregion

#region Test: Implicit and Explicit Conversions

		[Fact]
		public void Convert_FromInteger_ReturnsCorrectFixed64() {
			Fixed64 result = (Fixed64)5;
			Assert.Equal(5, result);
			Assert.Equal(5, (int)result);
		}

		[Fact]
		public void Convert_FromFloat_ReturnsCorrectFixed64() {
			Fixed64 result = (Fixed64)5.5f;
			Assert.Equal(5.5f, (float)result);
		}

		[Fact]
		public void Convert_ToDouble_ReturnsCorrectDouble() {
			Fixed64 fixedValue = (Fixed64)5.5f;
			double result = (double)fixedValue;
			Assert.Equal(5.5, result);
		}
#endregion
	}
}