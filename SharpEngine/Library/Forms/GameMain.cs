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

		public GameMain()
		{
			InitializeComponent();
			_field = new Bitmap(gameField.Width, gameField.Height);


		}

		public void GameLoop()
		{
			while(_isRunning)
			{

			}
		}
	}
}
