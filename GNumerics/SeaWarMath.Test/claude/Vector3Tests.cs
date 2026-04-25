using Xunit;
using SeaWar.Mathematics;
using System;

namespace SeaWar.Mathematics.Tests.claude {
    public class Vector3Tests {
        private const double EPSILON = 0.0001;
        private const double LOOSE_EPSILON = 0.01;

        #region 构造函数和索引器测试

        [Fact]
        public void Constructor_应正确初始化分量() {
            var v = new Vector3(3, 4, 5);
            Assert.Equal((Single)3, v.x);
            Assert.Equal((Single)4, v.y);
            Assert.Equal((Single)5, v.z);
        }

        [Theory]
        [InlineData(3, 4, 5)]
        [InlineData(-1, 0, 1)]
        [InlineData(0, 0, 0)]
        public void Indexer_Get_Index0_应返回X分量(int x, int y, int z) {
            var v = new Vector3(x, y, z);
            Assert.Equal((Single)x, v[0]);
        }

        [Theory]
        [InlineData(3, 4, 5)]
        [InlineData(-1, 0, 1)]
        public void Indexer_Get_Index1_应返回Y分量(int x, int y, int z) {
            var v = new Vector3(x, y, z);
            Assert.Equal((Single)y, v[1]);
        }

        [Theory]
        [InlineData(3, 4, 5)]
        [InlineData(-1, 0, 1)]
        public void Indexer_Get_Index2_应返回Z分量(int x, int y, int z) {
            var v = new Vector3(x, y, z);
            Assert.Equal((Single)z, v[2]);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 20)]
        [InlineData(2, 30)]
        public void Indexer_Set_应正确设置分量(int index, int value) {
            var v = new Vector3(0, 0, 0);
            v[index] = (Single)value;
            Assert.Equal((Single)value, v[index]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        [InlineData(100)]
        public void Indexer_Get_无效索引_应抛出异常(int index) {
            var v = new Vector3(1, 2, 3);
            Assert.Throws<IndexOutOfRangeException>(() => v[index]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public void Indexer_Set_无效索引_应抛出异常(int index) {
            var v = new Vector3(1, 2, 3);
            Assert.Throws<IndexOutOfRangeException>(() => v[index] = 0);
        }

        #endregion

        #region 静态常量测试

        [Fact]
        public void StaticConstants_应具有正确值() {
            Assert.Equal(new Vector3(0, 0, 0), Vector3.Zero);
            Assert.Equal(new Vector3(1, 1, 1), Vector3.One);
            Assert.Equal(new Vector3(0, 1, 0), Vector3.Up);
            Assert.Equal(new Vector3(0, -1, 0), Vector3.Down);
            Assert.Equal(new Vector3(-1, 0, 0), Vector3.Left);
            Assert.Equal(new Vector3(1, 0, 0), Vector3.Right);
            Assert.Equal(new Vector3(0, 0, 1), Vector3.Forward);
            Assert.Equal(new Vector3(0, 0, -1), Vector3.Back);
        }

        #endregion

        #region 插值测试

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(0.5, 5, 5, 5)]
        [InlineData(1, 10, 10, 10)]
        public void Lerp_应正确插值(double t, double expectedX, double expectedY, double expectedZ) {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 10, 10);
            var result = Vector3.Lerp(a, b, (Single)t);

            AssertVector3Equal(new Vector3((Single)expectedX, (Single)expectedY, (Single)expectedZ), result);
        }

        [Theory]
        [InlineData(-0.5)]
        [InlineData(1.5)]
        public void Lerp_超出01范围_应钳制(double t) {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 10, 10);
            var result = Vector3.Lerp(a, b, (Single)t);

            if (t < 0)
                AssertVector3Equal(a, result);
            else if (t > 1)
                AssertVector3Equal(b, result);
        }

        [Theory]
        [InlineData(-0.5, -5, -5, -5)]
        [InlineData(1.5, 15, 15, 15)]
        public void LerpUnclamped_超出01范围_应外推(double t, double expectedX, double expectedY, double expectedZ) {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 10, 10);
            var result = Vector3.LerpUnclamped(a, b, (Single)t);

            AssertVector3Equal(new Vector3((Single)expectedX, (Single)expectedY, (Single)expectedZ), result);
        }

