using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Events
{
	public class CollisionEventArgs : EventArgs
	{
		public enum HitLocation
		{
			Top,
			Bottom,
			Left,
			Right
		};

		public UObject Who;
		public Vector2D Point;
		public HitLocation Location;
	}
}
