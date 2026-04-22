namespace Gal.Core;

public static class Fixed64Convert {
    public static long Convert(long sourceRaw, int sourceFractionBits, int targetFractionBits) {
        var d = sourceFractionBits - targetFractionBits;
        if (d == 0) return sourceRaw;
        return d > 0 ? sourceRaw >> d : unchecked(sourceRaw << d);
    }
}
