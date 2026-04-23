using System.Globalization;
using System.Runtime.InteropServices;

namespace SeaWarMath {
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
	public struct Vector2 : IEquatable<Vector2>, IFormattable
	{
		[FieldOffset(0)]
		public Single x;
#if USE_FIXED64
		[FieldOffset(8)]
#else
		[FieldOffset(4)]
#endif
		public Single y;

		public Single this[int index] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get =>
                index switch {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException("Invalid Vector2Fixed index!")
                };
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				switch (index) {
					case 0:
						x = value;
						break;
					case 1:
						y = value;
						break;
					default:
						throw new IndexOutOfRangeException("Invalid Vector2Fixed index!");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2(Single x, Single y) {
			this.x = x;
			this.y = y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(Single newX, Single newY) {
			x = newX;
			y = newY;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Lerp(Vector2 a, Vector2 b, Single t) {
			t = Math.Clamp(t, 0, 1);
			return new(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, Single t) => new(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 MoveTowards(Vector2 current, Vector2 target, Single maxDistanceDelta) {
			var num1 = target.x - current.x;
			var num2 = target.y - current.y;
			var d = num1 * num1 + num2 * num2;
			if (d == 0 || maxDistanceDelta >= 0 && d <= maxDistanceDelta * maxDistanceDelta) return target;
			var num3 = (Single)Math.Sqrt(d);
			return new(current.x + num1 / num3 * maxDistanceDelta, current.y + num2 / num3 * maxDistanceDelta);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Scale(Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scale(Vector2 scale) {
			x *= scale.x;
			y *= scale.y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Normalize() {
			var magnitude = this.Magnitude;
			if (magnitude != 0)
				this = this / magnitude;
			else
				this = Zero;
		}

		public Vector2 Normalized {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				var normalized = new Vector2(x, y);
				normalized.Normalize();
				return normalized;
			}
		}

		public override string ToString() => ToString(null, null);

		public string ToString(string format) => ToString(format, null);

		public string ToString(string format, IFormatProvider formatProvider) {
			if (string.IsNullOrEmpty(format)) format = "F2";
			formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
			return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other) => other is Vector2 other1 && Equals(other1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2 other) => x == other.x && y == other.y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal) {
			var num = -2 * Dot(inNormal, inDirection);
			return new(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(Vector2 inDirection) => new(-inDirection.y, inDirection.x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Single Dot(Vector2 lhs, Vector2 rhs) => lhs.x * rhs.x + lhs.y * rhs.y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Single Cross(Vector2 lhs, Vector2 rhs) => lhs.x * rhs.y - lhs.y * rhs.x;

		public readonly Single Magnitude {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (Single)Math.Sqrt(x * x + y * y);
		}

		public Single SqrMagnitude {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => x * x + y * y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Single Angle(Vector2 from, Vector2 to) {
			var num = (Single)Math.Sqrt(from.SqrMagnitude * to.SqrMagnitude);
			return (Single)Math.Acos(Math.Clamp(Dot(from, to) / num, -1, 1)) * Math.Rad2Deg;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Single SignedAngle(Vector2 from, Vector2 to) => Angle(from, to) * Math.Sign(from.x * to.y - from.y * to.x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Single Distance(Vector2 a, Vector2 b) {
			var num1 = a.x - b.x;
			var num2 = a.y - b.y;
			return (Single)Math.Sqrt(num1 * num1 + num2 * num2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ClampMagnitude(Vector2 vector, Single maxLength) {
			var sqrMagnitude = vector.SqrMagnitude;
			if (sqrMagnitude <= maxLength * maxLength) return vector;
			var num1 = (Single)Math.Sqrt(sqrMagnitude);
			var num2 = vector.x / num1;
			var num3 = vector.y / num1;
			return new(num2 * maxLength, num3 * maxLength);
		}

		public readonly Vector2 GetNormalizedAndLength(out Single length) {
			length = Magnitude;
			return length > 0 ? new(x / length, y / length) : Zero;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Min(Vector2 lhs, Vector2 rhs) => new(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Max(Vector2 lhs, Vector2 rhs) => new(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.x + b.x, a.y + b.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.x - b.x, a.y - b.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.x / b.x, a.y / b.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 a) => new(-a.x, -a.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 a, Single d) => new(a.x * d, a.y * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Single d, Vector2 a) => new(a.x * d, a.y * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 a, Single d) => new(a.x / d, a.y / d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2 lhs, Vector2 rhs) => lhs.x == rhs.x && lhs.y == rhs.y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

		/// <summary>
		///  The triple product of {@link Vector2}s is defined as:
		/// <pre>
		///	a x (b x c)
		///	 </pre>
		///	However, this method performs the following triple product:
		///	<pre>
		///	(a x b) x c
		///		</pre>
		///	this can be simplified to:
		///	<pre>
		///	-a * (b &middot; c) + b * (a &middot; c)
		///	</pre>
		///	or:
		///	<pre>
		///	b * (a &middot; c) - a * (b &middot; c)
		///	</pre>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c) {
			// expanded version of above formula
			Vector2 r = new Vector2();

			/*
			 * In the following we can substitute ac and bc  r.x and r.y
			 * and with some rearrangement get a much more efficient version
			 *
			 * double ac = a.x * c.x + a.y * c.y;
			 * double bc = b.x * c.x + b.y * c.y;
			 * r.x = b.x * ac - a.x * bc;
			 * r.y = b.y * ac - a.y * bc;
			 */

			Single dot = a.x * b.y - b.x * a.y;
			r.x = -c.y * dot;
			r.y = c.x * dot;

			return r;
		}

		public static Vector2 Zero {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		} = new(0, 0);

		public static Vector2 One {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		} = new(1, 1);

		public static Vector2 Up {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		} = new(0, 1);

		public static Vector2 Down {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		} = new(0, -1);

		public static Vector2 Left {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		} = new(-1, 0);

		public static Vector2 Right {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		} = new(1, 0);
	}
}
