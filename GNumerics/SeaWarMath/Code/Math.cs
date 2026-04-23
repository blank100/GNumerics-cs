namespace SeaWar.Mathematics;

/// <summary>
///
/// </summary>
/// <author>gouanlin</author>
public static class Math {
#if USE_FIXED64
    public static readonly Single MaxSingle = Single.MaxValue;
    public static readonly Single MinSingle = Single.MinValue;
#else
    public const Single MaxSingle = Single.MaxValue;
    public const Single MinSingle = Single.MinValue;
#endif

#if USE_FIXED64
    public static readonly Single Deg2Rad = Single.Deg2Rad;
    public static readonly Single Rad2Deg = Single.Rad2Deg;

    public static readonly Double Deg2RadDouble = Double.Deg2Rad;
    public static readonly Double Rad2DegDouble = Double.Rad2Deg;
#else
    public const Single Deg2Rad = 0.01745329251994329576f;
    public const Single Rad2Deg = 57.2957795130823208767f;

    public static readonly Double Deg2RadDouble = 0.01745329251994329576D;
    public static readonly Double Rad2DegDouble = 57.2957795130823208767D;
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Min(Single a, Single b) => a < b ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Max(Single a, Single b) => a > b ? a : b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Abs(Single a) => a < 0 ? -a : a;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Clamp(Single value, Single min, Single max) => value < min ? min : value > max ? max : value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Clamp(Double value, Double min, Double max) => value < min ? min : value > max ? max : value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(Single v) {
#if USE_FIXED64
        return Single.Sign(v);
#else
        return MathF.Sign(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Floor(Single v) {
#if USE_FIXED64
        return Single.Floor(v);
#else
        return MathF.Floor(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Floor(Double v) {
#if USE_FIXED64
        return Double.Floor(v);
#else
        return System.Math.Floor(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Ceiling(Double v) {
#if USE_FIXED64
        return Double.Ceiling(v);
#else
        return System.Math.Ceiling(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Ceiling(Single v) {
#if USE_FIXED64
        return Single.Ceiling(v);
#else
        return MathF.Ceiling(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Round(Double v) {
#if USE_FIXED64
        return Double.Round(v);
#else
        return System.Math.Round(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Round(Single v) {
#if USE_FIXED64
        return Single.Round(v);
#else
        return MathF.Round(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Truncate(Double v) {
#if USE_FIXED64
        return Double.Truncate(v);
#else
        return System.Math.Truncate(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Truncate(Single v) {
#if USE_FIXED64
        return Single.Truncate(v);
#else
        return MathF.Truncate(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Pow(Double v, int e) {
#if USE_FIXED64
        return Double.Pow(v, e);
#else
        return System.Math.Pow(v, e);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Pow(Single v, int e) {
#if USE_FIXED64
        return Single.Pow(v, e);
#else
        return MathF.Pow(v, e);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Sqrt(Single v) {
#if USE_FIXED64
        return Single.Sqrt(v);
#else
        return (Single)System.Math.Sqrt(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Sqrt(Double v) {
#if USE_FIXED64
        return Double.Sqrt(v);
#else
        return System.Math.Sqrt(v);
#endif
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Exp(Single v) {
#if USE_FIXED64
        return Single.Exp(v);
#else
        return (Single)System.Math.Exp(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Exp(Double v) {
#if USE_FIXED64
        return Double.Exp(v);
#else
        return System.Math.Exp(v);
#endif
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Exp2(Single v) {
#if USE_FIXED64
        return Single.Exp2(v);
#else
        return (Single)System.Math.Exp(v * 0.69314718f);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Exp2(Double v) {
#if USE_FIXED64
        return Double.Exp2(v);
#else
        return System.Math.Exp(v * 0.693147180559945309d);
#endif
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Exp10(Single v) {
#if USE_FIXED64
        return Single.Exp10(v);
#else
        return (Single)System.Math.Exp(v * 2.30258509f);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Exp10(Double v) {
#if USE_FIXED64
        return Double.Exp10(v);
#else
        return System.Math.Exp(v * 2.302585092994045684d);
#endif
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Log(Single v) {
#if USE_FIXED64
        return Single.Log(v);
#else
        return (Single)System.Math.Log(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Log(Double v) {
#if USE_FIXED64
        return Double.Log(v);
#else
        return System.Math.Log(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Log2(Single v) {
#if USE_FIXED64
        return Single.Log2(v);
#else
        return (Single)System.Math.Log(v, 2);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Log2(Double v) {
#if USE_FIXED64
        return Double.Log2(v);
#else
        return System.Math.Log(v, 2);
#endif
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Log10(Single v) {
#if USE_FIXED64
        return Single.Log10(v);
#else
        return (Single)System.Math.Log10(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Log10(Double v) {
#if USE_FIXED64
        return Double.Log10(v);
#else
        return System.Math.Log10(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Sin(Single v) {
#if USE_FIXED64
        return Single.Sin(v);
#else
        return MathF.Sin(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Sin(Double v) {
#if USE_FIXED64
        return Double.Sin(v);
#else
        return System.Math.Sin(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Asin(Single v) {
#if USE_FIXED64
        return Single.Asin(v);
#else
        return MathF.Asin(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Asin(Double v) {
#if USE_FIXED64
        return Double.Asin(v);
#else
        return System.Math.Asin(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Cos(Single v) {
#if USE_FIXED64
        return Single.Cos(v);
#else
        return (Single)System.Math.Cos(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Cos(Double v) {
#if USE_FIXED64
        return Double.Cos(v);
#else
        return System.Math.Cos(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Single Acos(Single v) {
#if USE_FIXED64
        return Single.Acos(v);
#else
        return (Single)System.Math.Acos(v);
#endif
    }

    public static Single Tan(Single v) {
#if USE_FIXED64
        return Single.Tan(v);
#else
        return MathF.Tan(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Tan(Double v) {
#if USE_FIXED64
        return Double.Tan(v);
#else
        return System.Math.Tan(v);
#endif
    }

    public static Single Atan(Single v) {
#if USE_FIXED64
        return Single.Atan(v);
#else
        return MathF.Atan(v);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Double Atan(Double v) {
#if USE_FIXED64
        return Double.Atan(v);
#else
        return System.Math.Atan(v);
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
        return System.Math.Atan2(y, x);
#endif
    }
}
