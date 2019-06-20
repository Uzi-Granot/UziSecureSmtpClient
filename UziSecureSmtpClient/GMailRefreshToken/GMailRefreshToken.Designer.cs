namespace GMailRefreshToken
{
	partial class GMailRefreshToken
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
			this.label2 = new System.Windows.Forms.Label();
			this.EmailAddressTextBox = new System.Windows.Forms.TextBox();
			this.CreateButton = new System.Windows.Forms.Button();
			this.BrowseButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.JsonFileTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 99);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "User email address";
			// 
			// EmailAddressTextBox
			// 
			this.EmailAddressTextBox.Location = new System.Drawing.Point(12, 120);
			this.EmailAddressTextBox.Name = "EmailAddressTextBox";
			this.EmailAddressTextBox.Size = new System.Drawing.Size(270, 22);
			this.EmailAddressTextBox.TabIndex = 11;
			// 
			// CreateButton
			// 
			this.CreateButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CreateButton.Location = new System.Drawing.Point(177, 176);
			this.CreateButton.Name = "CreateButton";
			this.CreateButton.Size = new System.Drawing.Size(221, 45);
			this.CreateButton.TabIndex = 12;
			this.CreateButton.Text = "Create Refresh Token";
			this.CreateButton.UseVisualStyleBackColor = true;
			this.CreateButton.Click += new System.EventHandler(this.OnCreateToken);
			// 
			// BrowseButton
			// 
			this.BrowseButton.Location = new System.Drawing.Point(489, 80);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(74, 39);
			this.BrowseButton.TabIndex = 9;
			this.BrowseButton.Text = "Browse";
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.OnBrowse);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Gmail API Client ID file";
			// 
			// JsonFileTextBox
			// 
			this.JsonFileTextBox.Location = new System.Drawing.Point(12, 40);
			this.JsonFileTextBox.Name = "JsonFileTextBox";
			this.JsonFileTextBox.Size = new System.Drawing.Size(551, 22);
			this.JsonFileTextBox.TabIndex = 8;
			// 
			// GMailRefreshToken
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(575, 238);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.EmailAddressTextBox);
			this.Controls.Add(this.CreateButton);
			this.Controls.Add(this.BrowseButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.JsonFileTextBox);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GMailRefreshToken";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "GMail Refresh Token";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox EmailAddressTextBox;
		private System.Windows.Forms.Button CreateButton;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox JsonFileTextBox;
	}
}

