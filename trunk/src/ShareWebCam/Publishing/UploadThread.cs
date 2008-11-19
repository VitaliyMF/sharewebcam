#region License
/*
 * ShareWebCam utility (http://sharewebcam.googlecode.com/)
 * Copyright 2007 Vitaliy Fedorchenko
 * Distributed under the LGPL licence
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices; 

using ShareWebCam.Capture;

namespace ShareWebCam.Publishing {
	
	public class UploadThread {
		PublishingService publishingService;
		ImageCapture imageCapture;
		bool isActive = false;
		bool restartCapture = false;
		Thread contextThread = null;
		ImageFormatDescriptor imageFormat;
		DeviceDescriptor deviceDescriptor;
		Rectangle subFrame = Rectangle.Empty;
		int uploadCounter = 0;

		public event SnapshotEventHandler SnapshotUploaded;

		public UploadThread() {
		}

		public bool IsActive {
			get { return isActive; }
		}

		public bool RestartCapture {
			get { return restartCapture; }
			set { restartCapture = value; }
		}


		public Rectangle SubFrame {
			get { return subFrame; }
			set { subFrame = value; }
		}

		public void Start(PublishingService publishingService,
							ImageCapture imageCapture,
							ImageFormatDescriptor imgFormat) {
			if (isActive) {
				Trace.WriteLine("Upload skipped: previous snapshot still in process.");
				return;
			}

			this.publishingService = publishingService;
			this.imageCapture = imageCapture;
			this.imageFormat = imgFormat;

			if (contextThread!=null && contextThread.IsAlive)
				contextThread.Abort();

			contextThread = new Thread(new ThreadStart(StartInternal));
			contextThread.Start();
		}

		protected void StartInternal() {
			isActive = true;
			try {
				// capture image
				IntPtr m_ip = imageCapture.Click();
				if (m_ip==IntPtr.Zero) {
					Trace.WriteLine("Cannot get snapshot from video input device. Capture graph rebuild requested.");
					restartCapture = true;
					return;
				}

				//PixelFormat.32bppR
				Bitmap b = new Bitmap(imageCapture.Width, imageCapture.Height, imageCapture.Stride,
						imageCapture.PixelFormat, m_ip);
				b.RotateFlip(RotateFlipType.RotateNoneFlipY);

				// process subframe
				if (!SubFrame.IsEmpty) {
					Bitmap subFrameB = new Bitmap(SubFrame.Width, SubFrame.Height);
					Graphics subFrameG = Graphics.FromImage(subFrameB);
					subFrameG.DrawImage(b, 0, 0, SubFrame, GraphicsUnit.Pixel);
					subFrameG.Dispose();
					b = subFrameB;
				}

				MemoryStream memStream = new MemoryStream();
				imageFormat.SaveImage(b, memStream);
				//b.Save(memStream, imageFormat.Format);

				//ImageFormat.
				if (SnapshotUploaded!=null)
					SnapshotUploaded(this, new SnapshotEventArgs(b, DateTime.Now));
				//SnapshotPictureBox.Image = (Image)b.GetThumbnailImage(SnapshotPictureBox.Width, SnapshotPictureBox.Height, null, IntPtr.Zero);
				//SnapshotPictureBox.Image.Res

				if (m_ip != IntPtr.Zero) {
					Marshal.FreeCoTaskMem(m_ip);
					m_ip = IntPtr.Zero;
				}

				byte[] imageBytes = memStream.ToArray();

				publishingService.Upload(imageBytes);
				uploadCounter++;

				string frameSizeInfo = String.Format("{0}x{1}", imageCapture.Width, imageCapture.Height);
				if (!SubFrame.IsEmpty)
					frameSizeInfo += String.Format("->{0}x{1}", SubFrame.Width, SubFrame.Height);
				Trace.WriteLine(String.Format("Uploaded image ({0}, {1}, {2} bytes).", imageFormat.Caption, frameSizeInfo, imageBytes.Length));

			} catch (Exception ex) {
				Trace.WriteLine("Upload failed: " + ex.Message);
			} finally {
				isActive = false;
			}
		}


	}
}
