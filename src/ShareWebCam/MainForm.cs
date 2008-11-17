using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices; 
using System.Threading;

using ShareWebCam.Capture;
using ShareWebCam.Publishing;
using ShareWebCam.IO;

namespace ShareWebCam {
	
	public partial class MainForm : Form {

		ImageCapture imageCapture = null;
		PublishingService publishingService;
		UploadThread uploadThread;
		ConfigFileEditor configEditor;
		bool isConnected = false;
		bool isInit = true;

		public MainForm() {
			InitializeComponent();

			configEditor = new ConfigFileEditor(Application.ExecutablePath+".config");

			// add trace listener
			Trace.Listeners.Add( new TextBoxTraceListener("statusListener", StatusTextBox) );
			uploadThread = new UploadThread();
			publishingService = new PublishingService();
			
			SnapshotHistoryListBox.DisplayMember = "Caption";
			uploadThread.SnapshotUploaded += new SnapshotEventHandler(uploadThread_SnapshotUploaded);

			UploadAddressTextBox.Text = publishingService.UploadAddress;
			UploadProxyAddressTextBox.Text = publishingService.ProxyAddress;

			DeviceComboBox.DisplayMember = "Caption";
			DeviceComboBox.ValueMember = "Index";
			DeviceComboBox.DataSource = ListsProvider.VideoInputDevices;
			if (DeviceComboBox.Items.Count == 0) {
				Trace.WriteLine("No video capture devices found!");
			}
			CheckWebCamTimer.Start(); // look for hardware changes

			UploadModeComboBox.DisplayMember = "Caption";
			UploadModeComboBox.ValueMember = "MSecs";
			UploadModeComboBox.DataSource = UploadModeListProvider.UploadModeList;
			try {
				UploadModeComboBox.SelectedValue = Convert.ToInt32( ConfigurationSettings.AppSettings["DefaultUploadMode"] );
			} catch (Exception ex) {
				Trace.WriteLine("Cannot switch to default upload mode: "+ex.Message);
			}

			ImageFormatComboBox.DisplayMember = "Caption";
			ImageFormatComboBox.DataSource = UploadModeListProvider.ImageFormatList;

			DeviceSnapshotParamsComboBox.DisplayMember = "Caption";
			DeviceSnapshotParamsComboBox.ValueMember = "Caption";
			DeviceSnapshotParamsComboBox.DataSource = ListsProvider.GetVideoDeviceModes(0);
			try {
				DeviceSnapshotParamsComboBox.SelectedValue = Convert.ToString(ConfigurationSettings.AppSettings["DefaultSnapshotParams"]);
			} catch (Exception ex) {
				Trace.WriteLine("Cannot switch to default snapshot params: "+ex.Message);
			}

			WebCamTitleTextBox.Text = publishingService.WebCamTitle;
			WebCamDescriptionTextBox.Text = publishingService.WebCamDescription;
			//WebCamNameTextBox.Text = Environment.UserName+" WebCam";

			isInit = false;

		}

		void uploadThread_SnapshotUploaded(object sender, SnapshotEventArgs args) {
			SnapshotHistoryListBox.Items.Insert(0,
				new SnapshotEventArgs(
					args.Snapshot.GetThumbnailImage(
						SnapshotPictureBox.Width,
						SnapshotPictureBox.Height, null, IntPtr.Zero), args.Timestamp, 
						String.Format("{0}x{1}", args.Snapshot.Width, args.Snapshot.Height)+" {2}" ) );
			if (SnapshotHistoryListBox.Items.Count>20) {
				SnapshotEventArgs oldArgs = (SnapshotEventArgs)SnapshotHistoryListBox.Items[SnapshotHistoryListBox.Items.Count-1];
				SnapshotHistoryListBox.Items.RemoveAt(SnapshotHistoryListBox.Items.Count-1);
				// free mem
				oldArgs.Snapshot.Dispose();
			}
		}



		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			Refresh();
			InitImageCapture();

			InitTimer();

		}

