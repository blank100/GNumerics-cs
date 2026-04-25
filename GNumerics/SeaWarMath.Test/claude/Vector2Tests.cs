using Xunit;
using SeaWar.Mathematics;
using System;

namespace SeaWar.Mathematics.Tests.claude {
    public class Vector2Tests {
        private const double EPSILON = 0.0001;

        #region 构造函数和索引器测试

        [Fact]
        public void Constructor_应正确初始化分量() {
            var v = new Vector2(3, 4);
            Assert.Equal((Single)3, v.x);
            Assert.Equal((Single)4, v.y);
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(1, 4)]
        public void Indexer_Get_应返回正确分量(int index, double expected) {
            var v = new Vector2(3, 4);
            Assert.Equal((Single)expected, v[index]);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 20)]
        public void Indexer_Set_应正确设置分量(int index, double value) {
            var v = new Vector2(0, 0);
            v[index] = (Single)value;
            Assert.Equal((Single)value, v[index]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        [InlineData(100)]
        public void Indexer_Get_无效索引_应抛出异常(int index) {
            var v = new Vector2(1, 2);
            Assert.Throws<IndexOutOfRangeException>(() => v[index]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Indexer_Set_无效索引_应抛出异常(int index) {
            var v = new Vector2(1, 2);
            Assert.Throws<IndexOutOfRangeException>(() => v[index] = 0);
        }

        #endregion

        #region 静态常量测试

        [Fact]
        public void StaticConstants_应具有正确值() {
            Assert.Equal(new Vector2(0, 0), Vector2.Zero);
            Assert.Equal(new Vector2(1, 1), Vector2.One);
            Assert.Equal(new Vector2(0, 1), Vector2.Up);
            Assert.Equal(new Vector2(0, -1), Vector2.Down);
            Assert.Equal(new Vector2(-1, 0), Vector2.Left);
            Assert.Equal(new Vector2(1, 0), Vector2.Right);
        }

        #endregion

        #region 插值测试

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0.5, 5, 5)]
        [InlineData(1, 10, 10)]
        public void Lerp_应正确插值(double t, double expectedX, double expectedY) {
            var a = new Vector2(0, 0);
            var b = new Vector2(10, 10);
            var result = Vector2.Lerp(a, b, (Single)t);

            AssertVector2Equal(new Vector2((Single)expectedX, (Single)expectedY), result);
        }

        [Theory]
        [InlineData(-0.5, -5, -5)]
        [InlineData(1.5, 15, 15)]
        public void Lerp_超出01范围_应钳制(double t, double notExpectedX, double notExpectedY) {
            var a = new Vector2(0, 0);
            var b = new Vector2(10, 10);
            var result = Vector2.Lerp(a, b, (Single)t);

            // Lerp 会钳制到 [0, 1]
            if (t < 0)
                AssertVector2Equal(a, result);
            else if (t > 1)
                AssertVector2Equal(b, result);
        }

        [Theory]
        [InlineData(-0.5, -5, -5)]
        [InlineData(1.5, 15, 15)]
        public void LerpUnclamped_超出01范围_应外推(double t, double expectedX, double expectedY) {
            var a = new Vector2(0, 0);
            var b = new Vector2(10, 10);
            var result = Vector2.LerpUnclamped(a, b, (Single)t);

            AssertVector2Equal(new Vector2((Single)expectedX, (Single)expectedY), result);
        }

        #endregion

        #region MoveTowards测试

        [Fact]
        public void MoveTowards_距离小于maxDelta_应返回目标() {
            var current = new Vector2(0, 0);
            var target = new Vector2(3, 4); // 距离 = 5
            var result = Vector2.MoveTowards(current, target, 10);

            AssertVector2Equal(target, result);
        }

        [Fact]
        public void MoveTowards_距离大于maxDelta_应移动指定距离() {
            var current = new Vector2(0, 0);
            var target = new Vector2(3, 4); // 距离 = 5
            var result = Vector2.MoveTowards(current, target, (Single)2.5);

            // 应移动到一半位置
            AssertVector2Equal(new Vector2((Single)1.5, (Single)2), result);
        }

        [Fact]
        public void MoveTowards_当前等于目标_应返回目标() {
            var current = new Vector2(1, 2);
            var result = Vector2.MoveTowards(current, current, 5);

            AssertVector2Equal(current, result);
        }

        #endregion

        #region Scale测试

        [Fact]
        public void Scale_应逐分量相乘() {
            var a = new Vector2(2, 3);
            var b = new Vector2(4, 5);
            var result = Vector2.Scale(a, b);

            AssertVector2Equal(new Vector2(8, 15), result);
        }

        #endregion

        #region 归一化测试

        [Fact]
        public void Normalized_应返回单位向量() {
            var v = new Vector2(3, 4);
            var result = v.Normalized;

            Assert.True(System.Math.Abs((double)result.Magnitude - 1.0) < EPSILON);
        }

        [Fact]
        public void Normalized_零向量_应返回零向量() {
            var v = Vector2.Zero;
            var result = v.Normalized;

            AssertVector2Equal(Vector2.Zero, result);
        }

        [Fact]
        public void FastNormalized_应返回近似单位向量() {
            var v = new Vector2(3, 4);
            var result = v.FastNormalized;

            // FastNormalized 精度略低
            Assert.True(System.Math.Abs((double)result.Magnitude - 1.0) < 0.01);
        }

        [Fact]
        public void GetNormalizedAndLength_应返回正确值() {
            var v = new Vector2(3, 4);
            var result = v.GetNormalizedAndLength(out var length);

            Assert.Equal((Single)5, length);
            Assert.True(System.Math.Abs((double)result.Magnitude - 1.0) < EPSILON);
        }

        #endregion

        #region 点积和叉积测试

        [Fact]
        public void Dot_应返回正确值() {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            var result = Vector2.Dot(a, b);

            Assert.Equal((Single)11, result); // 1*3 + 2*4 = 11
        }

        [Fact]
        public void Dot_垂直向量_应返回零() {
            var a = new Vector2(1, 0);
            var b = new Vector2(0, 1);
            var result = Vector2.Dot(a, b);

            Assert.True(System.Math.Abs((double)result) < EPSILON);
        }

        [Fact]
        public void Cross_应返回标量叉积() {
            var a = new Vector2(1, 0);
            var b = new Vector2(0, 1);
            var result = Vector2.Cross(a, b);

            Assert.Equal((Single)1, result);
        }

        [Fact]
        public void Cross_平行向量_应返回零() {
            var a = new Vector2(2, 4);
            var b = new Vector2(1, 2);
            var result = Vector2.Cross(a, b);

            Assert.True(System.Math.Abs((double)result) < EPSILON);
        }

        #endregion

        #region 反射和垂直向量测试

        [Fact]
        public void Reflect_应正确反射() {
            var inDirection = new Vector2(1, -1).Normalized;
            var inNormal = new Vector2(0, 1);
            var result = Vector2.Reflect(inDirection, inNormal);

            // 应反射为 (1, 1) 方向
            var expected = new Vector2(1, 1).Normalized;
            AssertVector2Equal(expected, result, 0.01);
        }

        [Fact]
        public void Perpendicular_应返回垂直向量() {
            var v = new Vector2(3, 4);
            var result = Vector2.Perpendicular(v);

            // 应为 (-4, 3)
            AssertVector2Equal(new Vector2(-4, 3), result);

            // 应垂直
            Assert.True(System.Math.Abs((double)Vector2.Dot(v, result)) < EPSILON);
        }

        #endregion

        #region 大小测试

        [Fact]
        public void Magnitude_应返回正确长度() {
            var v = new Vector2(3, 4);
            Assert.Equal((Single)5, v.Magnitude);
        }

        [Fact]
        public void SqrMagnitude_应返回长度平方() {
            var v = new Vector2(3, 4);
            Assert.Equal((Single)25, v.SqrMagnitude);
        }

        [Fact]
        public void FastMagnitude_应返回近似长度() {
            var v = new Vector2(3, 4);
            var result = v.FastMagnitude;

            Assert.True(System.Math.Abs((double)result - 5.0) < 0.1);
        }

        #endregion

        #region 角度测试

        [Theory]
        [InlineData(1, 0, 0, 1, 90)]
        [InlineData(1, 0, 1, 0, 0)]
        [InlineData(1, 0, -1, 0, 180)]
        public void Angle_应返回正确角度(double x1, double y1, double x2, double y2, double expectedAngle) {
            var from = new Vector2((Single)x1, (Single)y1);
            var to = new Vector2((Single)x2, (Single)y2);
            var angle = Vector2.Angle(from, to);

            Assert.True(System.Math.Abs((double)angle - expectedAngle) < 1.0);
        }

        [Fact]
        public void Angle_零向量_应返回零() {
            var result = Vector2.Angle(Vector2.Zero, Vector2.Right);
            Assert.Equal((Single)0, result);
        }

        [Theory]
        [InlineData(1, 0, 0, 1, 90)]
        [InlineData(1, 0, 0, -1, -90)]
        public void SignedAngle_应返回带符号角度(double x1, double y1, double x2, double y2, double expectedAngle) {
            var from = new Vector2((Single)x1, (Single)y1);
            var to = new Vector2((Single)x2, (Single)y2);
            var angle = Vector2.SignedAngle(from, to);

            Assert.True(System.Math.Abs((double)angle - expectedAngle) < 1.0);
        }

        /// <summary>
        /// 辅助方法：比较 Single 是否在容差范围内相等
        /// </summary>
        private void AssertSingleEqual(Single expected, Single actual, int precision) {
            Assert.Equal((double)expected, (double)actual, precision);
        }

        [Theory]
        [InlineData(1, 0, 0, 1, 90)] // Right -> Up: +90°
        [InlineData(0, 1, 1, 0, -90)] // Up -> Right: -90°
        [InlineData(1, 0, -1, 0, 180)] // Right -> Left: 180° (或 -180°)
        [InlineData(1, 0, 1, 0, 0)] // Right -> Right: 0°
        [InlineData(0, 1, 0, -1, 180)] // Up -> Down: 180°
        public void SignedAngle_WorksCorrectly(int ax, int ay, int bx, int by, double expectedDeg) {
            var a = new Vector2(ax, ay);
            var b = new Vector2(bx, by);
            var result = Vector2.SignedAngle(a, b);

            // 对于 180° 的情况，可能是 180 或 -180
            if (Math.Abs(expectedDeg) == 180) {
                Assert.True(
                    Math.Abs((double)result - 180) < 1 ||
                    Math.Abs((double)result + 180) < 1,
                    $"Expected ±180°, got {result}°"
                );
            } else {
                AssertSingleEqual((Single)expectedDeg, result, 3);
            }
        }

        [Fact]
        public void SignedAngle_逆时针旋转_应返回正角度() {
            var from = new Vector2(1, 0);
            var to = new Vector2(0, 1);
            var angle = Vector2.SignedAngle(from, to);

            Assert.True((double)angle > 0);
            AssertSingleEqual((Single)90, angle, 3);
        }

        [Fact]
        public void SignedAngle_顺时针旋转_应返回负角度() {
            var from = new Vector2(0, 1);
            var to = new Vector2(1, 0);
            var angle = Vector2.SignedAngle(from, to);

            Assert.True((double)angle < 0);
            AssertSingleEqual((Single)(-90), angle, 3);
        }

        [Fact]
        public void SignedAngle_同向向量_应返回零() {
            var from = new Vector2(1, 1);
            var to = new Vector2(2, 2);
            var angle = Vector2.SignedAngle(from, to);

            AssertSingleEqual((Single)0, angle, 3);
        }

        [Fact]
        public void SignedAngle_反向向量_应返回180度() {
            var from = new Vector2(1, 0);
            var to = new Vector2(-1, 0);
            var angle = Vector2.SignedAngle(from, to);

            // 可能是 180 或 -180
            Assert.True(
                Math.Abs((double)angle - 180) < 1 ||
                Math.Abs((double)angle + 180) < 1
            );
        }

        [Fact]
        public void SignedAngle_零向量_应返回零() {
            var from = Vector2.Zero;
            var to = Vector2.Right;
            var angle = Vector2.SignedAngle(from, to);

            Assert.Equal((Single)0, angle);
        }

        #endregion

        #region 距离测试

        [Fact]
        public void Distance_应返回正确距离() {
            var a = new Vector2(0, 0);
            var b = new Vector2(3, 4);
            var result = Vector2.Distance(a, b);

            Assert.Equal((Single)5, result);
        }

        [Fact]
        public void FastDistance_应返回近似距离() {
            var a = new Vector2(0, 0);
            var b = new Vector2(3, 4);
            var result = Vector2.FastDistance(a, b);

            Assert.True(System.Math.Abs((double)result - 5.0) < 0.1);
        }

        #endregion

        #region ClampMagnitude测试

        [Fact]
        public void ClampMagnitude_长度小于最大值_应保持不变() {
            var v = new Vector2(3, 4); // 长度 = 5
            var result = Vector2.ClampMagnitude(v, 10);

            AssertVector2Equal(v, result);
        }

        [Fact]
        public void ClampMagnitude_长度大于最大值_应截断() {
            var v = new Vector2(3, 4); // 长度 = 5
            var result = Vector2.ClampMagnitude(v, (Single)2.5);

            Assert.True(System.Math.Abs((double)result.Magnitude - 2.5) < EPSILON);
            // 方向应保持
            AssertVector2Equal(v.Normalized, result.Normalized, 0.01);
        }

        #endregion

        #region Min/Max测试

        [Fact]
        public void Min_应返回各分量最小值() {
            var a = new Vector2(1, 5);
            var b = new Vector2(3, 2);
            var result = Vector2.Min(a, b);

            AssertVector2Equal(new Vector2(1, 2), result);
        }

        [Fact]
        public void Max_应返回各分量最大值() {
            var a = new Vector2(1, 5);
            var b = new Vector2(3, 2);
            var result = Vector2.Max(a, b);

            AssertVector2Equal(new Vector2(3, 5), result);
        }

        #endregion

        #region SmoothDamp测试

        [Fact]
        public void SmoothDamp_应平滑移动向目标() {
            var current = new Vector2(0, 0);
            var target = new Vector2(10, 10);
            var velocity = Vector2.Zero;

            var result = Vector2.SmoothDamp(current, target, ref velocity, 1, 100, (Single)0.1);

            // 应该移动了，但未到达目标
            Assert.True(result.x > 0 && result.x < 10);
            Assert.True(result.y > 0 && result.y < 10);

            // velocity 应该被更新
            Assert.True(velocity.SqrMagnitude > 0);
        }

        [Fact]
        public void SmoothDamp_已到达目标_应保持在目标() {
            var current = new Vector2(10, 10);
            var target = new Vector2(10, 10);
            var velocity = Vector2.Zero;

            var result = Vector2.SmoothDamp(current, target, ref velocity, 1, 100, (Single)0.1);

            AssertVector2Equal(target, result);
        }

        #endregion

        #region 运算符测试

        [Fact]
        public void Operator_Add_应正确相加() {
            var a = new Vector2(1, 2);
            var b = new Vector2(3, 4);
            var result = a + b;

            AssertVector2Equal(new Vector2(4, 6), result);
        }

        [Fact]
        public void Operator_Subtract_应正确相减() {
            var a = new Vector2(5, 7);
            var b = new Vector2(2, 3);
            var result = a - b;

            AssertVector2Equal(new Vector2(3, 4), result);
        }

        [Fact]
        public void Operator_Multiply_Vector_应逐分量相乘() {
            var a = new Vector2(2, 3);
            var b = new Vector2(4, 5);
            var result = a * b;

            AssertVector2Equal(new Vector2(8, 15), result);
        }

        [Fact]
        public void Operator_Multiply_Scalar_应正确缩放() {
            var v = new Vector2(2, 3);
            var result = v * 2;

            AssertVector2Equal(new Vector2(4, 6), result);
        }

        [Fact]
        public void Operator_Multiply_Scalar_Left_应正确缩放() {
            var v = new Vector2(2, 3);
            var result = (Single)2 * v;

            AssertVector2Equal(new Vector2(4, 6), result);
        }

        [Fact]
        public void Operator_Divide_Vector_应逐分量相除() {
            var a = new Vector2(8, 15);
            var b = new Vector2(2, 3);
            var result = a / b;

            AssertVector2Equal(new Vector2(4, 5), result);
        }

        [Fact]
        public void Operator_Divide_Scalar_应正确缩放() {
            var v = new Vector2(4, 6);
            var result = v / 2;

            AssertVector2Equal(new Vector2(2, 3), result);
        }

        [Fact]
        public void Operator_Negate_应取反() {
            var v = new Vector2(2, -3);
            var result = -v;

            AssertVector2Equal(new Vector2(-2, 3), result);
        }

        [Fact]
        public void Operator_Equality_相等向量_应返回true() {
            var a = new Vector2(1, 2);
            var b = new Vector2(1, 2);

            Assert.True(a == b);
        }

        [Fact]
        public void Operator_Equality_不等向量_应返回false() {
            var a = new Vector2(1, 2);
            var b = new Vector2(2, 1);

            Assert.False(a == b);
        }

        [Fact]
        public void Operator_Inequality_应正确工作() {
            var a = new Vector2(1, 2);
            var b = new Vector2(2, 1);

            Assert.True(a != b);
        }

        #endregion

        #region Equals和GetHashCode测试

        [Fact]
        public void Equals_相等向量_应返回true() {
            var a = new Vector2(1, 2);
            var b = new Vector2(1, 2);

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_Object_相等向量_应返回true() {
            var a = new Vector2(1, 2);
            object b = new Vector2(1, 2);

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_Object_非Vector2_应返回false() {
            var a = new Vector2(1, 2);
            object b = "not a vector";

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void GetHashCode_相等向量_应返回相同哈希() {
            var a = new Vector2(1, 2);
            var b = new Vector2(1, 2);

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        #endregion

        #region ToString测试

        [Fact]
        public void ToString_默认格式_应正确格式化() {
            var v = new Vector2((Single)1.234, (Single)5.678);
            var result = v.ToString();

            Assert.Contains("1.23", result);
            Assert.Contains("5.68", result);
        }

        [Fact]
        public void ToString_指定格式_应使用指定格式() {
            var v = new Vector2((Single)1.234, (Single)5.678);
            var result = v.ToString("F1");

            Assert.Contains("1.2", result);
            Assert.Contains("5.7", result);
        }

        #endregion

        #region 边界和特殊情况测试

        [Fact]
        public void 零向量运算_应正确处理() {
            var zero = Vector2.Zero;
            var v = new Vector2(1, 2);

            AssertVector2Equal(v, zero + v);
            AssertVector2Equal(-v, zero - v);
            AssertVector2Equal(Vector2.Zero, zero * v);
        }

        [Fact]
        public void 极大值_应不溢出() {
            var large = new Vector2(Single.MaxValue / 2, Single.MaxValue / 2);
            // 不应抛出异常
            var _ = large + large;
        }

        #endregion

        #region 辅助方法

        private void AssertVector2Equal(Vector2 expected, Vector2 actual, double epsilon = EPSILON) {
            Assert.True(
                System.Math.Abs((double)(expected.x - actual.x)) < epsilon &&
                System.Math.Abs((double)(expected.y - actual.y)) < epsilon,
                $"Expected {expected}, but got {actual}"
            );
        }

        #endregion
    }
}
