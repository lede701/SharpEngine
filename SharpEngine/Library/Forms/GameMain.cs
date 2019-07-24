using SharpEngine.Library.Controller;
using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;
using SharpEngine.Library.Threading;
using SharpEngine.Library.User.Objects;
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
		// Graphics system
		private Bitmap _field;
		private Graphics _gfx;
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
		private ThreadManager.ThreadNode _directx;
		// Game engine base controller
		private IController controller;

		// Scene clear color
		private Brush _clrColor;

		public SimpleText ObjectCount;
		public GameMain()
		{
			InitializeComponent();
			_clrColor = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			gameField.Width = this.Width - 15;
			gameField.Height = this.Height - 39;
			gameField.Location = new Point { X = 0, Y = 0 };
			this.FormBorderStyle = FormBorderStyle.Fixed3D;


			// Create backbuffer render frame
			_field = new Bitmap(gameField.Width, gameField.Height);
			_gfx = Graphics.FromImage(_field);
			_gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			World.Instance.WorldSize.X = gameField.Width;
			World.Instance.WorldSize.Y = gameField.Height;

			// Start the running process
			_isRunning = true;
			// Create locking object for threading
			_lock = new Object();
			_gfxLock = new Object();

			controller = KeyboardController.Instance;
			SetupScene();
			// Start update thread for game loop
			_updateNode = ThreadManager.CreateThread(GameLoop);
			_updateNode.Start();

			_physicsNode = ThreadManager.CreateThread(PhysicsLoop);
			_physicsNode.Start();
		}

		private void InitDevice()
		{
		}

		public virtual void SetupScene()
		{
			lock (_lock)
			{
				SceneManager.Clear();
				// Create a test scene
				Scene gameScene = new Scene();
				SceneManager.Add(gameScene);
				// Create Ground object
				/*
				SimpleGround ground = new SimpleGround(gameField.Height - 25, Collider2DType.PlaneY);
				SimpleGround left = new SimpleGround(10, Collider2DType.PlaneX);
				SimpleGround right = new SimpleGround(gameField.Width - 10, Collider2DType.PlaneX);

				SceneManager.Add(ground, 1);
				SceneManager.Add(left, 1);
				SceneManager.Add(right, 1);
				*/
				// Create path to sprite sheet
				String fileName = Application.ExecutablePath;
				String backName = String.Empty;
				String pathName = String.Empty;
				Stack<String> pathParts = new Stack<String>(fileName.Split('\\').ToList());
				// Remove the development paths for now
				pathParts.Pop();
				pathParts.Pop();
				pathParts.Pop();

				pathName = String.Join("\\", pathParts.Reverse().ToArray());

				// Put path back together as a string
				fileName = String.Format("{0}\\Media\\Hero\\fighter.png", pathName);
				backName = String.Format("{0}\\Media\\Backgrounds\\nebula01.jpg", pathName);

				// Create the Hero sprite
				float colliderRadius = 42;

				// Create scene sprites
				Sprite hero = new Sprite(fileName);
				Sprite backdrop = new Sprite(backName);

				// Create Hero object
				SpriteShip player = new SpriteShip(hero);
				player.Position.X = (_field.Width / 2) - colliderRadius;
				player.Position.Y = _field.Height - 150;
				player.Velocity.X = 10;
				player.Velocity.Y = 8;
				// Create collider
				CircleCollider playerCollider = new CircleCollider();
				playerCollider.Position = player.Position;
				playerCollider.Radius = colliderRadius;
				player.Collider = playerCollider;
				// Connect keyboard controoler
				player.Controller = controller;

				// Create star field
				SpriteBackdrop back = new SpriteBackdrop(backdrop);
				SpriteStarField fld = new SpriteStarField();
				back.Collider = new NullCollider();
				back.Controller = new NullController();

				SceneManager.Add(fld, 2);
				SceneManager.Add(back, 1);

				SceneManager.Add(player, 5);

				ObjectCount = new SimpleText("Hello");
				ObjectCount.Position.X = World.Instance.WorldSize.X - 180;
				ObjectCount.Position.Y = 10;
				ObjectCount.Size = 10.0f;
#if DEBUG
				SceneManager.Add(ObjectCount, 6);
#endif
			}
		}

		public void GameLoop()
		{
			// Delay so the computer and stuff get settled
			ThreadManager.Sleep(2000, _updateNode);
			// Calculate how many milliseconds in a frame
			float frameSampleTime = Stopwatch.Frequency / 60.0f;
			
			float deltaTime = 1.0f;
			Stopwatch timer = new Stopwatch();
			while(_isRunning)
			{
				timer.Reset();
				timer.Start();
				lock (_lock)
				{
					ProcessUpdates(deltaTime);
					Render();
				}
				timer.Stop();
				float elapsed = timer.ElapsedTicks;
				frameTime = deltaTime;
				deltaTime = elapsed / frameSampleTime;
				/*
				if(elapsed < frameSampleTime)
				{
					// Mitigate the elapsed time so delta time doesn't change
					int sleep = (int)(frameSampleTime - elapsed / Stopwatch.Frequency) / 1000;
					ThreadManager.Sleep(sleep, _updateNode);
				}
				//*/
			}
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
			// Backbuffer rendering
			lock (_gfxLock)
			{
				ObjectCount.Text = String.Format("Game Objects: {0}", SceneManager.Scene.Count);
				_gfx.FillRectangle(_clrColor, 0, 0, _field.Width, _field.Height);
				SceneManager.Render(_gfx);
			}
			Invalidate();
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
					if (i + 1 < cnt)
					{
						for (int j = i + 1; j < cnt; ++j)
						{
							items[i].Collider.Hit(items[j]);
						}
					}
				}
			}// End if Scene is not null
		}

		private void GameMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			if(controller != null)
			{
				controller.Dispose();
				controller = null;
			}
			_isRunning = false;
			// Shutodwn all threads
			ThreadManager.MasterThread.IsRunning = false;
			// Kill all long running threads
			ThreadManager.Close();
			// After threads are closed dispose of Windows tools
			if (_gfx != null)
			{
				_gfx.Dispose();
			}
			if (_field != null)
			{
				_field.Dispose();
			}
		}

		private void GameMain_Paint(object sender, PaintEventArgs e)
		{
			// Transfer backbuffer to display image
			lock (_gfxLock)
			{
				Graphics g = gameField.CreateGraphics();
				g.DrawImage(_field, 0, 0, _field.Width, _field.Height);
			}
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
