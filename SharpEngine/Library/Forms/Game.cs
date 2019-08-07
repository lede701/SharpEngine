using SharpEngine.Library.GraphicsSystem;
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

		#endregion

		#region Public Parameters

		public String AssetsPath;
		public World World
		{
			get
			{
				return World.Instance;
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
			InitializeGame();
			this.FormBorderStyle = FormBorderStyle.Fixed3D;
		}

		public virtual void InitializeGame()
		{
			// Setup world global object
			World.Instance.WorldSize.X = this.ClientSize.Width;
			World.Instance.WorldSize.Y = this.ClientSize.Height;
			World.Instance.ScreenSize.X = this.ClientSize.Width;
			World.Instance.ScreenSize.Y = this.ClientSize.Height;
			World.Instance.WorldBoundary = new Rectangle
			{
				X = 0,
				Y = 0,
				Width = this.ClientSize.Width,
				Height = this.ClientSize.Height
			};

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
		}

		#endregion

		#region Game Loop

		private void GameLoop()
		{
			// Pasue this thread while app wamrs up
			ThreadManager.Sleep(2000, _gameLoop);
			float frameSampleTime = Stopwatch.Frequency / 60.0f;
			float deltaTime = 1.0f;
			int frameCount = 0;

			Stopwatch timer = new Stopwatch();
			Stopwatch longTimer = new Stopwatch();
			longTimer.Reset();
			longTimer.Start();
			while (_gameLoop.IsRunning)
			{

				timer.Reset();
				timer.Start();
				Update(deltaTime);
				Render(_gm);
				timer.Stop();
				float elapsed = timer.ElapsedTicks;
				//frameTime = deltaTime;
				deltaTime = elapsed / frameSampleTime;
				frameCount++;
				if (longTimer.ElapsedMilliseconds > 1000)
				{
					longTimer.Reset();
					//_currentFrameCnt = frameCount;
					frameCount = 0;
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
			g.BeginDraw();
				g.Clear(BackColor);
				_sm.Render(g);
			g.EndDraw();
			Invalidate();
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
