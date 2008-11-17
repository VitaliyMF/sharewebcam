using System;
using System.Collections;
using System.Text;
using System.Diagnostics;

using System.Windows.Forms;

namespace ShareWebCam {

	public class TextBoxTraceListener : TraceListener {
		RichTextBox StatusTextBox;

		public TextBoxTraceListener(string name, RichTextBox textBox)
			: base(name) {
			StatusTextBox = textBox;
		}

		public override void Write(string message) {
			lock (StatusTextBox) {
				string fmtMessage = String.Format("{0}: {1}", DateTime.Now, message);
				if (StatusTextBox.Lines.Length > 100) {
					string[] lines = new string[99];
					Array.Copy(StatusTextBox.Lines, 0, lines, 0, 99);
					StatusTextBox.Text = fmtMessage+String.Join(Environment.NewLine,lines);
				} else
					StatusTextBox.Text = fmtMessage+StatusTextBox.Text;
			}			
			//base.Write(String.Format("{0}: {1}", DateTime.Now, message));
		}



		public override void WriteLine(string message) {
			Write(message+Environment.NewLine);
		}

		public override void WriteLine(string message, string category) {
			Write(message + Environment.NewLine);
		}



	}


}
