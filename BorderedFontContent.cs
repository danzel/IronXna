using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace IronXna
{
	/// <summary>
	/// Represents the Design Time version of a BorderedFont.
	/// Does all of the heavy lifting to render characters out to a texture etc.
	/// </summary>
	[ContentSerializerRuntimeType("IronXna.BorderedFont, IronXna")]
	class BorderedFontContent
	{
		public string InnerDefStr, BorderedDefStr;
		public Bitmap InnerTexture, BorderedTexture;

		public string RetinaInnerDefStr, RetinaBorderedDefStr;
		public Bitmap RetinaInnerTexture, RetinaBorderedTexture;

		public string KerningInfo = string.Empty;
		public string RetinaKerningInfo = string.Empty;

		public BorderedFontContent(XmlBorderedFontDefinition definition)
		{
			Font font = new Font(definition.FontName, definition.Size * 96.0f / 72.0f, GraphicsUnit.Pixel);

			Generate(out BorderedDefStr, out BorderedTexture, font, definition.BorderThickness, definition.UseKerning);
			Generate(out InnerDefStr, out InnerTexture, font, 0, definition.UseKerning);

			if (definition.IncludeRetina)
			{
				var backup = KerningInfo;
				font = new Font(definition.FontName, 2 * definition.Size * 96.0f / 72.0f, GraphicsUnit.Pixel);

				Generate(out RetinaBorderedDefStr, out RetinaBorderedTexture, font, definition.BorderThickness * 2, definition.UseKerning);
				Generate(out RetinaInnerDefStr, out RetinaInnerTexture, font, 0, definition.UseKerning);

				RetinaKerningInfo = KerningInfo;
				KerningInfo = backup;
			}
		}

		private void Generate(out string defStr, out Bitmap texture, Font font, int borderThickness, bool useKerning)
		{
			defStr = null;
			texture = null;

			#region Get the characters for output
			//Store all DrawnCharacters
			Dictionary<char, DrawnCharacter> drawnCharacters = new Dictionary<char, DrawnCharacter>();
			//Size up each character
			for (char c = '!'; c <= '~'; c++)
			{
				drawnCharacters.Add(c, DrawnCharacter.GetCharacter(font, c, borderThickness));
			}
			#endregion

			if (useKerning)
			{
				Graphics g = Graphics.FromImage(new Bitmap(1, 1));
				var charsToDo =
					Enumerable.Range('0', '9' - '0' + 1)
					.Concat(Enumerable.Range('A', 'Z' - 'A' + 1))
					.Concat(Enumerable.Range('a', 'z' - 'a' + 1)
					.Concat(new int[] { ':', '.', ',', '!' })).ToArray();

				StringBuilder kerningBuilder = new StringBuilder();

				//List<int> occurence = new List<int>(10);
				var spaceSize = g.MeasureString(" ", font);
				foreach (char first in charsToDo)
				{
					foreach (char second in charsToDo)
					{
						var a = drawnCharacters[first];
						var b = drawnCharacters[second];
						var abSize = g.MeasureString(" " + first + "" + second + " ", font);

						var manualAdv = a.XAdvance + b.XAdvance;
						//var manualW = a.Width + b.Width;

						var abW = abSize.Width - 2 * spaceSize.Width;

						int diff = (int) (abW - manualAdv);
						if (diff != 0)
							kerningBuilder.AppendFormat("{0}{1}{2} ", first, second, diff);
					}
				}
				KerningInfo = kerningBuilder.ToString();
			}

			int totalSize = 0;
			foreach (var c in drawnCharacters.Values)
				totalSize += (c.Width + (_padCharacters ? 1 : 0)) * (c.Height + (_padCharacters ? 1 : 0));

			int[] sizes = new[] { /*16, 32, 64, */128, 256, 512, 1024, 2048 };
			foreach (Size s in sizes.SelectMany(x => sizes.Select(y => new Size(x, y)))
				.OrderBy(s => s.Width * s.Height)
				.ThenBy(s => s.Width))
			{
				if (totalSize > s.Height * s.Width)
					continue;

				//Console.WriteLine("Trying " + s);
				if (TryGenerateImage(font, drawnCharacters, s, out defStr, out texture))
					break;
			}
		}

		private bool _padCharacters = true;

		private bool TryGenerateImage(Font font, Dictionary<char, DrawnCharacter> drawnCharacters, Size imageSize, out string defStr, out Bitmap texture)
		{
			if (!TryGenerateImage(font, drawnCharacters, imageSize, false, out defStr, out texture))
				return false;

			return TryGenerateImage(font, drawnCharacters, imageSize, true, out defStr, out texture);
		}

		private bool TryGenerateImage(Font font, Dictionary<char, DrawnCharacter> drawnCharacters, Size imageSize, bool generateBmp, out string defStr, out Bitmap texture)
		{
			defStr = null;

			int lineOffset = (int)(font.Size * font.FontFamily.GetCellAscent(font.Style) / font.FontFamily.GetEmHeight(font.Style));

			//Actual image
			texture = generateBmp ? new Bitmap(imageSize.Width, imageSize.Height) : null;
			Graphics g = generateBmp ? Graphics.FromImage(texture) : null;
			if (g != null)
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			StringBuilder textBuffer = new StringBuilder();

			int paddingSize = _padCharacters ? 1 : 0;
			int xPos = paddingSize;
			int yPos = paddingSize;
			int maxHeight = 0;

			//Store the details to be output about each char
			Dictionary<char, string> textData = new Dictionary<char, string>();

			//Put all of the chars into the sprite using greedy bin packing
			foreach (var charKvp in drawnCharacters
				.OrderByDescending(x => x.Value.Height)
				.ThenByDescending(x => x.Value.Width))
			{
				DrawnCharacter character = charKvp.Value;

				if (xPos + character.Width + paddingSize > imageSize.Width) //If we will go off the end, go to the next line
				{
					xPos = paddingSize;
					yPos += maxHeight + paddingSize;
					maxHeight = 0;
				}

				if (yPos + character.Height > imageSize.Height)
					return false;

				if (g != null)
					g.DrawImage(character.Bitmap, xPos - character.X, yPos - character.Y);

				//X-f.Height because TrimMeasureChar renders the char at f.Height, Y-f.Height so we don't miss any of the character that gets drawn in the -'ves
				// (lower case y in harlowsoliditalic.ttf has this problem)

				//Output bits to the .txt
				textData.Add(charKvp.Key, string.Format("{0} {1} {2} {3} {4} {5} {6} ", character.Width, character.Height, xPos, yPos, character.X - font.Height, lineOffset + font.Height - character.Y, character.XAdvance + font.Height - character.X));

				xPos += character.Width + paddingSize;
				if (character.Height > maxHeight)
					maxHeight = character.Height;
			}

			if (!generateBmp)
				return true;

			//Output details on each char
			for (char c = '!'; c <= '~'; c++)
				textBuffer.Append(textData[c]);
			//Add on space Width, Line Height
			textBuffer.AppendFormat("{0} {1}", (int)(g.MeasureString(" ", font).Width), font.Height);

			g.Dispose();
			g = null;

			//texture.Save(DateTime.Now.Millisecond + ".png", System.Drawing.Imaging.ImageFormat.Png);
			//File.WriteAllText(fileName + "txt", textBuffer.ToString());
			defStr = textBuffer.ToString();
			return true;
		}

		class DrawnCharacter
		{
			public readonly Bitmap Bitmap;

			//Position within the bitmap the char is at
			public readonly int X;
			public readonly int Y;

			//Size of the actual char sprite
			public readonly int Width;
			public readonly int Height;

			/// <summary>
			/// Width used to draw the character as part of a string
			/// </summary>
			public readonly int XAdvance;

			private DrawnCharacter(Bitmap bitmap, int xAdvance)
			{
				Bitmap = bitmap;

				#region Find the character in the bitmap
				int minX = -1; //First pixel that has something
				int maxX = -1; //Last pixel that has something
				int minY = -1;
				int maxY = -1;

				for (int x = 0; x < bitmap.Width; x++)
				{
					for (int y = 0; y < bitmap.Height; y++)
					{
						//There is a pixel here
						Color color = bitmap.GetPixel(x, y);
						if (color.A != 0)
						{
							if (minX == -1)
								minX = x;
							maxX = x;

							if (minY == -1 || y < minY)
								minY = y;

							if (y > maxY)
								maxY = y;
						}
					}
				}
				#endregion

				X = minX;
				Y = minY;

				//+1 based on definition of min/max (they are the first and last pixel with something in it)
				Width = maxX - minX + 1;
				Height = maxY - minY + 1;

				XAdvance = xAdvance;
			}

			/// <summary>
			/// Renders the given char grid aligned and returns it and its details.
			/// </summary>
			private static DrawnCharacter GetGridAlignedCharacter(Font font, char c)
			{
				Bitmap b = new Bitmap(font.Height * 3, font.Height * 3);
				SizeF fullSize;
				SizeF spaceSize;
				using (Graphics g = Graphics.FromImage(b))
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; //For best quality
					g.Clear(Color.Transparent);
					g.DrawString(c.ToString(), font, Brushes.White, font.Height, font.Height);

					fullSize = g.MeasureString(" " + c + " ", font);
					spaceSize = g.MeasureString(" ", font);
				}

				return new DrawnCharacter(b, (int)(fullSize.Width - 2 * spaceSize.Width));
			}

			/// <summary>
			/// Renders the given char antialiased and returns it and its details.
			/// </summary>
			private static DrawnCharacter GetUnborderedCharacter(Font font, char c)
			{
				Bitmap b = new Bitmap(font.Height * 3, font.Height * 3);
				SizeF fullSize;
				SizeF spaceSize;
				using (Graphics g = Graphics.FromImage(b))
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.Clear(Color.Transparent);

					GraphicsPath path = new GraphicsPath();
					path.AddString(c.ToString(), font.FontFamily, (int)FontStyle.Regular, font.Size, new Point(font.Height, font.Height), StringFormat.GenericDefault);
					g.FillPath(Brushes.White, path);

					fullSize = g.MeasureString(" " + c + " ", font);
					spaceSize = g.MeasureString(" ", font);
				}

				return new DrawnCharacter(b, (int)(fullSize.Width - 2 * spaceSize.Width));
			}

			/// <summary>
			/// Renders the given char antialiased and returns it and its details.
			/// </summary>
			private static DrawnCharacter GetBorderedCharacter(Font font, char c, int borderThickness)
			{
				Bitmap b = new Bitmap(font.Height * 3, font.Height * 3);
				SizeF fullSize;
				SizeF spaceSize;
				using (Graphics g = Graphics.FromImage(b))
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.Clear(Color.Transparent);

					GraphicsPath path = new GraphicsPath();
					path.AddString(c.ToString(), font.FontFamily, (int)FontStyle.Regular, font.Size, new Point(font.Height, font.Height), StringFormat.GenericDefault);

					Pen pen = new Pen(Color.White, borderThickness * 2);
					pen.LineJoin = LineJoin.Round;
					g.DrawPath(pen, path);

					fullSize = g.MeasureString(" " + c + " ", font);
					spaceSize = g.MeasureString(" ", font);
				}

				return new DrawnCharacter(b, (int)(fullSize.Width - 2 * spaceSize.Width));
			}

			/// <summary>
			/// Gets the character with the style as decided by borderThickness
			/// </summary>
			/// <param name="font"></param>
			/// <param name="c"></param>
			/// <param name="borderThickness">null: No border. 0: Inner part of bordered text. >0: Border part of bordered text</param>
			internal static DrawnCharacter GetCharacter(Font font, char c, int? borderThickness)
			{
				if (!borderThickness.HasValue)
					return GetGridAlignedCharacter(font, c);
				if (borderThickness.Value == 0)
					return GetUnborderedCharacter(font, c);
				return GetBorderedCharacter(font, c, borderThickness.Value);
			}
		}

	}
}
