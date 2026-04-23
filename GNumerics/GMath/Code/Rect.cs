using System.Globalization;

namespace Gal.Core
{
	/// <summary>
	///
	/// </summary>
	/// <author>gouanlin</author>
	[Serializable]
	public struct Rect : IEquatable<Rect>, IFormattable
	{
		private Single _xMin;
		private Single _yMin;
		private Single _width;
		private Single _height;

		public Rect(Single x, Single y, Single width, Single height) {
			_xMin = x;
			_yMin = y;
			_width = width;
			_height = height;
		}

		public Rect(Vec2 position, Vec2 size) {
			_xMin = position.x;
			_yMin = position.y;
			_width = size.x;
			_height = size.y;
		}

		public Rect(Rect source) {
			_xMin = source._xMin;
			_yMin = source._yMin;
			_width = source._width;
			_height = source._height;
		}

		public static Rect Zero => new(0, 0, 0, 0);

		public static Rect MinMaxRect(Single xMin, Single yMin, Single xMax, Single yMax) => new(xMin, yMin, xMax - xMin, yMax - yMin);

		public void Set(Single x, Single y, Single width, Single height) {
			_xMin = x;
			_yMin = y;
			_width = width;
			_height = height;
		}

		public Single X {
			get => _xMin;
			set => _xMin = value;
		}

		public Single Y {
			get => _yMin;
			set => _yMin = value;
		}

		public Vec2 Position {
			get => new(_xMin, _yMin);
			set {
				_xMin = value.x;
				_yMin = value.y;
			}
		}

		public Vec2 Center {
			get => new(X + _width / 2, Y + _height / 2);
			set {
				_xMin = value.x - _width / 2;
				_yMin = value.y - _height / 2;
			}
		}

		public Vec2 Min {
			get => new(XMin, YMin);
			set {
				XMin = value.x;
				YMin = value.y;
			}
		}

		public Vec2 Max {
			get => new(XMax, YMax);
			set {
				XMax = value.x;
				YMax = value.y;
			}
		}

		public Single Width {
			get => _width;
			set => _width = value;
		}

		public Single Height {
			get => _height;
			set => _height = value;
		}

		public Vec2 Size {
			get => new(_width, _height);
			set {
				_width = value.x;
				_height = value.y;
			}
		}

		public Single XMin {
			get => _xMin;
			set {
				var xMax = this.XMax;
				_xMin = value;
				_width = xMax - _xMin;
			}
		}

		public Single YMin {
			get => _yMin;
			set {
				var yMax = this.YMax;
				_yMin = value;
				_height = yMax - _yMin;
			}
		}

		public Single XMax {
			get => _width + _xMin;
			set => _width = value - _xMin;
		}

		public Single YMax {
			get => _height + _yMin;
			set => _height = value - _yMin;
		}

		public bool Contains(Vec2 point) => point.x >= XMin && point.x < XMax && point.y >= YMin && point.y < YMax;

		private static Rect OrderMinMax(Rect rect) {
			if (rect.XMin > rect.XMax) (rect.XMin, rect.XMax) = (rect.XMax, rect.XMin);
			if (rect.YMin > rect.YMax) (rect.YMin, rect.YMax) = (rect.YMax, rect.YMin);
			return rect;
		}

		public bool Overlaps(in Rect other) =>
            other.XMax >= XMin &&
            other.XMin <= XMax &&
            other.YMax >= YMin &&
            other.YMin <= YMax;

		public bool Overlaps(Rect other, bool allowInverse) {
			var rectFixed = this;
			if (allowInverse) {
				rectFixed = OrderMinMax(rectFixed);
				other = OrderMinMax(other);
			}
			return rectFixed.Overlaps(other);
		}

		public static bool operator !=(Rect lhs, Rect rhs) => !(lhs == rhs);

		public static bool operator ==(Rect lhs, Rect rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Width == rhs.Width && lhs.Height == rhs.Height;

		public override int GetHashCode() {
			var num1 = X;
			var hashCode = num1.GetHashCode();
			num1 = Width;
			var num2 = num1.GetHashCode() << 2;
			var num3 = hashCode ^ num2;
			num1 = Y;
			var num4 = num1.GetHashCode() >> 2;
			var num5 = num3 ^ num4;
			num1 = Height;
			var num6 = num1.GetHashCode() >> 1;
			return num5 ^ num6;
		}

		public override bool Equals(object other) => other is Rect other1 && Equals(other1);

		public bool Equals(Rect other) {
			int num1;
			if (X.Equals(other.X)) {
				var num2 = Y;
				if (num2.Equals(other.Y)) {
					num2 = Width;
					if (num2.Equals(other.Width)) {
						num2 = Height;
						num1 = num2.Equals(other.Height) ? 1 : 0;
						goto label_5;
					}
				}
			}
			num1 = 0;
		label_5:
			return num1 != 0;
		}

		public override string ToString() => ToString(null, null);

		public string ToString(string format) => ToString(format, null);

		public string ToString(string format, IFormatProvider formatProvider) {
			if (string.IsNullOrEmpty(format)) format = "F2";
			formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
			return $"(x:{X.ToString(format, formatProvider)}, y:{Y.ToString(format, formatProvider)}, width:{Width.ToString(format, formatProvider)}, height:{Height.ToString(format, formatProvider)})";
		}
	}
}
