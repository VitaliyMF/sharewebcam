using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Diagnostics;

using NI.Common.Operations;

namespace ShareWebCam.Web {
	public class PingOperation : IOperation {
		IOperation[] _Operations;
		int _Period = 5000;

		public int Period {
			get { return _Period; }
			set { _Period = value; }
		}

		public IOperation[] Operations {
			get { return _Operations; }
			set { _Operations = value; }
		}

		public void Execute(IDictionary context) {
			while (true) {
				for (int i=0; i<Operations.Length; i++)
					try {
						Operations[i].Execute(context);
					} catch (Exception ex) {
						Console.WriteLine("Ping operation failed: "+ex.Message);
					}
				Thread.Sleep(Period);
			}
		}

	}
}
