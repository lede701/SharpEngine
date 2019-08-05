using SharpEngine.Library.GraphicsSystem;
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
	public partial class DxMain : Form
	{
		private SceneManager _sm;
		private GraphicsManager _gm;
		public DxMain()
		{
			InitializeComponent();

			// Initialize GTraphics Manager
			_gm = new GraphicsManager(this);
			_sm = SceneManager.Instance;
			Scene top = new Scene();
			_sm.Add(top);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			_gm.Render();
			base.OnPaint(e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
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
	}
}
