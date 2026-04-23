using System.Runtime.InteropServices;

namespace SeaWarMath;

/// <summary>
/// 2D向量
/// </summary>
/// <author>gouanlin</author>
[Serializable]
#if USE_FIXED64
[StructLayout(LayoutKind.Explicit, Size = 16)]
#else
	[StructLayout(LayoutKind.Explicit, Size = 8)]
#endif
public struct float2 : IEquatable<float2>, IFormattable {
    [FieldOffset(0)]
    public Single x;

#if USE_FIXED64
    [FieldOffset(8)]
#else
		[FieldOffset(4)]
#endif
    public Single y;

    /// <summary>float2 zero value.</summary>
        public static readonly float2 zero;

        /// <summary>Constructs a float2 vector from two Single values.</summary>
        /// <param name="x">The constructed vector's x component will be set to this value.</param>
        /// <param name="y">The constructed vector's y component will be set to this value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(Single x, Single y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>Constructs a float2 vector from a float2 vector.</summary>
        /// <param name="xy">The constructed vector's xy components will be set to this value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(float2 xy)
        {
            this.x = xy.x;
            this.y = xy.y;
        }

        /// <summary>Constructs a float2 vector from a single Single value by assigning it to every component.</summary>
        /// <param name="v">Single to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(Single v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>Constructs a float2 vector from a single bool value by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">bool to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(bool v)
        {
            this.x = v ? 1 : 0;
            this.y = v ? 1 : 0;
        }

        /// <summary>Constructs a float2 vector from a bool2 vector by componentwise conversion.</summary>
        /// <param name="v">bool2 to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(bool2 v)
        {
            this.x = v.x ? 1 : 0;
            this.y = v.y ? 1 : 0;
        }

        /// <summary>Constructs a float2 vector from a single int value by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">int to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(int v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>Constructs a float2 vector from a int2 vector by componentwise conversion.</summary>
        /// <param name="v">int2 to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(int2 v)
        {
            this.x = v.x;
            this.y = v.y;
        }

        /// <summary>Constructs a float2 vector from a single uint value by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">uint to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(uint v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>Constructs a float2 vector from a uint2 vector by componentwise conversion.</summary>
        /// <param name="v">uint2 to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(uint2 v)
        {
            this.x = v.x;
            this.y = v.y;
        }

        /// <summary>Constructs a float2 vector from a single half value by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">half to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(half v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>Constructs a float2 vector from a half2 vector by componentwise conversion.</summary>
        /// <param name="v">half2 to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(half2 v)
        {
            this.x = v.x;
            this.y = v.y;
        }

        /// <summary>Constructs a float2 vector from a single double value by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">double to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(double v)
        {
            this.x = (Single)v;
            this.y = (Single)v;
        }

        /// <summary>Constructs a float2 vector from a double2 vector by componentwise conversion.</summary>
        /// <param name="v">double2 to convert to float2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float2(double2 v)
        {
            this.x = (Single)v.x;
            this.y = (Single)v.y;
        }


        /// <summary>Implicitly converts a single Single value to a float2 vector by assigning it to every component.</summary>
        /// <param name="v">Single to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float2(Single v) { return new float2(v); }

        /// <summary>Explicitly converts a single bool value to a float2 vector by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">bool to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float2(bool v) { return new float2(v); }

        /// <summary>Explicitly converts a bool2 vector to a float2 vector by componentwise conversion.</summary>
        /// <param name="v">bool2 to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float2(bool2 v) { return new float2(v); }

        /// <summary>Implicitly converts a single int value to a float2 vector by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">int to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float2(int v) { return new float2(v); }

        /// <summary>Implicitly converts a int2 vector to a float2 vector by componentwise conversion.</summary>
        /// <param name="v">int2 to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float2(int2 v) { return new float2(v); }

        /// <summary>Implicitly converts a single uint value to a float2 vector by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">uint to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float2(uint v) { return new float2(v); }

        /// <summary>Implicitly converts a uint2 vector to a float2 vector by componentwise conversion.</summary>
        /// <param name="v">uint2 to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float2(uint2 v) { return new float2(v); }

        /// <summary>Implicitly converts a single half value to a float2 vector by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">half to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float2(half v) { return new float2(v); }

        /// <summary>Implicitly converts a half2 vector to a float2 vector by componentwise conversion.</summary>
        /// <param name="v">half2 to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float2(half2 v) { return new float2(v); }

        /// <summary>Explicitly converts a single double value to a float2 vector by converting it to Single and assigning it to every component.</summary>
        /// <param name="v">double to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float2(double v) { return new float2(v); }

