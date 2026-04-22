using System;
using Gal.Core;
using Xunit;
using static System.Int32;

namespace Fixed64Test
{
    public class Fixed64Test
    {
        [Fact]
        public void TestInt() {
            Fixed64 f = 100;
            Assert.Equal(100, (int)f);

            f += 100;
            Assert.Equal(200, (int)f);

            f -= 100;
            Assert.Equal(100, (int)f);

            f *= 100;
            Assert.Equal(10000, (int)f);

            f /= 100;
            Assert.Equal(100, (int)f);
        }

        [Fact]
        public void TestFloat() {
            Fixed64 f = (Fixed64)100f;
            Assert.Equal(100f, (float)f);

            f += (Fixed64)100f;
            Assert.Equal(200f, (float)f);

            f -= (Fixed64)100f;
            Assert.Equal(100f, (float)f);

            f *= (Fixed64)100f;
            Assert.Equal(10000f, (float)f);

            f /= (Fixed64)100f;
            Assert.Equal(100f, (float)f);
        }

        [Fact]
        public void TestLong() {
            Fixed64 f = (Fixed64)100L;
            Assert.Equal(100L, (long)f);

            f += (Fixed64)100L;
            Assert.Equal(200L, (long)f);

            f -= (Fixed64)100L;
            Assert.Equal(100L, (long)f);

            f *= (Fixed64)100L;
            Assert.Equal(10000L, (long)f);

            f /= (Fixed64)100L;
            Assert.Equal(100L, (long)f);
        }

        [Fact]
        public void TestDouble() {
            Fixed64 f = (Fixed64)100d;
            Assert.Equal(100d, (double)f);

            f += (Fixed64)100d;
            Assert.Equal(200d, (double)f);

            f -= (Fixed64)100d;
            Assert.Equal(100f, (double)f);

            f *= (Fixed64)100d;
            Assert.Equal(10000d, (double)f);

            f /= (Fixed64)100d;
            Assert.Equal(100d, (double)f);
        }

        [Fact]
        public void TestAll() {
            Fixed64 f = 100;
            f += (Fixed64)0.5f;
            Assert.Equal(100.5f, (float)f);
        }

        [Fact]
        public void LongToFix64AndBack() {
            for (var i = 0; i < 10000; ++i) {
                var expected = Random.Shared.Next(MinValue, MaxValue);
                var f = (Fixed64)expected;
                var actual = (long)f;
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void DoubleToFix64AndBack() {
            var sources = new[] { (double)int.MinValue, -(double)Math.PI, -(double)Math.E, -1.0, -0.0, 0.0, 1.0, (double)Math.PI, (double)Math.E, (double)int.MaxValue };

            foreach (var value in sources) {
                Assert.True(Math.Abs((decimal)value - (Fixed64)value) < Fixed64.Precision);
            }
        }

        [Fact]
        public void TestAddition() {
            Fixed64 a = (Fixed64)1.5m;
            Fixed64 b = (Fixed64)2.5m;
            Fixed64 c = a + b;
            Assert.Equal(4.0d, (double)c);
        }

        [Fact]
        public void TestSubtraction() {
            Fixed64 a = (Fixed64)3.5m;
            Fixed64 b = (Fixed64)2.5m;
            Fixed64 c = a - b;
            Assert.Equal(1.0d, (double)c);
        }

        [Fact]
        public void TestMultiplication() {
            Fixed64 a = (Fixed64)2.5m;
            Fixed64 b = (Fixed64)3.0m;
            Fixed64 c = a * b;
            Assert.Equal(7.5d, (double)c);
        }

        [Fact]
        public void TestDivision() {
            Fixed64 a = (Fixed64)10.0m;
            Fixed64 b = (Fixed64)2.0m;
            Fixed64 c = a / b;
            Assert.Equal(5.0d, (double)c);
        }

        [Fact]
        public void TestEquality() {
            Fixed64 a = (Fixed64)1.0m;
            Fixed64 b = (Fixed64)1.0m;
            Assert.True(a == b);
        }

        [Fact]
        public void TestInequality() {
            Fixed64 a = (Fixed64)1.0m;
            Fixed64 b = (Fixed64)2.0m;
            Assert.True(a != b);
        }
    }
}