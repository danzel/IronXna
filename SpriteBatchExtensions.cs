using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public static class SpriteBatchExtensions
	{
		public static void DrawCentered(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float scale = 1, Color? color = null, float depth = 0, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Draw(texture, position, null, color ?? Color.White, 0, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, effects, depth);
		}
	}
}
