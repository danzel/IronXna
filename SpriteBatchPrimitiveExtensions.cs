﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Color = Microsoft.Xna.Framework.Color;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace IronXna
{
	/// <summary>
	/// A class to draw 2D Primitives.
	/// If you want to use it, call SpriteBatchPrimitiveExtensions.Initialize in your LoadContent function
	/// </summary>
	public static class SpriteBatchPrimitiveExtensions
	{
		public static SubTexture2D WhitePixel;

		public static void Initialize(GraphicsDevice graphics, SubTexture2D whitePixel = null)
		{
			//1px x 1px white texture
			if (whitePixel != null)
			{
				WhitePixel = whitePixel;
			}
			else
			{
				Texture2D pixel;
#if WINDOWS_PHONE
				pixel = new Texture2D(graphics, 1, 1);
				pixel.SetData(new [] { 0xffffffff });
#else
				pixel = Texture2D.FromStream(graphics, new MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAAxJREFUCNdj+P//PwAF/gL+3MxZ5wAAAABJRU5ErkJggg==")));
#endif
				WhitePixel = new SubTexture2D(pixel);
			}
		}

		public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, float depth = 0)
		{
			spriteBatch.Draw(WhitePixel, rectangle, color, 0, Vector2.Zero, SpriteEffects.None, depth);
		}

		/// <summary>
		/// Renders a primitive line object.
		/// </summary>
		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 p, Vector2 p1, Color color, int width = 1, float depth = 0)
		{
			float distance = Vector2.Distance(p, p1);
			float angle = (float)Math.Atan2(p1.Y - p.Y, p1.X - p.X);

			spriteBatch.Draw(WhitePixel, p, color, angle, Vector2.Zero, new Vector2(distance, width), SpriteEffects.None, depth);
		}

		/// <summary>
		/// Renders a primitive line object.
		/// </summary>
		public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Vector2 p, Vector2 p1, Color color, int width = 1, float depth = 0)
		{
			float distance = Vector2.Distance(p, p1);
			float angle = (float)Math.Atan2(p1.Y - p.Y, p1.X - p.X);

			spriteBatch.Draw(texture, p, null, color, angle, Vector2.Zero, new Vector2(distance, width / (float)texture.Height), SpriteEffects.None, depth);
		}
		/*

		/// <summary>
		/// Creates a circle starting from 0, 0.
		/// </summary>
		/// <param name="radius">The radius (half the width) of the circle.</param>
		/// <param name="sides">The number of sides on the circle (the more the detailed).</param>
		public void CreateCircle(float radius, int sides)
		{
			_vectors.Clear();

			const float max = 2 * (float)Math.PI;
			float step = max / sides;

			for (float theta = 0; theta < max; theta += step)
			{
				_vectors.Add(new Vector2(radius * (float)Math.Cos(theta), radius * (float)Math.Sin(theta)));
			}

			// then add the first vector again so it's a complete loop
			_vectors.Add(new Vector2(radius * (float)Math.Cos(0), radius * (float)Math.Sin(0)));
		}
		 * */
	}
}
