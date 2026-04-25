using Gal.Core;
using SeaWar.Mathematics;

namespace SeaWarMath.Test.gpt;

public class Vector2Tests {
    private const float EPS = 1e-4f;

    #region MoveTowards

    [Fact]
    public void MoveTowards_Basic()
    {
        var current = new Vector2(0, 0);
        var target = new Vector2(10, 0);

        var result = Vector2.MoveTowards(current, target, 1);

        Assert.Equal(1, (double)result.x, 5);
        Assert.Equal(0, (double)result.y, 5);
    }

    [Fact]
    public void MoveTowards_NoOvershoot()
    {
        var current = new Vector2(0, 0);
        var target = new Vector2(1, 0);

        var result = Vector2.MoveTowards(current, target, 10);

        Assert.Equal((double)target.x, (double)result.x, 5);
        Assert.Equal((double)target.y, (double)result.y, 5);
    }

    [Fact]
    public void MoveTowards_SamePoint()
    {
        var current = new Vector2(1, 1);
        var result = Vector2.MoveTowards(current, current, 5);

        Assert.Equal(current.x, result.x);
        Assert.Equal(current.y, result.y);
    }

    #endregion

    #region Normalized

    [Fact]
    public void Normalized_LengthIsOne()
    {
        var v = new Vector2(3, 4);
        var n = v.Normalized;

        var len = n.Magnitude;
        Assert.True((double)Math.Abs(len - 1) < EPS);
    }

    [Fact]
    public void Normalized_ZeroVector()
    {
        var v = Vector2.Zero;
        var n = v.Normalized;

        Assert.Equal(0, (double)n.x);
        Assert.Equal(0, (double)n.y);
    }

    #endregion

    #region FastNormalized

    [Fact]
    public void FastNormalized_LengthCloseToOne()
    {
        var v = new Vector2(3, 4);
        var n = v.FastNormalized;

        var len = n.Magnitude;
        Assert.True((double)Math.Abs(len - 1) < 1e-2f); // Fast 允许误差大一点
    }

    [Fact]
    public void FastNormalized_ZeroSafe()
    {
        var v = Vector2.Zero;
        var n = v.FastNormalized;

        Assert.False(float.IsNaN((float)n.x));
        Assert.False(float.IsNaN((float)n.y));
    }

    #endregion

    #region Angle

    [Fact]
    public void Angle_90Degrees()
    {
        var a = new Vector2(1, 0);
        var b = new Vector2(0, 1);

        var angle = Vector2.Angle(a, b);

        Assert.True((double)Math.Abs(angle - 90) < 0.01f);
    }

    [Fact]
    public void SignedAngle_Positive()
    {
        var a = new Vector2(1, 0);
        var b = new Vector2(0, 1);

        var angle = Vector2.SignedAngle(a, b);

        Assert.True(angle > 0);
    }

    [Fact]
    public void SignedAngle_Negative()
    {
        var a = new Vector2(0, 1);
        var b = new Vector2(1, 0);

        var angle = Vector2.SignedAngle(a, b);

        Assert.True(angle < 0);
    }

    #endregion

    #region ClampMagnitude

    [Fact]
    public void ClampMagnitude_Limit()
    {
        var v = new Vector2(10, 0);
        var result = Vector2.ClampMagnitude(v, 5);

        Assert.True((double)Math.Abs(result.Magnitude - 5) < EPS);
    }

    [Fact]
    public void ClampMagnitude_NoChange()
    {
        var v = new Vector2(2, 2);
        var result = Vector2.ClampMagnitude(v, 10);

        Assert.Equal(v.x, result.x);
        Assert.Equal(v.y, result.y);
    }

    #endregion

    #region SmoothDamp

    [Fact]
    public void SmoothDamp_ApproachesTarget()
    {
        var current = new Vector2(0, 0);
        var target = new Vector2(10, 0);
        var velocity = Vector2.Zero;

        for (int i = 0; i < 100; i++)
        {
            current = Vector2.SmoothDamp(current, target, ref velocity, (Single)0.3f, (Single)100f, (Single)0.016f);
        }

        Assert.True((Single)Math.Abs(current.x - (Single)10f) < 0.01f);
    }

    // [Fact]
    // public void SmoothDamp_NoOvershoot()
    // {
    //     var current = new Vector2(0, 0);
    //     var target = new Vector2(1, 0);
    //     var velocity = Vector2.Zero;
    //
    //     for (int i = 0; i < 10; i++)
    //     {
    //         current = Vector2.SmoothDamp(current, target, ref velocity, 0.1f, 100f, 0.016f);
    //     }
    //
    //     Assert.True(current.x <= 1.0001f);
    // }
    //
    // [Fact]
    // public void SmoothDamp_StableAtTarget()
    // {
    //     var current = new Vector2(5, 5);
    //     var target = new Vector2(5, 5);
    //     var velocity = Vector2.Zero;
    //
    //     var result = Vector2.SmoothDamp(current, target, ref velocity, 0.3f, 100f, 0.016f);
    //
    //     Assert.Equal(5f, result.x, EPS);
    //     Assert.Equal(5f, result.y, EPS);
    // }
    //
    // [Fact]
    // public void SmoothDamp_MaxSpeedLimit()
    // {
    //     var current = new Vector2(0, 0);
    //     var target = new Vector2(100, 0);
    //     var velocity = Vector2.Zero;
    //
    //     var result = Vector2.SmoothDamp(current, target, ref velocity, 0.3f, 1f, 0.016f);
    //
    //     // 每帧移动应该非常小（被 maxSpeed 限制）
    //     Assert.True(result.x < 1f);
    // }
    //
    // #endregion
    //
    // #region Distance
    //
    // [Fact]
    // public void Distance_Correct()
    // {
    //     var a = new Vector2(0, 0);
    //     var b = new Vector2(3, 4);
    //
    //     var d = Vector2.Distance(a, b);
    //
    //     Assert.True(Math.Abs(d - 5f) < EPS);
    // }
    //
    // #endregion
    //
    // #region Operators
    //
    // [Fact]
    // public void Operator_Add()
    // {
    //     var a = new Vector2(1, 2);
    //     var b = new Vector2(3, 4);
    //
    //     var c = a + b;
    //
    //     Assert.Equal(4f, c.x);
    //     Assert.Equal(6f, c.y);
    // }
    //
    // [Fact]
    // public void Operator_MulScalar()
    // {
    //     var v = new Vector2(2, 3);
    //
    //     var r = v * 2;
    //
    //     Assert.Equal(4f, r.x);
    //     Assert.Equal(6f, r.y);
    // }

    #endregion
}
