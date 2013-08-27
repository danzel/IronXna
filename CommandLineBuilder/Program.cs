using System.IO;
using IronXna;

namespace CommandLineBuilder
{
	class Program
	{
		static void Main(string[] args)
		{
			var def = new XmlBorderedFontDefinition(args[0]);
			var content = new BorderedFontContent(def);

			string fileName = Path.GetFileNameWithoutExtension(args[0]);

			content.InnerTexture.Save(Path.Combine(args[1], fileName + ".inner.png"));
			content.BorderedTexture.Save(Path.Combine(args[1], fileName + ".border.png"));
			File.WriteAllText(Path.Combine(args[1], fileName + ".inner.txt"), content.InnerDefStr);
			File.WriteAllText(Path.Combine(args[1], fileName + ".border.txt"), content.BorderedDefStr);
			File.WriteAllText(Path.Combine(args[1], fileName + ".kerning.txt"), content.KerningInfo);
		}
	}
}
