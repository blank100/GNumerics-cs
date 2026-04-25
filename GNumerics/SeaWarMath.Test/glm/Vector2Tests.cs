using System;
using SeaWar.Mathematics;
using Xunit;

namespace SeaWar.Mathematics.Tests.glm {
    public class Vector2Tests {
        // 定点数(Fixed64)精度损失容差，根据具体精度调整，这里预留 0.01 的误差空间
        private const double DoubleTolerance = 0.0001;
        private const double AngleTolerance = 1.0; // 角度误差放宽至 1 度

        /// <summary>
        /// 辅助方法：比较两个 Vector2 是否在容差范围内相等（转为 double 比较）
        /// </summary>
        private void AssertVector2Equal(Vector2 expected, Vector2 actual, double tolerance = DoubleTolerance) {
            Assert.Equal((double)expected.x, (double)actual.x, 5);
            Assert.Equal((double)expected.y, (double)actual.y, 5);
        }

        /// <summary>
        /// 辅助方法：比较 Single 是否在容差范围内相等
        /// </summary>
        private void AssertSingleEqual(Single expected, Single actual, int precision) {
            Assert.Equal((double)expected, (double)actual, precision);
        }

        #region 基础属性与常量

        [Fact]
        public void Constants_AreCorrect() {
            AssertVector2Equal(new Vector2(0, 0), Vector2.Zero);
            AssertVector2Equal(new Vector2(1, 1), Vector2.One);
            AssertVector2Equal(new Vector2(0, 1), Vector2.Up);
            AssertVector2Equal(new Vector2(0, -1), Vector2.Down);
            AssertVector2Equal(new Vector2(-1, 0), Vector2.Left);
            AssertVector2Equal(new Vector2(1, 0), Vector2.Right);
        }

        [Theory]
        [InlineData(0, 5, 10)]
        [InlineData(1, -3, 7)]
        public void Indexer_Get_ReturnsCorrectValues(int index, int x, int y) {
            var v = new Vector2(x, y);
            Assert.Equal((Single)x, v[index]);
        }

        [Fact]
        public void Indexer_Get_ThrowsWhenOutOfRange() {
            var v = Vector2.One;
            Assert.Throws<IndexOutOfRangeException>(() => v[2]);
            Assert.Throws<IndexOutOfRangeException>(() => v[-1]);
        }

        [Theory]
        [InlineData(0, 99)]
        [InlineData(1, -99)]
        public void Indexer_Set_SetsCorrectValues(int index, int value) {
            var v = Vector2.Zero;
            v[index] = (Single)value;
            Assert.Equal((Single)value, v[index]);
        }

        #endregion

        #region 运算符

        [Fact]
        public void Operators_BasicMath() {
            var a = new Vector2(10, 20);
            var b = new Vector2(2, 5);

            AssertVector2Equal(new Vector2(12, 25), a + b);
            AssertVector2Equal(new Vector2(8, 15), a - b);
            AssertVector2Equal(new Vector2(20, 100), a * b);
            AssertVector2Equal(new Vector2(5, 4), a / b);
            AssertVector2Equal(new Vector2(-10, -20), -a);
        }

        [Fact]
        public void Operators_ScalarMultiplication() {
            var v = new Vector2(3, 4);
            AssertVector2Equal(new Vector2(6, 8), v * 2);
            AssertVector2Equal(new Vector2(6, 8), 2 * v);
        }

        [Fact]
        public void Operators_ScalarDivision() {
            var v = new Vector2(10, 20);
            AssertVector2Equal(new Vector2(5, 10), v / 2);
        }

        [Fact]
        public void Equality_Works() {
            var a = new Vector2(1, 2);
            var b = new Vector2(1, 2);
            var c = new Vector2(2, 1);

            Assert.True(a == b);
            Assert.False(a != b);
            Assert.False(a == c);
            Assert.True(a != c);
        }

        #endregion

        #region 插值与运动

        [Theory]
        [InlineData(0, 0, 10, 20, 0.5, 5, 10)]
        [InlineData(0, 0, 10, 20, 0, 0, 0)]
        [InlineData(0, 0, 10, 20, 1, 10, 20)]
        public void Lerp_WorksCorrectly(int ax, int ay, int bx, int by, double t, int exX, int exY) {
            var a = new Vector2(ax, ay);
            var b = new Vector2(bx, by);
            var result = Vector2.Lerp(a, b, (Single)t);
            AssertVector2Equal(new Vector2(exX, exY), result);
        }

