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
using System.Net;
using System.IO;
using System.Configuration;

using NI.Common.Text;

namespace ShareWebCam.Publishing {
	
	public class PublishingService {

		string _ProxyAddress;
		string _UploadAddress;
		string _WebCamUid = null;
		string _WebCamDescription = null;
		string _WebCamTitle = null;
		string _SessionUid = null;

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

		public string UploadAddress {
			get { return _UploadAddress; }
			set { _UploadAddress = value; }
		}

		public string ProxyAddress {
			get { return _ProxyAddress; }
			set { _ProxyAddress = value; }
		}

		protected string SessionUid {
			get { return _SessionUid; }
			set { _SessionUid = value; }
		}
		
		public PublishingService() {
			SessionUid = Guid.NewGuid().ToString();
			UploadAddress = Convert.ToString( ConfigurationSettings.AppSettings["UploadAddress"] );
			WebCamTitle = Convert.ToString( ConfigurationSettings.AppSettings["DefaultWebCamTitle"] );
			WebCamDescription = Convert.ToString( ConfigurationSettings.AppSettings["DefaultWebCamDescription"] );				
			ProxyAddress = Convert.ToString( ConfigurationSettings.AppSettings["UploadProxyAddress"] );
		}

		public void StartWebCamPage() {
			string webCamPageAddress = Convert.ToString(ConfigurationSettings.AppSettings["WebcamPageAddress"]);
			System.Diagnostics.Process.Start(
				String.Format(webCamPageAddress, SessionUid) );
		}

		public void Upload(byte[] imageBytes) {
			// compose full upload address
			string uploadAddress = UploadAddress;
			uploadAddress = StringHelper.SetUrlParam(uploadAddress, "action", "upload");
			uploadAddress = StringHelper.SetUrlParam(uploadAddress, "session_id", SessionUid);
			uploadAddress = StringHelper.SetUrlParam(uploadAddress, "uid", Uri.EscapeDataString(WebCamUid) );
			uploadAddress = StringHelper.SetUrlParam(uploadAddress, "title", Uri.EscapeDataString(WebCamTitle) );
			uploadAddress = StringHelper.SetUrlParam(uploadAddress, "description", Uri.EscapeDataString(WebCamDescription));

			WebRequest webRequest = WebRequest.Create(uploadAddress);
			
			if (ProxyAddress!=null && ProxyAddress.Trim().Length>0) {
				WebProxy proxy = new WebProxy();
				proxy.Address = new Uri(ProxyAddress);
				webRequest.Proxy = proxy;
			}
			
			webRequest.ContentLength = imageBytes.Length;
			webRequest.Method = "POST";
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(imageBytes,0,imageBytes.Length);
			requestStream.Close();

			WebResponse webResponse = webRequest.GetResponse();
		}

	}
}