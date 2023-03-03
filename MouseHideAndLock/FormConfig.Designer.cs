namespace MouseHideAndLock
{
	partial class FormConfig
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
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.buttonSave = new System.Windows.Forms.Button();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.Location = new System.Drawing.Point(21, 39);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(579, 589);
			this.checkedListBox1.TabIndex = 0;
			// 
			// buttonSave
			// 
			this.buttonSave.Location = new System.Drawing.Point(466, 634);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(123, 70);
			this.buttonSave.TabIndex = 6;
			this.buttonSave.Text = "Save";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// FormConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(612, 713);
			this.Controls.Add(this.buttonSave);
			this.Controls.Add(this.checkedListBox1);
			this.Name = "FormConfig";
			this.Text = "FormConfig";
			this.Load += new System.EventHandler(this.FormConfig_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Button buttonSave;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}