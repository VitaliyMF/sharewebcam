using System;
using System.Collections;
using System.Text;
using System.Drawing;

namespace ShareWebCam.Capture {

	public class VideoModeDescriptor {
		int _Width;
		int _Height;
		short _BitsPerPixel;
		Size[] _SubFrameSizes = null;

		public int Width { get { return _Width; } }
		public int Height { get { return _Height; } }
		public short BitsPerPixel { get { return _BitsPerPixel; } }
		public Size[] SubFrameSizes { get { return _SubFrameSizes; } }
		string _CaptionFormat = "{0}x{1} {2}bpp";

		public string Caption {
			get { return String.Format(_CaptionFormat, Width, Height, BitsPerPixel); }
		}

		public string StrValue {
			get { 
				return BitsPerPixel>0 ? 
					String.Format("{0}x{1}x{2}", Width, Height, BitsPerPixel) :
					String.Format("{0}x{1}", Width, Height);
			}
		}

		public VideoModeDescriptor(int width, int height, short bpp, Size[] subFrameSizes, string captionFmt) :
				this (width, height, bpp, subFrameSizes) {
			_CaptionFormat = captionFmt;
		}

		public VideoModeDescriptor(int width, int height, short bpp, Size[] subFrameSizes) {
			_Width = width;
			_Height = height;
			_BitsPerPixel = bpp;
			_SubFrameSizes = subFrameSizes;
		}

	}


}
