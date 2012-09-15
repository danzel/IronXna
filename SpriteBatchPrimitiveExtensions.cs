﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace IronXna
{
	/// <summary>
	/// A class to draw 2D Primitives.
	/// If you want to use it, call SpriteBatchPrimitiveExtensions.Initialize in your LoadContent function
	/// </summary>
	public static class SpriteBatchPrimitiveExtensions
	{
		private static Texture2D _pixel;
		public static void Initialize(GraphicsDevice graphics)
		{
			//1px x 1px white texture
			_pixel = Texture2D.FromStream(graphics, new MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAAxJREFUCNdj+P//PwAF/gL+3MxZ5wAAAABJRU5ErkJggg==")));
		}

		public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
		{
			spriteBatch.Draw(_pixel, rectangle, color);
		}

		/*

		readonly List<Vector2> _vectors = new List<Vector2>();

		/// <summary>
		/// Gets/sets the colour of the primitive line object.
		/// </summary>
		public Color Colour;

		/// <summary>
		/// Gets/sets the position of the primitive line object.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Gets/sets the render depth of the primitive line object (0 = front, 1 = back)
		/// </summary>
		public float Depth;

		/// <summary>
		/// Gets the number of vectors which make up the primtive line object.
		/// </summary>
		public int CountVectors
		{
			get
			{
				return _vectors.Count;
			}
		}

		/// <summary>
		/// Creates a new primitive line object.
		/// </summary>
		/// <param name="graphicsDevice">The Graphics Device object to use.</param>
		public PrimitiveLine(GraphicsDevice graphicsDevice)
		{
			// 1 pixel white texture

			Colour = Color.White;
			Position = new Vector2(0, 0);
			Depth = 0;
		}

		/// <summary>
		/// Adds a vector to the primive live object.
		/// </summary>
		/// <param name="vector">The vector to add.</param>
		public void AddVector(Vector2 vector)
		{
			_vectors.Add(vector);
		}

		/// <summary>
		/// Insers a vector into the primitive line object.
		/// </summary>
		/// <param name="index">The index to insert it at.</param>
		/// <param name="vector">The vector to insert.</param>
		public void InsertVector(int index, Vector2 vector)
		{
			_vectors.Insert(index, vector);
		}

		/// <summary>
		/// Removes a vector from the primitive line object.
		/// </summary>
		/// <param name="vector">The vector to remove.</param>
		public void RemoveVector(Vector2 vector)
		{
			_vectors.Remove(vector);
		}

		/// <summary>
		/// Removes a vector from the primitive line object.
		/// </summary>
		/// <param name="index">The index of the vector to remove.</param>
		public void RemoveVector(int index)
		{
			_vectors.RemoveAt(index);
		}

		/// <summary>
		/// Clears all vectors from the primitive line object.
		/// </summary>
		public void ClearVectors()
		{
			_vectors.Clear();
		}

		/// <summary>
		/// Renders the primtive line object.
		/// </summary>
		/// <param name="spriteBatch">The sprite batch to use to render the primitive line object.</param>
		public void Render(SpriteBatch spriteBatch)
		{
			if (_vectors.Count < 2)
				return;

			for (int i = 1; i < _vectors.Count; i++)
			{
				Vector2 vector1 = _vectors[i - 1];
				Vector2 vector2 = _vectors[i];

				// calculate the distance between the two vectors
				float distance = Vector2.Distance(vector1, vector2);

				// calculate the angle between the two vectors
				float angle = (float)Math.Atan2(vector2.Y - vector1.Y, vector2.X - vector1.X);

				// stretch the pixel between the two vectors
				spriteBatch.Draw(_pixel,
					Position + vector1,
					null,
					Colour,
					angle,
					Vector2.Zero,
					new Vector2(distance, 1),
					SpriteEffects.None,
					Depth);
			}
		}

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