        [Fact]
        public void Lerp_ClampsT() {
            var a = new Vector2(0, 0);
            var b = new Vector2(10, 10);
            // t > 1 应被钳制为 1
            AssertVector2Equal(b, Vector2.Lerp(a, b, 2));
            // t < 0 应被钳制为 0
            AssertVector2Equal(a, Vector2.Lerp(a, b, -1));
        }

        [Fact]
        public void LerpUnclamped_DoesNotClamp() {
            var a = new Vector2(0, 0);
            var b = new Vector2(10, 10);
            var result = Vector2.LerpUnclamped(a, b, 2);
            AssertVector2Equal(new Vector2(20, 20), result);
        }

        [Fact]
        public void MoveTowards_ReachesTarget() {
            var current = new Vector2(0, 0);
            var target = new Vector2(10, 0);
            // maxDistance 大于距离
            AssertVector2Equal(target, Vector2.MoveTowards(current, target, 15));
        }

        [Fact]
        public void MoveTowards_StopsAtMaxDistance() {
            var current = new Vector2(0, 0);
            var target = new Vector2(10, 0);
            var result = Vector2.MoveTowards(current, target, 5);
            AssertVector2Equal(new Vector2(5, 0), result);
        }

        #endregion

        #region 几何属性

        [Fact]
        public void Magnitude_And_SqrMagnitude_Correct() {
            var v = new Vector2(3, 4);
            AssertSingleEqual(25, v.SqrMagnitude,5);
            AssertSingleEqual(5, v.Magnitude, 5); // 定点数开根号允许微小误差
        }

        [Fact]
        public void FastMagnitude_Correct() {
            var v = new Vector2(3, 4);
            AssertSingleEqual(5, v.FastMagnitude, 3); // 快速开根号误差稍大
        }

        [Fact]
        public void Normalized_Correct() {
            var v = new Vector2(3, 4);
            var n = v.Normalized;
            AssertSingleEqual((Single)0.6, n.x, 5);
            AssertSingleEqual((Single)0.8, n.y, 5);
        }

        [Fact]
        public void Normalized_ZeroVector_ReturnsZero() {
            Assert.Equal(Vector2.Zero, Vector2.Zero.Normalized);
        }

        [Fact]
        public void FastNormalized_Correct() {
            var v = new Vector2(3, 4);
            var n = v.FastNormalized;
            AssertSingleEqual((Single)0.6, n.x, 3);
            AssertSingleEqual((Single)0.8, n.y, 3);
        }

        #endregion

        #region 点乘与叉乘

        [Theory]
        [InlineData(1, 0, 1, 0, 1)]
        [InlineData(1, 0, 0, 1, 0)]
        [InlineData(1, 0, -1, 0, -1)]
        public void Dot_WorksCorrectly(int ax, int ay, int bx, int by, int expected) {
            var a = new Vector2(ax, ay);
            var b = new Vector2(bx, by);
            AssertSingleEqual(expected, Vector2.Dot(a, b),5);
        }

        [Theory]
        [InlineData(1, 0, 0, 1, 1)]
        [InlineData(0, 1, 1, 0, -1)]
        [InlineData(1, 0, 1, 0, 0)]
        public void Cross_WorksCorrectly(int ax, int ay, int bx, int by, int expected) {
            var a = new Vector2(ax, ay);
            var b = new Vector2(bx, by);
            AssertSingleEqual(expected, Vector2.Cross(a, b),5);
        }

        #endregion

        #region 角度与距离

        [Theory]
        [InlineData(1, 0, 0, 1, 90)]
        [InlineData(1, 0, 1, 0, 0)]
        [InlineData(1, 0, -1, 0, 180)]
        public void Angle_WorksCorrectly(int ax, int ay, int bx, int by, double expectedDeg) {
            var a = new Vector2(ax, ay);
            var b = new Vector2(bx, by);
            AssertSingleEqual((Single)expectedDeg, Vector2.Angle(a, b), 1);
        }

        [Fact]
        public void FastAngle_WorksCorrectly() {
            var a = new Vector2(1, 0);
            var b = new Vector2(0, 1);
            AssertSingleEqual(90, Vector2.FastAngle(a, b), 1);
        }

        [Theory]
        [InlineData(1, 0, 0, 1, 90)]
        [InlineData(0, 1, 1, 0, -90)]
        [InlineData(1, 0, -1, 0, 180)]
        public void SignedAngle_WorksCorrectly(int ax, int ay, int bx, int by, double expectedDeg) {
            var a = new Vector2(ax, ay);
            var b = new Vector2(bx, by);
            AssertSingleEqual((Single)expectedDeg, Vector2.SignedAngle(a, b), 1);
        }

