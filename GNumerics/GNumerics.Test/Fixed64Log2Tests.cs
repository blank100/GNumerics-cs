using System;
using Gal.Core;
using Xunit;

namespace Fixed64Test
{
	public class Fixed64Log2Tests
	{
		static readonly double Tolerance = (double)Fixed64.Precision * 10;

		[Fact]
		public void Value_ReturnsCorrectLog() {
			Fixed64 value = 8;
			var result = Fixed64.Log2(value);
			Assert.Equal(3, (int)result);// log2(8) = 3
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

		[Theory]
		[InlineData(3.0, 1.584962500721156)]// log2(3)
		[InlineData(5.0, 2.321928094887362)]// log2(5)
		[InlineData(10.0, 3.321928094887362)]// log2(10)
		[InlineData(0.25, -2.0)]// log2(1/4)
		[InlineData(6.0, 2.584962500721156)]// log2(6)
		public void Log2Pure_NonIntegerValues(double input, double expected) {
			var x = (Fixed64)input;
			var result = Fixed64.Log2(x);
			double actual = (double)result;
			Assert.InRange(actual, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Log2Pure_ValueOnePointOne() {
			var x = (Fixed64)1.1;
			var result = Fixed64.Log2(x);
			double expected = Math.Log2(1.1);
			Assert.InRange((double)result, expected - Tolerance, expected + Tolerance);
		}

		[Fact]
		public void Log2Pure_MinPositiveValue() {
			// smallest positive representable fixed = 1 / 2^FRACTION_BITS
			var minPos = Fixed64.FromRaw(1);
			var result = Fixed64.Log2(minPos);
			// log2(2^-24) = -24
			double actual = (double)result;
			Assert.InRange(actual, -Fixed64.FRACTION_BITS - Tolerance, -Fixed64.FRACTION_BITS + Tolerance);
		}

		[Fact]
		public void Log2Pure_ThrowsOnZero() {
			Assert.Throws<ArgumentOutOfRangeException>(() => Fixed64.Log2(Fixed64.Zero));
		}

		[Fact]
		public void Log2Pure_ThrowsOnNegative() {
			Assert.Throws<ArgumentOutOfRangeException>(() => Fixed64.Log2(-Fixed64.One));
		}
	}
}