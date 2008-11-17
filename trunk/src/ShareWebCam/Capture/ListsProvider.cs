using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using DirectShowLib;

namespace ShareWebCam.Capture {
	
	public class ListsProvider {

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
			Size[] mode640frameSizes = new Size[] {
				new Size(320, 240),
				new Size(480, 360)
			};
			return new VideoModeDescriptor[] {
				new VideoModeDescriptor(320, 240, 16, null),
				new VideoModeDescriptor(320, 240, 24, null),
				new VideoModeDescriptor(320, 240, 32, null),
				new VideoModeDescriptor(640, 480, 16, mode640frameSizes ),
				new VideoModeDescriptor(640, 480, 24, mode640frameSizes ),
				new VideoModeDescriptor(640, 480, 32, mode640frameSizes )
			};
		}

		public static VideoModeDescriptor[] GetSubFrameModes(VideoModeDescriptor videoMode) {
			int subFrameModes = videoMode.SubFrameSizes!=null ? videoMode.SubFrameSizes.Length : 0;
			VideoModeDescriptor[] descriptors = new VideoModeDescriptor[subFrameModes+1];
			descriptors[0] = new VideoModeDescriptor(0,0,0,null,"Take whole snapshot");
			for (int i=1; i<=subFrameModes; i++)
				descriptors[i] = new VideoModeDescriptor(
					videoMode.SubFrameSizes[i-1].Width,
					videoMode.SubFrameSizes[i-1].Height,
					0, null, "Take subframe {0}x{1}");
			return descriptors;
		}



	}



}
