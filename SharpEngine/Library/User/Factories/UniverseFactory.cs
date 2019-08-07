using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.User.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Factories
{
	public class UniverseFactory
	{
		private static UniverseFactory _instance;
		public static UniverseFactory Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new UniverseFactory();
				}
				return _instance;
			}
		}
		private UniverseFactory()
		{

		}

		public String AssetPath;
		private Sprite _spriteAsteroid;
		public SpriteAsteroid CreateAsteroid(float x, float y, float rotation)
		{
			if (_spriteAsteroid == null)
			{
				_spriteAsteroid = GraphicsManager.Instance.LoadSpriteFromImagePath(String.Format("{0}\\Spritesheets\\asteroid_01.png", AssetPath));
				_spriteAsteroid.Frames.Add(new System.Drawing.Rectangle { X = 0, Y = 0, Width = 120, Height = 120 });
			}

			SpriteAsteroid sprite = new SpriteAsteroid(_spriteAsteroid);
			sprite.Position.X = x;
			sprite.Position.Y = y;
			sprite.Velocity.X = 0f;
			sprite.Velocity.Y = 0f;

			return sprite;
		}
	}
}
