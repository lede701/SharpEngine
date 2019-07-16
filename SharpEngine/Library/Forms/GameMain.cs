using SharpEngine.Library.Controller;
using SharpEngine.Library.Objects;
using SharpEngine.Library.Randomizer;
using SharpEngine.Library.Threading;
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
		private Bitmap _field;
		private bool _isRunning;
		private SceneManager _scManager;

		private Object _lock;
		private ThreadManager.ThreadNode _updateNode;
		private IController controller;

		private bool[] _keyPressed;

		public GameMain()
		{
			InitializeComponent();
			// TODO: This is messy in Windows so we will need to get direct input
			_keyPressed = new bool[256];
			for(int i=0; i<256; ++i)
			{
				_keyPressed[i] = false;
			}

			// Create backbuffer render frame
			_field = new Bitmap(gameField.Width, gameField.Height);

			// Start the running process
			_isRunning = true;
			// Create locking object for threading
			_lock = new Object();
			// Instantiatre scene manager
			_scManager = SceneManager.Instance;

			controller = KeyboardController.Instance;
			SetupScene();
			// Start update thread for game loop
			_updateNode = ThreadManager.CreateThread(GameLoop);
			_updateNode.Start();
		}

		public virtual void SetupScene()
		{
			_scManager.Clear();
			// Create a test scene
			Scene gameScene = new Scene();
			_scManager.Add(gameScene);
			// Create a test object for the scene
			SimpleGround ground = new SimpleGround(gameField.Height - 50, Math.Collider2DType.PlaneY);
			_scManager.Add(ground, 6);
			AddBall();
		}

		private void AddBall()
		{
			RandomManager rm = RandomManager.Instance;
			SimpleBall ball = new SimpleBall(25, new Math.Vector2D { X = rm.Next(25, gameField.Width), Y = rm.Next(10, 100) }, new Rectangle { X = 2, Y = 2, Width = gameField.Width - 27, Height = gameField.Height - 27 });
			ball.Velocity.X = rm.Next(0, 4);
			_scManager.Add(ball, 2);
		}

		public void GameLoop()
		{
			// Calculate how many milliseconds in a frame
			float frameTime = 1000.0f / 60.0f;
			while(_isRunning)
			{
				Stopwatch timer = Stopwatch.StartNew();
				lock (_lock)
				{
					ProcessUpdates(1.0f);
					Render();
					Physics();
				}
				timer.Stop();
				float elapsed = timer.ElapsedMilliseconds;
				if(elapsed < frameTime)
				{
					int sleetTime = (int)(frameTime - elapsed);
					ThreadManager.Sleep(sleetTime, _updateNode);
				}

			}
		}

		public void ProcessUpdates(float deltaTime)
		{
			_scManager.Update(deltaTime);
		}

		public void Render()
		{
			// Backbuffer rendering
			Graphics g = Graphics.FromImage(_field);
			g.FillRectangle(Brushes.Black, 0, 0, _field.Width, _field.Height);
			_scManager.Render(g);
			Invalidate();
		}

		public void Physics()
		{
			// Check for physics collisions
			if (_scManager.Scene != null)
			{
				List<UObject> items = _scManager.Scene.UserObjects;
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
			ThreadManager.MasterThread.IsRunning = false;
		}

		private void GameMain_Paint(object sender, PaintEventArgs e)
		{
			// Transfer backbuffer to display image
			lock(_lock)
			{
				Graphics g = gameField.CreateGraphics();
				g.DrawImage(_field, 0, 0, _field.Width, _field.Height);
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			// Record key was pressed
			_keyPressed[e.KeyValue] = true;
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			// Create a key pressed event
			_keyPressed[e.KeyValue] = false;
			switch(e.KeyCode)
			{
				case Keys.Escape:
				case Keys.Q:
					Close();
					break;
				case Keys.R:
					SetupScene();
					break;
				case Keys.Add:
					AddBall();
					break;
			}
		}
	}
}
