using System;
using SeaWar.Mathematics;
using Xunit;

namespace SeaWar.Mathematics.Tests.glm {
    public class Vector3Tests {
        // 定点数容差：针对普通运算
        private const double DoubleTolerance = 0.01;
        // 角度容差：定点数三角函数误差较大，放宽至 1 度
        private const double AngleTolerance = 1.0;

        /// <summary>
        /// 辅助方法：比较两个 Vector3 是否在容差范围内相等（转为 double 比较）
        /// </summary>
        private void AssertVector3Equal(Vector3 expected, Vector3 actual, double tolerance = DoubleTolerance) {
            Assert.Equal((double)expected.x, (double)actual.x, 3);
            Assert.Equal((double)expected.y, (double)actual.y, 3);
            Assert.Equal((double)expected.z, (double)actual.z, 3);
        }

        /// <summary>
        /// 辅助方法：比较 Single 是否在容差范围内相等
        /// </summary>
        private void AssertSingleEqual(Single expected, Single actual, double tolerance = DoubleTolerance) {
            Assert.Equal((double)expected, (double)actual, 3);
        }

        #region 基础属性与常量
        [Fact]
        public void Constants_AreCorrect() {
            AssertVector3Equal(new Vector3(0, 0, 0), Vector3.Zero);
            AssertVector3Equal(new Vector3(1, 1, 1), Vector3.One);
            AssertVector3Equal(new Vector3(0, 1, 0), Vector3.Up);
            AssertVector3Equal(new Vector3(0, -1, 0), Vector3.Down);
            AssertVector3Equal(new Vector3(-1, 0, 0), Vector3.Left);
            AssertVector3Equal(new Vector3(1, 0, 0), Vector3.Right);
            AssertVector3Equal(new Vector3(0, 0, 1), Vector3.Forward);
            AssertVector3Equal(new Vector3(0, 0, -1), Vector3.Back);
        }

        [Theory]
        [InlineData(0, 5, 10, 15)]
        [InlineData(1, 5, 10, 15)]
        [InlineData(2, 5, 10, 15)]
        public void Indexer_Get_ReturnsCorrectValues(int index, int x, int y, int z) {
            var v = new Vector3(x, y, z);
            var expected = index == 0 ? (Single)x : (index == 1 ? (Single)y : (Single)z);
            Assert.Equal(expected, v[index]);
        }

        [Fact]
        public void Indexer_Get_ThrowsWhenOutOfRange() {
            var v = Vector3.One;
            Assert.Throws<IndexOutOfRangeException>(() => v[3]);
            Assert.Throws<IndexOutOfRangeException>(() => v[-1]);
        }

        [Theory]
        [InlineData(0, 99)]
        [InlineData(1, -99)]
        [InlineData(2, 100)]
        public void Indexer_Set_SetsCorrectValues(int index, int value) {
            var v = Vector3.Zero;
            v[index] = (Single)value;
            Assert.Equal((Single)value, v[index]);
        }
        #endregion

        #region 运算符
        [Fact]
        public void Operators_BasicMath() {
            var a = new Vector3(10, 20, 30);
            var b = new Vector3(2, 5, 6);

            AssertVector3Equal(new Vector3(12, 25, 36), a + b);
            AssertVector3Equal(new Vector3(8, 15, 24), a - b);
            AssertVector3Equal(new Vector3(20, 100, 180), a * b);
            AssertVector3Equal(new Vector3(5, 4, 5), a / b);
            AssertVector3Equal(new Vector3(-10, -20, -30), -a);
        }

        [Fact]
        public void Operators_ScalarMultiplication() {
            var v = new Vector3(3, 4, 5);
            AssertVector3Equal(new Vector3(6, 8, 10), v * 2);
            AssertVector3Equal(new Vector3(6, 8, 10), 2 * v);
        }

        [Fact]
        public void Operators_ScalarDivision() {
            var v = new Vector3(10, 20, 30);
            AssertVector3Equal(new Vector3(5, 10, 15), v / 2);
        }

        [Fact]
        public void Equality_Works() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(1, 2, 3);
            var c = new Vector3(3, 2, 1);

            Assert.True(a == b);
            Assert.False(a != b);
            Assert.False(a == c);
            Assert.True(a != c);
        }
        #endregion

