using System;
using Gal.Core;
using Xunit;

namespace Fixed64Test;

public class Fixed47_16_Test {
    private static long[] s_TestCases = new[] {
        // Small numbers
        0L, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10,

        // Integer numbers
        0x100000000, -0x100000000, 0x200000000, -0x200000000, 0x300000000, -0x300000000, 0x400000000, -0x400000000, 0x500000000, -0x500000000, 0x600000000, -0x600000000,

        // Fractions (1/2, 1/4, 1/8)
        0x80000000, -0x80000000, 0x40000000, -0x40000000, 0x20000000, -0x20000000,

        // Problematic carry
        0xFFFFFFFF, -0xFFFFFFFF, 0x1FFFFFFFF, -0x1FFFFFFFF, 0x3FFFFFFFF, -0x3FFFFFFFF,

        // Smallest and largest values
        long.MaxValue, long.MinValue,

        // Large random numbers
        6791302811978701836, -8192141831180282065, 6222617001063736300, -7871200276881732034, 8249382838880205112, -7679310892959748444, 7708113189940799513, -5281862979887936768, 8220231180772321456,
        -5204203381295869580, 6860614387764479339, -9080626825133349457, 6658610233456189347, -6558014273345705245, 6700571222183426493,

        // Small random numbers
        -436730658, -2259913246, 329347474, 2565801981, 3398143698, 137497017, 1060347500, -3457686027, 1923669753, 2891618613, 2418874813, 2899594950, 2265950765, -1962365447, 3077934393

        // Tiny random numbers
        - 171,
        -359, 491, 844, 158, -413, -422, -737, -575, -330, -376, 435, -311, 116, 715, -1024, -487, 59, 724, 993
    };

    [Fact]
    public void SqrtTest() {
        foreach (var @case in s_TestCases) {
            var v = Fixed64.FromRaw(@case);
            if (v < 0) {
                Assert.Throws<ArgumentOutOfRangeException>(() => Fixed64.Sqrt(v));
            } else {
                var actual = (double)Fixed64.Sqrt(v);
                var expected = Math.Sqrt((double)v);
                var delta = (decimal)Math.Abs(expected - actual);
                Assert.True(delta <= Fixed64.Precision, $"source:{@case}, fixed64:{v}, expected:{expected}, actual:{actual}");
            }
        }
    }

    [Fact]
    public void DivTest() {
        foreach (var i in s_TestCases) {
            foreach (var j in s_TestCases) {
                var x = Fixed64.FromRaw(i);
                var y = Fixed64.FromRaw(j);

                var xm = (decimal)x;
                var ym = (decimal)y;
                if (j == 0) Assert.Throws<DivideByZeroException>(() => x / y);
                else {
                    var expected = xm / ym;
                    expected =
                        expected > Fixed64.MaxValue
                            ? Fixed64.MaxValue
                            : expected <= Fixed64.MinValue
                                ? Fixed64.MinValue
                                : expected;
                    var actual = x / y;
                    var delta = Math.Abs(expected - actual);
                    Assert.True(delta <= Fixed64.Precision * 2,
                        $"dividend:{i}, divisor:{j}, decimalDividend:{xm},decimalDivisor:{ym} , expected:{expected}, actual:{actual}, delta:{delta}, Fixed64.Precision:{Fixed64.Precision}"
                    );
                }
            }
        }
    }
}
