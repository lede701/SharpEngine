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
		Vector2D Scale { get; }
		float Rotation { get; }
		ObjectType Type { get; set; }
		int Layer { get; set; }
	}
}
