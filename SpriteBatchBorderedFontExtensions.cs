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
			DrawString(spriteBatch, font, text, position, borderColor, innerColor, 0, Vector2.Zero, 1);
		}

		public static void DrawString(this SpriteBatch spriteBatch, BorderedFont font, string text, Vector2 position, Color borderColor, Color innerColor, float rotation, Vector2 origin, float scale)
		{
			//These changes seem to make the text end up in the same(ish) place as DrawString for SpriteFont
			origin.Y -= font.Inner.AboveLineSize * (font.Inner.IsRetina ? 0.5f : 1);
			origin.X += font.Inner.SpaceWidth * (font.Inner.IsRetina ? 0.25f : 0.5f);

			if (borderColor.A != 0)
				font.Border.DrawString(spriteBatch, text, position, borderColor, rotation, origin, scale);
			if (innerColor.A != 0)
				font.Inner.DrawString(spriteBatch, text, position, innerColor, rotation, origin, scale);
		}

		/// <summary>
		/// Adds a string to a batch of sprites for rendering using the specified font, text, position, and color.
		/// </summary>
		public static void DrawString(this SpriteBatch spriteBatch, BorderedFont font, string text, Vector2 position,  Color innerColor)
		{
			DrawString(spriteBatch, font, text, position, innerColor, 0, Vector2.Zero, 1);
		}

		public static void DrawString(this SpriteBatch spriteBatch, BorderedFont font, string text, Vector2 position, Color innerColor, float rotation, Vector2 origin, float scale)
		{
			//These changes seem to make the text end up in the same(ish) place as DrawString for SpriteFont
			origin.Y -= font.Inner.AboveLineSize * (font.Inner.IsRetina ? 0.5f : 1);
			origin.X += font.Inner.SpaceWidth * (font.Inner.IsRetina ? 0.25f : 0.5f);

			font.Inner.DrawString(spriteBatch, text, position, innerColor, rotation, origin, scale);
		}
	}
}