		protected void InitImageCapture() {
			bool firstInit = imageCapture==null;
			if (imageCapture!=null) {
				imageCapture.Dispose();
				imageCapture = null;
			}

			if (DeviceComboBox.Items.Count==0) return;

			Cursor.Current = Cursors.WaitCursor;
			try {
				DeviceDescriptor selectedDeviceDescriptor = DeviceComboBox.SelectedItem as DeviceDescriptor;
				if (selectedDeviceDescriptor==null)
					throw new Exception("Please select input device first!");
				int deviceIndex = selectedDeviceDescriptor.Index;
				publishingService.WebCamUid = selectedDeviceDescriptor.Uid;
				VideoModeDescriptor videoMode = (VideoModeDescriptor)DeviceSnapshotParamsComboBox.SelectedItem;

				Trace.WriteLine( String.Format("Initializing {0} ({1})... (this may take some time)",
					selectedDeviceDescriptor.Caption, videoMode.Caption ) );
				imageCapture = new ImageCapture( 
					deviceIndex,
					videoMode.Width, videoMode.Height, videoMode.BitsPerPixel, PreviewPictureBox);
			} catch (COMException comEx) {
				if (firstInit && comEx.ErrorCode==(-2147220969) )
					if (DeviceSnapshotParamsComboBox.SelectedIndex<(DeviceSnapshotParamsComboBox.Items.Count-1) ) {
						Trace.WriteLine("Trying another videomode...");
						DeviceSnapshotParamsComboBox.SelectedIndex++;
						return;
					}
				Trace.WriteLine(comEx.Message);	
			} catch (Exception ex) {
				Trace.WriteLine(ex.Message);
			} finally {
				Cursor.Current = Cursors.Default;
			}
		}

		protected void InitTimer() {
			int msecs = Convert.ToInt32(UploadModeComboBox.SelectedValue);
			if (UploadTimer.Enabled) {
				UploadTimer.Stop();
				Trace.WriteLine("Automatic upload is disabled.");
			}
			if (msecs > 0 && isConnected) {
				UploadTimer.Interval = msecs;
				UploadTimer.Start();
				Trace.WriteLine("Automatic upload is enabled.");
			}
		}

		protected void InitFrameSelector() {
			VideoModeDescriptor videoMode = (VideoModeDescriptor)DeviceSnapshotParamsComboBox.SelectedItem;

			SubFrameComboBox.DataSource = ListsProvider.GetSubFrameModes(videoMode);
			try {
				Rectangle savedRect = configEditor.GetRectangle("DefaultSubFrameRectangle", Rectangle.Empty);
				if (!savedRect.IsEmpty)
					SubFrameComboBox.SelectedValue = String.Format("{0}x{1}", savedRect.Width, savedRect.Height);
			} catch {
				SubFrameComboBox.SelectedIndex = 0;
			}
			

			if (videoMode.SubFrameSizes != null) {
				SubFrameComboBox.Enabled = true;
			} else {
				SubFrameComboBox.Enabled = false;
				SelectSubFrameButton.Enabled = false;
			}

		}

		protected void UploadSnapshot() {
			if (imageCapture!=null && publishingService!=null && !FramePictureBox.Capture) {
				if (uploadThread.RestartCapture) {
					uploadThread.RestartCapture = false;
					InitImageCapture();
				}
				uploadThread.Start(
					publishingService,
					imageCapture,
					(ImageFormatDescriptor)ImageFormatComboBox.SelectedItem );
			}
		}


		private void UploadAddressTextBox_TextChanged(object sender, EventArgs e) {
			publishingService.UploadAddress = ((TextBox)sender).Text;
			if (!isInit)
				configEditor.SetValue("UploadAddress", publishingService.UploadAddress);
		}

		private void UploadProxyAddressTextBox_TextChanged(object sender, EventArgs e) {
			publishingService.ProxyAddress = ((TextBox)sender).Text;
			if (!isInit)
				configEditor.SetValue("UploadProxyAddress", publishingService.ProxyAddress);
		}

