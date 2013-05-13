using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronXna
{
	public class BorderedFont
	{
		internal SubFont Border, Inner;
		internal KerningDef Kerning;

		/// <summary>
		/// Returns the size we will draw the inner text of the given string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string text)
		{
			return Inner.MeasureString(text);
		}

		internal BorderedFont(Texture2D borderedTexture, Texture2D innerTexture, string borderedDefStr, string innerDefStr, string kerning, bool isRetina)
		{
			Kerning = kerning == "" ? null : new KerningDef(kerning);

			Border = new SubFont(borderedTexture, borderedDefStr, Kerning, isRetina);
			Inner = new SubFont(innerTexture, innerDefStr, Kerning, isRetina);
		}

		internal class KerningDef
		{
			private readonly Dictionary<int, int> _kerningDef = new Dictionary<int, int>();

			public KerningDef(string kerning)
			{
				var split = kerning.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				foreach (var s in split)
				{
					char first = s[0];
					char second = s[1];
					int kern = int.Parse(s.Substring(2));

					int index = first * 256 + second;
					_kerningDef[index] = kern;
				}
			}

			public int KerningFor(char first, char second)
			{
				int index = first * 256 + second;
				int res;
				if (_kerningDef.TryGetValue(index, out res))
					return res;
				return 0;
			}
		}


		internal class SubFont
		{
			readonly Texture2D _texture;
			private readonly KerningDef _kerning;
			internal readonly bool IsRetina;
			readonly Dictionary<char, CharDetail> _characters = new Dictionary<char, CharDetail>();

			/// <summary>
			/// Width of a space character
			/// </summary>
			public readonly int SpaceWidth;
			/// <summary>
			/// Height of a line in pixels
			/// </summary>
			public readonly int LineHeight;

			public readonly int AboveLineSize;

			public SubFont(Texture2D texture, string def, KerningDef kerning, bool isRetina)
			{
				_texture = texture;
				_kerning = kerning;
				IsRetina = isRetina;

				string[] split = def.Split(' ', '\r', '\n');

				int idx = 0;

				while (idx < split.Length - 2)
				{
					//Read this char
					char i = split[idx][0]; idx++;

					var width = int.Parse(split[idx]); idx++;
					var height = int.Parse(split[idx]); idx++;
					var x = int.Parse(split[idx]); idx++;
					var y = int.Parse(split[idx]); idx++;
					var xOffset = int.Parse(split[idx]); idx++;
					var yOffset = int.Parse(split[idx]); idx++;
					var xAdvance = int.Parse(split[idx]); idx++;

					_characters[i] = new CharDetail
						{
							Width = width,
							Height = height,
							X = x,
							Y = y,
							XOffset = xOffset,
							YOffset = yOffset,
							XAdvance = xAdvance
						};
				}

				AboveLineSize = _characters.Select(x => x.Value.YOffset).Max();

				SpaceWidth = int.Parse(split[idx]); idx++;
				LineHeight = int.Parse(split[idx]);

				if (IsRetina)
					LineHeight /= 2;
			}

			public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
			{
				if (IsRetina)
				{
					origin *= 2;
					scale *= 0.5f;
				}

				for (int a = 0; a < text.Length; a++)
				{
					if (_kerning != null && a > 0)
						origin.X -= _kerning.KerningFor(text[a - 1], text[a]);

					RenderChar(spriteBatch, text[a], position, color, rotation, ref origin, scale);
				}
			}

			private void RenderChar(SpriteBatch spriteBatch, char c, Vector2 position, Color color, float rotation, ref Vector2 origin, float scale)
			{
				if (c >= 256)
					return;

				if (c == ' ')
				{
					origin.X -= SpaceWidth;
					return;
				}

				if (_characters[c].Width == 0)
					return;

				if (_characters[c].XOffset != 0)
					origin.X -= _characters[c].XOffset;

				spriteBatch.Draw(_texture, position, new Rectangle(_characters[c].X, _characters[c].Y, _characters[c].Width, _characters[c].Height), color, rotation, origin + new Vector2(0, _characters[c].YOffset), scale, SpriteEffects.None, 0);

				origin.X -= _characters[c].XAdvance;
			}

			public int GetCharWidth(char c)
			{
				if (c == ' ')
					return SpaceWidth;
				return _characters[c].XOffset + _characters[c].XAdvance;
			}

			public Vector2 MeasureString(string text)
			{
				int width = 0;
				int maxYOffset = 0, maxHeightMinusYOffset = 0, maxHeight = 0;

				for (int i = 0; i < text.Length; i++)
				{
					var c = text[i];

					if (_kerning != null && i > 0)
						width += _kerning.KerningFor(text[i - 1], text[i]);
					width += GetCharWidth(c);
					maxHeight = Math.Max(maxHeight, _characters[c].Height);
					maxYOffset = Math.Max(maxYOffset, _characters[c].YOffset);
					maxHeightMinusYOffset = Math.Max(maxHeightMinusYOffset, _characters[c].Height - _characters[c].YOffset);
				}
				if (IsRetina)
					return new Vector2(width/2+2, LineHeight+2);
				return new Vector2(width+2, LineHeight+2);
			}
		}


		struct CharDetail
		{
			//Texture details
			public int Y;
			public int X;
			public int Width;
			public int Height;

			//Character position details
			public int XOffset;
			public int YOffset;
			public int XAdvance;
		}
	}
}
