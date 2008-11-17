using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace ShareWebCam.IO {

	/// <summary>
	/// FFMpeg-based transcoder stream
	/// </summary>
	public class TranscodeOutputStream : Stream {
		Process ffmpegProcess;
		AsyncReader outputReader;
		Thread outputReaderThread;
		Stream outputStream;
		int timeout = 2000;

		string ffmpegFilename = @"ffmpeg.exe";

		public TranscodeOutputStream(string ffmpegArgs, Stream transcodedDataOutputStream) {
			outputStream = transcodedDataOutputStream;

			ProcessStartInfo ffmpegInfo = new ProcessStartInfo(ffmpegFilename);
			ffmpegInfo.Arguments = ffmpegArgs;
			ffmpegInfo.CreateNoWindow = false;
			ffmpegInfo.UseShellExecute = false;
			ffmpegInfo.RedirectStandardInput = true;
			ffmpegInfo.RedirectStandardOutput = true;

			ffmpegProcess = Process.Start(ffmpegInfo);

			outputReader = new AsyncReader(ffmpegProcess.StandardOutput.BaseStream, outputStream);
			outputReaderThread = new Thread(new ThreadStart(outputReader.ReadOutput));
			outputReaderThread.Start();

		}

		public override bool CanRead {
			get { return false; }
		}

		public override bool CanSeek {
			get { return false; }
		}

		public override bool CanWrite {
			get { return true; }
		}

		public override void Flush() {
			ffmpegProcess.StandardInput.BaseStream.Flush();
		}

		public override long Length {
			get { return 0; }
		}

		public override long Position {
			get { return 0; }
			set {
			}
		}

		public override int Read(byte[] buffer, int offset, int count) {
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin) {
			throw new NotSupportedException();
		}

		public override void SetLength(long value) {
		}

		public override void Write(byte[] buffer, int offset, int count) {
			ffmpegProcess.StandardInput.BaseStream.Write(buffer, offset, count);
		}

		public override void Close() {
			base.Close();

			// finishing ffmpeg
			ffmpegProcess.StandardInput.BaseStream.Flush();
			ffmpegProcess.StandardInput.Close();
			ffmpegProcess.StandardOutput.BaseStream.Flush();

			ffmpegProcess.WaitForExit(timeout);
			if (!ffmpegProcess.HasExited)
				ffmpegProcess.Kill();

			// stop reading
			outputReader.Stop = true;
			outputReaderThread.Join(timeout);
			if (outputReaderThread.ThreadState == System.Threading.ThreadState.Running)
				outputReaderThread.Abort();
			// final read
			outputReader.Stop = false;
			outputReader.ReadOutput(false);

			Console.WriteLine("Output stream writes counter: {0}", outputReader.Counter);

			outputStream.Close();
		}


		public class AsyncReader {
			Stream inStream;
			Stream outStream;
			public int Counter = 0;

			public bool Stop = false;

			public AsyncReader(Stream inS, Stream outS) {
				inStream = inS;
				outStream = outS;
			}

			public void ReadOutput() {
				ReadOutput(true);
			}

			public void ReadOutput(bool cycle) {
				byte[] buf = new byte[1024 * 10];
				int byteRead = 0;
				while (!Stop || byteRead > 0) {
					byteRead = inStream.Read(buf, 0, buf.Length);
					if (byteRead > 0) {
						Counter++;
						outStream.Write(buf, 0, byteRead);
						outStream.Flush();
					} else
						if (cycle)
							System.Threading.Thread.Sleep(30);
						else
							return;
				}
			}
		}


	}
}
