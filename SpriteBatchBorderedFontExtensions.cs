using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	/// <summary>
	/// Extensions to provide SpriteBatch.DrawString functionality for the BorderedFont class
	/// </summary>
	public static class SpriteBatchBorderedFontExtensions
	{
		/// <summary>
		/// Adds a string to a batch of sprites for rendering using the specified font, text, position, and color.
		/// </summary>
		public static void DrawString(this SpriteBatch spriteBatch, BorderedFont font, string text, Vector2 position, Color borderColor, Color innerColor)
		{
			//These changes seem to make the text end up in the same(ish) place as DrawString for SpriteFont
			position.Y += font.Border.CapitalHHeight;
			position.X -= font.Inner.SpaceWidth / 2;

			font.Border.DrawString(spriteBatch, text, position, borderColor);
			font.Inner.DrawString(spriteBatch, text, position, innerColor);
		}
	}
}
