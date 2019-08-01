using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Player.Weapons
{
	public interface IWeapon
	{
		UObject CreateBolt(Vector2D position, ref PlayerStatistics stats);
	}
}
