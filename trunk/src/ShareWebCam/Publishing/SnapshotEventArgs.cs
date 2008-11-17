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
