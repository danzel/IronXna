using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	[ContentProcessor(DisplayName = "Bordered Text Processor")]
	class BorderedFontProcessor : ContentProcessor<XmlBorderedFontDefinition, BorderedFontContent>
	{
		public override BorderedFontContent Process(XmlBorderedFontDefinition input, ContentProcessorContext context)
		{
			return new BorderedFontContent(input);
		}
	}
}
