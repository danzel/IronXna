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
		public static void Draw(this SpriteBatch spriteBatch, SubTexture2D texture, Rectangle destinationRectangle, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects spriteEffects = SpriteEffects.None)
		{
			spriteBatch.Draw(texture.Texture, destinationRectangle, texture.Rectangle, color ?? Color.White, rotation, origin ?? Vector2.Zero, spriteEffects, 0);
		}
		public static void Draw(this SpriteBatch spriteBatch, SubTexture2D texture, Vector2 position, Color? color = null, float rotation = 0, Vector2? origin = null, Vector2? scale = null)
		{
			spriteBatch.Draw(texture.Texture, position, texture.Rectangle, color ?? Color.White, rotation, origin ?? Vector2.Zero, scale ?? Vector2.One, SpriteEffects.None, 0);
		}
		public static void Draw(this SpriteBatch spriteBatch, SubTexture2D texture, Vector2 position, Color? color = null, float rotation = 0, Vector2? origin = null, float? scale = null)
		{
			spriteBatch.Draw(texture.Texture, position, texture.Rectangle, color ?? Color.White, rotation, origin ?? Vector2.Zero, scale ?? 1, SpriteEffects.None, 0);
		}
	}
}
