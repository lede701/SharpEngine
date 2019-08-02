using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Particles;
using SharpEngine.Library.Randomizer;
using SharpEngine.Library.Threading;
using SharpEngine.Library.User.Objects;
using SharpEngine.Library.User.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpEngine.Library.Forms
{
	public partial class GameMain : Form
	{
		private GraphicsManager _gm;

		private bool _isRunning;

		public SceneManager SceneManager
		{
			get
			{
				return SceneManager.Instance;
			}
		}

		// Game engine locking objects
		private Object _lock;
		private Object _gfxLock;
		// Game engine dedicated threads
		private ThreadManager.ThreadNode _updateNode;
		private ThreadManager.ThreadNode _physicsNode;
		private ThreadManager.ThreadNode _spawner;
		// Game engine base controller
		private IController controller;

		// Scene clear color
		private Color _backColor;

		public SimpleText GameMessage;
		SpriteShip player;

		public GameMain()
		{
			InitializeComponent();
			_backColor = Color.FromArgb(255, 0, 0, 0);
			this.FormBorderStyle = FormBorderStyle.Fixed3D;


			// Create backbuffer render frame
			World.Instance.WorldSize.X = this.ClientSize.Width;
			World.Instance.WorldSize.Y = this.ClientSize.Height;
			World.Instance.WorldBoundary = new Rectangle
			{
				X = 0,
				Y = 0,
				Width = this.ClientSize.Width,
				Height = this.ClientSize.Height
			};

			// Create locking object for threading
			_lock = new Object();
			_gfxLock = new Object();

			_gm = new GraphicsManager(this);


			// Load scene
			SetupScene();

			// Setup game threads
			_updateNode = ThreadManager.CreateThread(GameLoop);
			_physicsNode = ThreadManager.CreateThread(PhysicsLoop);

			// Start the running process
			_isRunning = true;

			_updateNode.Start();
			_physicsNode.Start();

		}

		private String backName = String.Empty;
		private String pathName = String.Empty;
		private String asteroidName = String.Empty;

		public virtual void SetupScene()
		{
			lock (_lock)
			{
				lock (_gfxLock)
				{
					SceneManager.Clear();
					// Create a test scene
					Scene gameScene = new Scene();
					SceneManager.Add(gameScene);
					// Create path to sprite sheet
					String fileName = Application.ExecutablePath;
					Stack<String> pathParts = new Stack<String>(fileName.Split('\\').ToList());
					// Remove the development paths for now
					pathParts.Pop();
					pathParts.Pop();
					pathParts.Pop();

					pathName = String.Join("\\", pathParts.Reverse().ToArray());

					// Put path back together as a string
					String heroName = String.Format("{0}\\Media\\Hero\\fighter.png", pathName);
					backName = String.Format("{0}\\Media\\Backgrounds\\nebula01.jpg", pathName);
					asteroidName = String.Format("{0}\\Media\\Spritesheets\\asteroid_01.png", pathName);

					float scale = 0.15f;

					Sprite hero = new Sprite(heroName);
					hero.Frames.Add(new Rectangle
					{
						X = 0,
						Y = 0,
						Width = 405,
						Height = 488
					});
					player = new SpriteShip(hero);

					player.Position.X = (World.Instance.WorldSize.X / 2) - 12;
					player.Position.Y = World.Instance.WorldSize.Y - 200f;
					player.Scale.X = scale;
					player.Scale.Y = scale;
					player.Velocity.X = 8.0f;
					player.Velocity.Y = 8.0f;
					player.Controller = KeyboardController.Instance;
					player.Type = ObjectType.PLAYER;

					PlayerUI plUI = new PlayerUI(ref player.PlayerStats);

					//*
					// Create game scene backdrop
					Sprite back = new Sprite(_gm.LoadImage(backName));
					SharpDX.Direct2D1.Bitmap backBitmap = (SharpDX.Direct2D1.Bitmap)back.SpriteSheet;
					back.Frames.Add(new Rectangle { X = 0, Y = 0, Width = (int)backBitmap.Size.Width, Height = (int)backBitmap.Size.Height });
					SpriteBackdrop bdrop = new SpriteBackdrop(back);

					SpriteStarField sfield = new SpriteStarField();

					GameMessage = new SimpleText("Hello, World!");
					GameMessage.Position.X = World.Instance.WorldSize.X - 250f;
					GameMessage.Position.Y = 10;
					//*/

					SceneManager.Add(player, 6);
					SceneManager.Add(GameMessage, 8);
					SceneManager.Add(plUI, 8);
					SceneManager.Add(sfield, 2);
					SceneManager.Add(bdrop, 1);
				}

				if(_spawner == null)
				{
					_spawner = ThreadManager.CreateThread(Spawn);
					_spawner.Start();
				}
			}
		}

		private int _currentFrameCnt;
		public void GameLoop()
		{
			// Pasue this thread while app wamrs up
			ThreadManager.Sleep(2000, _updateNode);
			float frameSampleTime = Stopwatch.Frequency / 60.0f;
			float deltaTime = 1.0f;
			int frameCount = 0;

			Stopwatch timer = new Stopwatch();
			Stopwatch longTimer = new Stopwatch();
			longTimer.Reset();
			longTimer.Start();
			while(_isRunning)
			{

				timer.Reset();
				timer.Start();
				ProcessUpdates(deltaTime);
				Render();
				timer.Stop();
				float elapsed = timer.ElapsedTicks;
				frameTime = deltaTime;
				deltaTime = elapsed / frameSampleTime;
				frameCount++;
				if(longTimer.ElapsedMilliseconds > 1000 )
				{
					longTimer.Reset();
					_currentFrameCnt = frameCount;
					frameCount = 0;
				}
			}
		}

		public void Spawn()
		{
			float lastX = 0;
			Sprite spAsteroid = new Sprite(_gm.LoadImage(asteroidName));
			spAsteroid.AutoDispose = false;
			while (_spawner.IsRunning)
			{
				SpriteAsteroid asteroid = new SpriteAsteroid(spAsteroid);
				do
				{
					asteroid.Position.X = RandomManager.Instance.Next(100, (int)World.Instance.WorldSize.X - 100);
				} while (lastX > asteroid.Position.X && lastX + 100f < asteroid.Position.X);
				lastX = asteroid.Position.X;
				asteroid.Position.Y = -200;
				asteroid.Velocity.Y = (float)(RandomManager.Instance.Next(500, 4500) / 1000f);
				asteroid.Life = (float)(RandomManager.Instance.Next(120, 400) / 100f);
				asteroid.Type = ObjectType.ENEMY;
				asteroid.EventDestroyed += EnemyDestoryed;
				player.DebugObject = asteroid;
				SceneManager.Add(asteroid, 5);
				int waitTime = RandomManager.Instance.Next(1000, 5000);
				ThreadManager.Sleep(waitTime, _spawner);
			}
			spAsteroid.Dispose();
		}

		public void EnemyDestoryed(Object enemy, EventArgs e)
		{

		}

		public void PhysicsLoop()
		{
			float frameTime = 1000.0f / 60.0f;
			Stopwatch timer = new Stopwatch();
			while (_isRunning)
			{
				timer.Reset();
				timer.Start();
				Physics();
				timer.Stop();
				float elapsed = timer.ElapsedMilliseconds;
				if (elapsed < frameTime)
				{
					int sleetTime = (int)(frameTime - elapsed);
					ThreadManager.Sleep(sleetTime, _updateNode);
				}
			}
		}

		public void ProcessUpdates(float deltaTime)
		{
			SceneManager.Update(deltaTime);
		}

		private float frameTime;
		public void Render()
		{
			if (GameMessage != null)
			{
				GameMessage.Text = String.Format("Position [{0}, {1}] FPS: {2}", player.Position.X, player.Position.Y, _currentFrameCnt);
			}
			// Backbuffer rendering
			lock (_gfxLock)
			{
				_gm.BeginDraw();
					_gm.Clear();
					SceneManager.Render(_gm);
				_gm.EndDraw();
				_gm.Render();
			}
			//Invalidate();
		}

		public void Physics()
		{
			// Check for physics collisions
			if (SceneManager.Scene != null)
			{
				List<UObject> items;
				lock (_lock)
				{
					items = SceneManager.Scene.UserObjects;
				}
				int cnt = items.Count;
				for (int i = 0; i < cnt; ++i)
				{
					UObject obj1 = items[i];
					if (i + 1 < cnt)
					{
						for (int j = i + 1; j < cnt; ++j)
						{
							obj1.Collider.Hit(items[j]);
						}
					}
				}
			}// End if Scene is not null
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			// Shutdown game threads
			_updateNode.Stop();
			ThreadManager.Close();

			_isRunning = false;
			_gm.Dispose();

			base.OnClosed(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			lock (_gfxLock)
			{
				_gm.Render();
			}
			base.OnPaint(e);
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			// Create a key pressed event
			//_keyPressed[e.KeyValue] = false;
			switch(e.KeyCode)
			{
				case Keys.Escape:
				case Keys.Q:
					Close();
					break;
				case Keys.R:
					SetupScene();
					break;
			}
		}
	}
}
