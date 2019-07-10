using SharpEngine.Library.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

		public GameMain()
		{
			InitializeComponent();
			_field = new Bitmap(gameField.Width, gameField.Height);

			_isRunning = true;
			_lock = new Object();
			_scManager = SceneManager.Instance;

		}

		public void GameLoop()
		{
			while(_isRunning)
			{
				ProcessUpdates(1.0f);
				Render();
			}
		}

		public void ProcessUpdates(float deltaTime)
		{
			_scManager.Update(deltaTime);
		}

		public void Render()
		{
			// Backbuffer rendering
			lock(_lock)
			{
				Graphics g = Graphics.FromImage(_field);
				_scManager.Render(g);
				Invalidate();
			}
		}

		private void GameMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			_isRunning = false;
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
	}
}
