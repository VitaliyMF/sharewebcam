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
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

using DirectShowLib;

namespace ShareWebCam.Capture {
	
	
	public class ImageCapture : ISampleGrabberCB, IDisposable {
		#region Member variables

		/// <summary> graph builder interface. </summary>
		private IFilterGraph2 m_graphBuilder = null;

		// Used to snap picture on Still pin
		private IAMVideoControl m_VidControl = null;
		private IPin m_pinStill = null;
		private DsDevice videoInputDevice = null;
		private Control videoParentControl = null;

		/// <summary> so we can wait for the async job to finish </summary>
		private ManualResetEvent m_PictureReady = null;

		private bool m_WantOne = false;

		/// <summary> Dimensions of the image, calculated once in constructor for perf. </summary>
		private int m_videoWidth;
		private int m_videoHeight;
		private int m_videoBPP;
		private int m_stride;

		/// <summary> buffer for bitmap data.  Always release by caller</summary>
		private IntPtr m_ipBuffer = IntPtr.Zero;

		private bool m_WantVideo = false;
		Stream videoOutputStream;
		int videoFps = 5;
		DateTime videoStartDate;
		int framesCount = 0;

#if DEBUG
		// Allow you to "Connect to remote graph" from GraphEdit
		DsROTEntry m_rot = null;
#endif
		#endregion

		#region APIs
		[DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
		private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
		#endregion

		// Zero based device index and device params and output window
		public ImageCapture(int iDeviceNum, int iWidth, int iHeight, short iBPP, Control hControl) {
			DsDevice[] capDevices;

			// Get the collection of video devices
			capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

			if (iDeviceNum + 1 > capDevices.Length) {
				throw new Exception("No video capture devices found at that index!");
			}

			try {
				// Set up the capture graph
				SetupGraph(capDevices[iDeviceNum], iWidth, iHeight, iBPP, hControl);

				// tell the callback to ignore new images
				m_PictureReady = new ManualResetEvent(false);
			} catch {
				Dispose();
				throw;
			}
		}

		/// <summary> release everything. </summary>
		public void Dispose() {
#if DEBUG
			if (m_rot != null) {
				m_rot.Dispose();
			}
#endif
			CloseInterfaces();
			if (m_PictureReady != null) {
				m_PictureReady.Close();
			}
		}
		// Destructor
		~ImageCapture() {
			Dispose();
		}

		/// <summary>
		/// Get the image from the Still pin.  The returned image can turned into a bitmap with
		/// Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
		/// If the image is upside down, you can fix it with
		/// b.RotateFlip(RotateFlipType.RotateNoneFlipY);
		/// </summary>
		/// <returns>Returned pointer to be freed by caller with Marshal.FreeCoTaskMem</returns>
		public IntPtr Click() {
			int hr;

			// get ready to wait for new image
			m_PictureReady.Reset();
			m_ipBuffer = Marshal.AllocCoTaskMem(Math.Abs(m_stride) * m_videoHeight);

			try {
				m_WantOne = true;

				// If we are using a still pin, ask for a picture
				if (m_VidControl != null) {
					// Tell the camera to send an image
					hr = m_VidControl.SetMode(m_pinStill, VideoControlFlags.Trigger);
					DsError.ThrowExceptionForHR(hr);
				}

				// Start waiting
				if (!m_PictureReady.WaitOne(9000, false)) {
					throw new Exception("Timeout waiting to get picture");
				}
			} catch (Exception ex) {
				//Trace.WriteLine(ex.ToString() );
				Marshal.FreeCoTaskMem(m_ipBuffer);
				m_ipBuffer = IntPtr.Zero;
			}

			// Got one
			return m_ipBuffer;
		}

		public int Width {
			get {
				return m_videoWidth;
			}
		}
		public int Height {
			get {
				return m_videoHeight;
			}
		}

		public int Bpp {
			get {
				return m_videoBPP;
			}
		}


		public int Stride {
			get {
				return m_stride;
			}
		}


		/// <summary> build the capture graph for grabber. </summary>
		private void SetupGraph(DsDevice dev, int iWidth, int iHeight, short iBPP, Control hControl) {
			int hr;

			ISampleGrabber sampGrabber = null;
			IBaseFilter capFilter = null;
			IPin pCaptureOut = null;
			IPin pSampleIn = null;
			IPin pRenderIn = null;

			videoInputDevice = dev;
			videoParentControl = hControl;

			// Get the graphbuilder object
			m_graphBuilder = new FilterGraph() as IFilterGraph2;

			try {
#if DEBUG
				m_rot = new DsROTEntry(m_graphBuilder);
#endif
				// add the video input device
				hr = m_graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
				DsError.ThrowExceptionForHR(hr);

				// Find the still pin
				m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Still, 0);

				// Didn't find one.  Is there a preview pin?
				if (m_pinStill == null) {
					m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Preview, 0);
				}

				// Still haven't found one.  Need to put a splitter in so we have
				// one stream to capture the bitmap from, and one to display.  Ok, we
				// don't *have* to do it that way, but we are going to anyway.
				if (m_pinStill == null) {
					IPin pRaw = null;
					IPin pSmart = null;

					// There is no still pin
					m_VidControl = null;

					// Add a splitter
					IBaseFilter iSmartTee = (IBaseFilter)new SmartTee();

					try {
						hr = m_graphBuilder.AddFilter(iSmartTee, "SmartTee");
						DsError.ThrowExceptionForHR(hr);

						// Find the find the capture pin from the video device and the
						// input pin for the splitter, and connnect them
						pRaw = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);
						pSmart = DsFindPin.ByDirection(iSmartTee, PinDirection.Input, 0);

						hr = m_graphBuilder.Connect(pRaw, pSmart);
						DsError.ThrowExceptionForHR(hr);

						// Now set the capture and still pins (from the splitter)
						m_pinStill = DsFindPin.ByName(iSmartTee, "Preview");
						pCaptureOut = DsFindPin.ByName(iSmartTee, "Capture");

						// If any of the default config items are set, perform the config
						// on the actual video device (rather than the splitter)
						if (iHeight + iWidth + iBPP > 0) {
							SetConfigParms(pRaw, iWidth, iHeight, iBPP);
						}
					} finally {
						if (pRaw != null) {
							Marshal.ReleaseComObject(pRaw);
						}
						if (pRaw != pSmart) {
							Marshal.ReleaseComObject(pSmart);
						}
						if (pRaw != iSmartTee) {
							Marshal.ReleaseComObject(iSmartTee);
						}
					}
				} else {
					// Get a control pointer (used in Click())
					m_VidControl = capFilter as IAMVideoControl;

					pCaptureOut = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);

