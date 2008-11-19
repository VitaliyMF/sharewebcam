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