        #endregion

        #region MoveTowards测试

        [Fact]
        public void MoveTowards_距离小于maxDelta_应返回目标() {
            var current = new Vector3(0, 0, 0);
            var target = new Vector3(3, 4, 0); // 距离 = 5
            var result = Vector3.MoveTowards(current, target, 10);

            AssertVector3Equal(target, result);
        }

        [Fact]
        public void MoveTowards_距离大于maxDelta_应移动指定距离() {
            var current = new Vector3(0, 0, 0);
            var target = new Vector3(3, 4, 0); // 距离 = 5
            var result = Vector3.MoveTowards(current, target, (Single)2.5);

            // 应移动到一半位置
            AssertVector3Equal(new Vector3((Single)1.5, (Single)2, 0), result, LOOSE_EPSILON);
        }

        [Fact]
        public void MoveTowards_当前等于目标_应返回目标() {
            var current = new Vector3(1, 2, 3);
            var result = Vector3.MoveTowards(current, current, 5);

            AssertVector3Equal(current, result);
        }

        #endregion

        #region Scale测试

        [Fact]
        public void Scale_应逐分量相乘() {
            var a = new Vector3(2, 3, 4);
            var b = new Vector3(5, 6, 7);
            var result = Vector3.Scale(a, b);

            AssertVector3Equal(new Vector3(10, 18, 28), result);
        }

        #endregion

        #region 归一化测试

        [Fact]
        public void Normalized_应返回单位向量() {
            var v = new Vector3(3, 4, 0);
            var result = v.Normalized;

            Assert.True(System.Math.Abs((double)result.Magnitude - 1.0) < EPSILON);
        }

        [Fact]
        public void Normalized_零向量_应返回零向量() {
            var v = Vector3.Zero;
            var result = v.Normalized;

            AssertVector3Equal(Vector3.Zero, result);
        }

        [Fact]
        public void FastNormalized_应返回近似单位向量() {
            var v = new Vector3(3, 4, 0);
            var result = v.FastNormalized;

            Assert.True(System.Math.Abs((double)result.Magnitude - 1.0) < LOOSE_EPSILON);
        }

        [Fact]
        public void GetNormalizedAndLength_应返回正确值() {
            var v = new Vector3(3, 4, 0);
            var result = v.GetNormalizedAndLength(out var length);

            Assert.Equal((Single)5, length);
            Assert.True(System.Math.Abs((double)result.Magnitude - 1.0) < EPSILON);
        }

        #endregion

        #region 点积和叉积测试

        [Fact]
        public void Dot_应返回正确值() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(4, 5, 6);
            var result = Vector3.Dot(a, b);

