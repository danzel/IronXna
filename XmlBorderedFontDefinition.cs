using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace IronXna
{
	/// <summary>
	/// Object representation of the .spritefont xml file
	/// </summary>
	public class XmlBorderedFontDefinition
	{
		public readonly string Filename;

		public readonly string FontName;
		
		public readonly int Size;
		public readonly int BorderThickness;

		public readonly bool IncludeRetina;

		public readonly float SpaceMultiplier;

		public readonly bool UseKerning;

		public List<char> CharactersToInclude = new List<char>();

		public XmlBorderedFontDefinition(string filename)
		{
			Filename = filename;

			XmlDocument document = new XmlDocument();
			document.Load(filename);

			var fontNameNode = document.SelectSingleNode("/XnaContent/Asset/FontName");
			var sizeNode = document.SelectSingleNode("/XnaContent/Asset/Size");
			var borderThicknessNode = document.SelectSingleNode("/XnaContent/Asset/BorderThickness");
			var useKerningNode = document.SelectSingleNode("/XnaContent/Asset/UseKerning");
			var characterRegionsNodes = document.SelectNodes("/XnaContent/Asset/CharacterRegions/CharacterRegion");

			if (fontNameNode == null)
				throw new Exception("No FontName Node found");
			if (sizeNode == null)
				throw new Exception("No Size Node found");
			if (borderThicknessNode == null)
				throw new Exception("No BorderThickness Node found");
			if (useKerningNode == null)
				throw new Exception("No UseKerning Node found");
			if (characterRegionsNodes == null || characterRegionsNodes.Count == 0)
				throw new Exception("No CharacterRegion/CharacterRegions Nodes found");

			FontName = fontNameNode.InnerText;
			Size = int.Parse(sizeNode.InnerText);
			BorderThickness = int.Parse(borderThicknessNode.InnerText);
			UseKerning = bool.Parse(useKerningNode.InnerText);

			var spaceMultiplier = document.SelectSingleNode("/XnaContent/Asset/SpaceMultiplier");
			if (spaceMultiplier == null)
				SpaceMultiplier = 1;
			else
				SpaceMultiplier = float.Parse(spaceMultiplier.InnerText);

			var includeRetinaNode = document.SelectSingleNode("/XnaContent/Asset/IncludeRetina");
			if (includeRetinaNode != null)
				IncludeRetina = bool.Parse(includeRetinaNode.InnerText);

			foreach (var node in characterRegionsNodes.Cast<XmlNode>())
			{
				var startNode = node.SelectSingleNode("Start");
				var endNode = node.SelectSingleNode("End");

				if (startNode == null)
					throw new Exception("No Start node in CharacterRegion");
				if (endNode == null)
					throw new Exception("No End node in CharacterRegion");

				CharactersToInclude.AddRange(Enumerable.Range(startNode.InnerText[0], endNode.InnerText[0] - startNode.InnerText[0] + 1).Select(x => (char)x));
			}

			CharactersToInclude.Remove(' ');
		}
	}
}
