namespace Gal.Core;

/// <summary>
///
/// </summary>
/// <author>gouanlin</author>
public static class GMath {
#if USE_FIXED64
    public static readonly Single MaxSingle = Single.MaxValue;
    public static readonly Single MinSingle = Single.MinValue;
#else
	public const Single MaxSingle = Single.MaxValue;
	public const Single MinSingle = Single.MinValue;
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Min(Single a, Single b) => a < b ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Max(Single a, Single b) => a > b ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Abs(Single a) => a < 0 ? -a : a;

    public static Single Clamp(Single value, Single min, Single max) => value < min ? min : value > max ? max : value;

    public static Double Clamp(Double value, Double min, Double max) => value < min ? min : value > max ? max : value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Sqrt(Single v) {
#if USE_FIXED64
        return Single.Sqrt(v);
#else
        return MathF.Sqrt(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Sqrt(Double v) {
#if USE_FIXED64
        return Double.Sqrt(v);
#else
        return Math.Sqrt(v);
#endif
    }

    public static Single Sin(Single v) {
#if USE_FIXED64
        return Single.Sin(v);
#else
        return MathF.Sin(v);
#endif
    }

    public static Double Sin(Double v) {
#if USE_FIXED64
        return Double.Sin(v);
#else
        return MathF.Sin(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Cos(Single v) {
#if USE_FIXED64
        return Single.Cos(v);
#else
        return Math.Cos(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Cos(Double v) {
#if USE_FIXED64
        return Double.Cos(v);
#else
        return Math.Cos(v);
#endif
    }

    public static Single Atan2(Single y, Single x) {
#if USE_FIXED64
        return Single.Atan2(y, x);
#else
        return MathF.Atan2(y, x);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Atan2(Double y, Double x) {
#if USE_FIXED64
        return Double.Atan2(y, x);
#else
        return Math.Atan2(y,x);
#endif
    }
}
