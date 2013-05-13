using System;
using System.Xml;

namespace IronXna
{
	/// <summary>
	/// Object representation of the .spritefont xml file
	/// </summary>
	class XmlBorderedFontDefinition
	{
		public readonly string Filename;

		public readonly string FontName;
		
		public readonly int Size;
		public readonly int BorderThickness;

		public readonly bool IncludeRetina;

		public readonly bool UseKerning;

		public XmlBorderedFontDefinition(string filename)
		{
			Filename = filename;

			XmlDocument document = new XmlDocument();
			document.Load(filename);

			var fontNameNode = document.SelectSingleNode("/XnaContent/Asset/FontName");
			var sizeNode = document.SelectSingleNode("/XnaContent/Asset/Size");
			var borderThicknessNode = document.SelectSingleNode("/XnaContent/Asset/BorderThickness");
			var useKerningNode = document.SelectSingleNode("/XnaContent/Asset/UseKerning");

			if (fontNameNode == null)
				throw new Exception("No FontName Node found");
			if (sizeNode == null)
				throw new Exception("No Size Node found");
			if (borderThicknessNode == null)
				throw new Exception("No BorderThickness Node found");
			if (useKerningNode == null)
				throw new Exception("No UseKerning Node found");


			FontName = fontNameNode.InnerText;
			Size = int.Parse(sizeNode.InnerText);
			BorderThickness = int.Parse(borderThicknessNode.InnerText);
			UseKerning = bool.Parse(useKerningNode.InnerText);

			var includeRetinaNode = document.SelectSingleNode("/XnaContent/Asset/IncludeRetina");
			if (includeRetinaNode != null)
				IncludeRetina = bool.Parse(includeRetinaNode.InnerText);
		}
	}
}