					// If any of the default config items are set
					if (iHeight + iWidth + iBPP > 0) {
						SetConfigParms(m_pinStill, iWidth, iHeight, iBPP);
					}
				}

				// Get the SampleGrabber interface
				sampGrabber = new SampleGrabber() as ISampleGrabber;

				// Configure the sample grabber
				IBaseFilter baseGrabFlt = sampGrabber as IBaseFilter;
				ConfigureSampleGrabber(sampGrabber, iBPP);
				pSampleIn = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);

				// Get the default video renderer
				IBaseFilter pRenderer = new VideoRendererDefault() as IBaseFilter;
				hr = m_graphBuilder.AddFilter(pRenderer, "Renderer");
				DsError.ThrowExceptionForHR(hr);

				pRenderIn = DsFindPin.ByDirection(pRenderer, PinDirection.Input, 0);

				// Add the sample grabber to the graph
				hr = m_graphBuilder.AddFilter(baseGrabFlt, "Ds.NET Grabber");
				DsError.ThrowExceptionForHR(hr);

				if (m_VidControl == null) {
					// Connect the Still pin to the sample grabber
					hr = m_graphBuilder.Connect(m_pinStill, pSampleIn);
					DsError.ThrowExceptionForHR(hr);

					// Connect the capture pin to the renderer
					hr = m_graphBuilder.Connect(pCaptureOut, pRenderIn);
					DsError.ThrowExceptionForHR(hr);
				} else {
					// Connect the capture pin to the renderer
					hr = m_graphBuilder.Connect(pCaptureOut, pRenderIn);
					DsError.ThrowExceptionForHR(hr);

					// Connect the Still pin to the sample grabber
					hr = m_graphBuilder.Connect(m_pinStill, pSampleIn);
					DsError.ThrowExceptionForHR(hr);
				}

				// Learn the video properties
				SaveSizeInfo(sampGrabber);
				ConfigVideoWindow(hControl);

				// Start the graph
				IMediaControl mediaCtrl = m_graphBuilder as IMediaControl;
				hr = mediaCtrl.Run();
				DsError.ThrowExceptionForHR(hr);
			} finally {
				if (sampGrabber != null) {
					Marshal.ReleaseComObject(sampGrabber);
					sampGrabber = null;
				}
				if (pCaptureOut != null) {
					Marshal.ReleaseComObject(pCaptureOut);
					pCaptureOut = null;
				}
				if (pRenderIn != null) {
					Marshal.ReleaseComObject(pRenderIn);
					pRenderIn = null;
				}
				if (pSampleIn != null) {
					Marshal.ReleaseComObject(pSampleIn);
					pSampleIn = null;
				}
			}
		}

		private void SaveSizeInfo(ISampleGrabber sampGrabber) {
			int hr;

			// Get the media type from the SampleGrabber
			AMMediaType media = new AMMediaType();

			hr = sampGrabber.GetConnectedMediaType(media);
			DsError.ThrowExceptionForHR(hr);

			if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero)) {
				throw new NotSupportedException("Unknown Grabber Media Format");
			}

			// Grab the size info
			VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
			m_videoWidth = videoInfoHeader.BmiHeader.Width;
			m_videoHeight = videoInfoHeader.BmiHeader.Height;
			m_videoBPP = videoInfoHeader.BmiHeader.BitCount;
			m_stride = m_videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

			DsUtils.FreeAMMediaType(media);
			media = null;
		}

		// Set the video window within the control specified by hControl
		private void ConfigVideoWindow(Control hControl) {
			int hr;

			IVideoWindow ivw = m_graphBuilder as IVideoWindow;

			// Set the parent
			hr = ivw.put_Owner(hControl.Handle);
			DsError.ThrowExceptionForHR(hr);

			// Turn off captions, etc
			hr = ivw.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings);
			DsError.ThrowExceptionForHR(hr);
			
			// Yes, make it visible
			hr = ivw.put_Visible(OABool.True);
			DsError.ThrowExceptionForHR(hr);

			// Move to upper left corner
			Rectangle rc = hControl.ClientRectangle;
			hr = ivw.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
			DsError.ThrowExceptionForHR(hr);
		}

		private Guid ResolveMediaSubType(short bpp) {
			if (bpp==8)
				return MediaSubType.RGB8;
			if (bpp==16)
				return MediaSubType.RGB16_D3D_DX7_RT;
			if (bpp==24)
				return MediaSubType.RGB24;
			if (bpp==32)
				return MediaSubType.RGB32;
			throw new Exception("Invalid bits per pixel value: "+bpp.ToString() );
		}

		private void ConfigureSampleGrabber(ISampleGrabber sampGrabber, short bpp) {
			int hr;
			AMMediaType media = new AMMediaType();

			// Set the media type to Video/RBG24
			media.majorType = MediaType.Video;
			media.subType = ResolveMediaSubType(bpp);
			media.formatType = FormatType.VideoInfo;
			hr = sampGrabber.SetMediaType(media);
			DsError.ThrowExceptionForHR(hr);

			DsUtils.FreeAMMediaType(media);
			media = null;

			// Configure the samplegrabber
			hr = sampGrabber.SetCallback(this, 1);
			DsError.ThrowExceptionForHR(hr);
		}

		// Set the Framerate, and video size
		private void SetConfigParms(IPin pStill, int iWidth, int iHeight, short iBPP) {
			int hr;
			AMMediaType media;
			VideoInfoHeader v;

			IAMStreamConfig videoStreamConfig = pStill as IAMStreamConfig;

			// Get the existing format block
			hr = videoStreamConfig.GetFormat(out media);
			DsError.ThrowExceptionForHR(hr);

			try {
				// copy out the videoinfoheader
				v = new VideoInfoHeader();
				Marshal.PtrToStructure(media.formatPtr, v);

				// if overriding the width, set the width
				if (iWidth > 0) {
					v.BmiHeader.Width = iWidth;
				}

				// if overriding the Height, set the Height
				if (iHeight > 0) {
					v.BmiHeader.Height = iHeight;
				}

				// if overriding the bits per pixel
				if (iBPP > 0) {
					v.BmiHeader.BitCount = iBPP;
				}

				// Copy the media structure back
				Marshal.StructureToPtr(v, media.formatPtr, false);

				// Set the new format
				hr = videoStreamConfig.SetFormat(media);
				DsError.ThrowExceptionForHR(hr);
			} finally {
				DsUtils.FreeAMMediaType(media);
				media = null;
			}
		}

		/// <summary> Shut down capture </summary>
		private void CloseInterfaces() {
			int hr;

			try {
				if (m_graphBuilder != null) {
					IMediaControl mediaCtrl = m_graphBuilder as IMediaControl;

					// Stop the graph
					hr = mediaCtrl.Stop();

					//IVideoWindow ivw = m_graphBuilder as IVideoWindow;
					//ivw.put_Owner(IntPtr.Zero);

				}
			} catch (Exception ex) {
				Debug.WriteLine(ex);
			}

			if (m_graphBuilder != null) {
				Marshal.ReleaseComObject(m_graphBuilder);
				m_graphBuilder = null;
			}

			if (m_VidControl != null) {
				Marshal.ReleaseComObject(m_VidControl);
				m_VidControl = null;
			}

			if (m_pinStill != null) {
				Marshal.ReleaseComObject(m_pinStill);
				m_pinStill = null;
			}
		}

		/// <summary> sample callback, NOT USED. </summary>
		int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample) {
			Marshal.ReleaseComObject(pSample);
			return 0;
		}

		public void StartVideoCapture(Stream videoOut, int fps) {
			videoOutputStream = videoOut;
			videoFps = fps;
			videoStartDate = DateTime.Now;
			framesCount = 0;
			m_WantVideo = true;
		}

		public void StopVideoCapture() {
			m_WantVideo = false;
		}

		public PixelFormat PixelFormat {
			get {
				int bpp = Bpp;
				if (bpp == 8)
					return PixelFormat.Format8bppIndexed;
				if (bpp == 16)
					return PixelFormat.Format16bppRgb555;
				if (bpp == 24)
					return PixelFormat.Format24bppRgb;
				if (bpp == 32)
					return PixelFormat.Format32bppRgb;
				throw new Exception("Invalid bits per pixel value: " + bpp.ToString());
			}
		}


		/// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
		int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen) {
			// Note that we depend on only being called once per call to Click.  Otherwise
			// a second call can overwrite the previous image.
			Debug.Assert(BufferLen == Math.Abs(m_stride) * m_videoHeight, "Incorrect buffer length");

			if (m_WantOne) {
				m_WantOne = false;
				Debug.Assert(m_ipBuffer != IntPtr.Zero, "Unitialized buffer");

				// Save the buffer
				CopyMemory(m_ipBuffer, pBuffer, BufferLen);

				// Picture is ready.
				m_PictureReady.Set();
			}
			if (m_WantVideo) {
				try {
					if (framesCount>0) {
						if (( ((double)framesCount) / DateTime.Now.Subtract(videoStartDate).TotalSeconds) > videoFps)
							return 0;
						else
							framesCount++;
					} else
						framesCount++;

					IntPtr bmpPtr = Marshal.AllocCoTaskMem(Math.Abs(m_stride) * m_videoHeight);

					// Save the buffer
					CopyMemory(bmpPtr, pBuffer, BufferLen);
					Bitmap b = new Bitmap(Width, Height, Stride, PixelFormat, bmpPtr);
					b.RotateFlip(RotateFlipType.RotateNoneFlipY);

					Bitmap bResized = new Bitmap(320, 240, PixelFormat.Format24bppRgb);
					Graphics g = Graphics.FromImage(bResized);
					g.DrawImage(b, new Rectangle(0,0,320,240));

					BitmapData bmpData = bResized.LockBits(new Rectangle(0,0,320,240), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
					byte[] bmpBytes = new byte[320*240*3];
					Trace.WriteLine((((double)framesCount) / DateTime.Now.Subtract(videoStartDate).TotalSeconds).ToString());

					Marshal.Copy(bmpData.Scan0, bmpBytes, 0, bmpBytes.Length);
					videoOutputStream.Write(bmpBytes, 0, bmpBytes.Length);
					bResized.UnlockBits(bmpData);

					Marshal.FreeCoTaskMem(bmpPtr);
					bmpPtr = IntPtr.Zero;
				} catch (Exception ex) {
					Trace.WriteLine(ex.ToString());
				}
			}

			return 0;
		}

		public void Pause() {
			IMediaControl mediaCtrl = m_graphBuilder as IMediaControl;
			int hr = mediaCtrl.Pause();
			DsError.ThrowExceptionForHR(hr);
		}

		public void Resume() {
			IMediaControl mediaCtrl = m_graphBuilder as IMediaControl;
			int hr = mediaCtrl.Run();
			DsError.ThrowExceptionForHR(hr);
		}

		public void Restart() {
			CloseInterfaces();
			SetupGraph(videoInputDevice, m_videoWidth, m_videoHeight, (short)m_videoBPP, videoParentControl); 
		}


	}
}
