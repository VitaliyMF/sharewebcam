using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Drawing;

using NI.Common.Operations;
using ShareWebCam.Common;

namespace ShareWebCam.Web {
	
	public class PrepareSnapshotOperation : IOperation {
		string _ImageDataKey = "data";
		string MediumImageDataKey = "data_medium";
		string SmallImageDataKey = "data_small";
		ImageTextWriter ImgTextWr = new ImageTextWriter();

		public string ImageDataKey {
			get { return _ImageDataKey; }
			set { _ImageDataKey = value; }
		}

		public void Execute(IDictionary context) {
			byte[] imgBytes = context[ImageDataKey] as byte[];
			if (imgBytes==null)
				throw new ArgumentException("image bytes");

			Image img = Image.FromStream( new MemoryStream(imgBytes) );
			ImgTextWr.WriteText( (Bitmap)img, 12, true);
			// recompress and save 
			SaveImg(img, context, ImageDataKey);

			// prepare medium version
			if (img.Size.Width>320) {
				Image mediumImg = img.GetThumbnailImage(320, 240, null, IntPtr.Zero);
				ImgTextWr.WriteText( (Bitmap)mediumImg, 12, true );
				SaveImg(mediumImg, context, MediumImageDataKey);
			} else {
				context[MediumImageDataKey] = context[ImageDataKey];
			}

			// prepare medium version
			if (img.Size.Width>160) {
				Image smallImg = img.GetThumbnailImage(160, 120, null, IntPtr.Zero);
				ImgTextWr.WriteText( (Bitmap)smallImg, 8, false );
				SaveImg(smallImg, context, SmallImageDataKey);
			} else {
				context[MediumImageDataKey] = context[ImageDataKey];
			}

		}

		protected void SaveImg(Image img, IDictionary context, string key) {
			MemoryStream memStream = new MemoryStream();
			img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg); // default settings
			context[key] = memStream.ToArray();
		}

	}
}
