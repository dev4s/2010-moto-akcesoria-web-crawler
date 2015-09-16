namespace MotoAkcesoriaWebCrawler
{
	partial class MotoAkcesoriaWebCrawler
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
			this.serverTextBox = new System.Windows.Forms.TextBox();
			this.databaseTextBox = new System.Windows.Forms.TextBox();
			this.userTextBox = new System.Windows.Forms.TextBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.informationLabel = new System.Windows.Forms.Label();
			this.stopBtn = new System.Windows.Forms.Button();
			this.pauseBtn = new System.Windows.Forms.Button();
			this.startBtn = new System.Windows.Forms.Button();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.startId = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.stopId = new System.Windows.Forms.TextBox();
			this.listBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// serverTextBox
			// 
			this.serverTextBox.Location = new System.Drawing.Point(97, 6);
			this.serverTextBox.Name = "serverTextBox";
			this.serverTextBox.Size = new System.Drawing.Size(159, 20);
			this.serverTextBox.TabIndex = 0;
			this.serverTextBox.Text = "localhost";
			// 
			// databaseTextBox
			// 
			this.databaseTextBox.Location = new System.Drawing.Point(97, 32);
			this.databaseTextBox.Name = "databaseTextBox";
			this.databaseTextBox.Size = new System.Drawing.Size(159, 20);
			this.databaseTextBox.TabIndex = 1;
			this.databaseTextBox.Text = "tt_testsklep";
			// 
			// userTextBox
			// 
			this.userTextBox.Location = new System.Drawing.Point(97, 58);
			this.userTextBox.Name = "userTextBox";
			this.userTextBox.Size = new System.Drawing.Size(159, 20);
			this.userTextBox.TabIndex = 2;
			this.userTextBox.Text = "root";
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(15, 165);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(241, 23);
			this.progressBar.TabIndex = 16;
			// 
			// informationLabel
			// 
			this.informationLabel.AutoSize = true;
			this.informationLabel.Location = new System.Drawing.Point(12, 193);
			this.informationLabel.Name = "informationLabel";
			this.informationLabel.Size = new System.Drawing.Size(59, 13);
			this.informationLabel.TabIndex = 17;
			this.informationLabel.Text = "Information";
			// 
			// stopBtn
			// 
			this.stopBtn.Location = new System.Drawing.Point(98, 209);
			this.stopBtn.Name = "stopBtn";
			this.stopBtn.Size = new System.Drawing.Size(75, 23);
			this.stopBtn.TabIndex = 7;
			this.stopBtn.Text = "Stop";
			this.stopBtn.UseVisualStyleBackColor = true;
			this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
			// 
			// pauseBtn
			// 
			this.pauseBtn.Location = new System.Drawing.Point(181, 209);
			this.pauseBtn.Name = "pauseBtn";
			this.pauseBtn.Size = new System.Drawing.Size(75, 23);
			this.pauseBtn.TabIndex = 8;
			this.pauseBtn.Text = "Wstrzymaj";
			this.pauseBtn.UseVisualStyleBackColor = true;
			this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
			// 
			// startBtn
			// 
			this.startBtn.Location = new System.Drawing.Point(15, 209);
			this.startBtn.Name = "startBtn";
			this.startBtn.Size = new System.Drawing.Size(75, 23);
			this.startBtn.TabIndex = 6;
			this.startBtn.Text = "Start";
			this.startBtn.UseVisualStyleBackColor = true;
			this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(97, 84);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.Size = new System.Drawing.Size(159, 20);
			this.passwordTextBox.TabIndex = 3;
			this.passwordTextBox.Text = "zaq12wsx";
			this.passwordTextBox.UseSystemPasswordChar = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Serwer";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Baza danych";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 61);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(62, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Użytkownik";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 87);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(36, 13);
			this.label5.TabIndex = 13;
			this.label5.Text = "Hasło";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 113);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Start Id";
			// 
			// startId
			// 
			this.startId.Location = new System.Drawing.Point(97, 110);
			this.startId.Name = "startId";
			this.startId.Size = new System.Drawing.Size(159, 20);
			this.startId.TabIndex = 4;
			this.startId.Text = "0";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 139);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(41, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "Stop Id";
			// 
			// stopId
			// 
			this.stopId.Location = new System.Drawing.Point(97, 136);
			this.stopId.Name = "stopId";
			this.stopId.Size = new System.Drawing.Size(159, 20);
			this.stopId.TabIndex = 5;
			this.stopId.Text = "1000000";
			// 
			// listBox
			// 
			this.listBox.FormattingEnabled = true;
			this.listBox.Location = new System.Drawing.Point(262, 6);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(502, 225);
			this.listBox.TabIndex = 9;
			// 
			// MotoAkcesoriaWebCrawler
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(776, 242);
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.stopId);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.startId);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.passwordTextBox);
			this.Controls.Add(this.startBtn);
			this.Controls.Add(this.pauseBtn);
			this.Controls.Add(this.stopBtn);
			this.Controls.Add(this.informationLabel);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.userTextBox);
			this.Controls.Add(this.databaseTextBox);
			this.Controls.Add(this.serverTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MotoAkcesoriaWebCrawler";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MotoAkcesoriaWebCrawler";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MotoAkcesoriaWebCrawler_FormClosing);
			this.Load += new System.EventHandler(this.MotoAkcesoriaWebCrawler_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox serverTextBox;
		private System.Windows.Forms.TextBox databaseTextBox;
		private System.Windows.Forms.TextBox userTextBox;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label informationLabel;
		private System.Windows.Forms.Button stopBtn;
		private System.Windows.Forms.Button pauseBtn;
		private System.Windows.Forms.Button startBtn;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox startId;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox stopId;
		private System.Windows.Forms.ListBox listBox;
	}
}