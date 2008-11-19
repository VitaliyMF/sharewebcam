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
	
	public class SnapshotEventArgs : EventArgs {
		Image _Snapshot;
		DateTime _Timestamp;
		string _CaptionFormat = "{0}x{1} {2}";

		public Image Snapshot {
			get { return _Snapshot; }
		}

		public DateTime Timestamp {
			get { return _Timestamp; }
		}

		public string Caption {
			get { return String.Format(_CaptionFormat, Snapshot.Width, Snapshot.Height, Timestamp); }
		}

		public SnapshotEventArgs(Image snapshot, DateTime timestamp, string captionFmt) :
				this(snapshot, timestamp) {
			_CaptionFormat = captionFmt;
		}


		public SnapshotEventArgs(Image snapshot, DateTime timestamp) {
			_Snapshot = snapshot;
			_Timestamp = timestamp;
		}

	}

	public delegate void SnapshotEventHandler(object sender, SnapshotEventArgs args);
	



}
