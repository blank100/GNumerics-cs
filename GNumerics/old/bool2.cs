using System.Runtime.InteropServices;

namespace SeaWarMath;

/// <summary>
/// 2D向量
/// </summary>
/// <author>gouanlin</author>
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct bool2 : IEquatable<bool2>, IFormattable {
    [FieldOffset(0)] public bool x;
    [FieldOffset(1)] public bool y;

    /// <summary>Constructs a bool2 vector from two bool values.</summary>
    /// <param name="x">The constructed vector's x component will be set to this value.</param>
    /// <param name="y">The constructed vector's y component will be set to this value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool2(bool x, bool y) {
        this.x = x;
        this.y = y;
    }

    /// <summary>Constructs a bool2 vector from a bool2 vector.</summary>
    /// <param name="xy">The constructed vector's xy components will be set to this value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool2(bool2 xy) {
        this.x = xy.x;
        this.y = xy.y;
    }

    /// <summary>Constructs a bool2 vector from a single bool value by assigning it to every component.</summary>
    /// <param name="v">bool to convert to bool2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool2(bool v) {
        this.x = v;
        this.y = v;
    }


    /// <summary>Implicitly converts a single bool value to a bool2 vector by assigning it to every component.</summary>
    /// <param name="v">bool to convert to bool2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator bool2(bool v) {
        return new bool2(v);
    }


    /// <summary>Returns the result of a componentwise equality operation on two bool2 vectors.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise equality.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise equality.</param>
    /// <returns>bool2 result of the componentwise equality.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ==(bool2 lhs, bool2 rhs) {
        return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
    }

    /// <summary>Returns the result of a componentwise equality operation on a bool2 vector and a bool value.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise equality.</param>
    /// <param name="rhs">Right hand side bool to use to compute componentwise equality.</param>
    /// <returns>bool2 result of the componentwise equality.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ==(bool2 lhs, bool rhs) {
        return new bool2(lhs.x == rhs, lhs.y == rhs);
    }

    /// <summary>Returns the result of a componentwise equality operation on a bool value and a bool2 vector.</summary>
    /// <param name="lhs">Left hand side bool to use to compute componentwise equality.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise equality.</param>
    /// <returns>bool2 result of the componentwise equality.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ==(bool lhs, bool2 rhs) {
        return new bool2(lhs == rhs.x, lhs == rhs.y);
    }


    /// <summary>Returns the result of a componentwise not equal operation on two bool2 vectors.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise not equal.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise not equal.</param>
    /// <returns>bool2 result of the componentwise not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator !=(bool2 lhs, bool2 rhs) {
        return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
    }

    /// <summary>Returns the result of a componentwise not equal operation on a bool2 vector and a bool value.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise not equal.</param>
    /// <param name="rhs">Right hand side bool to use to compute componentwise not equal.</param>
    /// <returns>bool2 result of the componentwise not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator !=(bool2 lhs, bool rhs) {
        return new bool2(lhs.x != rhs, lhs.y != rhs);
    }

    /// <summary>Returns the result of a componentwise not equal operation on a bool value and a bool2 vector.</summary>
    /// <param name="lhs">Left hand side bool to use to compute componentwise not equal.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise not equal.</param>
    /// <returns>bool2 result of the componentwise not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator !=(bool lhs, bool2 rhs) {
        return new bool2(lhs != rhs.x, lhs != rhs.y);
    }


    /// <summary>Returns the result of a componentwise not operation on a bool2 vector.</summary>
    /// <param name="val">Value to use when computing the componentwise not.</param>
    /// <returns>bool2 result of the componentwise not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator !(bool2 val) {
        return new bool2(!val.x, !val.y);
    }


    /// <summary>Returns the result of a componentwise bitwise and operation on two bool2 vectors.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise bitwise and.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise bitwise and.</param>
    /// <returns>bool2 result of the componentwise bitwise and.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator &(bool2 lhs, bool2 rhs) {
        return new bool2(lhs.x & rhs.x, lhs.y & rhs.y);
    }

    /// <summary>Returns the result of a componentwise bitwise and operation on a bool2 vector and a bool value.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise bitwise and.</param>
    /// <param name="rhs">Right hand side bool to use to compute componentwise bitwise and.</param>
    /// <returns>bool2 result of the componentwise bitwise and.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator &(bool2 lhs, bool rhs) {
        return new bool2(lhs.x & rhs, lhs.y & rhs);
    }

    /// <summary>Returns the result of a componentwise bitwise and operation on a bool value and a bool2 vector.</summary>
    /// <param name="lhs">Left hand side bool to use to compute componentwise bitwise and.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise bitwise and.</param>
    /// <returns>bool2 result of the componentwise bitwise and.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator &(bool lhs, bool2 rhs) {
        return new bool2(lhs & rhs.x, lhs & rhs.y);
    }


    /// <summary>Returns the result of a componentwise bitwise or operation on two bool2 vectors.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise bitwise or.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise bitwise or.</param>
    /// <returns>bool2 result of the componentwise bitwise or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator |(bool2 lhs, bool2 rhs) {
        return new bool2(lhs.x | rhs.x, lhs.y | rhs.y);
    }

    /// <summary>Returns the result of a componentwise bitwise or operation on a bool2 vector and a bool value.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise bitwise or.</param>
    /// <param name="rhs">Right hand side bool to use to compute componentwise bitwise or.</param>
    /// <returns>bool2 result of the componentwise bitwise or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator |(bool2 lhs, bool rhs) {
        return new bool2(lhs.x | rhs, lhs.y | rhs);
    }

    /// <summary>Returns the result of a componentwise bitwise or operation on a bool value and a bool2 vector.</summary>
    /// <param name="lhs">Left hand side bool to use to compute componentwise bitwise or.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise bitwise or.</param>
    /// <returns>bool2 result of the componentwise bitwise or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator |(bool lhs, bool2 rhs) {
        return new bool2(lhs | rhs.x, lhs | rhs.y);
    }


    /// <summary>Returns the result of a componentwise bitwise exclusive or operation on two bool2 vectors.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise bitwise exclusive or.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise bitwise exclusive or.</param>
    /// <returns>bool2 result of the componentwise bitwise exclusive or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ^(bool2 lhs, bool2 rhs) {
        return new bool2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
    }

    /// <summary>Returns the result of a componentwise bitwise exclusive or operation on a bool2 vector and a bool value.</summary>
    /// <param name="lhs">Left hand side bool2 to use to compute componentwise bitwise exclusive or.</param>
    /// <param name="rhs">Right hand side bool to use to compute componentwise bitwise exclusive or.</param>
    /// <returns>bool2 result of the componentwise bitwise exclusive or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ^(bool2 lhs, bool rhs) {
        return new bool2(lhs.x ^ rhs, lhs.y ^ rhs);
    }

    /// <summary>Returns the result of a componentwise bitwise exclusive or operation on a bool value and a bool2 vector.</summary>
    /// <param name="lhs">Left hand side bool to use to compute componentwise bitwise exclusive or.</param>
    /// <param name="rhs">Right hand side bool2 to use to compute componentwise bitwise exclusive or.</param>
    /// <returns>bool2 result of the componentwise bitwise exclusive or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ^(bool lhs, bool2 rhs) {
        return new bool2(lhs ^ rhs.x, lhs ^ rhs.y);
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xxxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, x, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xxxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, x, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xxyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, x, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xxyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, x, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xyxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, y, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xyxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, y, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xyyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, y, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 xyyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(x, y, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yxxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, x, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yxxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, x, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yxyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, x, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yxyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, x, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yyxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, y, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yyxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, y, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yyyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, y, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool4 yyyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool4(y, y, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 xxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(x, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 xxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(x, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 xyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(x, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 xyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(x, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 yxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(y, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 yxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(y, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 yyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(y, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool3 yyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool3(y, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool2 xx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool2(x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool2 xy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool2(x, y); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set {
            x = value.x;
            y = value.y;
        }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool2 yx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool2(y, x); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set {
            y = value.x;
            x = value.y;
        }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool2 yy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new bool2(y, y); }
    }


    /// <summary>Returns the bool element at a specified index.</summary>
    unsafe public bool this[int index] {
        get {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 2)
                    throw new System.ArgumentException("index must be between[0...1]");
#endif
            fixed (bool2* array = &this) {
                return ((bool*)array)[index];
            }
        }
        set {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 2)
                    throw new System.ArgumentException("index must be between[0...1]");
#endif
            fixed (bool* array = &x) {
                array[index] = value;
            }
        }
    }

    /// <summary>Returns true if the bool2 is equal to a given bool2, false otherwise.</summary>
    /// <param name="rhs">Right hand side argument to compare equality with.</param>
    /// <returns>The result of the equality comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(bool2 rhs) {
        return x == rhs.x && y == rhs.y;
    }

    /// <summary>Returns true if the bool2 is equal to a given bool2, false otherwise.</summary>
    /// <param name="o">Right hand side argument to compare equality with.</param>
    /// <returns>The result of the equality comparison.</returns>
    public override bool Equals(object o) {
        return o is bool2 converted && Equals(converted);
    }


    /// <summary>Returns a hash code for the bool2.</summary>
    /// <returns>The computed hash code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() {
        return (int)math.hash(this);
    }


    /// <summary>Returns a string representation of the bool2.</summary>
    /// <returns>String representation of the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() {
        return string.Format("bool2({0}, {1})", x, y);
    }

    public string ToString(string format, IFormatProvider formatProvider) => throw new NotImplementedException();

    internal sealed class DebuggerProxy {
        public bool x;
        public bool y;

        public DebuggerProxy(bool2 v) {
            x = v.x;
            y = v.y;
        }
    }
}
