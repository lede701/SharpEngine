using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Box2D.Collision
{
	public struct b2AABB
	{
		public bool IsValid;
		public Vector2D Center
		{
			get
			{
				return (LowerBound + UpperBound) * 0.5f;
			}
		}

		public Vector2D Extents
		{
			get
			{
				return (UpperBound - LowerBound) * 0.5f;
			}
		}

		public float Perimeter
		{
			get
			{
				float wx = UpperBound.X - LowerBound.X;
				float wy = UpperBound.Y - LowerBound.Y;
				return 2f * (wx + wy);
			}
		}

		public void Combine(b2AABB aabb)
		{
			LowerBound = Vector2D.Min(LowerBound, aabb.LowerBound);
			UpperBound = Vector2D.Max(UpperBound, aabb.UpperBound);
		}

		public bool Contains(b2AABB aabb)
		{
			bool result = true;

			result = result && LowerBound.X <= aabb.LowerBound.X;
			result = result && LowerBound.Y <= aabb.LowerBound.Y;
			result = result && aabb.UpperBound.X <= UpperBound.X;
			result = result && aabb.UpperBound.Y <= UpperBound.Y;

			return result;
		}

		public bool RayCast(out b2RayCastOutput output, b2RayCastInput input)
		{
			float tmin = -float.MaxValue;
			float tmax = float.MaxValue;

			Vector2D p = input.p1;
			Vector2D d = input.p2 - input.p1;
			Vector2D absD = Vector2D.Abs(d);

			Vector2D normal = Vector2D.Zero;
			for(int i=0; i<2; ++i)
			{
				if (absD[i] < float.Epsilon)
				{
					if (p[i] < LowerBound[i] || UpperBound[i] < p[i])
					{
						output.fraction = tmin;
						output.Normal = null;
						return false;
					}
				} else
				{
					float inv_d = 1.0f / d[i];
					float t1 = (LowerBound[i] - p[i]) * inv_d;
					float t2 = (UpperBound[i] - p[i]) * inv_d;

					// Sign of the normal vetor
					float s = -1.0f;
					if(t1 > t2)
					{
						// Swap t values
						float tmp = t1;
						t1 = t2;
						t2 = t1;
						s = 1.0f;
					}
					// Push the main up
					if(t1 > tmin)
					{
						normal = Vector2D.Zero;
						normal[i] = s;
						tmin = t1;
					}
					// Pull the max down
					tmax = System.Math.Min(tmax, t2);

					if(tmin > tmax)
					{
						output.fraction = 0f;
						output.Normal = null;
						return false;
					}
				}
			}

			if(tmin < 0.0f || input.maxFraction < tmin)
			{
				output.fraction = 0.0f;
				output.Normal = null;
				return false;
			}

			output.fraction = tmin;
			output.Normal = normal;
			return true;
		}

		public Vector2D LowerBound;
		public Vector2D UpperBound;
	}
}
