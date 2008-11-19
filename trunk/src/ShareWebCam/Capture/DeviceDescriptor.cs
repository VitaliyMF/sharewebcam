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

namespace ShareWebCam.Capture {

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
