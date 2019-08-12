﻿using SharpEngine.Library.Events;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Math.Physics
{
	public interface ICollider
	{
		String Key { get; }
		Vector2D Position { get; set; }
		bool IsActive { get; set; }
		UObject Owner { get; set; }
		Collider2DType Type { get; }
		bool Hit(ICollider other);
		bool Hit(UObject obj);
		event EventHandler CollisionEvent;
		void CallCollisionEvent(Object obj, CollisionEventArgs e);
		
	}
}