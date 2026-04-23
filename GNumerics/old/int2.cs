using System.Runtime.InteropServices;

namespace SeaWarMath;

/// <summary>
/// 2D向量
/// </summary>
/// <author>gouanlin</author>
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct int2 : IEquatable<int2>, IFormattable {
    [FieldOffset(0)] public int x;
    [FieldOffset(4)] public int y;

    /// <summary>int2 zero value.</summary>
    public static readonly int2 zero;

    /// <summary>Constructs a int2 vector from two int values.</summary>
    /// <param name="x">The constructed vector's x component will be set to this value.</param>
    /// <param name="y">The constructed vector's y component will be set to this value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    /// <summary>Constructs a int2 vector from an int2 vector.</summary>
    /// <param name="xy">The constructed vector's xy components will be set to this value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(int2 xy) {
        this.x = xy.x;
        this.y = xy.y;
    }

    /// <summary>Constructs a int2 vector from a single int value by assigning it to every component.</summary>
    /// <param name="v">int to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(int v) {
        this.x = v;
        this.y = v;
    }

    /// <summary>Constructs a int2 vector from a single bool value by converting it to int and assigning it to every component.</summary>
    /// <param name="v">bool to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(bool v) {
        this.x = v ? 1 : 0;
        this.y = v ? 1 : 0;
    }

    /// <summary>Constructs a int2 vector from a bool2 vector by componentwise conversion.</summary>
    /// <param name="v">bool2 to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(bool2 v) {
        this.x = v.x ? 1 : 0;
        this.y = v.y ? 1 : 0;
    }

    /// <summary>Constructs a int2 vector from a single uint value by converting it to int and assigning it to every component.</summary>
    /// <param name="v">uint to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(uint v) {
        this.x = (int)v;
        this.y = (int)v;
    }

    /// <summary>Constructs a int2 vector from a uint2 vector by componentwise conversion.</summary>
    /// <param name="v">uint2 to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(uint2 v) {
        this.x = (int)v.x;
        this.y = (int)v.y;
    }

    /// <summary>Constructs a int2 vector from a single float value by converting it to int and assigning it to every component.</summary>
    /// <param name="v">float to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(float v) {
        this.x = (int)v;
        this.y = (int)v;
    }

    /// <summary>Constructs a int2 vector from a float2 vector by componentwise conversion.</summary>
    /// <param name="v">float2 to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(float2 v) {
        this.x = (int)v.x;
        this.y = (int)v.y;
    }

    /// <summary>Constructs a int2 vector from a single double value by converting it to int and assigning it to every component.</summary>
    /// <param name="v">double to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(double v) {
        this.x = (int)v;
        this.y = (int)v;
    }

    /// <summary>Constructs a int2 vector from a double2 vector by componentwise conversion.</summary>
    /// <param name="v">double2 to convert to int2</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2(double2 v) {
        this.x = (int)v.x;
        this.y = (int)v.y;
    }


    /// <summary>Implicitly converts a single int value to a int2 vector by assigning it to every component.</summary>
    /// <param name="v">int to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int2(int v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a single bool value to a int2 vector by converting it to int and assigning it to every component.</summary>
    /// <param name="v">bool to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(bool v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a bool2 vector to a int2 vector by componentwise conversion.</summary>
    /// <param name="v">bool2 to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(bool2 v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a single uint value to a int2 vector by converting it to int and assigning it to every component.</summary>
    /// <param name="v">uint to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(uint v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a uint2 vector to a int2 vector by componentwise conversion.</summary>
    /// <param name="v">uint2 to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(uint2 v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a single float value to a int2 vector by converting it to int and assigning it to every component.</summary>
    /// <param name="v">float to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(float v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a float2 vector to a int2 vector by componentwise conversion.</summary>
    /// <param name="v">float2 to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(float2 v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a single double value to a int2 vector by converting it to int and assigning it to every component.</summary>
    /// <param name="v">double to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(double v) {
        return new int2(v);
    }

    /// <summary>Explicitly converts a double2 vector to a int2 vector by componentwise conversion.</summary>
    /// <param name="v">double2 to convert to int2</param>
    /// <returns>Converted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int2(double2 v) {
        return new int2(v);
    }


    /// <summary>Returns the result of a componentwise multiplication operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise multiplication.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise multiplication.</param>
    /// <returns>int2 result of the componentwise multiplication.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator *(int2 lhs, int2 rhs) {
        return new int2(lhs.x * rhs.x, lhs.y * rhs.y);
    }

    /// <summary>Returns the result of a componentwise multiplication operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise multiplication.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise multiplication.</param>
    /// <returns>int2 result of the componentwise multiplication.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator *(int2 lhs, int rhs) {
        return new int2(lhs.x * rhs, lhs.y * rhs);
    }

    /// <summary>Returns the result of a componentwise multiplication operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise multiplication.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise multiplication.</param>
    /// <returns>int2 result of the componentwise multiplication.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator *(int lhs, int2 rhs) {
        return new int2(lhs * rhs.x, lhs * rhs.y);
    }


    /// <summary>Returns the result of a componentwise addition operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise addition.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise addition.</param>
    /// <returns>int2 result of the componentwise addition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator +(int2 lhs, int2 rhs) {
        return new int2(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    /// <summary>Returns the result of a componentwise addition operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise addition.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise addition.</param>
    /// <returns>int2 result of the componentwise addition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator +(int2 lhs, int rhs) {
        return new int2(lhs.x + rhs, lhs.y + rhs);
    }

    /// <summary>Returns the result of a componentwise addition operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise addition.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise addition.</param>
    /// <returns>int2 result of the componentwise addition.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator +(int lhs, int2 rhs) {
        return new int2(lhs + rhs.x, lhs + rhs.y);
    }


    /// <summary>Returns the result of a componentwise subtraction operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise subtraction.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise subtraction.</param>
    /// <returns>int2 result of the componentwise subtraction.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator -(int2 lhs, int2 rhs) {
        return new int2(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    /// <summary>Returns the result of a componentwise subtraction operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise subtraction.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise subtraction.</param>
    /// <returns>int2 result of the componentwise subtraction.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator -(int2 lhs, int rhs) {
        return new int2(lhs.x - rhs, lhs.y - rhs);
    }

    /// <summary>Returns the result of a componentwise subtraction operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise subtraction.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise subtraction.</param>
    /// <returns>int2 result of the componentwise subtraction.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator -(int lhs, int2 rhs) {
        return new int2(lhs - rhs.x, lhs - rhs.y);
    }


    /// <summary>Returns the result of a componentwise division operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise division.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise division.</param>
    /// <returns>int2 result of the componentwise division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator /(int2 lhs, int2 rhs) {
        return new int2(lhs.x / rhs.x, lhs.y / rhs.y);
    }

    /// <summary>Returns the result of a componentwise division operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise division.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise division.</param>
    /// <returns>int2 result of the componentwise division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator /(int2 lhs, int rhs) {
        return new int2(lhs.x / rhs, lhs.y / rhs);
    }

    /// <summary>Returns the result of a componentwise division operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise division.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise division.</param>
    /// <returns>int2 result of the componentwise division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator /(int lhs, int2 rhs) {
        return new int2(lhs / rhs.x, lhs / rhs.y);
    }


    /// <summary>Returns the result of a componentwise modulus operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise modulus.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise modulus.</param>
    /// <returns>int2 result of the componentwise modulus.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator %(int2 lhs, int2 rhs) {
        return new int2(lhs.x % rhs.x, lhs.y % rhs.y);
    }

    /// <summary>Returns the result of a componentwise modulus operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise modulus.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise modulus.</param>
    /// <returns>int2 result of the componentwise modulus.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator %(int2 lhs, int rhs) {
        return new int2(lhs.x % rhs, lhs.y % rhs);
    }

    /// <summary>Returns the result of a componentwise modulus operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise modulus.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise modulus.</param>
    /// <returns>int2 result of the componentwise modulus.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator %(int lhs, int2 rhs) {
        return new int2(lhs % rhs.x, lhs % rhs.y);
    }


    /// <summary>Returns the result of a componentwise increment operation on an int2 vector.</summary>
    /// <param name="val">Value to use when computing the componentwise increment.</param>
    /// <returns>int2 result of the componentwise increment.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator ++(int2 val) {
        return new int2(++val.x, ++val.y);
    }


    /// <summary>Returns the result of a componentwise decrement operation on an int2 vector.</summary>
    /// <param name="val">Value to use when computing the componentwise decrement.</param>
    /// <returns>int2 result of the componentwise decrement.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator --(int2 val) {
        return new int2(--val.x, --val.y);
    }


    /// <summary>Returns the result of a componentwise less than operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise less than.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise less than.</param>
    /// <returns>bool2 result of the componentwise less than.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator <(int2 lhs, int2 rhs) {
        return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
    }

    /// <summary>Returns the result of a componentwise less than operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise less than.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise less than.</param>
    /// <returns>bool2 result of the componentwise less than.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator <(int2 lhs, int rhs) {
        return new bool2(lhs.x < rhs, lhs.y < rhs);
    }

    /// <summary>Returns the result of a componentwise less than operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise less than.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise less than.</param>
    /// <returns>bool2 result of the componentwise less than.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator <(int lhs, int2 rhs) {
        return new bool2(lhs < rhs.x, lhs < rhs.y);
    }


    /// <summary>Returns the result of a componentwise less or equal operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise less or equal.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise less or equal.</param>
    /// <returns>bool2 result of the componentwise less or equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator <=(int2 lhs, int2 rhs) {
        return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
    }

    /// <summary>Returns the result of a componentwise less or equal operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise less or equal.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise less or equal.</param>
    /// <returns>bool2 result of the componentwise less or equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator <=(int2 lhs, int rhs) {
        return new bool2(lhs.x <= rhs, lhs.y <= rhs);
    }

    /// <summary>Returns the result of a componentwise less or equal operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise less or equal.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise less or equal.</param>
    /// <returns>bool2 result of the componentwise less or equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator <=(int lhs, int2 rhs) {
        return new bool2(lhs <= rhs.x, lhs <= rhs.y);
    }


    /// <summary>Returns the result of a componentwise greater than operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise greater than.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise greater than.</param>
    /// <returns>bool2 result of the componentwise greater than.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator >(int2 lhs, int2 rhs) {
        return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
    }

    /// <summary>Returns the result of a componentwise greater than operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise greater than.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise greater than.</param>
    /// <returns>bool2 result of the componentwise greater than.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator >(int2 lhs, int rhs) {
        return new bool2(lhs.x > rhs, lhs.y > rhs);
    }

    /// <summary>Returns the result of a componentwise greater than operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise greater than.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise greater than.</param>
    /// <returns>bool2 result of the componentwise greater than.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator >(int lhs, int2 rhs) {
        return new bool2(lhs > rhs.x, lhs > rhs.y);
    }


    /// <summary>Returns the result of a componentwise greater or equal operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise greater or equal.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise greater or equal.</param>
    /// <returns>bool2 result of the componentwise greater or equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator >=(int2 lhs, int2 rhs) {
        return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
    }

    /// <summary>Returns the result of a componentwise greater or equal operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise greater or equal.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise greater or equal.</param>
    /// <returns>bool2 result of the componentwise greater or equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator >=(int2 lhs, int rhs) {
        return new bool2(lhs.x >= rhs, lhs.y >= rhs);
    }

    /// <summary>Returns the result of a componentwise greater or equal operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise greater or equal.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise greater or equal.</param>
    /// <returns>bool2 result of the componentwise greater or equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator >=(int lhs, int2 rhs) {
        return new bool2(lhs >= rhs.x, lhs >= rhs.y);
    }


    /// <summary>Returns the result of a componentwise unary minus operation on an int2 vector.</summary>
    /// <param name="val">Value to use when computing the componentwise unary minus.</param>
    /// <returns>int2 result of the componentwise unary minus.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator -(int2 val) {
        return new int2(-val.x, -val.y);
    }


    /// <summary>Returns the result of a componentwise unary plus operation on an int2 vector.</summary>
    /// <param name="val">Value to use when computing the componentwise unary plus.</param>
    /// <returns>int2 result of the componentwise unary plus.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator +(int2 val) {
        return new int2(+val.x, +val.y);
    }


    /// <summary>Returns the result of a componentwise left shift operation on an int2 vector by a number of bits specified by a single int.</summary>
    /// <param name="x">The vector to left shift.</param>
    /// <param name="n">The number of bits to left shift.</param>
    /// <returns>The result of the componentwise left shift.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator <<(int2 x, int n) {
        return new int2(x.x << n, x.y << n);
    }

    /// <summary>Returns the result of a componentwise right shift operation on an int2 vector by a number of bits specified by a single int.</summary>
    /// <param name="x">The vector to right shift.</param>
    /// <param name="n">The number of bits to right shift.</param>
    /// <returns>The result of the componentwise right shift.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator >> (int2 x, int n) {
        return new int2(x.x >> n, x.y >> n);
    }

    /// <summary>Returns the result of a componentwise equality operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise equality.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise equality.</param>
    /// <returns>bool2 result of the componentwise equality.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ==(int2 lhs, int2 rhs) {
        return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
    }

    /// <summary>Returns the result of a componentwise equality operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise equality.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise equality.</param>
    /// <returns>bool2 result of the componentwise equality.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ==(int2 lhs, int rhs) {
        return new bool2(lhs.x == rhs, lhs.y == rhs);
    }

    /// <summary>Returns the result of a componentwise equality operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise equality.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise equality.</param>
    /// <returns>bool2 result of the componentwise equality.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator ==(int lhs, int2 rhs) {
        return new bool2(lhs == rhs.x, lhs == rhs.y);
    }


    /// <summary>Returns the result of a componentwise not equal operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise not equal.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise not equal.</param>
    /// <returns>bool2 result of the componentwise not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator !=(int2 lhs, int2 rhs) {
        return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
    }

    /// <summary>Returns the result of a componentwise not equal operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise not equal.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise not equal.</param>
    /// <returns>bool2 result of the componentwise not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator !=(int2 lhs, int rhs) {
        return new bool2(lhs.x != rhs, lhs.y != rhs);
    }

    /// <summary>Returns the result of a componentwise not equal operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise not equal.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise not equal.</param>
    /// <returns>bool2 result of the componentwise not equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2 operator !=(int lhs, int2 rhs) {
        return new bool2(lhs != rhs.x, lhs != rhs.y);
    }


    /// <summary>Returns the result of a componentwise bitwise not operation on an int2 vector.</summary>
    /// <param name="val">Value to use when computing the componentwise bitwise not.</param>
    /// <returns>int2 result of the componentwise bitwise not.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator ~(int2 val) {
        return new int2(~val.x, ~val.y);
    }


    /// <summary>Returns the result of a componentwise bitwise and operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise bitwise and.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise bitwise and.</param>
    /// <returns>int2 result of the componentwise bitwise and.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator &(int2 lhs, int2 rhs) {
        return new int2(lhs.x & rhs.x, lhs.y & rhs.y);
    }

    /// <summary>Returns the result of a componentwise bitwise and operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise bitwise and.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise bitwise and.</param>
    /// <returns>int2 result of the componentwise bitwise and.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator &(int2 lhs, int rhs) {
        return new int2(lhs.x & rhs, lhs.y & rhs);
    }

    /// <summary>Returns the result of a componentwise bitwise and operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise bitwise and.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise bitwise and.</param>
    /// <returns>int2 result of the componentwise bitwise and.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator &(int lhs, int2 rhs) {
        return new int2(lhs & rhs.x, lhs & rhs.y);
    }


    /// <summary>Returns the result of a componentwise bitwise or operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise bitwise or.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise bitwise or.</param>
    /// <returns>int2 result of the componentwise bitwise or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator |(int2 lhs, int2 rhs) {
        return new int2(lhs.x | rhs.x, lhs.y | rhs.y);
    }

    /// <summary>Returns the result of a componentwise bitwise or operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise bitwise or.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise bitwise or.</param>
    /// <returns>int2 result of the componentwise bitwise or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator |(int2 lhs, int rhs) {
        return new int2(lhs.x | rhs, lhs.y | rhs);
    }

    /// <summary>Returns the result of a componentwise bitwise or operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise bitwise or.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise bitwise or.</param>
    /// <returns>int2 result of the componentwise bitwise or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator |(int lhs, int2 rhs) {
        return new int2(lhs | rhs.x, lhs | rhs.y);
    }


    /// <summary>Returns the result of a componentwise bitwise exclusive or operation on two int2 vectors.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise bitwise exclusive or.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise bitwise exclusive or.</param>
    /// <returns>int2 result of the componentwise bitwise exclusive or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator ^(int2 lhs, int2 rhs) {
        return new int2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
    }

    /// <summary>Returns the result of a componentwise bitwise exclusive or operation on an int2 vector and an int value.</summary>
    /// <param name="lhs">Left hand side int2 to use to compute componentwise bitwise exclusive or.</param>
    /// <param name="rhs">Right hand side int to use to compute componentwise bitwise exclusive or.</param>
    /// <returns>int2 result of the componentwise bitwise exclusive or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator ^(int2 lhs, int rhs) {
        return new int2(lhs.x ^ rhs, lhs.y ^ rhs);
    }

    /// <summary>Returns the result of a componentwise bitwise exclusive or operation on an int value and an int2 vector.</summary>
    /// <param name="lhs">Left hand side int to use to compute componentwise bitwise exclusive or.</param>
    /// <param name="rhs">Right hand side int2 to use to compute componentwise bitwise exclusive or.</param>
    /// <returns>int2 result of the componentwise bitwise exclusive or.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 operator ^(int lhs, int2 rhs) {
        return new int2(lhs ^ rhs.x, lhs ^ rhs.y);
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xxxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, x, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xxxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, x, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xxyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, x, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xxyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, x, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xyxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, y, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xyxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, y, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xyyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, y, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 xyyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(x, y, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yxxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, x, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yxxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, x, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yxyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, x, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yxyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, x, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yyxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, y, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yyxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, y, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yyyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, y, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int4 yyyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int4(y, y, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 xxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(x, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 xxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(x, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 xyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(x, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 xyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(x, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 yxx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(y, x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 yxy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(y, x, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 yyx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(y, y, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int3 yyy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int3(y, y, y); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int2 xx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int2(x, x); }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int2 xy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int2(x, y); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set {
            x = value.x;
            y = value.y;
        }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int2 yx {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int2(y, x); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set {
            y = value.x;
            x = value.y;
        }
    }


    /// <summary>Swizzles the vector.</summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public int2 yy {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new int2(y, y); }
    }


    /// <summary>Returns the int element at a specified index.</summary>
    unsafe public int this[int index] {
        get {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 2)
                    throw new System.ArgumentException("index must be between[0...1]");
#endif
            fixed (int2* array = &this) {
                return ((int*)array)[index];
            }
        }
        set {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 2)
                    throw new System.ArgumentException("index must be between[0...1]");
#endif
            fixed (int* array = &x) {
                array[index] = value;
            }
        }
    }

    /// <summary>Returns true if the int2 is equal to a given int2, false otherwise.</summary>
    /// <param name="rhs">Right hand side argument to compare equality with.</param>
    /// <returns>The result of the equality comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(int2 rhs) {
        return x == rhs.x && y == rhs.y;
    }

    /// <summary>Returns true if the int2 is equal to a given int2, false otherwise.</summary>
    /// <param name="o">Right hand side argument to compare equality with.</param>
    /// <returns>The result of the equality comparison.</returns>
    public override bool Equals(object o) {
        return o is int2 converted && Equals(converted);
    }


    /// <summary>Returns a hash code for the int2.</summary>
    /// <returns>The computed hash code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() {
        return (int)math.hash(this);
    }


    /// <summary>Returns a string representation of the int2.</summary>
    /// <returns>String representation of the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() {
        return string.Format("int2({0}, {1})", x, y);
    }

    /// <summary>Returns a string representation of the int2 using a specified format and culture-specific format information.</summary>
    /// <param name="format">Format string to use during string formatting.</param>
    /// <param name="formatProvider">Format provider to use during string formatting.</param>
    /// <returns>String representation of the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format, IFormatProvider formatProvider) {
        return string.Format("int2({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
    }

    internal sealed class DebuggerProxy {
        public int x;
        public int y;

        public DebuggerProxy(int2 v) {
            x = v.x;
            y = v.y;
        }
    }
}
