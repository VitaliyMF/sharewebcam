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
using System.IO;
using System.Drawing;
using System.Net;
using NI.Common.Operations;
using NI.Common;

namespace ShareWebCam.Web {
	
	public class ImportWebCamSnapshotOperation : IOperation {

		IOperation _UploadOperation;
		string _WebCamUid;
		string _WebCamTitle;
		string _WebCamDescription;
		string _WebCamUrl;
		Rectangle _SubFrame;
		Size _FinalSize;

		public Size FinalSize {
			get { return _FinalSize; }
			set { _FinalSize = value; }
		}

		public Rectangle SubFrame {
			get { return _SubFrame; }
			set { _SubFrame = value; }
		}

		public string WebCamUrl {
			get { return _WebCamUrl; }
			set { _WebCamUrl = value; }
		}

		public string WebCamUid {
			get { return _WebCamUid; }
			set { _WebCamUid = value; }
		}

		public string WebCamTitle {
			get { return _WebCamTitle; }
			set { _WebCamTitle = value; }
		}

		public string WebCamDescription {
			get { return _WebCamDescription; }
			set { _WebCamDescription = value; }
		}

		public IOperation UploadOperation {
			get { return _UploadOperation; }
			set { _UploadOperation = value; }
		}


		public void Execute(IDictionary context) {
			Console.WriteLine("Requesting "+WebCamUrl+"...");

			WebRequest webRequest = WebRequest.Create(WebCamUrl);
			//WebProxy proxy = new WebProxy();
			//proxy.Address = new Uri("http://82.193.104.251:3140");
			//webRequest.Proxy = proxy;
			
			WebResponse response = webRequest.GetResponse();
			Stream responseStream = response.GetResponseStream();
			
			MemoryStream bufStream = new MemoryStream();
			byte[] buf = new byte[1024*10];
			int bytesRead = 0;
			while ((bytesRead = responseStream.Read(buf, 0, buf.Length))>0) {
				bufStream.Write(buf, 0, bytesRead);
			}
			
			System.Drawing.Image img = System.Drawing.Image.FromStream(bufStream);

			System.Drawing.Image subFrameImg = new Bitmap(SubFrame.Width, SubFrame.Height );
			Graphics subFrameG = Graphics.FromImage(subFrameImg);
			subFrameG.DrawImage(img, 0, 0, SubFrame, GraphicsUnit.Pixel);
			subFrameG.Dispose();
			subFrameImg = subFrameImg.GetThumbnailImage(FinalSize.Width, FinalSize.Height, null, IntPtr.Zero);


			MemoryStream imgStream = new MemoryStream();
			subFrameImg.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			
			Hashtable opContext = new Hashtable();
			opContext["data"] = imgStream.GetBuffer();
			opContext["session_id"] = WebCamUid;
			opContext["uid"] = WebCamUid;
			opContext["title"] = WebCamTitle;
			opContext["description"] = WebCamDescription;
			UploadOperation.Execute(opContext);


			response.Close();
		}

	}
}
