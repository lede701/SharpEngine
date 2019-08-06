using SharpEngine.Library.Controller;
using SharpEngine.Library.Forms;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Objects;
using SharpEngine.Library.User.Objects;
using SharpEngine.Library.User.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpEngine.Library.User.Form
{
	public partial class StarMine : Game
	{
		public StarMine() : base()
		{
			InitializeComponent();
		}

		public override void InitializeGame()
		{
			base.InitializeGame();

			String heroPath = String.Format("{0}\\Hero\\fighter.png", AssetsPath);
			SpriteShip player = new SpriteShip(GraphicsManager.LoadSpriteFromImagePath(heroPath));
			player.Position.X = World.WorldSize.X / 2f;
			player.Position.Y = World.WorldSize.Y / 2f;
			player.Controller = KeyboardController.Instance;
			PlayerUI pui = new PlayerUI(ref player.PlayerStats);

			Add(player);
			Add(pui, 7);
		}

	}
}
