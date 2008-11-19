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
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;

namespace ShareWebCam.Publishing {

	public class ImageFormatDescriptor {
		ImageFormat _Format = null;
		string _Caption;
		string _ContentType;
		ImageCodecInfo _CodecInfo = null;
		EncoderParameters _CodecParams = null;

		public ImageFormat Format { get { return _Format; } }

		public string Caption { get { return _Caption; } }

		public string ContentType { get { return _ContentType; } }

		public ImageCodecInfo CodecInfo { get { return _CodecInfo; } }

		public EncoderParameters CodecParams { get { return _CodecParams; } }

		public ImageFormatDescriptor(ImageFormat fmt, string caption, string contentType) {
			_Caption = caption;
			_ContentType = contentType;
			_Format = fmt;
		}

		public ImageFormatDescriptor(ImageCodecInfo codecInfo, EncoderParameters codecParams, string caption, string contentType) {
			_Caption = caption;
			_ContentType = contentType;
			_CodecInfo = codecInfo;
			_CodecParams = codecParams;
		}

		public void SaveImage(Image img, Stream outStream) {
			if (Format!=null)
				img.Save(outStream, Format);
			else
				img.Save(outStream, CodecInfo, CodecParams);
		}

		public static ImageCodecInfo GetEncoderInfo(String mimeType) {
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for (j = 0; j < encoders.Length; ++j) {
				if (encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}
		public static EncoderParameters GetEncoderParams(long lCompression) {
			EncoderParameters eps = new EncoderParameters(1);
			eps.Param[0] = new EncoderParameter(Encoder.Quality, lCompression);
			return eps;
		}

	}


}
