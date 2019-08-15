using SharpEngine.Library.Math;
using SharpEngine.Library.Math.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Data.Trees
{
	public class TRectangle
	{
		public Vector2D LeftTop { get; set; }
		public Vector2D RightBottom { get; set; }

		public TRectangle()
		{
			LeftTop = Vector2D.Zero;
			RightBottom = Vector2D.Zero;
		}

		public TRectangle(Vector2D leftTop, Vector2D rightBottom)
		{
			LeftTop = Vector2D.Zero;
			LeftTop.X = leftTop.X < rightBottom.X ? leftTop.X : rightBottom.X;
			LeftTop.Y = leftTop.Y < rightBottom.Y ? leftTop.Y : rightBottom.X;
			RightBottom.X = leftTop.X > rightBottom.X ? leftTop.X : rightBottom.X;
			RightBottom.Y = leftTop.Y > rightBottom.Y ? leftTop.Y : rightBottom.X;
		}

		public double Area
		{
			get
			{
				return (double)Height * Width;
			}
		}

		public float Height
		{
			get
			{
				return RightBottom.Y - LeftTop.Y;
			}
		}

		public float Width
		{
			get
			{
				return RightBottom.X - LeftTop.X;
			}
		}
		public ICollider Collider { get; set; }

		public double GetEnlargementArea(TRectangle rectToFit)
		{
			return System.Math.Abs(rectToFit.Area - Area);
		}

		public void Merge(TRectangle rectToMerge)
		{
			LeftTop.X = rectToMerge.LeftTop.X < LeftTop.X ? rectToMerge.LeftTop.X : LeftTop.X;
			LeftTop.Y = rectToMerge.LeftTop.Y < LeftTop.Y ? rectToMerge.LeftTop.Y : LeftTop.Y;
			RightBottom.X = rectToMerge.RightBottom.X > RightBottom.X ? rectToMerge.RightBottom.X : RightBottom.X;
			RightBottom.Y = rectToMerge.RightBottom.Y > RightBottom.Y ? rectToMerge.RightBottom.Y : RightBottom.Y;
		}
	}
}
