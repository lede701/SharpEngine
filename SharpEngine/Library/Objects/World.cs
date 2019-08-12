using SharpEngine.Library.Math;
using System.Drawing;

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
			_worldSize = Vector2D.Zero;
			_screenSize = Vector2D.Zero;
			_gravity = Vector2D.Zero;
			_wind = Vector2D.Zero;
			_worldPos = Vector2D.Zero;
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

		private Vector2D _screenSize;
		public Vector2D ScreenSize
		{
			get
			{
				return _screenSize;
			}
			set
			{
				_screenSize = value;
			}
		}

		private Vector2D _worldPos;
		public Vector2D WorldPosition
		{
			get
			{
				return _worldPos;
			}
			set
			{
				_worldPos = value;
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

		public Vector2D ToScreen(Vector2D vec)
		{
			Vector2D retVec = new Vector2D(vec);
			switch(vec.Type)
			{
				case VectorType.WORLD:
					{
						retVec.X -= WorldPosition.X;
						retVec.Y -= WorldPosition.Y;
						retVec.Type = VectorType.SCREEN;
					}
					break;
			}

			return retVec;
		}

		public Vector2D ToWorld(Vector2D vec)
		{
			Vector2D retVec = new Vector2D(vec);

			switch (vec.Type)
			{
				case VectorType.SCREEN:
					{
						retVec.X += WorldPosition.X;
						retVec.Y += WorldPosition.Y;
						retVec.Type = VectorType.WORLD;
					}
					break;
			}

			return retVec;
		}
	}
}
