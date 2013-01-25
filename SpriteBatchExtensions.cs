using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public static class SpriteBatchExtensions
	{
		public static void DrawCentered(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position)
		{
			spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), 1, SpriteEffects.None, 0);
		}
	}
}
