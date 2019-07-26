using SharpEngine.Library.GraphicsSystem;
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
		public DxMain()
		{
			InitializeComponent();

			GraphicsManager gm = new GraphicsManager(this);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			GraphicsManager.Instance.Render();
			base.OnPaint(e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			GraphicsManager.Instance.Dispose();
			base.OnFormClosing(e);
		}
	}
}