        /// <summary>Explicitly converts a double2 vector to a float2 vector by componentwise conversion.</summary>
        /// <param name="v">double2 to convert to float2</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float2(double2 v) { return new float2(v); }


        /// <summary>Returns the result of a componentwise multiplication operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise multiplication.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise multiplication.</param>
        /// <returns>float2 result of the componentwise multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator * (float2 lhs, float2 rhs) { return new float2 (lhs.x * rhs.x, lhs.y * rhs.y); }

        /// <summary>Returns the result of a componentwise multiplication operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise multiplication.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise multiplication.</param>
        /// <returns>float2 result of the componentwise multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator * (float2 lhs, Single rhs) { return new float2 (lhs.x * rhs, lhs.y * rhs); }

        /// <summary>Returns the result of a componentwise multiplication operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise multiplication.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise multiplication.</param>
        /// <returns>float2 result of the componentwise multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator * (Single lhs, float2 rhs) { return new float2 (lhs * rhs.x, lhs * rhs.y); }


        /// <summary>Returns the result of a componentwise addition operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise addition.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise addition.</param>
        /// <returns>float2 result of the componentwise addition.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator + (float2 lhs, float2 rhs) { return new float2 (lhs.x + rhs.x, lhs.y + rhs.y); }

        /// <summary>Returns the result of a componentwise addition operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise addition.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise addition.</param>
        /// <returns>float2 result of the componentwise addition.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator + (float2 lhs, Single rhs) { return new float2 (lhs.x + rhs, lhs.y + rhs); }

        /// <summary>Returns the result of a componentwise addition operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise addition.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise addition.</param>
        /// <returns>float2 result of the componentwise addition.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator + (Single lhs, float2 rhs) { return new float2 (lhs + rhs.x, lhs + rhs.y); }


        /// <summary>Returns the result of a componentwise subtraction operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise subtraction.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise subtraction.</param>
        /// <returns>float2 result of the componentwise subtraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator - (float2 lhs, float2 rhs) { return new float2 (lhs.x - rhs.x, lhs.y - rhs.y); }

        /// <summary>Returns the result of a componentwise subtraction operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise subtraction.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise subtraction.</param>
        /// <returns>float2 result of the componentwise subtraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator - (float2 lhs, Single rhs) { return new float2 (lhs.x - rhs, lhs.y - rhs); }

        /// <summary>Returns the result of a componentwise subtraction operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise subtraction.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise subtraction.</param>
        /// <returns>float2 result of the componentwise subtraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator - (Single lhs, float2 rhs) { return new float2 (lhs - rhs.x, lhs - rhs.y); }


        /// <summary>Returns the result of a componentwise division operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise division.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise division.</param>
        /// <returns>float2 result of the componentwise division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator / (float2 lhs, float2 rhs) { return new float2 (lhs.x / rhs.x, lhs.y / rhs.y); }

        /// <summary>Returns the result of a componentwise division operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise division.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise division.</param>
        /// <returns>float2 result of the componentwise division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator / (float2 lhs, Single rhs) { return new float2 (lhs.x / rhs, lhs.y / rhs); }

        /// <summary>Returns the result of a componentwise division operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise division.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise division.</param>
        /// <returns>float2 result of the componentwise division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator / (Single lhs, float2 rhs) { return new float2 (lhs / rhs.x, lhs / rhs.y); }


        /// <summary>Returns the result of a componentwise modulus operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise modulus.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise modulus.</param>
        /// <returns>float2 result of the componentwise modulus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator % (float2 lhs, float2 rhs) { return new float2 (lhs.x % rhs.x, lhs.y % rhs.y); }

        /// <summary>Returns the result of a componentwise modulus operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise modulus.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise modulus.</param>
        /// <returns>float2 result of the componentwise modulus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator % (float2 lhs, Single rhs) { return new float2 (lhs.x % rhs, lhs.y % rhs); }

        /// <summary>Returns the result of a componentwise modulus operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise modulus.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise modulus.</param>
        /// <returns>float2 result of the componentwise modulus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator % (Single lhs, float2 rhs) { return new float2 (lhs % rhs.x, lhs % rhs.y); }


        /// <summary>Returns the result of a componentwise increment operation on a float2 vector.</summary>
        /// <param name="val">Value to use when computing the componentwise increment.</param>
        /// <returns>float2 result of the componentwise increment.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator ++ (float2 val) { return new float2 (++val.x, ++val.y); }


