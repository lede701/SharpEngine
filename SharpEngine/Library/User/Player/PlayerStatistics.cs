using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Player
{
	public class PlayerStatistics
	{
		public String Name;
		public int Score;
		public int Lives;
		public int Level;
		public int Experience;
		public int NextExperienceLevel;

		public float ShieldEnergy;
		public float MaxShieldEnergy;
		public float ShieldEnergyRechargeRate;

		public float WeaponEnergy;
		public float WeaponEnergyRechargeRate;
		public float MaxWeaponEnergy;
		public float WeaponEnergyUse;
		public float WeaponDamage;
		public float WeaponPauseRate;

		public PlayerStatistics()
		{
			Name = "Norbert";
			Score = 0;
			Level = 1;
			Lives = 3;
			Experience = 0;
			NextExperienceLevel = 500;

			ShieldEnergy = 1.0f;
			MaxShieldEnergy = 5.0f;
			ShieldEnergyRechargeRate = 0.001f;

			WeaponEnergy = 50.0f;
			MaxWeaponEnergy = 50.0f;
			WeaponEnergyRechargeRate = 0.13f;
			WeaponEnergyUse = 3.0f;
			WeaponDamage = 2.0f;
			WeaponPauseRate = 50f;
		}

		public void Update(float deltaTime)
		{
			ShieldEnergy = System.Math.Min(ShieldEnergy + (ShieldEnergyRechargeRate * deltaTime), MaxShieldEnergy);
			WeaponEnergy = System.Math.Min(WeaponEnergy + (WeaponEnergyRechargeRate * deltaTime), MaxWeaponEnergy);
			_frameCnt = _frameCnt + (4 * deltaTime);
		}

		private float _frameCnt;
		public bool CanFire
		{
			get
			{
				return WeaponEnergy > WeaponEnergyUse && _frameCnt > WeaponPauseRate;
			}
		}

		public float Fire
		{
			get
			{
				float damage = 0.0f;
				if (WeaponEnergy > WeaponEnergyUse)
				{
					WeaponEnergy -= WeaponEnergyUse;
					damage = WeaponDamage;
				}
				_frameCnt = 0;

				return damage;
			}
		}

		public bool TakeDamage(float damage)
		{
			bool bRetVal = ShieldEnergy - damage > 0;
			if(bRetVal)
			{
				ShieldEnergy -= damage;
			}
			return bRetVal;
		}

		public float TotalLife
		{
			get
			{
				return ShieldEnergy;
			}
		}
		
	}
}
