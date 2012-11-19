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

			FontName = document.SelectSingleNode("/XnaContent/Asset/FontName").InnerText;
			Size = int.Parse(document.SelectSingleNode("/XnaContent/Asset/Size").InnerText);
			BorderThickness = int.Parse(document.SelectSingleNode("/XnaContent/Asset/BorderThickness").InnerText);
			UseKerning = bool.Parse(document.SelectSingleNode("/XnaContent/Asset/UseKerning").InnerText);

			var includeRetinaNode = document.SelectSingleNode("/XnaContent/Asset/IncludeRetina");
			if (includeRetinaNode != null)
				IncludeRetina = bool.Parse(includeRetinaNode.InnerText);
		}
	}
}
