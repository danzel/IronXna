using Microsoft.Xna.Framework.Content.Pipeline;

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
