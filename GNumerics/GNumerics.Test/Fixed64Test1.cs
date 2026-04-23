using System;
using Gal.Core;
using Xunit;
using Xunit.Abstractions;

namespace Fixed64Test {
    public class Fixed64Test1 {
        private readonly ITestOutputHelper _TestOutputHelper;

        public Fixed64Test1(ITestOutputHelper testOutputHelper) {
            _TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public void DoubleToFix64AndBack() {
            var sources = new[] { int.MinValue, -Math.PI, -Math.E, -1.0, -0.0, 0.0, 1.0, Math.PI, Math.E, int.MaxValue };

            foreach (var value in sources) {
                AreEqualWithinPrecision((decimal)value, (decimal)(Fixed64)value);
            }
        }

        static void AreEqualWithinPrecision(decimal value1, decimal value2) {
            Assert.True(Math.Abs(value2 - value1) < Fixed64.Precision);
        }

        static void AreEqualWithinPrecision(double value1, double value2) {
            Assert.True(Math.Abs(value2 - value1) < (double)Fixed64.Precision);
        }

        [Fact]
        public void DecimalToFix64AndBack() {
            Assert.Equal(Fixed64.MaxValue, (Fixed64)(decimal)Fixed64.MaxValue);
            Assert.Equal(Fixed64.MinValue, (Fixed64)(decimal)Fixed64.MinValue);

            Assert.Equal(0m, (Fixed64)0m);
            Assert.Equal(1m, (Fixed64)1m);
            Assert.Equal(2m, (Fixed64)2m);
            Assert.Equal(3m, (Fixed64)3m);
            Assert.Equal(4m, (Fixed64)4m);
            Assert.Equal(5m, (Fixed64)5m);
            Assert.Equal(6m, (Fixed64)6m);
            Assert.Equal(7m, (Fixed64)7m);
            Assert.Equal(8m, (Fixed64)8m);
            Assert.Equal(9m, (Fixed64)9m);
            Assert.Equal(-1m, (Fixed64)(-1m));
            Assert.Equal(-2m, (Fixed64)(-2m));
            Assert.Equal(-3m, (Fixed64)(-3m));
            Assert.Equal(-4m, (Fixed64)(-4m));
            Assert.Equal(-5m, (Fixed64)(-5m));
            Assert.Equal(-6m, (Fixed64)(-6m));
            Assert.Equal(-7m, (Fixed64)(-7m));
            Assert.Equal(-8m, (Fixed64)(-8m));
            Assert.Equal(-9m, (Fixed64)(-9m));

            Assert.Equal(int.MaxValue, (decimal)(Fixed64)int.MaxValue);
            Assert.Equal(int.MinValue, (decimal)(Fixed64)int.MinValue);

            Assert.Equal(short.MaxValue, (decimal)(Fixed64)short.MaxValue);
            Assert.Equal(short.MinValue, (decimal)(Fixed64)short.MinValue);

            Assert.Equal(sbyte.MaxValue, (decimal)(Fixed64)sbyte.MaxValue);
            Assert.Equal(sbyte.MinValue, (decimal)(Fixed64)sbyte.MinValue);

            Assert.Equal(Fixed64.Parse("3.1415"), (Fixed64)(3.1415m));
            Assert.Equal(Fixed64.Parse("-3.1415"), (Fixed64)(-3.1415m));
            Assert.Equal(Fixed64.Parse("2.71828"), (Fixed64)(2.71828m));
            Assert.Equal(Fixed64.Parse("-2.71828"), (Fixed64)(-2.71828m));
        }

        [Fact]
        public void BasicMultiplication() {
            var term1s = new[] { 0m, 1m, -1m, 5m, -5m, 0.5m, -0.5m, -1.0m };
            var term2s = new[] { 16m, 16m, 16m, 16m, 16m, 16m, 16m, -1.0m };
            var expecteds = new[] { 0L, 16, -16, 80, -80, 8, -8, 1 };
            for (var i = 0; i < term1s.Length; ++i) {
                var expected = expecteds[i];
                var actual = (long)(term1s[i] * (Fixed64)term2s[i]);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Abs() {
            // Assert.Equal(Fixed64.maxValue, Fixed64.Abs(Fixed64.minValue));
            var sources = new[] { -1, 0, 1, int.MaxValue };
            var expecteds = new[] { 1, 0, 1, int.MaxValue };
            for (var i = 0; i < sources.Length; ++i) {
                var actual = Fixed64.Abs(sources[i]);
                var expected = (Fixed64)expecteds[i];
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Floor() {
            var sources = new[] { -5.1m, -1, 0, 1, 5.1m };
            var expecteds = new[] { -6m, -1, 0, 1, 5m };
            for (var i = 0; i < sources.Length; ++i) {
                var actual = (decimal)Fixed64.Floor((Fixed64)sources[i]);
                var expected = expecteds[i];
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Floor_Random() {
            for (var i = 0; i < 1000; i++) {
                var v = Random.Shared.NextDouble() * 100000 - 5000;
                var expected = (decimal)Math.Floor(v);
                var actual = (decimal)Fixed64.Floor((Fixed64)v);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Truncate() {
            var sources = new[] { -5.1m, -1, 0, 1, 5.1m };
            var expecteds = new[] { -5m, -1, 0, 1, 5m };
            for (var i = 0; i < sources.Length; ++i) {
                var actual = (decimal)Fixed64.Truncate((Fixed64)sources[i]);
                var t = Math.Truncate(sources[i]);
                var expected = expecteds[i];
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Truncate_Random() {
            for (var i = 0; i < 1000; i++) {
                var v = Random.Shared.NextDouble() * 100000 - 5000;
                var expected = (decimal)Math.Truncate(v);
                var actual = (decimal)Fixed64.Truncate((Fixed64)v);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Ceiling() {
            var sources = new[] { -5.1m, -1, 0, 1, 5.1m };
            var expecteds = new[] { -5m, -1, 0, 1, 6m };
            for (var i = 0; i < sources.Length; ++i) {
                var actual = (decimal)Fixed64.Ceiling((Fixed64)sources[i]);
                var expected = expecteds[i];
                Assert.Equal(expected, actual);
            }

            // Assert.NotEqual(Fixed64.maxValue, Fixed64.Ceiling(Fixed64.maxValue));
        }

        [Fact]
        public void Round() {
            {
                var f = Fixed64.Round((Fixed64)(-5.5m));
                Assert.Equal(-6L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(-5.1m));
                Assert.Equal(-5L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(-4.5d));
                Assert.Equal(-4L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(-4.4d));
                Assert.Equal(-4L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(-1d));
                Assert.Equal(-1L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(0d));
                Assert.Equal(0L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(1d));
                Assert.Equal(1L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(4.5d));
                Assert.Equal(4L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(4.6d));
                Assert.Equal(5L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(5.4d));
                Assert.Equal(5L, (long)f);
            }

            {
                var f = Fixed64.Round((Fixed64)(5.5d));
                Assert.Equal(6L, (long)f);
            }
        }
    }
}
