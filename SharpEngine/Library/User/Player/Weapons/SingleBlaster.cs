﻿using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.User.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Player.Weapons
{
	public class SingleBlaster : IWeapon, IDisposable
	{
		private String _key;
		public string Key
		{
			get
			{
				return _key;
			}
		}

		public SingleBlaster(float width)
		{
			_firePos = new Vector2D { X = (width / 2f)-10, Y = -5f };
		}

		private Vector2D _firePos;

		public UObject CreateBolt(Vector2D position, ref PlayerStatistics stats)
		{
			// Spawn blaster object
			ShipBlaster bolt = new ShipBlaster(false);
			bolt.Position.X = position.X +_firePos.X;
			bolt.Position.Y = position.Y + _firePos.Y;
			stats.WeaponDamage = 1.2f;
			stats.WeaponEnergyUse = 3.5f;

			//bolt.Position.X = Position.X + (_canon ? offset : Sprite.Frame.Width * Scale.X - offset);
			//_canon = !_canon;
			//bolt.Position.Y = Position.Y - 5.0f;

			bolt.Velocity.Y = -12.0f;
			bolt.Type = ObjectType.MISSLE;
			stats.Score -= 15;
			bolt.Damage = stats.Fire;
			return bolt;
		}

		public void Dispose()
		{
			
		}
	}
}
