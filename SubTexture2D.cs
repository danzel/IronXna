using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public class SubTexture2D
	{
		public readonly Texture2D Texture;
		public readonly Rectangle Rectangle;

		public SubTexture2D(Texture2D texture, Rectangle rect)
		{
			Texture = texture;
			Rectangle = rect;
		}

		public SubTexture2D(Texture2D texture)
		{
			Texture = texture;
			Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
		}

		public int Width { get { return Rectangle.Width; } }
		public int Height { get { return Rectangle.Height; } }
	}
}
