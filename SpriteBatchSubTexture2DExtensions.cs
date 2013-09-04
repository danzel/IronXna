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
		public static void Draw(this SpriteBatch spriteBatch, SubTexture2D texture, Rectangle destinationRectangle, Color? color = null, float rotation = 0, Vector2? origin = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0)
		{
			spriteBatch.Draw(texture.Texture, destinationRectangle, texture.Rectangle, color ?? Color.White, rotation, origin ?? Vector2.Zero, spriteEffects, depth);
		}
		public static void Draw(this SpriteBatch spriteBatch, SubTexture2D texture, Vector2 position, Color? color = null, float rotation = 0, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0)
		{
			spriteBatch.Draw(texture.Texture, position, texture.Rectangle, color ?? Color.White, rotation, origin ?? Vector2.Zero, scale ?? Vector2.One, spriteEffects, depth);
		}
		public static void Draw(this SpriteBatch spriteBatch, SubTexture2D texture, Vector2 position, Color? color = null, float rotation = 0, Vector2? origin = null, float? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0)
		{
			spriteBatch.Draw(texture.Texture, position, texture.Rectangle, color ?? Color.White, rotation, origin ?? Vector2.Zero, scale ?? 1, spriteEffects, depth);
		}
	}
}
