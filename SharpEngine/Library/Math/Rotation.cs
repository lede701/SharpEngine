using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public class Rotation
	{
		public float s;
		public float c;

		public Rotation()
		{

		}

		public Rotation(float angle)
		{
			Set(angle);
		}

		public void Set(float angle)
		{
			s = (float)System.Math.Sin(angle);
			c = (float)System.Math.Cos(angle);
		}

		public void SetIdentity()
		{
			s = 0.0f;
			c = 1.0f;
		}

		public float Angle
		{
			get
			{
				return (float)System.Math.Atan2(s, c);
			}
			set
			{
				Set(value);
			}
		}

		public Vector2D XAxis
		{
			get
			{
				return new Vector2D(c, s);
			}
		}

		public Vector2D YAxis
		{
			get
			{
				return new Vector2D(-s, c);
			}
		}
	}
}
