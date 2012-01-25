using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public static class SpriteBatchBorderedFontExtensions
	{
		/// <summary>
		/// Adds a string to a batch of sprites for rendering using the specified font, text, position, and color.
		/// </summary>
		public static void DrawString(this SpriteBatch spriteBatch, BorderedFont font, string text, Vector2 position, Color borderColor, Color innerColor)
		{
			position.Y += font.Border.CapitalHHeight;
			position.X -= font.Inner.SpaceWidth / 2;
			font.Border.DrawString(spriteBatch, text, position, borderColor);
			font.Inner.DrawString(spriteBatch, text, position, innerColor);
		}
	}
}
