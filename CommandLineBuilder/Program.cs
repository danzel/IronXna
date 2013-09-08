using System.IO;
using IronXna;

namespace CommandLineBuilder
{
	class Program
	{
		private static void Main(string[] args)
		{
			var def = new XmlBorderedFontDefinition(args[0]);
			var content = new BorderedFontContent(def);

			string fileName = Path.GetFileNameWithoutExtension(args[0]);

			if (args.Length == 2 || args[2] == "normalres")
			{
				content.InnerTexture.Save(Path.Combine(args[1], fileName + ".inner.png"));
				content.BorderedTexture.Save(Path.Combine(args[1], fileName + ".border.png"));
				File.WriteAllText(Path.Combine(args[1], fileName + ".inner.txt"), content.InnerDefStr);
				File.WriteAllText(Path.Combine(args[1], fileName + ".border.txt"), content.BorderedDefStr);
				File.WriteAllText(Path.Combine(args[1], fileName + ".kerning.txt"), content.KerningInfo);
			}
			if (args.Length == 2 || args[2] == "retina")
			{
				content.RetinaInnerTexture.Save(Path.Combine(args[1], fileName + ".inner@2x.png"));
				content.RetinaBorderedTexture.Save(Path.Combine(args[1], fileName + ".border@2x.png"));
				File.WriteAllText(Path.Combine(args[1], fileName + ".inner@2x.txt"), content.RetinaInnerDefStr);
				File.WriteAllText(Path.Combine(args[1], fileName + ".border@2x.txt"), content.RetinaBorderedDefStr);
				File.WriteAllText(Path.Combine(args[1], fileName + ".kerning@2x.txt"), content.RetinaKerningInfo);
			}
		}
	}
}
