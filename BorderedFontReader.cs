using System.Runtime.Serialization;
#if WINDOWS
using System.Drawing;
using System.Drawing.Imaging;
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
			bool hasPng = input.ReadBoolean();
#if IOS
			bool useRetina = containsRetina && MonoTouch.UIKit.UIScreen.MainScreen.Scale == 2; //Device is retina
#else
			bool useRetina = false;
#endif
			Texture2D borderedTexture = null, innerTexture = null;
			int width, height, len;
			byte[] data;

			//Do the reverse of BorderedFontWriter.Write
			string borderedDefStr = input.ReadString();
			string innerDefStr = input.ReadString();
			string kerningInfo = input.ReadString();

			borderedTexture = ReadTexture(gd, input, hasPng, !useRetina);

			innerTexture = ReadTexture(gd, input, hasPng, !useRetina);

			if (useRetina)
			{
				//Do the reverse of BorderedFontWriter.Write
				borderedDefStr = input.ReadString();
				innerDefStr = input.ReadString();
				kerningInfo = input.ReadString();

				borderedTexture = ReadTexture(gd, input, hasPng, true);
				innerTexture = ReadTexture(gd, input, hasPng, true);
			}

			return new BorderedFont(borderedTexture, innerTexture, borderedDefStr, innerDefStr, kerningInfo, useRetina);
		}

		private Texture2D ReadTexture(GraphicsDevice gd, ContentReader input, bool hasPng, bool actuallyMakeTexture)
		{
			if (hasPng)
			{
				var len = input.ReadInt32();
				var data = input.ReadBytes(len);
				if (!actuallyMakeTexture)
					return null;
				return Texture2DFromPngBytes(gd, data);
			}
			else
			{
				var width = input.ReadInt32();
				var height = input.ReadInt32();
				var len = input.ReadInt32();
				var data = input.ReadBytes(len);

				if (!actuallyMakeTexture)
					return null;
				return GenerateTexture(gd, width, height, data);
			}
		}

		private Texture2D GenerateTexture(GraphicsDevice gd, int width, int height, byte[] data)
		{
			var tex = new Texture2D(gd, width, height, false, SurfaceFormat.Color);
			tex.SetData(data);
			return tex;
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
