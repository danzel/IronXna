using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public static class SpriteBatchSubTexture2DExtensions
	{
		public static void Draw(this SpriteBatch spriteBatch, SubTexture2D texture, Vector2 position, Vector2? origin = null, float rotation = 0, Vector2? scale = null, Color? color = null)
		{
			spriteBatch.Draw(texture.Texture, position, texture.Rectangle, color ?? Color.White, rotation, origin ?? Vector2.Zero, scale ?? Vector2.One, SpriteEffects.None, 0);
		}
	}
}
