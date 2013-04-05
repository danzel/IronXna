#if WINDOWS
using System.Drawing;
using System.Drawing.Imaging;
#elif MAC
//TODO?
#endif
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public class BorderedFontReader : ContentTypeReader<BorderedFont>
	{
		protected override BorderedFont Read(ContentReader input, BorderedFont existingInstance)
		{
			IGraphicsDeviceService graphicsDeviceService =
				(IGraphicsDeviceService)input.ContentManager.ServiceProvider.
				GetService(typeof(IGraphicsDeviceService));
			var gd = graphicsDeviceService.GraphicsDevice;

			bool containsRetina = input.ReadBoolean();
#if IOS
			bool useRetina = containsRetina && MonoTouch.UIKit.UIScreen.MainScreen.Scale == 2; //Device is retina
#else
			bool useRetina = false;
#endif

			//Do the reverse of BorderedFontWriter.Write
			string borderedDefStr = input.ReadString();
			string innerDefStr = input.ReadString();
			string kerningInfo = input.ReadString();

			int len = input.ReadInt32();
			byte[] borderedTexturePngBytes = input.ReadBytes(len);
			//File.WriteAllBytes("border.png", borderedTexturePngBytes);
			Texture2D borderedTexture = useRetina ? null : Texture2DFromPngBytes(gd, borderedTexturePngBytes);

			len = input.ReadInt32();
			byte[] innerTexturePngBytes = input.ReadBytes(len);
			//File.WriteAllBytes("inner.png", innerTexturePngBytes);
			Texture2D innerTexture = useRetina ? null : Texture2DFromPngBytes(gd, innerTexturePngBytes);

			if (useRetina)
			{
				//Do the reverse of BorderedFontWriter.Write
				borderedDefStr = input.ReadString();
				innerDefStr = input.ReadString();
				kerningInfo = input.ReadString();

				len = input.ReadInt32();
				borderedTexturePngBytes = input.ReadBytes(len);
				//File.WriteAllBytes("border.png", borderedTexturePngBytes);
				borderedTexture = Texture2DFromPngBytes(gd, borderedTexturePngBytes);

				len = input.ReadInt32();
				innerTexturePngBytes = input.ReadBytes(len);
				//File.WriteAllBytes("inner.png", innerTexturePngBytes);
				innerTexture = Texture2DFromPngBytes(gd, innerTexturePngBytes);
			}

			return new BorderedFont(borderedTexture, innerTexture, borderedDefStr, innerDefStr, kerningInfo, useRetina);
		}

		private Texture2D Texture2DFromPngBytes(GraphicsDevice gd, byte[] pngBytes)
		{
#if WINDOWS
			Bitmap bmp = new Bitmap(new MemoryStream(pngBytes));

			BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			int bufferSize = bmd.Height * bmd.Stride;

			//create data buffer 
			byte[] bytes = new byte[bufferSize];
	
			// copy bitmap data into buffer
			Marshal.Copy(bmd.Scan0, bytes, 0, bytes.Length);

			//Apply premultiplied alpha!
			for (int i = 0; i < bufferSize; i+= 4)
			{
				bytes[i] = bytes[i + 1] = bytes[i + 2] = bytes[i + 3];
			}

			// copy our buffer to the texture
			Texture2D t2D = new Texture2D(gd, bmp.Width, bmp.Height, false, SurfaceFormat.Color);
			t2D.SetData(bytes);
			// unlock the bitmap data
			bmp.UnlockBits(bmd);
			return t2D;
#else
			return Texture2D.FromStream(gd, new MemoryStream(pngBytes));
#endif
		}
	}
}
