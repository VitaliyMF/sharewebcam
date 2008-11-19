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
using System.Windows.Forms;

namespace ShareWebCam {
	public class ResizableRectangle : System.Windows.Forms.Control {

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
				cp.ExStyle |= 0x00000008; // TOPMOST
				return cp;
			}
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			base.OnPaint(e);
			e.Graphics.DrawRectangle( new Pen(new SolidBrush(Color.White)), new Rectangle(0,0,Width-1,Height-1));
		}

		protected override void OnPaintBackground(PaintEventArgs pevent) {
			//do not allow the background to be painted
			//base.OnPaintBackground(pevent);
			//InvalidateEx();
		}

		protected override void OnMove(EventArgs e)
		{
			if (Parent != null)
				Parent.Invalidate(Bounds, true);
			base.OnMove(e);
		}

		Timer Wriggler=new  Timer();

		public ResizableRectangle() {
			Wriggler.Interval = 200;
			Wriggler.Tick += new EventHandler(Wriggler_Tick);
		}

		void Wriggler_Tick(object sender, EventArgs e) {
			InvalidateEx();
		}

		public bool EnableRedraw {
			get { return Wriggler.Enabled; }
			set { 
				if (value)
					Wriggler.Start();
				else 
					Wriggler.Stop();
			}
		}

		public void InvalidateEx() {
			if (Parent == null)
				return;
			Rectangle rc = new Rectangle(this.Location, this.Size);
			Parent.Invalidate(rc, true);
		}

	}
}
