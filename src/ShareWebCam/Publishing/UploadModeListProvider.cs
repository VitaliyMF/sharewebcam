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

using System.Drawing;
using System.Drawing.Imaging;

namespace ShareWebCam.Publishing {
	
	public class UploadModeListProvider {
		public static ModeDescriptor[] UploadModeList {
			get {
				return new ModeDescriptor[] {
					new ModeDescriptor(-1, "Upload by demand"),
					new ModeDescriptor(500, "Upload every 500 msec"),
					new ModeDescriptor(1000, "Upload every second"),
					new ModeDescriptor(2000, "Upload every 2 seconds"),
					new ModeDescriptor(5000, "Upload every 5 seconds"),
					new ModeDescriptor(30000, "Upload every 30 seconds"),
					new ModeDescriptor(1000*60*5, "Upload every 5 minutes")
				};
			}
		}

		public static ImageFormatDescriptor[] ImageFormatList {
			get {
				return new ImageFormatDescriptor[] {
					new ImageFormatDescriptor(ImageFormat.Jpeg, "Jpeg (default settings)", "image/jpeg"),
					new ImageFormatDescriptor(
						ImageFormatDescriptor.GetEncoderInfo("image/jpeg"),
						ImageFormatDescriptor.GetEncoderParams(90),
						"Jpeg (90%)", "image/jpeg"),
					new ImageFormatDescriptor(
						ImageFormatDescriptor.GetEncoderInfo("image/jpeg"),
						ImageFormatDescriptor.GetEncoderParams(80),
						"Jpeg (80%)", "image/jpeg"),
					new ImageFormatDescriptor(
						ImageFormatDescriptor.GetEncoderInfo("image/jpeg"),
						ImageFormatDescriptor.GetEncoderParams(70),
						"Jpeg (70%)", "image/jpeg"),
					new ImageFormatDescriptor(
						ImageFormatDescriptor.GetEncoderInfo("image/jpeg"),
						ImageFormatDescriptor.GetEncoderParams(60),
						"Jpeg (60%)", "image/jpeg"),
					new ImageFormatDescriptor(
						ImageFormatDescriptor.GetEncoderInfo("image/jpeg"),
						ImageFormatDescriptor.GetEncoderParams(50),
						"Jpeg (50%)", "image/jpeg"),
					new ImageFormatDescriptor(ImageFormat.Png, "Png", "image/png")
				};
			}
		}

		public class ModeDescriptor {
			int _MSecs;
			string _Caption;

			public int MSecs {
				get { return _MSecs; }
			}

			public string Caption {
				get { return _Caption; }
			}

			public ModeDescriptor(int msecs, string caption) {
				_MSecs = msecs;
				_Caption = caption;
			}

		}


	}
}