            Assert.Equal((Single)32, result); // 1*4 + 2*5 + 3*6 = 32
        }

        [Fact]
        public void Dot_垂直向量_应返回零() {
            var a = new Vector3(1, 0, 0);
            var b = new Vector3(0, 1, 0);
            var result = Vector3.Dot(a, b);

            Assert.True(System.Math.Abs((double)result) < EPSILON);
        }

        [Fact]
        public void Cross_应返回垂直向量() {
            var a = new Vector3(1, 0, 0);
            var b = new Vector3(0, 1, 0);
            var result = Vector3.Cross(a, b);

            // Right × Up = Forward
            AssertVector3Equal(new Vector3(0, 0, 1), result);

            // 结果应垂直于两个输入向量
            Assert.True(System.Math.Abs((double)Vector3.Dot(result, a)) < EPSILON);
            Assert.True(System.Math.Abs((double)Vector3.Dot(result, b)) < EPSILON);
        }

        [Fact]
        public void Cross_平行向量_应返回零向量() {
            var a = new Vector3(2, 4, 6);
            var b = new Vector3(1, 2, 3);
            var result = Vector3.Cross(a, b);

            Assert.True((double)result.SqrMagnitude < EPSILON);
        }

        [Fact]
        public void Cross_反交换律() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(4, 5, 6);

            var cross1 = Vector3.Cross(a, b);
            var cross2 = Vector3.Cross(b, a);

            AssertVector3Equal(-cross1, cross2, LOOSE_EPSILON);
        }

        #endregion

        #region 投影测试

        [Fact]
        public void Project_应正确投影() {
            var vector = new Vector3(3, 4, 0);
            var onNormal = new Vector3(1, 0, 0);
            var result = Vector3.Project(vector, onNormal);

            AssertVector3Equal(new Vector3(3, 0, 0), result);
        }

        [Fact]
        public void ProjectOnPlane_应投影到平面() {
            var vector = new Vector3(1, 1, 0);
            var planeNormal = new Vector3(0, 1, 0);
            var result = Vector3.ProjectOnPlane(vector, planeNormal);

            AssertVector3Equal(new Vector3(1, 0, 0), result);
        }

        #endregion

        #region 反射测试

        [Fact]
        public void Reflect_应正确反射() {
            var inDirection = new Vector3(1, -1, 0).Normalized;
            var inNormal = new Vector3(0, 1, 0);
            var result = Vector3.Reflect(inDirection, inNormal);

            // 应反射为 (1, 1, 0) 方向
            var expected = new Vector3(1, 1, 0).Normalized;
            AssertVector3Equal(expected, result, LOOSE_EPSILON);
        }

        [Fact]
        public void Reflect_垂直入射_应原路返回() {
            var inDirection = new Vector3(0, -1, 0);
            var inNormal = new Vector3(0, 1, 0);
            var result = Vector3.Reflect(inDirection, inNormal);

            AssertVector3Equal(new Vector3(0, 1, 0), result);
        }

        #endregion

        #region 大小测试

        [Fact]
        public void Magnitude_应返回正确长度() {
            var v = new Vector3(3, 4, 0);
            Assert.Equal((Single)5, v.Magnitude);
        }

        [Fact]
        public void Magnitude_3D向量_应返回正确长度() {
            var v = new Vector3(2, 3, 6);
            var expected = System.Math.Sqrt(4 + 9 + 36); // = 7
            Assert.True(System.Math.Abs((double)v.Magnitude - expected) < EPSILON);
        }

        [Fact]
        public void SqrMagnitude_应返回长度平方() {
            var v = new Vector3(3, 4, 0);
            Assert.Equal((Single)25, v.SqrMagnitude);
        }

        [Fact]
        public void FastMagnitude_应返回近似长度() {
            var v = new Vector3(3, 4, 0);
            var result = v.FastMagnitude;

            Assert.True(System.Math.Abs((double)result - 5.0) < LOOSE_EPSILON);
        }

        #endregion

        #region 角度测试

        [Theory]
        [InlineData(1, 0, 0, 0, 1, 0, 90)]
        [InlineData(1, 0, 0, 1, 0, 0, 0)]
        [InlineData(1, 0, 0, -1, 0, 0, 180)]
        public void Angle_应返回正确角度(double x1, double y1, double z1, double x2, double y2, double z2, double expectedAngle) {
            var from = new Vector3((Single)x1, (Single)y1, (Single)z1);
            var to = new Vector3((Single)x2, (Single)y2, (Single)z2);
            var angle = Vector3.Angle(from, to);

            Assert.True(System.Math.Abs((double)angle - expectedAngle) < 1.0);
        }

        [Fact]
        public void Angle_零向量_应返回零() {
            var result = Vector3.Angle(Vector3.Zero, Vector3.Right);
            Assert.Equal((Single)0, result);
        }

        [Theory]
        [InlineData(1, 0, 0, 0, 1, 0, 0, 0, 1, 90)]
        [InlineData(1, 0, 0, 0, 1, 0, 0, 0, -1, -90)]
        public void SignedAngle_应返回带符号角度(
            double x1, double y1, double z1,
            double x2, double y2, double z2,
            double axisX, double axisY, double axisZ,
            double expectedAngle) {
            var from = new Vector3((Single)x1, (Single)y1, (Single)z1);
            var to = new Vector3((Single)x2, (Single)y2, (Single)z2);
            var axis = new Vector3((Single)axisX, (Single)axisY, (Single)axisZ);
            var angle = Vector3.SignedAngle(from, to, axis);

            Assert.True(System.Math.Abs((double)angle - expectedAngle) < 1.0);
        }

        #endregion

        #region 距离测试

        [Fact]
        public void Distance_应返回正确距离() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(3, 4, 0);
            var result = Vector3.Distance(a, b);

            Assert.Equal((Single)5, result);
        }

        [Fact]
        public void Distance_3D_应返回正确距离() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(4, 6, 3);
            var result = Vector3.Distance(a, b);

            var expected = System.Math.Sqrt(9 + 16); // = 5
            Assert.True(System.Math.Abs((double)result - expected) < EPSILON);
        }

        [Fact]
        public void FastDistance_应返回近似距离() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(3, 4, 0);
            var result = Vector3.FastDistance(a, b);

            Assert.True(System.Math.Abs((double)result - 5.0) < LOOSE_EPSILON);
        }

        #endregion

        #region ClampMagnitude测试

        [Fact]
        public void ClampMagnitude_长度小于最大值_应保持不变() {
            var v = new Vector3(3, 4, 0); // 长度 = 5
            var result = Vector3.ClampMagnitude(v, 10);

            AssertVector3Equal(v, result);
        }

        [Fact]
        public void ClampMagnitude_长度大于最大值_应截断() {
            var v = new Vector3(3, 4, 0); // 长度 = 5
            var result = Vector3.ClampMagnitude(v, (Single)2.5);

            Assert.True(System.Math.Abs((double)result.Magnitude - 2.5) < EPSILON);
            // 方向应保持
            AssertVector3Equal(v.Normalized, result.Normalized, LOOSE_EPSILON);
        }

        #endregion

        #region Min/Max测试

        [Fact]
        public void Min_应返回各分量最小值() {
            var a = new Vector3(1, 5, 3);
            var b = new Vector3(3, 2, 4);
            var result = Vector3.Min(a, b);

            AssertVector3Equal(new Vector3(1, 2, 3), result);
        }

        [Fact]
        public void Max_应返回各分量最大值() {
            var a = new Vector3(1, 5, 3);
            var b = new Vector3(3, 2, 4);
            var result = Vector3.Max(a, b);

            AssertVector3Equal(new Vector3(3, 5, 4), result);
        }

        #endregion

        // #region OrthoNormalize测试
        //
        // [Fact]
        // public void OrthoNormalize_应生成正交归一化向量组()
        // {
        //     var normal = new Vector3(1, 1, 0);
        //     var tangent = new Vector3(0, 1, 1);
        //
        //     Vector3.OrthoNormalize(ref normal, ref tangent);
        //
        //     // 应该是单位向量
        //     Assert.True(System.Math.Abs((double)normal.Magnitude - 1.0) < EPSILON);
        //     Assert.True(System.Math.Abs((double)tangent.Magnitude - 1.0) < EPSILON);
        //
        //     // 应该垂直
        //     Assert.True(System.Math.Abs((double)Vector3.Dot(normal, tangent)) < EPSILON);
        // }
        //
        // [Fact]
        // public void OrthoNormalize_三向量_应生成正交归一化向量组()
        // {
        //     var normal = new Vector3(1, 1, 0);
        //     var tangent = new Vector3(0, 1, 1);
        //     var binormal = new Vector3(1, 0, 1);
        //
        //     Vector3.OrthoNormalize(ref normal, ref tangent, ref binormal);
        //
        //     // 应该都是单位向量
        //     Assert.True(System.Math.Abs((double)normal.Magnitude - 1.0) < EPSILON);
        //     Assert.True(System.Math.Abs((double)tangent.Magnitude - 1.0) < EPSILON);
        //     Assert.True(System.Math.Abs((double)binormal.Magnitude - 1.0) < EPSILON);
        //
        //     // 应该两两垂直
        //     Assert.True(System.Math.Abs((double)Vector3.Dot(normal, tangent)) < EPSILON);
        //     Assert.True(System.Math.Abs((double)Vector3.Dot(normal, binormal)) < EPSILON);
        //     Assert.True(System.Math.Abs((double)Vector3.Dot(tangent, binormal)) < EPSILON);
        // }
        //
        // #endregion

        #region RotateTowards测试

        [Fact]
        public void RotateTowards_角度小于maxDelta_应到达目标() {
            var current = Vector3.Right;
            var target = Vector3.Up;
            var result = Vector3.RotateTowards(current, target, 100, 1);

            AssertVector3Equal(target.Normalized, result.Normalized, LOOSE_EPSILON);
        }

        [Fact]
        public void RotateTowards_角度大于maxDelta_应旋转指定角度() {
            var current = Vector3.Right;
            var target = Vector3.Up;
            var maxDegrees = (Single)45;
            var maxRadians = maxDegrees * GMath.Deg2Rad; // ✅ 转换

            var result = Vector3.RotateTowards(current, target, maxRadians, 10);

            var angle = Vector3.Angle(current, result);

            Assert.True(
                System.Math.Abs((double)angle - (double)maxDegrees) < 1.0,
                $"Expected {maxDegrees}°, got {angle}°"
            );
        }

        [Theory]
        [InlineData(15)]
        [InlineData(30)]
        [InlineData(45)]
        [InlineData(60)]
        [InlineData(75)]
        public void RotateTowards_不同角度_应正确旋转(double degrees) {
            var current = Vector3.Right;
            var target = Vector3.Up;
            var radians = (Single)(degrees * System.Math.PI / 180.0);

            var result = Vector3.RotateTowards(current, target, radians, 10);

            var actualAngle = Vector3.Angle(current, result);

            Assert.True(
                System.Math.Abs((double)actualAngle - degrees) < 1.0,
                $"For {degrees}°, expected ~{degrees}°, got {actualAngle}°"
            );
        }

        [Fact]
        public void RotateTowards_完整测试集() {
            // 小角度旋转
            {
                var current = Vector3.Right;
                var target = Vector3.Up;
                var maxRad = 30 * GMath.Deg2Rad;
                var result = Vector3.RotateTowards(current, target, maxRad, 10);
                var angle = Vector3.Angle(current, result);

                Assert.True(System.Math.Abs((double)angle - 30) < 1.0);
            }

            // 到达目标
            {
                var current = Vector3.Right;
                var target = Vector3.Up;
                var maxRad = 100 * GMath.Deg2Rad;
                var result = Vector3.RotateTowards(current, target, maxRad, 10);

                AssertVector3Equal(target.Normalized, result.Normalized, 0.01);
            }

            // 零旋转
            {
                var current = Vector3.Right;
                var result = Vector3.RotateTowards(current, current, 1, 10);

                AssertVector3Equal(current, result, 0.001);
            }

            // 180度反向
            {
                var current = Vector3.Right;   // (1, 0, 0)
                var target = Vector3.Left;     // (-1, 0, 0)
                var maxDegrees = (Single)90;
                var maxRadians = maxDegrees * GMath.Deg2Rad;  // ✅ 正确转换

                var result = Vector3.RotateTowards(current, target, maxRadians, 10);
                var angle = Vector3.Angle(current, result);

                // ✅ 使用正确的断言方式
                Assert.True(
                    System.Math.Abs((double)angle - 90.0) < 1.0,
                    $"Expected 90°, got {angle}°"
                );
            }
        }

        [Fact]
        public void RotateTowards_长度变化_应正确处理() {
            var current = new Vector3(2, 0, 0); // 长度 = 2
            var target = new Vector3(0, 5, 0); // 长度 = 5
            var maxRad = 45 * GMath.Deg2Rad;
            var maxMagDelta = (Single)1;

            var result = Vector3.RotateTowards(current, target, maxRad, maxMagDelta);

            // 角度应该旋转约 45°
            var angle = Vector3.Angle(current, result);
            Assert.True(System.Math.Abs((double)angle - 45) < 2.0);

            // 长度应该变化不超过 maxMagDelta
            var magDiff = GMath.Abs(result.Magnitude - current.Magnitude);
            Assert.True((double)magDiff <= (double)maxMagDelta + 0.1);
        }

        #endregion

        #region SmoothDamp测试

        [Fact]
        public void SmoothDamp_应平滑移动向目标() {
            var current = new Vector3(0, 0, 0);
            var target = new Vector3(10, 10, 10);
            var velocity = Vector3.Zero;

            var result = Vector3.SmoothDamp(current, target, ref velocity, 1, 100, (Single)0.1);

            // 应该移动了，但未到达目标
            Assert.True(result.x > 0 && result.x < 10);
            Assert.True(result.y > 0 && result.y < 10);
            Assert.True(result.z > 0 && result.z < 10);

            // velocity 应该被更新
            Assert.True(velocity.SqrMagnitude > 0);
        }

        #endregion

        #region Slerp测试

        [Fact]
        public void Slerp_应球面插值() {
            var from = Vector3.Right;
            var to = Vector3.Up;
            var result = Vector3.Slerp(from, to, (Single)0.5);

            // 结果应该在两个向量之间
            var angleToFrom = Vector3.Angle(result, from);
            var angleToTo = Vector3.Angle(result, to);

            Assert.True(System.Math.Abs((double)(angleToFrom - angleToTo)) < 5.0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Slerp_边界值_应返回正确端点(double t) {
            var from = Vector3.Right;
            var to = Vector3.Up;
            var result = Vector3.Slerp(from, to, (Single)t);

            if (t == 0)
                AssertVector3Equal(from.Normalized, result.Normalized, LOOSE_EPSILON);
            else
                AssertVector3Equal(to.Normalized, result.Normalized, LOOSE_EPSILON);
        }

        #endregion

        #region 运算符测试

        [Fact]
        public void Operator_Add_应正确相加() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(4, 5, 6);
            var result = a + b;

            AssertVector3Equal(new Vector3(5, 7, 9), result);
        }

        [Fact]
        public void Operator_Subtract_应正确相减() {
            var a = new Vector3(5, 7, 9);
            var b = new Vector3(2, 3, 4);
            var result = a - b;

            AssertVector3Equal(new Vector3(3, 4, 5), result);
        }

        [Fact]
        public void Operator_Multiply_Vector_应逐分量相乘() {
            var a = new Vector3(2, 3, 4);
            var b = new Vector3(5, 6, 7);
            var result = a * b;

            AssertVector3Equal(new Vector3(10, 18, 28), result);
        }

        [Fact]
        public void Operator_Multiply_Scalar_应正确缩放() {
            var v = new Vector3(2, 3, 4);
            var result = v * 2;

            AssertVector3Equal(new Vector3(4, 6, 8), result);
        }

        [Fact]
        public void Operator_Multiply_Scalar_Left_应正确缩放() {
            var v = new Vector3(2, 3, 4);
            var result = (Single)2 * v;

            AssertVector3Equal(new Vector3(4, 6, 8), result);
        }

        [Fact]
        public void Operator_Divide_Vector_应逐分量相除() {
            var a = new Vector3(10, 18, 28);
            var b = new Vector3(2, 3, 4);
            var result = a / b;

            AssertVector3Equal(new Vector3(5, 6, 7), result);
        }

        [Fact]
        public void Operator_Divide_Scalar_应正确缩放() {
            var v = new Vector3(4, 6, 8);
            var result = v / 2;

            AssertVector3Equal(new Vector3(2, 3, 4), result);
        }

        [Fact]
        public void Operator_Negate_应取反() {
            var v = new Vector3(2, -3, 4);
            var result = -v;

            AssertVector3Equal(new Vector3(-2, 3, -4), result);
        }

        [Fact]
        public void Operator_Equality_相等向量_应返回true() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(1, 2, 3);

            Assert.True(a == b);
        }

        [Fact]
        public void Operator_Equality_不等向量_应返回false() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 2, 1);

            Assert.False(a == b);
        }

        [Fact]
        public void Operator_Inequality_应正确工作() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(3, 2, 1);

            Assert.True(a != b);
        }

        #endregion

        #region Equals和GetHashCode测试

        [Fact]
        public void Equals_相等向量_应返回true() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(1, 2, 3);

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_Object_相等向量_应返回true() {
            var a = new Vector3(1, 2, 3);
            object b = new Vector3(1, 2, 3);

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_Object_非Vector3_应返回false() {
            var a = new Vector3(1, 2, 3);
            object b = "not a vector";

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void GetHashCode_相等向量_应返回相同哈希() {
            var a = new Vector3(1, 2, 3);
            var b = new Vector3(1, 2, 3);

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        #endregion

        #region ToString测试

        [Fact]
        public void ToString_默认格式_应正确格式化() {
            var v = new Vector3((Single)1.234, (Single)5.678, (Single)9.012);
            var result = v.ToString();

            Assert.Contains("1.23", result);
            Assert.Contains("5.68", result);
            Assert.Contains("9.01", result);
        }

        [Fact]
        public void ToString_指定格式_应使用指定格式() {
            var v = new Vector3((Single)1.234, (Single)5.678, (Single)9.012);
            var result = v.ToString("F1");

            Assert.Contains("1.2", result);
            Assert.Contains("5.7", result);
            Assert.Contains("9.0", result);
        }

        #endregion

        #region 边界和特殊情况测试

        [Fact]
        public void 零向量运算_应正确处理() {
            var zero = Vector3.Zero;
            var v = new Vector3(1, 2, 3);

            AssertVector3Equal(v, zero + v);
            AssertVector3Equal(-v, zero - v);
            AssertVector3Equal(Vector3.Zero, zero * v);
        }

        [Fact]
        public void 单位向量_应有正确长度() {
            var vectors = new[] { Vector3.Right, Vector3.Up, Vector3.Forward };

            foreach (var v in vectors) {
                Assert.True(System.Math.Abs((double)v.Magnitude - 1.0) < EPSILON);
            }
        }

        [Fact]
        public void 坐标轴_应两两垂直() {
            Assert.True(System.Math.Abs((double)Vector3.Dot(Vector3.Right, Vector3.Up)) < EPSILON);
            Assert.True(System.Math.Abs((double)Vector3.Dot(Vector3.Right, Vector3.Forward)) < EPSILON);
            Assert.True(System.Math.Abs((double)Vector3.Dot(Vector3.Up, Vector3.Forward)) < EPSILON);
        }

        [Fact]
        public void 右手坐标系_应满足叉积关系() {
            // Right × Up = Forward
            AssertVector3Equal(Vector3.Forward, Vector3.Cross(Vector3.Right, Vector3.Up), EPSILON);

            // Up × Forward = Right
            AssertVector3Equal(Vector3.Right, Vector3.Cross(Vector3.Up, Vector3.Forward), EPSILON);

            // Forward × Right = Up
            AssertVector3Equal(Vector3.Up, Vector3.Cross(Vector3.Forward, Vector3.Right), EPSILON);
        }

        #endregion

        #region 性能对比测试

        [Fact]
        public void FastNormalized_与_Normalized_结果应接近() {
            var v = new Vector3(123, 456, 789);

            var normal = v.Normalized;
            var fastNormal = v.FastNormalized;

            // 应该比较接近
            var diff = (normal - fastNormal).Magnitude;
            Assert.True((double)diff < LOOSE_EPSILON);
        }

        [Fact]
        public void FastMagnitude_与_Magnitude_结果应接近() {
            var v = new Vector3(123, 456, 789);

            var mag = v.Magnitude;
            var fastMag = v.FastMagnitude;

            var diff = System.Math.Abs((double)(mag - fastMag));
            Assert.True(diff < LOOSE_EPSILON * (double)mag);
        }

        #endregion

        #region 辅助方法

        private void AssertVector3Equal(Vector3 expected, Vector3 actual, double epsilon = EPSILON) {
            Assert.True(
                System.Math.Abs((double)(expected.x - actual.x)) < epsilon &&
                System.Math.Abs((double)(expected.y - actual.y)) < epsilon &&
                System.Math.Abs((double)(expected.z - actual.z)) < epsilon,
                $"Expected {expected}, but got {actual}"
            );
        }

        private void AssertSingleEqual(Single expected, Single actual, double epsilon = EPSILON) {
            Assert.True(
                System.Math.Abs((double)(expected - actual)) < epsilon,
                $"Expected {expected}, but got {actual}"
            );
        }

        #endregion
    }
}