		private void DeviceComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (!isInit)
				InitImageCapture();
		}

		private void UploadModeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (!isInit)
				InitTimer();
		}

		private void UploadTimer_Tick(object sender, EventArgs e) {
			UploadSnapshot();
		}

		private void UploadButton_Click(object sender, EventArgs e) {
		}

		private void ConnectButton_Click(object sender, EventArgs e) {
			isConnected = !isConnected;
			ConnectButton.Text = isConnected ? "Disconnect" : "Connect";

			InitTimer();
		}

		protected bool AreEquals(DeviceDescriptor[] a, DeviceDescriptor[] b) {
			if (a.Length!=b.Length)
				return false;
			for (int i=0; i<a.Length; i++)
				if (a[i].Caption != b[i].Caption)
					return false;
			return true;
		}

		private void CheckWebCamTimer_Tick(object sender, EventArgs e) {
			DeviceDescriptor[] newDescriptors = ListsProvider.VideoInputDevices;
			DeviceDescriptor[] oldDescriptors = DeviceComboBox.DataSource as DeviceDescriptor[];
			if (!AreEquals(newDescriptors,oldDescriptors)) {
				Trace.WriteLine("Capture devices list was updated. Applying changes...");
				DeviceComboBox.DataSource = newDescriptors;
				InitImageCapture();
			}

		}

		private void DeviceSnapshotParamsComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (!isInit)
				InitImageCapture();
			// subframe selection allowed ?
			InitFrameSelector();
			// save choice
			configEditor.SetValue("DefaultSnapshotParams", Convert.ToString(DeviceSnapshotParamsComboBox.SelectedValue));
		}

		private void OpenWebCamPageButton_Click(object sender, EventArgs e) {
		}

		private void SnapshotHistoryListBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (SnapshotHistoryListBox.SelectedItem!=null) {
				SnapshotEventArgs eventArgs = (SnapshotEventArgs)SnapshotHistoryListBox.SelectedItem;
				SnapshotPictureBox.Image = eventArgs.Snapshot.GetThumbnailImage(SnapshotPictureBox.Width, SnapshotPictureBox.Height, null, IntPtr.Zero);
			}
		}

		private void WebCamTitleTextBox_TextChanged(object sender, EventArgs e) {
			publishingService.WebCamTitle = WebCamTitleTextBox.Text;
			if (!isInit)
				configEditor.SetValue("DefaultWebCamTitle", publishingService.WebCamTitle);
		}

		private void WebCamDescriptionTextBox_TextChanged(object sender, EventArgs e) {
			publishingService.WebCamDescription = WebCamDescriptionTextBox.Text;
			if (!isInit)
				configEditor.SetValue("DefaultWebCamDescription", publishingService.WebCamDescription);
		}

		private void OpenWebCamLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			publishingService.StartWebCamPage();
		}

		private void UploadImmediatelyLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Trace.WriteLine("Processing immediate upload request...");
			UploadSnapshot();

		}

		private void SelectSubFrameButton_Click(object sender, EventArgs e) {
			if (imageCapture!=null)
				imageCapture.Pause();
			FramePictureBox.Visible = true;
			FramePictureBox.Capture = true;
			FramePictureBox.EnableRedraw = true;

			//subFrameRectangle.Visible = true;
			//subFrameRectangle.Update();
			//subFrameRectangle.Capture = true;
		}

		Point subFrameMovePrevPoint = Point.Empty;

		private void FramePictureBox_MouseMove(object sender, MouseEventArgs e) {
			if (!FramePictureBox.Visible) return;
			if (!subFrameMovePrevPoint.IsEmpty) {
				int deltaX = e.X-subFrameMovePrevPoint.X;
				int deltaY = e.Y-subFrameMovePrevPoint.Y;
				if (deltaX!=0 || deltaY!=0) {
					int newX = FramePictureBox.Left+deltaX;
					int newY = FramePictureBox.Top+deltaY;
					if (newX<0)
						newX = 0;
					if (newX>(PreviewPictureBox.Width-FramePictureBox.Width))
						newX = PreviewPictureBox.Width-FramePictureBox.Width;
					if (newY<0)
						newY = 0;
					if (newY>(PreviewPictureBox.Height-FramePictureBox.Height))
						newY = PreviewPictureBox.Height-FramePictureBox.Height;
					FramePictureBox.Left = newX;
					FramePictureBox.Top = newY;
					FramePictureBox.InvalidateEx();
					//Trace.WriteLine(FramePictureBox.Location.ToString());
					//PreviewPictureBox.Update();
					subFrameMovePrevPoint = Point.Empty;// new Point(e.X-deltaX, e.Y-deltaY);
				}
				
			} else
				subFrameMovePrevPoint = new Point(e.X, e.Y);
			FramePictureBox.Capture = true;
		}

		private void FramePictureBox_MouseDown(object sender, MouseEventArgs e) {
			FramePictureBox.Capture = false;
			FramePictureBox.Visible = false;
			FramePictureBox.EnableRedraw = false;

			VideoModeDescriptor videoMode = (VideoModeDescriptor)DeviceSnapshotParamsComboBox.SelectedItem;
			VideoModeDescriptor subFrameMode = (VideoModeDescriptor)SubFrameComboBox.SelectedItem;
			
			double scaleX = videoMode.Width / PreviewPictureBox.Width;
			double scaleY = videoMode.Height / PreviewPictureBox.Height;

			uploadThread.SubFrame = GetCurrentSubFrameRectangle();
			configEditor.SetRectangle("DefaultSubFrameRectangle", uploadThread.SubFrame);

			subFrameMovePrevPoint = Point.Empty;
			if (imageCapture!=null)
				imageCapture.Resume();
		}

		protected Rectangle GetCurrentSubFrameRectangle() {
			VideoModeDescriptor videoMode = (VideoModeDescriptor)DeviceSnapshotParamsComboBox.SelectedItem;
			VideoModeDescriptor subFrameMode = (VideoModeDescriptor)SubFrameComboBox.SelectedItem;

			double scaleX = videoMode.Width / PreviewPictureBox.Width;
			double scaleY = videoMode.Height / PreviewPictureBox.Height;

			return new Rectangle(
				(int)Math.Floor(FramePictureBox.Location.X * scaleX), (int)Math.Floor(FramePictureBox.Location.Y * scaleY),
				subFrameMode.Width, subFrameMode.Height);
		}

		private void SubFrameComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			VideoModeDescriptor videoMode = (VideoModeDescriptor)DeviceSnapshotParamsComboBox.SelectedItem;
			VideoModeDescriptor subFrameMode = (VideoModeDescriptor)SubFrameComboBox.SelectedItem;
			
			if (subFrameMode.Width>0 && subFrameMode.Height>0) {
				double scaleX = videoMode.Width / PreviewPictureBox.Width;
				double scaleY = videoMode.Height / PreviewPictureBox.Height;

				Rectangle defaultRect = new Rectangle(0, 0, subFrameMode.Width, subFrameMode.Height);
				Rectangle initRect = configEditor.GetRectangle(
					"DefaultSubFrameRectangle", defaultRect);
				if (initRect.Width!=defaultRect.Width || initRect.Height!=defaultRect.Height ||
					(initRect.X+initRect.Width)>videoMode.Width ||
					(initRect.Y+initRect.Height)>videoMode.Height)
					initRect = defaultRect;

				FramePictureBox.Location = new Point(
					(int)(initRect.X/scaleX), (int)(initRect.Y/scaleY) );
				FramePictureBox.Size = new Size(
					(int)(subFrameMode.Width / scaleX),
					(int)(subFrameMode.Height / scaleY));
				
				uploadThread.SubFrame = GetCurrentSubFrameRectangle();
				SelectSubFrameButton.Enabled = true;
			} else {
				uploadThread.SubFrame = Rectangle.Empty;
				SelectSubFrameButton.Enabled = false;
			}

		}

		Stream fs;
		/// -f rawvideo -pix_fmt rgb24 -r 5 -s 320x240 -vcodec bmp -i - -s 320x240 -r 5 -vcodec flv -f flv -

		private void button1_Click(object sender, EventArgs e) {
			fs = new FileStream("out.flv", FileMode.OpenOrCreate);
			fs = new TranscodeOutputStream(
				@"-f rawvideo -pix_fmt rgb24 -r 5 -s 320x240 -vcodec bmp -i - -s 160x120 -r 5 -vcodec flv -f flv -",
				fs);
			imageCapture.StartVideoCapture(fs, 5);
		}

		private void button2_Click(object sender, EventArgs e) {
			imageCapture.StopVideoCapture();
			fs.Close();
		}




	}
}