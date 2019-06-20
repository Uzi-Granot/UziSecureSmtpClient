namespace TestSecureSmtpClient
{
	partial class TestSecureSmtpClient
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
			this.button1 = new System.Windows.Forms.Button();
			this.TypeComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.HostTextBox = new System.Windows.Forms.TextBox();
			this.UserNameTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.UserPasswordTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.RefreshTokenTextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ToAddressTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.ToNameTextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.FromAddressTextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.FromNameTextBox = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.PortTextBox = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.TimeoutTextBox = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.BrowseButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.PreviousButton = new System.Windows.Forms.Button();
			this.NextButton = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(196, 398);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(205, 52);
			this.button1.TabIndex = 25;
			this.button1.Text = "Send Email";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnSendEmail);
			// 
			// TypeComboBox
			// 
			this.TypeComboBox.FormattingEnabled = true;
			this.TypeComboBox.Location = new System.Drawing.Point(27, 34);
			this.TypeComboBox.Name = "TypeComboBox";
			this.TypeComboBox.Size = new System.Drawing.Size(226, 24);
			this.TypeComboBox.TabIndex = 2;
			this.TypeComboBox.SelectedIndexChanged += new System.EventHandler(this.OnTypeChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(27, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(207, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Outgoing Mail Server (SMTP) Type";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(27, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(209, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Server Name (i.e. smtp.gmail.com)";
			// 
			// HostTextBox
			// 
			this.HostTextBox.Location = new System.Drawing.Point(27, 100);
			this.HostTextBox.Name = "HostTextBox";
			this.HostTextBox.Size = new System.Drawing.Size(226, 22);
			this.HostTextBox.TabIndex = 4;
			// 
			// UserNameTextBox
			// 
			this.UserNameTextBox.Location = new System.Drawing.Point(27, 204);
			this.UserNameTextBox.Name = "UserNameTextBox";
			this.UserNameTextBox.Size = new System.Drawing.Size(226, 22);
			this.UserNameTextBox.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(27, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(237, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "User Name (i.e. username@gmail.com)";
			// 
			// UserPasswordTextBox
			// 
			this.UserPasswordTextBox.Location = new System.Drawing.Point(27, 256);
			this.UserPasswordTextBox.Name = "UserPasswordTextBox";
			this.UserPasswordTextBox.Size = new System.Drawing.Size(226, 22);
			this.UserPasswordTextBox.TabIndex = 10;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(27, 236);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(161, 16);
			this.label4.TabIndex = 9;
			this.label4.Text = "User Password (plain text)";
			// 
			// RefreshTokenTextBox
			// 
			this.RefreshTokenTextBox.Location = new System.Drawing.Point(27, 308);
			this.RefreshTokenTextBox.Name = "RefreshTokenTextBox";
			this.RefreshTokenTextBox.Size = new System.Drawing.Size(256, 22);
			this.RefreshTokenTextBox.TabIndex = 12;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(27, 288);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(209, 16);
			this.label5.TabIndex = 11;
			this.label5.Text = "Refresh Token File Name (OAuth2)";
			// 
			// ToAddressTextBox
			// 
			this.ToAddressTextBox.Location = new System.Drawing.Point(342, 254);
			this.ToAddressTextBox.Name = "ToAddressTextBox";
			this.ToAddressTextBox.Size = new System.Drawing.Size(226, 22);
			this.ToAddressTextBox.TabIndex = 23;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(342, 234);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 16);
			this.label6.TabIndex = 22;
			this.label6.Text = "To Address";
			// 
			// ToNameTextBox
			// 
			this.ToNameTextBox.Location = new System.Drawing.Point(342, 204);
			this.ToNameTextBox.Name = "ToNameTextBox";
			this.ToNameTextBox.Size = new System.Drawing.Size(226, 22);
			this.ToNameTextBox.TabIndex = 21;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(342, 184);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(59, 16);
			this.label7.TabIndex = 20;
			this.label7.Text = "To Name";
			// 
			// FromAddressTextBox
			// 
			this.FromAddressTextBox.Location = new System.Drawing.Point(342, 152);
			this.FromAddressTextBox.Name = "FromAddressTextBox";
			this.FromAddressTextBox.Size = new System.Drawing.Size(226, 22);
			this.FromAddressTextBox.TabIndex = 19;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(342, 132);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(89, 16);
			this.label8.TabIndex = 18;
			this.label8.Text = "From Address";
			// 
			// FromNameTextBox
			// 
			this.FromNameTextBox.Location = new System.Drawing.Point(342, 100);
			this.FromNameTextBox.Name = "FromNameTextBox";
			this.FromNameTextBox.Size = new System.Drawing.Size(226, 22);
			this.FromNameTextBox.TabIndex = 17;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(342, 80);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(76, 16);
			this.label9.TabIndex = 16;
			this.label9.Text = "From Name";
			// 
			// PortTextBox
			// 
			this.PortTextBox.Location = new System.Drawing.Point(27, 152);
			this.PortTextBox.Name = "PortTextBox";
			this.PortTextBox.Size = new System.Drawing.Size(57, 22);
			this.PortTextBox.TabIndex = 6;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(27, 132);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(198, 16);
			this.label10.TabIndex = 5;
			this.label10.Text = "Port (Secure-465, Unsecure 587)";
			// 
			// TimeoutTextBox
			// 
			this.TimeoutTextBox.Location = new System.Drawing.Point(27, 360);
			this.TimeoutTextBox.Name = "TimeoutTextBox";
			this.TimeoutTextBox.Size = new System.Drawing.Size(57, 22);
			this.TimeoutTextBox.TabIndex = 14;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(27, 340);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(122, 16);
			this.label11.TabIndex = 13;
			this.label11.Text = "Timeout (default 20)";
			// 
			// BrowseButton
			// 
			this.BrowseButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BrowseButton.Location = new System.Drawing.Point(214, 338);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(69, 35);
			this.BrowseButton.TabIndex = 15;
			this.BrowseButton.Text = "Browse";
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.OnBrowse);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.PreviousButton);
			this.groupBox1.Controls.Add(this.NextButton);
			this.groupBox1.Location = new System.Drawing.Point(342, 305);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(226, 76);
			this.groupBox1.TabIndex = 24;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Scroll Previous Test Parameters";
			// 
			// PreviousButton
			// 
			this.PreviousButton.Location = new System.Drawing.Point(124, 27);
			this.PreviousButton.Name = "PreviousButton";
			this.PreviousButton.Size = new System.Drawing.Size(81, 36);
			this.PreviousButton.TabIndex = 1;
			this.PreviousButton.Text = "Previous";
			this.PreviousButton.UseVisualStyleBackColor = true;
			this.PreviousButton.Click += new System.EventHandler(this.OnPrevoius);
			// 
			// NextButton
			// 
			this.NextButton.Location = new System.Drawing.Point(22, 27);
			this.NextButton.Name = "NextButton";
			this.NextButton.Size = new System.Drawing.Size(81, 36);
			this.NextButton.TabIndex = 0;
			this.NextButton.Text = "Next";
			this.NextButton.UseVisualStyleBackColor = true;
			this.NextButton.Click += new System.EventHandler(this.OnNext);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.label12.Location = new System.Drawing.Point(310, 25);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(258, 32);
			this.label12.TabIndex = 0;
			this.label12.Text = "Copyrights © 2016-2019 Uzi Granot\r\nAll rights reserved";
			// 
			// TestSecureSmtpClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(597, 471);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.BrowseButton);
			this.Controls.Add(this.TimeoutTextBox);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.PortTextBox);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.ToAddressTextBox);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.ToNameTextBox);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.FromAddressTextBox);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.FromNameTextBox);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.RefreshTokenTextBox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.UserPasswordTextBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.UserNameTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.HostTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TypeComboBox);
			this.Controls.Add(this.button1);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TestSecureSmtpClient";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Test UziSecureSmtpClient Library";
			this.Load += new System.EventHandler(this.OnLoad);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox TypeComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox HostTextBox;
		private System.Windows.Forms.TextBox UserNameTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox UserPasswordTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox RefreshTokenTextBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox ToAddressTextBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox ToNameTextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox FromAddressTextBox;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox FromNameTextBox;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox PortTextBox;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox TimeoutTextBox;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button PreviousButton;
		private System.Windows.Forms.Button NextButton;
		private System.Windows.Forms.Label label12;
	}
}

