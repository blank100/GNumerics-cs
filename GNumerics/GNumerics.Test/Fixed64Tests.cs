using System;
using Xunit;
using Gal.Core; // 你的 Fixed64 所在命名空间

namespace Fixed64Test {
    public class Fixed64Tests {
        private static double Tolerance = (double)(Fixed64.Epsilon);

        [Fact]
        public void FromInt_And_ToInt_Roundtrip() {
            Fixed64 a = (Fixed64)123;
            int ai = (int)a;
            Assert.Equal(123, ai);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.5)]
        [InlineData(-2.75)]
        [InlineData(123456.789)]
        public void FromDouble_And_ToDouble_Roundtrip(double v) {
            Fixed64 a = (Fixed64)v;
            double dv = (double)a;
            Assert.InRange(dv, v - Tolerance, v + Tolerance);
        }

        [Theory]
        [InlineData("0", 0.0)]
        [InlineData("123", 123.0)]
        [InlineData("-45", -45.0)]
        [InlineData("3.14159", 3.14159)]
        [InlineData("-0.125", -0.125)]
        public void FromChars_ParsesCorrectly(string s, double expected) {
            Fixed64 a = Fixed64.Parse(s.AsSpan());
            Assert.InRange((double)a, expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void FromChars_Invalid_Throws() {
            Assert.Throws<FormatException>(() => Fixed64.Parse("12a34".AsSpan()));
            Assert.Throws<FormatException>(() => Fixed64.Parse("12.12.3"));
        }

        [Theory]
        [InlineData(2.5, 2.0)]
        [InlineData(2.0, 2.0)]
        [InlineData(-1.1, -2.0)]
        public void Floor_Works(double v, double expectedFloor) {
            var a = (Fixed64)v;
            var f = Fixed64.Floor(a);
            Assert.InRange((double)f, expectedFloor - Tolerance, expectedFloor + Tolerance);
        }

        [Theory]
        [InlineData(2.4, 2.0)]
        [InlineData(2.5, 2.0)]
        [InlineData(2.5001, 3.0)]
        [InlineData(-1.4, -1.0)]
        [InlineData(-1.5, -2.0)]
        public void Round_Works(double v, double expected) {
            var a = (Fixed64)v;
            var r = Fixed64.Round(a);
            Assert.InRange((double)r, expected - Tolerance, expected + Tolerance);
        }

        [Theory]
        [InlineData(-5.5, 5.5)]
        [InlineData(3.3, 3.3)]
        public void Abs_Works(double v, double expected) {
            var a = (Fixed64)v;
            var ab = Fixed64.Abs(a);
            Assert.InRange((double)ab, Math.Abs(v) - Tolerance, Math.Abs(v) + Tolerance);
        }

        [Theory]
        [InlineData(1.2, 0.0, 2.0, 1.2)]
        [InlineData(-1.0, 0.0, 2.0, 0.0)]
        [InlineData(3.0, 0.0, 2.0, 2.0)]
        public void Clamp_Works(double v, double min, double max, double expected) {
            var a = (Fixed64)v;
            var c = Fixed64.Clamp(a, (Fixed64)min, (Fixed64)max);
            Assert.InRange((double)c, expected - Tolerance, expected + Tolerance);
        }

        [Theory]
        [InlineData(4.0, 2.0)]
        [InlineData(2.0, 1.4142135)]
        public void Sqrt_Works(double v, double expected) {
            var a = (Fixed64)v;
            var s = Fixed64.Sqrt(a);
            Assert.InRange((double)s, expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void Arithmetic_Add_Subtract() {
            var a = (Fixed64)1.5;
            var b = (Fixed64)2.25;
            Assert.InRange((double)(a + b), 3.75 - Tolerance, 3.75 + Tolerance);
            Assert.InRange((double)(b - a), 0.75 - Tolerance, 0.75 + Tolerance);
        }

        [Fact]
        public void Arithmetic_Multiply_Divide() {
            var a = (Fixed64)3.0;
            var b = (Fixed64)1.5;
            Assert.InRange((double)(a * b), 4.5 - Tolerance, 4.5 + Tolerance);
            Assert.InRange((double)(a / b), 2.0 - Tolerance, 2.0 + Tolerance);
        }

        [Fact]
        public void Comparison_Operators() {
            var a = (Fixed64)1.0;
            var b = (Fixed64)2.0;
            Assert.True(a < b);
            Assert.True(b > a);
            Assert.True(a <= a);
            Assert.True(b >= a);
            Assert.True(a != b);
            Assert.True(a == (Fixed64)1.0);
        }

        [Fact]
        public void ToString_FormatsCorrectly() {
            var a = (Fixed64)3.14159;
            string s = a.ToString("F3", null);
            Assert.Equal(((double)a).ToString("F3"), s);
        }
    }
}
