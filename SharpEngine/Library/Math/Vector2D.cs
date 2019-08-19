using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class Vector2D
	{
		public float X;
		public float Y;
		public VectorType Type = VectorType.WORLD;
		public static float Lerp(float v1, float v2, float t)
		{
			return (1.0f - t) * v1 + t * v1;
		}

		public Vector2D()
		{

		}
		public Vector2D(float x, float y)
		{
			X = x;
			Y = y;
		}
		public Vector2D(Vector2D vec)
		{
			X = vec.X;
			Y = vec.Y;
			Type = vec.Type;
		}

		public static Vector2D Zero
		{
			get
			{
				return new Vector2D { X = 0, Y = 0 };
			}
		}

		public static Vector2D One
		{
			get
			{
				return new Vector2D { X = 1f, Y = 1f };
			}
		}

		public float Distance(Vector2D p1)
		{
			return (float)System.Math.Sqrt(Square(X - p1.X) + Square(Y - p1.Y));
		}

		public Vector2D Subtract(Vector2D p1)
		{
			return new Vector2D { X = X - p1.X, Y = Y - p1.Y };
		}

		public Vector2D Add(Vector2D p1)
		{
			return new Vector2D { X = X + p1.X, Y = Y + p1.Y };
		}

		public Vector2D Normal()
		{
			float length = (float)System.Math.Sqrt(Square(X) + Square(Y));
			return new Vector2D { X = X / length, Y = Y / length };
		}

		public Vector2D Scale(float s)
		{
			return new Vector2D { X = X * s, Y = Y * s };
		}

		public Vector2D MidPoint(Vector2D p1)
		{
			return new Vector2D { X = (X - p1.X) / 2, Y = (Y - p1.Y) / 2 };
		}

		public Vector2D RotateVector(Vector2D pt, float angle)
		{
			float x = pt.X - X;
			float y = pt.Y - Y;
			float xRot = (float)(x * System.Math.Cos(angle) + y * System.Math.Sin(angle));
			float yRot = (float)(y * System.Math.Cos(angle) - x * System.Math.Sin(angle));
			return new Vector2D { X = X + xRot, Y = Y + yRot };
		}

		public float Square(float val)
		{
			return val * val;
		}

		public float Length
		{
			get
			{
				return (float)System.Math.Sqrt((X * X) + (Y * Y));
			}
		}

		public float this[int idx]
		{
			get
			{
				return idx == 0 ? X : Y;
			}
			set
			{
				if (idx == 0)
				{
					X = value;
				}
				else if (idx == 1)
				{
					Y = value;
				}
			}
		}

		public static Vector2D operator +(Vector2D v1, Vector2D v2)
		{
			Vector2D plus = new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
			return plus;
		}
		public static Vector2D operator -(Vector2D v1, Vector2D v2)
		{
			Vector2D minus = new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
			return minus;
		}

		public static Vector2D operator *(Vector2D vec, int val)
		{
			return new Vector2D(vec.X * val, vec.Y * val);
		}

		public static Vector2D operator *(Vector2D vec, float val)
		{
			return new Vector2D(vec.X * val, vec.Y * val);
		}

		public static Vector2D Abs(Vector2D v)
		{
			return new Vector2D(System.Math.Abs(v.X), System.Math.Abs(v.Y));
		}

		public static Vector2D Min(Vector2D v1, Vector2D v2)
		{
			return new Vector2D(System.Math.Min(v1.X, v2.X), System.Math.Min(v1.Y, v2.Y));
		}

		public static Vector2D Max(Vector2D v1, Vector2D v2)
		{
			return new Vector2D(System.Math.Max(v1.X, v2.X), System.Math.Max(v1.Y, v2.Y));
		}

		public static float Dot(Vector2D v1, Vector2D v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

	}
}
