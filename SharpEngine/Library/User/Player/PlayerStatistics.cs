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
		public int WeaponPauseRate;

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
			ShieldEnergyRechargeRate = 0.01f;

			WeaponEnergy = 20.0f;
			MaxWeaponEnergy = 20.0f;
			WeaponEnergyRechargeRate = 0.1f;
			WeaponEnergyUse = 3.0f;
			WeaponDamage = 2.0f;
			WeaponPauseRate = 50;
		}

		public void Update(float deltaTime)
		{
			ShieldEnergy = System.Math.Min(ShieldEnergy + (ShieldEnergyRechargeRate * deltaTime), MaxShieldEnergy);
			WeaponEnergy = System.Math.Min(WeaponEnergy + (WeaponEnergyRechargeRate * deltaTime), MaxWeaponEnergy);
		}

		private int _frameCnt;
		public bool CanFire
		{
			get
			{
				_frameCnt = (_frameCnt + 1) % WeaponPauseRate;
				return WeaponEnergy > WeaponEnergyUse && _frameCnt == 0;
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
