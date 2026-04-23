namespace Gal.Core {
    public partial struct Fixed64 {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Fixed64(Fixed40_24 v) =>
            FromRaw(Fixed64Convert.Convert(v.Raw, Fixed40_24.FRACTION_BITS, FRACTION_BITS));
    }

    public partial struct Fixed40_24 {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Fixed40_24(Fixed64 v) =>
            FromRaw(Fixed64Convert.Convert(v.Raw, Fixed64.FRACTION_BITS, FRACTION_BITS));
    }
}
