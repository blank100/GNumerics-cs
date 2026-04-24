using Gal.Core;

namespace System.Numerics;

/// <summary>
/// 碰撞系统的一些尝试定义
/// </summary>
/// <author>gouanlin</author>
public static class CollisionConstDefine {
    // ===== 基础 =====
    public static readonly Fixed64 Precision = Fixed64.FromRaw(1); // 2^-32
    public static readonly Fixed64 Epsilon = Fixed64.FromRaw(1); // 同 Precision

// ===== 几何通用 =====
    public static readonly Fixed64 Tolerance = Fixed64.FromRaw(1L << 8); // ≈ 6e-8
    public static readonly Fixed64 TightTolerance = Fixed64.FromRaw(1L << 6); // ≈ 1.5e-8
    public static readonly Fixed64 LooseTolerance = Fixed64.FromRaw(1L << 10); // ≈ 2.4e-7

// ===== 向量 / 归一化 =====
    public static readonly Fixed64 NormalizeEpsilon = Fixed64.FromRaw(1L << 10);

// ===== SAT（分离轴）=====
    public static readonly Fixed64 SAT_Epsilon = Fixed64.FromRaw(1L << 7);
    public static readonly Fixed64 SAT_PenetrationSlop = Fixed64.FromRaw(1L << 9);

// ===== GJK / EPA =====
    public static readonly Fixed64 GJK_Epsilon = Fixed64.FromRaw(1L << 6);
    public static readonly Fixed64 EPA_Epsilon = Fixed64.FromRaw(7);

// ===== TOI（时间碰撞）=====
    public static readonly Fixed64 TOI_Tolerance = Fixed64.FromRaw(1L << 10);
    public static readonly Fixed64 TOI_RootEpsilon = Fixed64.FromRaw(1L << 6);

// ===== 接触稳定（非常关键）=====
    public static readonly Fixed64 ContactSlop = Fixed64.FromRaw(1L << 10);
    public static readonly Fixed64 Baumgarte = (Fixed64)0.2; // 位置修正系数
}
