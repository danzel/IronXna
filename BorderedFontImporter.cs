using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace IronXna
{
	[ContentImporter(".spritefont", DefaultProcessor = "BorderedFontProcessor", DisplayName="Bordered Font Importer", CacheImportedData = true)]
	class BorderedFontImporter : ContentImporter<XmlBorderedFontDefinition>
	{
		public override XmlBorderedFontDefinition Import(string filename, ContentImporterContext context)
		{
			return new XmlBorderedFontDefinition(filename);
		}
	}
}
