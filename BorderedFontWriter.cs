using System.Drawing.Imaging;
using System.IO;
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
			output.Write(value.RetinaBorderedDefStr != null);

			output.Write(value.BorderedDefStr);
			output.Write(value.InnerDefStr);
			output.Write(value.KerningInfo);

			MemoryStream temp = new MemoryStream();
			value.BorderedTexture.Save(temp, ImageFormat.Png);
			output.Write((int)temp.Length);
			output.Write(temp.ToArray());

			temp = new MemoryStream();
			value.InnerTexture.Save(temp, ImageFormat.Png);
			output.Write((int)temp.Length);
			output.Write(temp.ToArray());

			if (value.RetinaBorderedDefStr != null)
			{
				output.Write(value.RetinaBorderedDefStr);
				output.Write(value.RetinaInnerDefStr);
				output.Write(value.RetinaKerningInfo);

				temp = new MemoryStream();
				value.RetinaBorderedTexture.Save(temp, ImageFormat.Png);
				output.Write((int)temp.Length);
				output.Write(temp.ToArray());

				temp = new MemoryStream();
				value.RetinaInnerTexture.Save(temp, ImageFormat.Png);
				output.Write((int)temp.Length);
				output.Write(temp.ToArray());
			}
		}
	}
}