        /// <summary>Returns the result of a componentwise decrement operation on a float2 vector.</summary>
        /// <param name="val">Value to use when computing the componentwise decrement.</param>
        /// <returns>float2 result of the componentwise decrement.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator -- (float2 val) { return new float2 (--val.x, --val.y); }


        /// <summary>Returns the result of a componentwise less than operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise less than.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise less than.</param>
        /// <returns>bool2 result of the componentwise less than.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator < (float2 lhs, float2 rhs) { return new bool2 (lhs.x < rhs.x, lhs.y < rhs.y); }

        /// <summary>Returns the result of a componentwise less than operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise less than.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise less than.</param>
        /// <returns>bool2 result of the componentwise less than.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator < (float2 lhs, Single rhs) { return new bool2 (lhs.x < rhs, lhs.y < rhs); }

        /// <summary>Returns the result of a componentwise less than operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise less than.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise less than.</param>
        /// <returns>bool2 result of the componentwise less than.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator < (Single lhs, float2 rhs) { return new bool2 (lhs < rhs.x, lhs < rhs.y); }


        /// <summary>Returns the result of a componentwise less or equal operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise less or equal.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise less or equal.</param>
        /// <returns>bool2 result of the componentwise less or equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator <= (float2 lhs, float2 rhs) { return new bool2 (lhs.x <= rhs.x, lhs.y <= rhs.y); }

        /// <summary>Returns the result of a componentwise less or equal operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise less or equal.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise less or equal.</param>
        /// <returns>bool2 result of the componentwise less or equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator <= (float2 lhs, Single rhs) { return new bool2 (lhs.x <= rhs, lhs.y <= rhs); }

        /// <summary>Returns the result of a componentwise less or equal operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise less or equal.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise less or equal.</param>
        /// <returns>bool2 result of the componentwise less or equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator <= (Single lhs, float2 rhs) { return new bool2 (lhs <= rhs.x, lhs <= rhs.y); }


        /// <summary>Returns the result of a componentwise greater than operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise greater than.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise greater than.</param>
        /// <returns>bool2 result of the componentwise greater than.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator > (float2 lhs, float2 rhs) { return new bool2 (lhs.x > rhs.x, lhs.y > rhs.y); }

        /// <summary>Returns the result of a componentwise greater than operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise greater than.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise greater than.</param>
        /// <returns>bool2 result of the componentwise greater than.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator > (float2 lhs, Single rhs) { return new bool2 (lhs.x > rhs, lhs.y > rhs); }

        /// <summary>Returns the result of a componentwise greater than operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise greater than.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise greater than.</param>
        /// <returns>bool2 result of the componentwise greater than.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator > (Single lhs, float2 rhs) { return new bool2 (lhs > rhs.x, lhs > rhs.y); }


        /// <summary>Returns the result of a componentwise greater or equal operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise greater or equal.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise greater or equal.</param>
        /// <returns>bool2 result of the componentwise greater or equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator >= (float2 lhs, float2 rhs) { return new bool2 (lhs.x >= rhs.x, lhs.y >= rhs.y); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise greater or equal.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise greater or equal.</param>
        /// <returns>bool2 result of the componentwise greater or equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator >= (float2 lhs, Single rhs) { return new bool2 (lhs.x >= rhs, lhs.y >= rhs); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise greater or equal.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise greater or equal.</param>
        /// <returns>bool2 result of the componentwise greater or equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator >= (Single lhs, float2 rhs) { return new bool2 (lhs >= rhs.x, lhs >= rhs.y); }


        /// <summary>Returns the result of a componentwise unary minus operation on a float2 vector.</summary>
        /// <param name="val">Value to use when computing the componentwise unary minus.</param>
        /// <returns>float2 result of the componentwise unary minus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator - (float2 val) { return new float2 (-val.x, -val.y); }


        /// <summary>Returns the result of a componentwise unary plus operation on a float2 vector.</summary>
        /// <param name="val">Value to use when computing the componentwise unary plus.</param>
        /// <returns>float2 result of the componentwise unary plus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 operator + (float2 val) { return new float2 (+val.x, +val.y); }


        /// <summary>Returns the result of a componentwise equality operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise equality.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise equality.</param>
        /// <returns>bool2 result of the componentwise equality.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator == (float2 lhs, float2 rhs) { return new bool2 (lhs.x == rhs.x, lhs.y == rhs.y); }

