using SharpEngine.Library.GraphicsSystem;
using SharpEngine.Library.Math.Physics;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpEngine.Library.Forms
{
	public partial class Game : Form
	{
		#region Private Parameters

		private SceneManager _sm;
		private GraphicsManager _gm;
		private ThreadManager.ThreadNode _gameLoop;
		private ThreadManager.ThreadNode _physicsLoop;

		#endregion

		#region Public Parameters

		private bool _singleThread;
		public bool SingleThread
		{
			get
			{
				return _singleThread;
			}
			set
			{
				_singleThread = value;
			}
		}

		private bool _inDebugMode = false;
		public bool InDebugMode
		{
			get
			{
				return _inDebugMode;
			}
			set
			{
				_inDebugMode = value;
			}
		}

		public String AssetsPath;
		public World World
		{
			get
			{
				return World.Instance;
			}
		}

		private PhysicsWorld _physicsWorld;
		public PhysicsWorld PhysicsWorld
		{
			get
			{
				return _physicsWorld;
			}
		}

		private PhysicsFactory _physicsFactory;
		public PhysicsFactory PhysicsFactory
		{
			get
			{
				return _physicsFactory;
			}
		}

		public int DefaultLayer = 5;

		#endregion

		#region Accessors

		public SceneManager SceneManager
		{
			get
			{
				return _sm;
			}
		}

		public GraphicsManager GraphicsManager
		{
			get
			{
				return _gm;
			}
		}

		#endregion

		#region Ctor's

		public Game()
		{
			InitializeComponent();
			this.FormBorderStyle = FormBorderStyle.Fixed3D;

			// Set world default values
			World.WorldSize.X = this.ClientSize.Width;
			World.WorldSize.Y = this.ClientSize.Height;
			World.ScreenSize.X = this.ClientSize.Width;
			World.ScreenSize.Y = this.ClientSize.Height;
			World.WorldBoundary = new Rectangle
			{
				X = 0,
				Y = 0,
				Width = this.ClientSize.Width,
				Height = this.ClientSize.Height
			};

			InitializeGame();
			SingleThread = true;
		}

		public virtual void InitializeGame()
		{
			// Setup world global object


			// Initialize GTraphics Manager
			_gm = new GraphicsManager(this);
			_sm = SceneManager.Instance;
			Scene top = new Scene();
			_sm.Add(top);

			// Start the game loop thread
			_gameLoop = ThreadManager.CreateThread(GameLoop);
			_gameLoop.Start();


			AssetsPath = Application.ExecutablePath + "\\Content";
			if (!Directory.Exists(AssetsPath))
			{
				// Break directory into it parts
				List<String> path = new List<String>(Application.ExecutablePath.Split('\\'));
				path.Remove(path.Last());
				// Go back a directory until we find content
				do
				{
					path.Remove(path.Last());
					AssetsPath = String.Format("{0}\\Content", String.Join("\\", path));
				} while (!Directory.Exists(AssetsPath) && path.Count > 0);
			}

			_physicsWorld = new PhysicsWorld(World.WorldSize);
			_physicsFactory = new PhysicsFactory(_physicsWorld);
			if (!SingleThread)
			{
				// Start the Physics loop now that the world is created
				_physicsLoop = ThreadManager.CreateThread(PhysicsLoop);
				_physicsLoop.Start();
			}
		}

		#endregion

		#region Game Loop

		private float _currentFrameCount;
		private void GameLoop()
		{
			// Pasue this thread while app wamrs up
			ThreadManager.Sleep(2000, _gameLoop);
			float frameSampleTime = Stopwatch.Frequency / 60.0f;
			float deltaTime = 1.0f;
			int frameCount = 0;

			// Setup timers to track game loop metrics
			Stopwatch timer = new Stopwatch();
			Stopwatch longTimer = new Stopwatch();
			longTimer.Reset();
			longTimer.Start();
			while (_gameLoop.IsRunning)
			{
				// Reset time for performance metrics
				timer.Reset();
				timer.Start();
				// Process game updates
				Update(deltaTime);
				// Render scene
				Render(_gm);
				if (SingleThread)
				{
					PhysicsWorld.Update(deltaTime);
				}
				// Stop timer
				timer.Stop();
				// Calculate current deltaTime
				float elapsed = timer.ElapsedTicks;
				deltaTime = elapsed / frameSampleTime;
				frameCount++;
				// Update Frames Per Second
				if (longTimer.ElapsedMilliseconds > 1000)
				{
					longTimer.Reset();
					_currentFrameCount = frameCount;
					frameCount = 0;
					longTimer.Start();
				}
			}
		}

		#endregion

		#region Physics Loop

		private float _physicsFrameCount;

		public virtual void PhysicsLoop()
		{
			// Pasue this thread while app wamrs up
			float frameSampleTime = Stopwatch.Frequency / 60.0f;
			float deltaTime = 1.0f;
			int frameCount = 0;

			// Setup physics metrics
			Stopwatch timer = new Stopwatch();
			Stopwatch longTimer = new Stopwatch();
			longTimer.Reset();
			longTimer.Start();
			// Start physics iteration of the world
			while (_physicsLoop.IsRunning)
			{
				timer.Reset();
				timer.Start();
				PhysicsWorld.Update(deltaTime);
				timer.Stop();
				float elapsed = timer.ElapsedTicks;
				deltaTime = elapsed / frameSampleTime;
				frameCount++;
				if (longTimer.ElapsedMilliseconds > 1000)
				{
					longTimer.Reset();
					_physicsFrameCount = frameCount;
					frameCount = 0;
					longTimer.Start();
				}
			}
		}

		#endregion

		#region Core Game Methods

		protected virtual void Update(float deltaTime)
		{
			_sm.Update(deltaTime);
		}

		protected virtual void Render(IGraphics g)
		{
			// Start the drawing process
			g.BeginDraw();
				// Clear buffer to background color
				g.Clear(BackColor);
				// Render Scene Objects
				_sm.Render(g);
				// Call Render Metrics
				RenderMetrics(g);
			// Done drawing so tell graphics engine we are done
			g.EndDraw();
			// Invalide form so it will redraw
			Invalidate();
		}

		protected virtual void RenderMetrics(IGraphics g)
		{
			if(InDebugMode)
			{
				String fps = String.Format("FPS: {0}", _currentFrameCount);
				String pfps = String.Format("Physics Frames: {0}", _physicsFrameCount);

				g.DrawText(fps, "Ariel", 12f, Color.White, new Rectangle { X = (int)World.ScreenSize.X - 200, Y = (int)World.ScreenSize.Y - 50, Width = 200, Height = 25 });
				g.DrawText(pfps, "Ariel", 12f, Color.White, new Rectangle { X = (int)World.ScreenSize.X - 200, Y = (int)World.ScreenSize.Y - 25, Width = 200, Height = 25 });
			}
		}

		public void Add(GObject gobj)
		{
			SceneManager.Add(gobj, DefaultLayer);
		}

		public void Add(GObject gobj, int layer)
		{
			SceneManager.Add(gobj, layer);
		}

		public void Add(UObject uobj)
		{
			_sm.Add(uobj, DefaultLayer);
		}
		public void Add(UObject uobj, int layer)
		{
			_sm.Add(uobj, layer);
		}

		#endregion

		#region Window Events

		protected override void OnPaint(PaintEventArgs e)
		{
			_gm.Render();
			base.OnPaint(e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			ThreadManager.Close();
			_gm.Dispose();
			_sm.Clear();
			base.OnFormClosing(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			switch(e.KeyCode)
			{
				case Keys.Escape:
					{
						Close();
					}
					break;
			}
		}

		#endregion
	}
}
