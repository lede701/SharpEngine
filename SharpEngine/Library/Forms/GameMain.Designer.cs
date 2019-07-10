namespace SharpEngine.Library.Forms
{
	partial class GameMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gameField = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.gameField)).BeginInit();
			this.SuspendLayout();
			// 
			// gameField
			// 
			this.gameField.Location = new System.Drawing.Point(12, 12);
			this.gameField.Name = "gameField";
			this.gameField.Size = new System.Drawing.Size(909, 600);
			this.gameField.TabIndex = 0;
			this.gameField.TabStop = false;
			// 
			// GameMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(933, 624);
			this.Controls.Add(this.gameField);
			this.Name = "GameMain";
			this.Text = "GameMain";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameMain_FormClosed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameMain_Paint);
			((System.ComponentModel.ISupportInitialize)(this.gameField)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox gameField;
	}
}