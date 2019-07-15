using SharpEngine.Library.Controller;
using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Objects
{
	public interface UObject : GObject
	{
		IController Controller { get; set; }
		ICollider Collider { get; set; }
		Transform Transform { get; }
		Vector2D Position { get; }
		Vector2D Velocity { get; }
		float Rotation { get; }

	}
}
