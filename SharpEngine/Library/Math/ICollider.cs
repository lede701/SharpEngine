using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math
{
	public interface ICollider
	{
		UObject Owner { get; set; }
		Collider2DType Type { get; }
		bool Hit(ICollider other);
		bool Hit(UObject obj);
		event EventHandler CollisionEvent;
		
	}
}
