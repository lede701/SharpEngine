using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.User.Player
{
	public class PlayerUI : GObject
	{
		private String _key;
		public string Key
		{
			get
			{
				return _key;
			}
		}

		private PlayerStatistics _plStats;

		public PlayerUI(ref PlayerStatistics plStats)
		{
			_key = Guid.NewGuid().ToString();
			_plStats = plStats;
		}

		public void Dispose()
		{
			
		}

		public void Render(IGraphics g)
		{
			// Render player stats
			float x = 10;
			float y = World.Instance.ScreenSize.Y - 25;
			float maxWidth = (World.Instance.ScreenSize.X - 20);
			float wWidth = maxWidth * (_plStats.WeaponEnergy / _plStats.MaxWeaponEnergy);
			float sWidth = maxWidth * (_plStats.ShieldEnergy / _plStats.MaxShieldEnergy);

			// Draw energy levels
			g.FillRectangle(x, y, wWidth, 5, Color.FromArgb(120, 252, 119, 3));
			g.DrawRectangle(x, y, maxWidth, 5, Color.FromArgb(120, 252, 119, 3));
			// Draw shield levels
			g.FillRectangle(x, y + 7, sWidth, 5, Color.FromArgb(120, 60, 177, 255));
			g.DrawRectangle(x, y + 7, maxWidth, 5, Color.FromArgb(120, 60, 177, 255));

			// Draw player score
			String lblScore = String.Format("Score: {0}", _plStats.Score);
			g.DrawText(lblScore, "Ariel", 16f, Color.White, new Rectangle { X = 100, Y = 10, Width = 200, Height = 50 });
		}

		public void Update(float deltaTime)
		{

		}
	}
}
