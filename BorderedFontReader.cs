using System.Drawing;
using System.Drawing.Imaging;
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

			//Do the reverse of BorderedFontWriter.Write
			string borderedDefStr = input.ReadString();
			string innerDefStr = input.ReadString();
			string kerningInfo = input.ReadString();

			int len = input.ReadInt32();
			byte[] borderedTexturePngBytes = input.ReadBytes(len);
			File.WriteAllBytes("border.png", borderedTexturePngBytes);
			var borderedTexture = Texture2DFromPngBytes(gd, borderedTexturePngBytes);

			len = input.ReadInt32();
			byte[] innerTexturePngBytes = input.ReadBytes(len);
			File.WriteAllBytes("inner.png", innerTexturePngBytes);
			var innerTexture = Texture2DFromPngBytes(gd, innerTexturePngBytes);

			return new BorderedFont(borderedTexture, innerTexture, borderedDefStr, innerDefStr, kerningInfo);
		}

		private Texture2D Texture2DFromPngBytes(GraphicsDevice gd, byte[] pngBytes)
		{
			Bitmap bmp = new Bitmap(new MemoryStream(pngBytes));

			BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			int bufferSize = bmd.Height * bmd.Stride;

			//create data buffer 
			byte[] bytes = new byte[bufferSize];
	
			// copy bitmap data into buffer
			Marshal.Copy(bmd.Scan0, bytes, 0, bytes.Length);

			// copy our buffer to the texture
			Texture2D t2D = new Texture2D(gd, bmp.Width, bmp.Height, false, SurfaceFormat.Color);
			t2D.SetData(bytes);
			// unlock the bitmap data
			bmp.UnlockBits(bmd);
			return t2D;
		}
	}
}