        #region 插值与运动
        [Theory]
        [InlineData(0, 0, 0, 10, 20, 30, 0.5, 5, 10, 15)]
        [InlineData(0, 0, 0, 10, 20, 30, 0, 0, 0, 0)]
        [InlineData(0, 0, 0, 10, 20, 30, 1, 10, 20, 30)]
        public void Lerp_WorksCorrectly(int ax, int ay, int az, int bx, int by, int bz, double t, int exX, int exY, int exZ) {
            var a = new Vector3(ax, ay, az);
            var b = new Vector3(bx, by, bz);
            var result = Vector3.Lerp(a, b, (Single)t);
            AssertVector3Equal(new Vector3(exX, exY, exZ), result);
        }

        [Fact]
        public void Lerp_ClampsT() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 10, 10);
            AssertVector3Equal(b, Vector3.Lerp(a, b, 2));
            AssertVector3Equal(a, Vector3.Lerp(a, b, -1));
        }

        [Fact]
        public void LerpUnclamped_DoesNotClamp() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 10, 10);
            AssertVector3Equal(new Vector3(20, 20, 20), Vector3.LerpUnclamped(a, b, 2));
        }

        [Fact]
        public void MoveTowards_ReachesTarget() {
            var current = new Vector3(0, 0, 0);
            var target = new Vector3(10, 0, 0);
            AssertVector3Equal(target, Vector3.MoveTowards(current, target, 15));
        }

        [Fact]
        public void MoveTowards_StopsAtMaxDistance() {
            var current = new Vector3(0, 0, 0);
            var target = new Vector3(10, 0, 0);
            AssertVector3Equal(new Vector3(5, 0, 0), Vector3.MoveTowards(current, target, 5));
        }
        #endregion

        #region 几何属性
        [Fact]
        public void Magnitude_And_SqrMagnitude_Correct() {
            var v = new Vector3(3, 4, 0);
            AssertSingleEqual(25, v.SqrMagnitude);
            AssertSingleEqual(5, v.Magnitude, 0.1);
        }

        [Fact]
        public void FastMagnitude_Correct() {
            var v = new Vector3(3, 4, 0);
            AssertSingleEqual(5, v.FastMagnitude, 0.2);
        }

        [Fact]
        public void Normalized_Correct() {
            var v = new Vector3(3, 4, 0);
            var n = v.Normalized;
            AssertSingleEqual((Single)0.6, n.x, 0.01);
            AssertSingleEqual((Single)0.8, n.y, 0.01);
            AssertSingleEqual(0, n.z, 0.01);
        }

        [Fact]
        public void Normalized_ZeroVector_ReturnsZero() {
            Assert.Equal(Vector3.Zero, Vector3.Zero.Normalized);
        }

        [Fact]
        public void FastNormalized_Correct() {
            var v = new Vector3(3, 4, 0);
            var n = v.FastNormalized;
            AssertSingleEqual((Single)0.6, n.x, 0.05);
            AssertSingleEqual((Single)0.8, n.y, 0.05);
        }
        #endregion

        #region 点乘与叉乘
        [Theory]
        [InlineData(1, 0, 0, 1, 0, 0, 1)]
        [InlineData(1, 0, 0, 0, 1, 0, 0)]
        [InlineData(1, 0, 0, -1, 0, 0, -1)]
        public void Dot_WorksCorrectly(int ax, int ay, int az, int bx, int by, int bz, int expected) {
            var a = new Vector3(ax, ay, az);
            var b = new Vector3(bx, by, bz);
            AssertSingleEqual(expected, Vector3.Dot(a, b));
        }

        [Fact]
        public void Cross_WorksCorrectly() {
            // X x Y = Z
            var x = new Vector3(1, 0, 0);
            var y = new Vector3(0, 1, 0);
            AssertVector3Equal(new Vector3(0, 0, 1), Vector3.Cross(x, y));

            // Y x X = -Z
            AssertVector3Equal(new Vector3(0, 0, -1), Vector3.Cross(y, x));

            // 自身叉乘为 0
            AssertVector3Equal(Vector3.Zero, Vector3.Cross(x, x));
        }
        #endregion

        #region 角度与距离
        [Theory]
        [InlineData(1, 0, 0, 0, 1, 0, 90)]
        [InlineData(1, 0, 0, 1, 0, 0, 0)]
        [InlineData(1, 0, 0, -1, 0, 0, 180)]
        public void Angle_WorksCorrectly(int ax, int ay, int az, int bx, int by, int bz, double expectedDeg) {
            var a = new Vector3(ax, ay, az);
            var b = new Vector3(bx, by, bz);
            AssertSingleEqual((Single)expectedDeg, Vector3.Angle(a, b), AngleTolerance);
        }

        [Fact]
        public void FastAngle_WorksCorrectly() {
            var a = new Vector3(1, 0, 0);
            var b = new Vector3(0, 1, 0);
            AssertSingleEqual(90, Vector3.FastAngle(a, b), AngleTolerance);
        }

        [Fact]
        public void Distance_WorksCorrectly() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(3, 4, 0);
            AssertSingleEqual(5, Vector3.Distance(a, b), 0.1);
        }

        [Fact]
        public void FastDistance_WorksCorrectly() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(3, 4, 0);
            AssertSingleEqual(5, Vector3.FastDistance(a, b), 0.2);
        }
        #endregion

        #region 其他几何操作
        [Fact]
        public void Reflect_WorksCorrectly() {
            var dir = new Vector3(1, -1, 0);
            var normal = new Vector3(0, 1, 0);
            var res = Vector3.Reflect(dir, normal);
            AssertVector3Equal(new Vector3(1, 1, 0), res, 0.1);
        }

        [Fact]
        public void Scale_WorksCorrectly() {
            var a = new Vector3(2, 3, 4);
            var b = new Vector3(5, 6, 7);
            AssertVector3Equal(new Vector3(10, 18, 28), Vector3.Scale(a, b));
        }

        [Fact]
        public void ClampMagnitude_ClampsCorrectly() {
            var v = new Vector3(3, 4, 0); // 长度5
            var res = Vector3.ClampMagnitude(v, 2);
            AssertSingleEqual(2, res.Magnitude, 0.1);
            AssertSingleEqual((Single)1.2, res.x, 0.1);
            AssertSingleEqual((Single)1.6, res.y, 0.1);
        }

        [Fact]
        public void ClampMagnitude_DoesNotClampIfSmaller() {
            var v = new Vector3(3, 4, 0);
            var res = Vector3.ClampMagnitude(v, 10);
            AssertVector3Equal(v, res, 0.1);
        }

        [Fact]
        public void Min_Max_WorksCorrectly() {
            var a = new Vector3(1, 5, 3);
            var b = new Vector3(3, 2, 4);
            AssertVector3Equal(new Vector3(1, 2, 3), Vector3.Min(a, b));
            AssertVector3Equal(new Vector3(3, 5, 4), Vector3.Max(a, b));
        }
        #endregion

        #region SmoothDamp
        [Fact]
        public void SmoothDamp_MovesTowardsTarget() {
            var current = new Vector3(0, 0, 0);
            var target = new Vector3(100, 100, 100);
            var velocity = Vector3.Zero;
            var smoothTime = (Single)0.5;
            var maxSpeed = (Single)1000;
            var deltaTime = (Single)0.1;

            var result = Vector3.SmoothDamp(current, target, ref velocity, smoothTime, maxSpeed, deltaTime);

            Assert.True(result.x > current.x && result.x < target.x);
            Assert.True(result.y > current.y && result.y < target.y);
            Assert.True(result.z > current.z && result.z < target.z);
        }

        [Fact]
        public void SmoothDamp_PreventsOvershoot() {
            var current = new Vector3(0, 0, 0);
            var target = new Vector3(10, 10, 10);
            var velocity = new Vector3(1000, 1000, 1000); // 极大的初始速度
            var smoothTime = (Single)1.0;
            var maxSpeed = (Single)1000;
            var deltaTime = (Single)0.1;

            var result = Vector3.SmoothDamp(current, target, ref velocity, smoothTime, maxSpeed, deltaTime);

            // 不应该冲过目标点
            Assert.True(result.x <= target.x + 1, $"Overshot X: {result.x}");
            Assert.True(result.y <= target.y + 1, $"Overshot Y: {result.y}");
            Assert.True(result.z <= target.z + 1, $"Overshot Z: {result.z}");
        }
        #endregion

        #region 格式化与相等性
        [Fact]
        public void ToString_DefaultFormat() {
            var v = new Vector3((Single)1.23456, (Single)2.34567, (Single)3.45678);
            var str = v.ToString();
            // 假设默认 F2 格式
            Assert.Equal("(1.23, 2.35, 3.46)", str);
        }

        [Fact]
        public void Equals_SameValues_ReturnsTrue() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(1, 2, 3);
            Assert.True(a.Equals(b));
            Assert.True(a.Equals((object)b));
        }

        [Fact]
        public void Equals_DifferentValues_ReturnsFalse() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 2, 1);
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void GetHashCode_SameValues_SameHash() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(1, 2, 3);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
        #endregion
    }
}
