using SharpEngine.Library.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpEngine.Library.Objects
{
	public class World
	{
		private static World _world;
		public static World Instance
		{
			get
			{
				if(_world == null)
				{
					_world = new World();
				}

				return _world;
			}
		}

		private World()
		{
			_worldSize = new Vector2D();
			_gravity = new Vector2D();
			_wind = new Vector2D();
		}

		private Vector2D _worldSize;
		public Vector2D WorldSize
		{
			get
			{
				return _worldSize;
			}
			set
			{
				_worldSize = value;
			}
		}

		private Vector2D _gravity;
		public Vector2D Gravity
		{
			get
			{
				return _gravity;
			}
		}

		private Vector2D _wind;
		public Vector2D Wind
		{
			get
			{
				return _wind;
			}
		}

		private Rectangle _boundary;
		public Rectangle WorldBoundary
		{
			get
			{
				return _boundary;
			}
			set
			{
				_boundary = value;
			}
		}

		private Vector2D _centerOfWorld;
		public Vector2D CenterOfWorld
		{
			get
			{
				if(_centerOfWorld == null)
				{
					_centerOfWorld = new Vector2D();
				}
				_centerOfWorld.X = _worldSize.X / 2;
				_centerOfWorld.Y = _worldSize.Y / 2;
				return _centerOfWorld;
			}
		}
	}
}
