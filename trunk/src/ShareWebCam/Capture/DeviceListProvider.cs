using System;
using System.Collections;
using System.Text;

using DirectShowLib;

namespace ShareWebCam.Capture {
	
	public class DeviceListProvider {

		public static DeviceDescriptor[] VideoInputDevices {
			get {
				// Get the collection of video devices
				DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
				DeviceDescriptor[] list = new DeviceDescriptor[capDevices.Length];
				for (int i=0; i<capDevices.Length; i++) {
					list[i] = new DeviceDescriptor(i, capDevices[i].Name, capDevices[i].DevicePath);
					//System.Diagnostics.Trace.WriteLine( capDevices[i].Name + " " + capDevices[i].DevicePath );
				}
				return list;
			}
		}

		public static VideoModeDescriptor[] GetVideoDeviceModes(int deviceIdx) {
			return new VideoModeDescriptor[] {
				new VideoModeDescriptor(320, 240, 16),
				new VideoModeDescriptor(320, 240, 24),
				new VideoModeDescriptor(320, 240, 32),
				new VideoModeDescriptor(640, 480, 16),
				new VideoModeDescriptor(640, 480, 24),
				new VideoModeDescriptor(640, 480, 32)
			};
		}



	}

	public class VideoModeDescriptor {
		int _Width;
		int _Height;
		short _BitsPerPixel;

		public int Width { get { return _Width; } }
		public int Height { get { return _Height; } }
		public short BitsPerPixel { get { return _BitsPerPixel; } }

		public string Caption {
			get { return String.Format("{0}x{1} {2}bpp", Width, Height, BitsPerPixel); }
		}

		public VideoModeDescriptor(int width, int height, short bpp) {
			_Width = width;
			_Height = height;
			_BitsPerPixel = bpp;
		}
	}


	public class DeviceDescriptor {
		int _Index;
		string _Caption;
		string _Uid;

		public int Index {
			get { return _Index; }
		}

		public string Caption {
			get { return _Caption; }
		}

		public string Uid {
			get { return _Uid; }
		}

		public DeviceDescriptor(int idx, string caption, string uid) {
			_Index = idx;
			_Caption = caption;
			_Uid = uid;
		}

	}



}
