﻿using SharpEngine.Library.Controller;
using SharpEngine.Library.Forms;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Objects;
using SharpEngine.Library.User.Factories;
using SharpEngine.Library.User.Objects;
using SharpEngine.Library.User.Player;
using SharpEngine.Library.User.Universe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// https://graphicriver.net/item/spaceship-hyper-pack/15911780
// https://graphicriver.net/item/spaceship-hyper-pack/15911780

namespace SharpEngine.Library.User.Form
{
	public partial class StarMine : Game
	{
		public static int GUILAYER = 9;


		private SimpleText _debug;
		private UniverseMaster _theUniverse;

		public StarMine() : base()
		{
			InitializeComponent();
		}

		public override void InitializeGame()
		{
			int size = 50000;
			// Setup world
			World.WorldSize.X = size;
			World.WorldSize.Y = size;
			World.WorldBoundary = new Rectangle { X = 0, Y = 0, Height = size, Width = size };
			// Call core Initialzation of game engine
			base.InitializeGame();

			// Create the Universe factory
			UniverseFactory.Instance.AssetPath = AssetsPath;
			UniverseFactory.Instance.PhysicsFactory = PhysicsFactory;

			String heroPath = String.Format("{0}\\Hero\\fighter.png", AssetsPath);
			SpriteShip player = new SpriteShip(GraphicsManager.LoadSpriteFromImagePath(heroPath));
			player.Position.X = World.ScreenSize.X / 2f;
			player.Position.Y = World.ScreenSize.Y / 2f;
			player.Controller = KeyboardController.Instance;
			PlayerUI pui = new PlayerUI(ref player.PlayerStats);

			// Create the universe
			_theUniverse = new UniverseMaster
			{
				BlockSize = new Math.Vector2D { X = World.ScreenSize.X, Y = World.ScreenSize.Y }
			};

			// Build player control object
			PlayerController pc = new PlayerController(KeyboardController.Instance);
			KeyboardController.Instance.Map(Input.RightShift, Keys.RShiftKey);
			pc.Player = player;
			pc.Universe = _theUniverse;

			_debug = new SimpleText("");
			_debug.Position.X = World.ScreenSize.X - 200;
			_debug.Position.Y = 10;

			Add(pc, 7);
			Add(pui, GUILAYER);
			Add(_debug, GUILAYER);
			InDebugMode = true;
		}

		protected override void Render(IGraphics g)
		{
			if(_debug != null)
			{
				_debug.Text = String.Format("[{0}, {1}]", (int)World.WorldPosition.X, (int)World.WorldPosition.Y);
			}
			base.Render(g);
		}

	}
}