        /// <summary>Returns the result of a componentwise equality operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise equality.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise equality.</param>
        /// <returns>bool2 result of the componentwise equality.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator == (float2 lhs, Single rhs) { return new bool2 (lhs.x == rhs, lhs.y == rhs); }

        /// <summary>Returns the result of a componentwise equality operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise equality.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise equality.</param>
        /// <returns>bool2 result of the componentwise equality.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator == (Single lhs, float2 rhs) { return new bool2 (lhs == rhs.x, lhs == rhs.y); }


        /// <summary>Returns the result of a componentwise not equal operation on two float2 vectors.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise not equal.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise not equal.</param>
        /// <returns>bool2 result of the componentwise not equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator != (float2 lhs, float2 rhs) { return new bool2 (lhs.x != rhs.x, lhs.y != rhs.y); }

        /// <summary>Returns the result of a componentwise not equal operation on a float2 vector and a Single value.</summary>
        /// <param name="lhs">Left hand side float2 to use to compute componentwise not equal.</param>
        /// <param name="rhs">Right hand side Single to use to compute componentwise not equal.</param>
        /// <returns>bool2 result of the componentwise not equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator != (float2 lhs, Single rhs) { return new bool2 (lhs.x != rhs, lhs.y != rhs); }

        /// <summary>Returns the result of a componentwise not equal operation on a Single value and a float2 vector.</summary>
        /// <param name="lhs">Left hand side Single to use to compute componentwise not equal.</param>
        /// <param name="rhs">Right hand side float2 to use to compute componentwise not equal.</param>
        /// <returns>bool2 result of the componentwise not equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2 operator != (Single lhs, float2 rhs) { return new bool2 (lhs != rhs.x, lhs != rhs.y); }




        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xxxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, x, x, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xxxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, x, x, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xxyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, x, y, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xxyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, x, y, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xyxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, y, x, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xyxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, y, x, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xyyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, y, y, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 xyyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(x, y, y, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yxxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, x, x, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yxxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, x, x, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yxyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, x, y, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yxyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, x, y, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yyxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, y, x, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yyxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, y, x, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yyyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, y, y, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float4 yyyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float4(y, y, y, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 xxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(x, x, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 xxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(x, x, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 xyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(x, y, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 xyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(x, y, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 yxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(y, x, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 yxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(y, x, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 yyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(y, y, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float3 yyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float3(y, y, y); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float2 xx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float2(x, x); }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float2 xy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float2(x, y); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { x = value.x; y = value.y; }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float2 yx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float2(y, x); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { y = value.x; x = value.y; }
        }


        /// <summary>Swizzles the vector.</summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public float2 yy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new float2(y, y); }
        }



        /// <summary>Returns the Single element at a specified index.</summary>
        unsafe public Single this[int index]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 2)
                    throw new System.ArgumentException("index must be between[0...1]");
#endif
                fixed (float2* array = &this) { return ((Single*)array)[index]; }
            }
            set
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 2)
                    throw new System.ArgumentException("index must be between[0...1]");
#endif
                fixed (Single* array = &x) { array[index] = value; }
            }
        }

        /// <summary>Returns true if the float2 is equal to a given float2, false otherwise.</summary>
        /// <param name="rhs">Right hand side argument to compare equality with.</param>
        /// <returns>The result of the equality comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(float2 rhs) { return x == rhs.x && y == rhs.y; }

        public bool Equals(float3 other) => throw new NotImplementedException();

        /// <summary>Returns true if the float2 is equal to a given float2, false otherwise.</summary>
        /// <param name="o">Right hand side argument to compare equality with.</param>
        /// <returns>The result of the equality comparison.</returns>
        public override bool Equals(object o) { return o is float2 converted && Equals(converted); }


        /// <summary>Returns a hash code for the float2.</summary>
        /// <returns>The computed hash code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() { return (int)math.hash(this); }


        /// <summary>Returns a string representation of the float2.</summary>
        /// <returns>String representation of the value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("float2({0}f, {1}f)", x, y);
        }

        /// <summary>Returns a string representation of the float2 using a specified format and culture-specific format information.</summary>
        /// <param name="format">Format string to use during string formatting.</param>
        /// <param name="formatProvider">Format provider to use during string formatting.</param>
        /// <returns>String representation of the value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("float2({0}f, {1}f)", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

        internal sealed class DebuggerProxy
        {
            public Single x;
            public Single y;
            public DebuggerProxy(float2 v)
            {
                x = v.x;
                y = v.y;
            }
        }
}
