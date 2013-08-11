using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace IronXna
{
	[ContentTypeWriter]
	class BorderedFontWriter : ContentTypeWriter<BorderedFontContent>
	{
		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return typeof(BorderedFontReader).AssemblyQualifiedName;
		}

		protected override void Write(ContentWriter output, BorderedFontContent value)
		{
			bool usePng = output.TargetPlatform != (TargetPlatform)11; //MonoGame Windows Phone 8

			output.Write(value.RetinaBorderedDefStr != null);
			output.Write(usePng);

			output.Write(value.BorderedDefStr);
			output.Write(value.InnerDefStr);
			output.Write(value.KerningInfo);

			WriteBitmap(output, value.BorderedTexture, usePng);

			WriteBitmap(output, value.InnerTexture, usePng);

			if (value.RetinaBorderedDefStr != null)
			{
				output.Write(value.RetinaBorderedDefStr);
				output.Write(value.RetinaInnerDefStr);
				output.Write(value.RetinaKerningInfo);

				WriteBitmap(output, value.RetinaBorderedTexture, usePng);

				WriteBitmap(output, value.RetinaInnerTexture, usePng);
			}
		}

		private void WriteBitmap(ContentWriter output, Bitmap bitmap, bool usePng)
		{
			if (!usePng)
			{
				//Save raw bytes as WinPhone8 can't load pngs
				BitmapData bmd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
				int bufferSize = bmd.Height * bmd.Stride;

				//create data buffer 
				byte[] bytes = new byte[bufferSize];

				// copy bitmap data into buffer
				Marshal.Copy(bmd.Scan0, bytes, 0, bytes.Length);

				if (bmd.Stride != bitmap.Width * 4)
					throw new Exception("Image width is weird");

				output.Write(bitmap.Width);
				output.Write(bitmap.Height);
				output.Write(bytes.Length);
				output.Write(bytes);
			}
			else
			{
				//store pngs
				MemoryStream temp = new MemoryStream();
				bitmap.Save(temp, ImageFormat.Png);
				output.Write((int)temp.Length);
				output.Write(temp.ToArray());
			}
		}
	}
}
