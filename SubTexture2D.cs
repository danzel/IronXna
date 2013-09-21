using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public class SubTexture2D
	{
		public readonly Texture2D Texture;
		public readonly Rectangle Rectangle;
		
		public readonly bool IsDoubleResolution;
		public readonly float ResolutionScale = 1;
		public readonly float ResolutionScaleInv = 1;

		public SubTexture2D(Texture2D texture, Rectangle rect, bool isDoubleResolution)
		{
			Texture = texture;
			Rectangle = rect;
			IsDoubleResolution = isDoubleResolution;

			Width = rect.Width;
			Height = rect.Height;

			if (isDoubleResolution)
			{
				Width = Math.Max(1, Width / 2);
				Height = Math.Max(1, Height / 2);
				
				ResolutionScale = 2;
				ResolutionScaleInv = 0.5f;
			}
		}

		public SubTexture2D(Texture2D texture, bool isDoubleResolution = false)
			: this(texture, new Rectangle(0, 0, texture.Width, texture.Height), isDoubleResolution)
		{
		}

		public readonly int Width;
		public readonly int Height;

		/// <summary>
		/// Create a sub-sub texture from the given bounds
		/// </summary>
		public SubTexture2D Clip(int x, int y, int width, int height)
		{
			if (IsDoubleResolution)//todo: test this
			{
				x *= 2;
				y *= y;
				width *= 2;
				height *= 2;
			}

			return new SubTexture2D(Texture, new Rectangle(Rectangle.X + x, Rectangle.Y + y, width, height), IsDoubleResolution);
		}
	}
}
