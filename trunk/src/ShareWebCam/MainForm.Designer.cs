namespace ShareWebCam {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.WebCamPage = new System.Windows.Forms.TabPage();
			this.SubFrameComboBox = new System.Windows.Forms.ComboBox();
			this.SelectSubFrameButton = new System.Windows.Forms.Button();
			this.UploadImmediatelyLinkLabel = new System.Windows.Forms.LinkLabel();
			this.OpenWebCamLinkLabel = new System.Windows.Forms.LinkLabel();
			this.label4 = new System.Windows.Forms.Label();
			this.UploadModeComboBox = new System.Windows.Forms.ComboBox();
			this.ConnectButton = new System.Windows.Forms.Button();
			this.DeviceSnapshotParamsComboBox = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.DeviceComboBox = new System.Windows.Forms.ComboBox();
			this.PreviewPictureBox = new System.Windows.Forms.PictureBox();
			this.SnapshotsPage = new System.Windows.Forms.TabPage();
			this.SnapshotHistoryListBox = new System.Windows.Forms.ListBox();
			this.SnapshotPictureBox = new System.Windows.Forms.PictureBox();
			this.UploadSettingsPage = new System.Windows.Forms.TabPage();
			this.WebCamDescriptionTextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.ImageFormatComboBox = new System.Windows.Forms.ComboBox();
			this.WebCamTitleTextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.UploadAddressTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.UploadProxyAddress = new System.Windows.Forms.Label();
			this.UploadProxyAddressTextBox = new System.Windows.Forms.TextBox();
			this.UploadTimer = new System.Windows.Forms.Timer(this.components);
			this.CheckWebCamTimer = new System.Windows.Forms.Timer(this.components);
			this.StatusTextBox = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.FramePictureBox = new ShareWebCam.ResizableRectangle();
			this.button2 = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.WebCamPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).BeginInit();
			this.PreviewPictureBox.SuspendLayout();
			this.SnapshotsPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SnapshotPictureBox)).BeginInit();
			this.UploadSettingsPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.WebCamPage);
			this.tabControl1.Controls.Add(this.SnapshotsPage);
			this.tabControl1.Controls.Add(this.UploadSettingsPage);
			this.tabControl1.Location = new System.Drawing.Point(1, 1);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(523, 291);
			this.tabControl1.TabIndex = 0;
			// 
			// WebCamPage
			// 
			this.WebCamPage.Controls.Add(this.button2);
			this.WebCamPage.Controls.Add(this.button1);
			this.WebCamPage.Controls.Add(this.SubFrameComboBox);
			this.WebCamPage.Controls.Add(this.SelectSubFrameButton);
			this.WebCamPage.Controls.Add(this.UploadImmediatelyLinkLabel);
			this.WebCamPage.Controls.Add(this.OpenWebCamLinkLabel);
			this.WebCamPage.Controls.Add(this.label4);
			this.WebCamPage.Controls.Add(this.UploadModeComboBox);
			this.WebCamPage.Controls.Add(this.ConnectButton);
			this.WebCamPage.Controls.Add(this.DeviceSnapshotParamsComboBox);
			this.WebCamPage.Controls.Add(this.label3);
			this.WebCamPage.Controls.Add(this.label1);
			this.WebCamPage.Controls.Add(this.DeviceComboBox);
			this.WebCamPage.Controls.Add(this.PreviewPictureBox);
			this.WebCamPage.Location = new System.Drawing.Point(4, 22);
			this.WebCamPage.Name = "WebCamPage";
			this.WebCamPage.Padding = new System.Windows.Forms.Padding(3);
			this.WebCamPage.Size = new System.Drawing.Size(515, 265);
			this.WebCamPage.TabIndex = 0;
			this.WebCamPage.Text = "WebCam";
			this.WebCamPage.UseVisualStyleBackColor = true;
			// 
			// SubFrameComboBox
			// 
			this.SubFrameComboBox.DisplayMember = "Caption";
			this.SubFrameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SubFrameComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.SubFrameComboBox.FormattingEnabled = true;
			this.SubFrameComboBox.Location = new System.Drawing.Point(334, 92);
			this.SubFrameComboBox.Name = "SubFrameComboBox";
			this.SubFrameComboBox.Size = new System.Drawing.Size(173, 21);
			this.SubFrameComboBox.TabIndex = 20;
			this.SubFrameComboBox.ValueMember = "StrValue";
			this.SubFrameComboBox.SelectedIndexChanged += new System.EventHandler(this.SubFrameComboBox_SelectedIndexChanged);
			// 
			// SelectSubFrameButton
			// 
			this.SelectSubFrameButton.Enabled = false;
			this.SelectSubFrameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.SelectSubFrameButton.Location = new System.Drawing.Point(333, 119);
			this.SelectSubFrameButton.Name = "SelectSubFrameButton";
			this.SelectSubFrameButton.Size = new System.Drawing.Size(174, 23);
			this.SelectSubFrameButton.TabIndex = 19;
			this.SelectSubFrameButton.Text = "Select Frame";
			this.SelectSubFrameButton.UseVisualStyleBackColor = true;
			this.SelectSubFrameButton.Click += new System.EventHandler(this.SelectSubFrameButton_Click);
			// 
			// UploadImmediatelyLinkLabel
			// 
			this.UploadImmediatelyLinkLabel.AutoSize = true;
			this.UploadImmediatelyLinkLabel.LinkColor = System.Drawing.Color.Black;
			this.UploadImmediatelyLinkLabel.Location = new System.Drawing.Point(180, 249);
			this.UploadImmediatelyLinkLabel.Name = "UploadImmediatelyLinkLabel";
			this.UploadImmediatelyLinkLabel.Size = new System.Drawing.Size(147, 13);
			this.UploadImmediatelyLinkLabel.TabIndex = 17;
			this.UploadImmediatelyLinkLabel.TabStop = true;
			this.UploadImmediatelyLinkLabel.Text = "Upload Snapshot Immediately";
			this.UploadImmediatelyLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UploadImmediatelyLinkLabel_LinkClicked);
			// 
			// OpenWebCamLinkLabel
			// 
			this.OpenWebCamLinkLabel.AutoSize = true;
			this.OpenWebCamLinkLabel.LinkColor = System.Drawing.Color.Black;
			this.OpenWebCamLinkLabel.Location = new System.Drawing.Point(3, 249);
			this.OpenWebCamLinkLabel.Name = "OpenWebCamLinkLabel";
			this.OpenWebCamLinkLabel.Size = new System.Drawing.Size(108, 13);
			this.OpenWebCamLinkLabel.TabIndex = 16;
			this.OpenWebCamLinkLabel.TabStop = true;
			this.OpenWebCamLinkLabel.Text = "Open WebCam Page";
			this.OpenWebCamLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenWebCamLinkLabel_LinkClicked);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(330, 148);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Upload Mode";
			// 
			// UploadModeComboBox
			// 
			this.UploadModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.UploadModeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.UploadModeComboBox.FormattingEnabled = true;
			this.UploadModeComboBox.Location = new System.Drawing.Point(333, 164);
			this.UploadModeComboBox.Name = "UploadModeComboBox";
			this.UploadModeComboBox.Size = new System.Drawing.Size(174, 21);
			this.UploadModeComboBox.TabIndex = 9;
			this.UploadModeComboBox.SelectedIndexChanged += new System.EventHandler(this.UploadModeComboBox_SelectedIndexChanged);
			// 
			// ConnectButton
			// 
			this.ConnectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ConnectButton.Location = new System.Drawing.Point(333, 191);
			this.ConnectButton.Name = "ConnectButton";
			this.ConnectButton.Size = new System.Drawing.Size(174, 23);
			this.ConnectButton.TabIndex = 7;
			this.ConnectButton.Text = "Connect";
			this.ConnectButton.UseVisualStyleBackColor = true;
			this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
			// 
			// DeviceSnapshotParamsComboBox
			// 
			this.DeviceSnapshotParamsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DeviceSnapshotParamsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.DeviceSnapshotParamsComboBox.FormattingEnabled = true;
			this.DeviceSnapshotParamsComboBox.Location = new System.Drawing.Point(334, 64);
			this.DeviceSnapshotParamsComboBox.Name = "DeviceSnapshotParamsComboBox";
			this.DeviceSnapshotParamsComboBox.Size = new System.Drawing.Size(173, 21);
			this.DeviceSnapshotParamsComboBox.TabIndex = 5;
			this.DeviceSnapshotParamsComboBox.SelectedIndexChanged += new System.EventHandler(this.DeviceSnapshotParamsComboBox_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(333, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Snapshot Params";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(333, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Input Device";
			// 
			// DeviceComboBox
			// 
			this.DeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DeviceComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.DeviceComboBox.FormattingEnabled = true;
			this.DeviceComboBox.Location = new System.Drawing.Point(336, 23);
			this.DeviceComboBox.Name = "DeviceComboBox";
			this.DeviceComboBox.Size = new System.Drawing.Size(171, 21);
			this.DeviceComboBox.TabIndex = 0;
			this.DeviceComboBox.SelectedIndexChanged += new System.EventHandler(this.DeviceComboBox_SelectedIndexChanged);
			// 
			// PreviewPictureBox
			// 
			this.PreviewPictureBox.BackColor = System.Drawing.Color.Black;
			this.PreviewPictureBox.Controls.Add(this.FramePictureBox);
			this.PreviewPictureBox.Location = new System.Drawing.Point(7, 6);
			this.PreviewPictureBox.Name = "PreviewPictureBox";
			this.PreviewPictureBox.Size = new System.Drawing.Size(320, 240);
			this.PreviewPictureBox.TabIndex = 0;
			this.PreviewPictureBox.TabStop = false;
			// 
			// SnapshotsPage
			// 
			this.SnapshotsPage.Controls.Add(this.SnapshotHistoryListBox);
			this.SnapshotsPage.Controls.Add(this.SnapshotPictureBox);
			this.SnapshotsPage.Location = new System.Drawing.Point(4, 22);
			this.SnapshotsPage.Name = "SnapshotsPage";
			this.SnapshotsPage.Padding = new System.Windows.Forms.Padding(3);
			this.SnapshotsPage.Size = new System.Drawing.Size(515, 265);
			this.SnapshotsPage.TabIndex = 1;
			this.SnapshotsPage.Text = "Snapshot History";
			this.SnapshotsPage.UseVisualStyleBackColor = true;
			// 
			// SnapshotHistoryListBox
			// 
			this.SnapshotHistoryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SnapshotHistoryListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SnapshotHistoryListBox.FormattingEnabled = true;
			this.SnapshotHistoryListBox.Location = new System.Drawing.Point(333, 6);
			this.SnapshotHistoryListBox.Name = "SnapshotHistoryListBox";
			this.SnapshotHistoryListBox.Size = new System.Drawing.Size(174, 236);
			this.SnapshotHistoryListBox.TabIndex = 1;
			this.SnapshotHistoryListBox.SelectedIndexChanged += new System.EventHandler(this.SnapshotHistoryListBox_SelectedIndexChanged);
			// 
			// SnapshotPictureBox
			// 
			this.SnapshotPictureBox.BackColor = System.Drawing.Color.Black;
			this.SnapshotPictureBox.Location = new System.Drawing.Point(7, 6);
			this.SnapshotPictureBox.Name = "SnapshotPictureBox";
			this.SnapshotPictureBox.Size = new System.Drawing.Size(320, 240);
			this.SnapshotPictureBox.TabIndex = 0;
			this.SnapshotPictureBox.TabStop = false;
			// 
			// UploadSettingsPage
			// 
			this.UploadSettingsPage.Controls.Add(this.WebCamDescriptionTextBox);
			this.UploadSettingsPage.Controls.Add(this.label7);
			this.UploadSettingsPage.Controls.Add(this.label6);
			this.UploadSettingsPage.Controls.Add(this.ImageFormatComboBox);
			this.UploadSettingsPage.Controls.Add(this.WebCamTitleTextBox);
			this.UploadSettingsPage.Controls.Add(this.label5);
			this.UploadSettingsPage.Controls.Add(this.UploadAddressTextBox);
			this.UploadSettingsPage.Controls.Add(this.label2);
			this.UploadSettingsPage.Controls.Add(this.UploadProxyAddress);
			this.UploadSettingsPage.Controls.Add(this.UploadProxyAddressTextBox);
			this.UploadSettingsPage.Location = new System.Drawing.Point(4, 22);
			this.UploadSettingsPage.Name = "UploadSettingsPage";
			this.UploadSettingsPage.Size = new System.Drawing.Size(515, 265);
			this.UploadSettingsPage.TabIndex = 2;
			this.UploadSettingsPage.Text = "Upload Settings";
			this.UploadSettingsPage.UseVisualStyleBackColor = true;
			// 
			// WebCamDescriptionTextBox
			// 
			this.WebCamDescriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.WebCamDescriptionTextBox.Location = new System.Drawing.Point(6, 145);
			this.WebCamDescriptionTextBox.Multiline = true;
			this.WebCamDescriptionTextBox.Name = "WebCamDescriptionTextBox";
			this.WebCamDescriptionTextBox.Size = new System.Drawing.Size(501, 100);
			this.WebCamDescriptionTextBox.TabIndex = 11;
			this.WebCamDescriptionTextBox.TextChanged += new System.EventHandler(this.WebCamDescriptionTextBox_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(3, 129);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(107, 13);
			this.label7.TabIndex = 10;
			this.label7.Text = "WebCam Description";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(178, 89);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(74, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "WebCam Title";
			// 
			// ImageFormatComboBox
			// 
			this.ImageFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ImageFormatComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ImageFormatComboBox.FormattingEnabled = true;
			this.ImageFormatComboBox.Location = new System.Drawing.Point(7, 105);
			this.ImageFormatComboBox.Name = "ImageFormatComboBox";
			this.ImageFormatComboBox.Size = new System.Drawing.Size(164, 21);
			this.ImageFormatComboBox.TabIndex = 8;
			// 
			// WebCamTitleTextBox
			// 
			this.WebCamTitleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.WebCamTitleTextBox.Location = new System.Drawing.Point(181, 106);
			this.WebCamTitleTextBox.Name = "WebCamTitleTextBox";
			this.WebCamTitleTextBox.Size = new System.Drawing.Size(326, 20);
			this.WebCamTitleTextBox.TabIndex = 7;
			this.WebCamTitleTextBox.TextChanged += new System.EventHandler(this.WebCamTitleTextBox_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(4, 89);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Image Format";
			// 
			// UploadAddressTextBox
			// 
			this.UploadAddressTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.UploadAddressTextBox.Location = new System.Drawing.Point(7, 27);
			this.UploadAddressTextBox.Name = "UploadAddressTextBox";
			this.UploadAddressTextBox.Size = new System.Drawing.Size(500, 20);
			this.UploadAddressTextBox.TabIndex = 5;
			this.UploadAddressTextBox.Text = "http://ni.kiev.ua/ShareWebCam/service.aspx";
			this.UploadAddressTextBox.TextChanged += new System.EventHandler(this.UploadAddressTextBox_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Upload Address";
			// 
			// UploadProxyAddress
			// 
			this.UploadProxyAddress.AutoSize = true;
			this.UploadProxyAddress.Location = new System.Drawing.Point(4, 50);
			this.UploadProxyAddress.Name = "UploadProxyAddress";
			this.UploadProxyAddress.Size = new System.Drawing.Size(74, 13);
			this.UploadProxyAddress.TabIndex = 1;
			this.UploadProxyAddress.Text = "Proxy Address";
			// 
			// UploadProxyAddressTextBox
			// 
			this.UploadProxyAddressTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.UploadProxyAddressTextBox.Location = new System.Drawing.Point(7, 66);
			this.UploadProxyAddressTextBox.Name = "UploadProxyAddressTextBox";
			this.UploadProxyAddressTextBox.Size = new System.Drawing.Size(500, 20);
			this.UploadProxyAddressTextBox.TabIndex = 0;
			this.UploadProxyAddressTextBox.TextChanged += new System.EventHandler(this.UploadProxyAddressTextBox_TextChanged);
			// 
			// UploadTimer
			// 
			this.UploadTimer.Interval = 200;
			this.UploadTimer.Tick += new System.EventHandler(this.UploadTimer_Tick);
			// 
			// CheckWebCamTimer
			// 
			this.CheckWebCamTimer.Interval = 5000;
			this.CheckWebCamTimer.Tick += new System.EventHandler(this.CheckWebCamTimer_Tick);
			// 
			// StatusTextBox
			// 
			this.StatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.StatusTextBox.Location = new System.Drawing.Point(2, 298);
			this.StatusTextBox.Name = "StatusTextBox";
			this.StatusTextBox.ReadOnly = true;
			this.StatusTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.StatusTextBox.Size = new System.Drawing.Size(522, 83);
			this.StatusTextBox.TabIndex = 1;
			this.StatusTextBox.Text = "";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(353, 222);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(55, 23);
			this.button1.TabIndex = 21;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// FramePictureBox
			// 
			this.FramePictureBox.EnableRedraw = false;
			this.FramePictureBox.Location = new System.Drawing.Point(38, 35);
			this.FramePictureBox.Name = "FramePictureBox";
			this.FramePictureBox.Size = new System.Drawing.Size(160, 120);
			this.FramePictureBox.TabIndex = 20;
			this.FramePictureBox.TabStop = false;
			this.FramePictureBox.Visible = false;
			this.FramePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FramePictureBox_MouseDown);
			this.FramePictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FramePictureBox_MouseMove);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(414, 222);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(63, 23);
			this.button2.TabIndex = 22;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(525, 384);
			this.Controls.Add(this.StatusTextBox);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Share WebCam!";
			this.tabControl1.ResumeLayout(false);
			this.WebCamPage.ResumeLayout(false);
			this.WebCamPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPictureBox)).EndInit();
			this.PreviewPictureBox.ResumeLayout(false);
			this.SnapshotsPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.SnapshotPictureBox)).EndInit();
			this.UploadSettingsPage.ResumeLayout(false);
			this.UploadSettingsPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage WebCamPage;
		private System.Windows.Forms.TabPage SnapshotsPage;
		private System.Windows.Forms.PictureBox PreviewPictureBox;
		private System.Windows.Forms.TabPage UploadSettingsPage;
		private System.Windows.Forms.Label UploadProxyAddress;
		private System.Windows.Forms.TextBox UploadProxyAddressTextBox;
		private System.Windows.Forms.TextBox UploadAddressTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Timer UploadTimer;
		private System.Windows.Forms.PictureBox SnapshotPictureBox;
		private System.Windows.Forms.ComboBox DeviceComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox DeviceSnapshotParamsComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button ConnectButton;
		private System.Windows.Forms.ComboBox UploadModeComboBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Timer CheckWebCamTimer;
		private System.Windows.Forms.RichTextBox StatusTextBox;
		private System.Windows.Forms.ListBox SnapshotHistoryListBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox WebCamTitleTextBox;
		private System.Windows.Forms.ComboBox ImageFormatComboBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox WebCamDescriptionTextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.LinkLabel OpenWebCamLinkLabel;
		private System.Windows.Forms.LinkLabel UploadImmediatelyLinkLabel;
		private System.Windows.Forms.Button SelectSubFrameButton;
		private ResizableRectangle FramePictureBox;
		private System.Windows.Forms.ComboBox SubFrameComboBox;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}

