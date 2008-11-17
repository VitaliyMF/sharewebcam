using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Drawing;

namespace ShareWebCam.Common {
	
	public class ImageTextWriter {

		protected float GetBrightness(Bitmap b, Rectangle rect) {
			float value = 0;
			for (int x=rect.Left; x<=rect.Right; x++)
				for (int y=rect.Top; y<=rect.Bottom; y++) {
					Color pixel = b.GetPixel(x,y);
					value += pixel.GetBrightness();
				}
			return value/(rect.Width*rect.Height);
		}

		int uploadCounter = 1;

		protected Color GetColorForBrightness(float brightness) {
			Color darkColor = uploadCounter%2==0 ? Color.Black : Color.DarkBlue;
			Color brightColor = uploadCounter%2==0 ? Color.White : Color.LightBlue;
			return brightness > 0.6 ? darkColor : brightColor;
		}

		public void WriteText(Bitmap b, int fontSize, bool writeRightText) {
			// write timestamp
			DateTime timeStamp = DateTime.Now;
			Graphics g = Graphics.FromImage(b);

			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
			FontFamily timeStampFontFamily = new FontFamily( "Tahoma"/*,pfc*/);
			Font timeStampFont = new Font(timeStampFontFamily, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
			
			string leftText = String.Format("{0:G}", timeStamp);
			SizeF leftTextSize = g.MeasureString(leftText, timeStampFont);

			float topLeftCornerBrightness = GetBrightness(b, new Rectangle(0,0, 
				(int)leftTextSize.Width+10, (int)leftTextSize.Height+10 ) );
			Brush timeStampBrush = new SolidBrush( GetColorForBrightness(topLeftCornerBrightness) );
			g.DrawString(leftText, timeStampFont, timeStampBrush, 5, 5);

			if (writeRightText) {
				string rightText = "zatory.kiev.ua";
				SizeF rightTextSize = g.MeasureString(rightText, timeStampFont);
				int rightTextX = b.Width-(int)rightTextSize.Width-5;
				float topRightCornerBrightness = GetBrightness(b,
						new Rectangle(rightTextX,0,(int)rightTextSize.Width, (int)rightTextSize.Height+10 ) );
				Brush rightTextBrush = new SolidBrush( GetColorForBrightness(topRightCornerBrightness) );
				g.DrawString( rightText, timeStampFont, rightTextBrush, rightTextX+5, 5);
			}
		}


	}
}
