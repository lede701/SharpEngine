using SharpEngine.Library.Box2D.Collision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Box2D.Dynamics
{
	public class b2Body
	{
		public enum b2BodyType
		{
			b2_STATICBODY = 0,
			b2_KINEMATICBODY,
			b2_DYNAMICBODY
		}

		public b2Fixture CreateFixture(b2FixtureDef def)
		{
			return null;
		}

		public b2Fixture CreateFixture(b2IShape shape, float density)
		{
			return null;
		}


	}
}
