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

		public PlayerStatistics()
		{
			Name = "Norbert";
			Score = 0;
			Level = 1;
			Lives = 3;
			Experience = 0;
			NextExperienceLevel = 500;

			ShieldEnergy = 3.0f;
			MaxShieldEnergy = 5.0f;
			ShieldEnergyRechargeRate = 0.01f;

			WeaponEnergy = 8.0f;
			MaxWeaponEnergy = 8.0f;
			WeaponEnergyRechargeRate = 0.1f;
			WeaponEnergyUse = 1.6f;
			WeaponDamage = 2.0f;
		}

		public void Update(float deltaTime)
		{
			ShieldEnergy = System.Math.Min(ShieldEnergy + (ShieldEnergyRechargeRate * deltaTime), MaxShieldEnergy);
			WeaponEnergy = System.Math.Min(WeaponEnergy + (WeaponEnergyRechargeRate * deltaTime), MaxWeaponEnergy);
		}

		public bool CanFire
		{
			get
			{
				return WeaponEnergy > WeaponEnergyUse;
			}
		}

		public float Fire
		{
			get
			{
				float damage = 0.0f;
				if(CanFire)
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
		
	}
}
