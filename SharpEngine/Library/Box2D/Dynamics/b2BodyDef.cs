using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Box2D.Dynamics
{
	public struct b2BodyDef
	{
		public b2Body.b2BodyType Type;
		public Vector2D position;
		public float angle;
		public Vector2D LinearVelocity;
		public float AngularVelocity;
		public float LinearDampining;
		public float AngualerDampining;
		public bool AllowSleep;
		public bool Awake;
		public bool FixedRotation;
		public bool Bullet;
		public bool Active;
		public Object UserData;
		public float GravityScale;
	}
}
