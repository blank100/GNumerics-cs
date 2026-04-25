using SeaWar.Mathematics;

namespace SeaWarMath.Test.gpt;

public class Vector3Tests {
    public static class TestHelper {
        public static void AssertNearlyEqual(Single a, Single b, Single eps) {
            Assert.True(GMath.Abs(a - b) <= eps, $"Expected {a} ≈ {b}");
        }

        public static void AssertNearlyEqual(Vector3 a, Vector3 b, Single eps) {
            AssertNearlyEqual(a.x, b.x, eps);
            AssertNearlyEqual(a.y, b.y, eps);
            AssertNearlyEqual(a.z, b.z, eps);
        }
    }

    public class Vector3_Lerp_Tests {
        [Fact]
        public void Lerp_Basic() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 10, 10);

            var r = Vector3.Lerp(a, b, (Single)0.5f);

            TestHelper.AssertNearlyEqual(r, new Vector3(5, 5, 5), (Single)0.0001f);
        }

        [Fact]
        public void Lerp_Clamp() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 0, 0);

            var r = Vector3.Lerp(a, b, (Single)2f);

            TestHelper.AssertNearlyEqual(r, b, (Single)0.0001f); // clamp
        }

        [Fact]
        public void LerpUnclamped_NoClamp() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 0, 0);

            var r = Vector3.LerpUnclamped(a, b, (Single)2f);

            TestHelper.AssertNearlyEqual(r, new Vector3(20, 0, 0), (Single)0.0001f);
        }
    }

    public class Vector3_MoveTowards_Tests {
        [Fact]
        public void MoveTowards_NoOvershoot() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(1, 0, 0);

            var r = Vector3.MoveTowards(a, b, (Single)2f);

            TestHelper.AssertNearlyEqual(r, b, (Single)0.0001f);
        }

        [Fact]
        public void MoveTowards_NormalStep() {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(10, 0, 0);

            var r = Vector3.MoveTowards(a, b, (Single)2f);

            TestHelper.AssertNearlyEqual(r, new Vector3(2, 0, 0), (Single)0.0001f);
        }

        [Fact]
        public void MoveTowards_ZeroDistance() {
            var a = new Vector3(1, 1, 1);

            var r = Vector3.MoveTowards(a, a, (Single)1f);

            TestHelper.AssertNearlyEqual(r, a, (Single)0.0001f);
        }
    }
}
