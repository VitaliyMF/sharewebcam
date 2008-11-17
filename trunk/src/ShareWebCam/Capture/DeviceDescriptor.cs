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
