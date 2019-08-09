﻿using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math.Physics;
using SharpEngine.Library.Objects;
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

		public PhysicsFactory PhysicsFactory { get; set; }

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
			CircleCollider collider = (CircleCollider)PhysicsFactory.CreateCircleCollider(sprite.Position, 40);
			collider.Center.X = 125f / 2f;
			collider.Center.Y = 125f / 2f;

			sprite.Collider = PhysicsFactory.CreateCircleCollider(sprite.Position, 40);

			return sprite;
		}

		private Sprite _playerSprite;

		public SpriteShip CreatePlayer(float x, float y)
		{
			if (_playerSprite == null)
			{
				String heroPath = String.Format("{0}\\Hero\\fighter.png", AssetPath);
				_playerSprite = GraphicsManager.Instance.LoadSpriteFromImagePath(heroPath);
			}

			SpriteShip ship = new SpriteShip(_playerSprite);


			return ship;
		}

		public ShipBlaster CreateBlaster(float x, float y, float radius, ObjectType type)
		{
			ShipBlaster bolt = new ShipBlaster();
			bolt.Position.X = x;
			bolt.Position.Y = y;
			bolt.Collider = PhysicsFactory.CreateCircleCollider(bolt.Position, radius);
			((CircleCollider)bolt.Collider).Center.X = 1.5f;
			((CircleCollider)bolt.Collider).Center.Y = 1.5f;

			return bolt;
		}
	}
}