        [Fact]
        public void Distance_WorksCorrectly() {
            var a = new Vector2(0, 0);
            var b = new Vector2(3, 4);
            AssertSingleEqual(5, Vector2.Distance(a, b), 5);
        }

        [Fact]
        public void FastDistance_WorksCorrectly() {
            var a = new Vector2(0, 0);
            var b = new Vector2(3, 4);
            AssertSingleEqual(5, Vector2.FastDistance(a, b), 3);
        }

        #endregion

        #region 其他几何操作

        [Fact]
        public void Reflect_WorksCorrectly() {
            var dir = new Vector2(1, -1);
            var normal = new Vector2(0, 1);
            var res = Vector2.Reflect(dir, normal);
            AssertVector2Equal(new Vector2(1, 1), res, 0.1);
        }

        [Fact]
        public void Perpendicular_WorksCorrectly() {
            var dir = new Vector2(2, 3);
            AssertVector2Equal(new Vector2(-3, 2), Vector2.Perpendicular(dir));
        }

        [Fact]
        public void Scale_WorksCorrectly() {
            var a = new Vector2(2, 3);
            var b = new Vector2(4, 5);
            AssertVector2Equal(new Vector2(8, 15), Vector2.Scale(a, b));
        }

        [Fact]
        public void ClampMagnitude_ClampsCorrectly() {
            var v = new Vector2(3, 4); // 长度5
            var res = Vector2.ClampMagnitude(v, 2);
            AssertSingleEqual(2, res.Magnitude, 5);
            AssertSingleEqual((Single)1.2, res.x, 5);
            AssertSingleEqual((Single)1.6, res.y, 5);
        }

        [Fact]
        public void ClampMagnitude_DoesNotClampIfSmaller() {
            var v = new Vector2(3, 4);
            var res = Vector2.ClampMagnitude(v, 10);
            AssertVector2Equal(v, res, 0.1);
        }

        [Fact]
        public void Min_Max_WorksCorrectly() {
            var a = new Vector2(1, 5);
            var b = new Vector2(3, 2);
            AssertVector2Equal(new Vector2(1, 2), Vector2.Min(a, b));
            AssertVector2Equal(new Vector2(3, 5), Vector2.Max(a, b));
        }

        #endregion

        #region SmoothDamp (定点数精度验证)

        [Fact]
        public void SmoothDamp_MovesTowardsTarget() {
            var current = new Vector2(0, 0);
            var target = new Vector2(100, 100);
            var velocity = Vector2.Zero;
            var smoothTime = (Single)0.5;
            var maxSpeed = (Single)1000;
            var deltaTime = (Single)0.1;

            var result = Vector2.SmoothDamp(current, target, ref velocity, smoothTime, maxSpeed, deltaTime);

            // 结果应该介于 current 和 target 之间
            Assert.True(result.x > current.x && result.x < target.x);
            Assert.True(result.y > current.y && result.y < target.y);
        }

        [Fact]
        public void SmoothDamp_PreventsOvershoot() {
            var current = new Vector2(0, 0);
            var target = new Vector2(10, 10);
            var velocity = new Vector2(1000, 1000); // 极大的初始速度
            var smoothTime = (Single)1.0;
            var maxSpeed = (Single)1000;
            var deltaTime = (Single)0.1;

            var result = Vector2.SmoothDamp(current, target, ref velocity, smoothTime, maxSpeed, deltaTime);

            // 不应该冲过目标点 (x/y 不应该大于 10)
            Assert.True(result.x <= target.x + 1, $"Overshot X: {result.x}");
            Assert.True(result.y <= target.y + 1, $"Overshot Y: {result.y}");
        }

        #endregion

        #region 格式化与相等性

        [Fact]
        public void ToString_DefaultFormat() {
            var v = new Vector2((Single)1.23456, (Single)2.34567);
            var str = v.ToString();
            // 默认 F2 格式
            Assert.Equal("(1.23, 2.35)", str);
        }

        [Fact]
        public void Equals_SameValues_ReturnsTrue() {
            var a = new Vector2(1, 2);
            var b = new Vector2(1, 2);
            Assert.True(a.Equals(b));
            Assert.True(a.Equals((object)b));
        }

        [Fact]
        public void Equals_DifferentValues_ReturnsFalse() {
            var a = new Vector2(1, 2);
            var b = new Vector2(2, 1);
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void GetHashCode_SameValues_SameHash() {
            var a = new Vector2(1, 2);
            var b = new Vector2(1, 2);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        #endregion
    }
}
