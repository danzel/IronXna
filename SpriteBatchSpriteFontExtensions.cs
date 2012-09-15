using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public static class SpriteBatchSpriteFontExtensions
	{
		public static void DrawString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color, TextAlignment alignment)
		{
			if (alignment == TextAlignment.Left)
			{
				spriteBatch.DrawString(spriteFont, text, position, color);
			}
			else
			{
				var size = spriteFont.MeasureString(text);

				if (alignment == TextAlignment.Right)
					spriteBatch.DrawString(spriteFont, text, new Vector2(position.X - size.X, position.Y), color);
				else if (alignment == TextAlignment.Center)
					spriteBatch.DrawString(spriteFont, text, new Vector2(position.X - size.X / 2, position.Y), color);
				else
					throw new Exception("Unknown TextAlignment");
			}
			//ViewController.SpriteBatch.DrawString(_spriteFont, _text, new Vector2(DerivedPosition.X + TouchArea.Size.X / 2, DerivedPosition.Y + TouchArea.Size.Y / 2), Color.White, Alignment.Center);
		}
	}
}
