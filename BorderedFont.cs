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

		internal BorderedFont(Texture2D borderedTexture, Texture2D innerTexture, string borderedDefStr, string innerDefStr, string kerning)
		{
			Kerning = kerning == "" ? null : new KerningDef(kerning);

			Border = new SubFont(borderedTexture, borderedDefStr, Kerning);
			Inner = new SubFont(innerTexture, innerDefStr, Kerning);
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
			readonly CharDetail[] _characters = new CharDetail['~' + 1];

			/// <summary>
			/// Width of a space character
			/// </summary>
			public readonly int SpaceWidth;
			/// <summary>
			/// Height of a line in pixels
			/// </summary>
			public readonly int LineHeight;

			public readonly int AboveLineSize;

			public SubFont(Texture2D texture, string def, KerningDef kerning)
			{
				_texture = texture;
				_kerning = kerning;

				string[] split = def.Split(' ', '\r', '\n');

				if (split.Length != 660)
					throw new Exception("def isn't correct");

				int idx = 0;

				for (int i = '!'; i <= '~'; i++)
				{
					//Read this char
					_characters[i].Width = int.Parse(split[idx]); idx++;
					_characters[i].Height = int.Parse(split[idx]); idx++;
					_characters[i].X = int.Parse(split[idx]); idx++;
					_characters[i].Y = int.Parse(split[idx]); idx++;
					_characters[i].XOffset = int.Parse(split[idx]); idx++;
					_characters[i].YOffset = int.Parse(split[idx]); idx++;
					_characters[i].XAdvance = int.Parse(split[idx]); idx++;
				}

				AboveLineSize = _characters.Select(x => x.YOffset).Max();

				SpaceWidth = int.Parse(split[idx]); idx++;
				LineHeight = int.Parse(split[idx]);
			}

			public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
			{
				//origin.X = -origin.X;
				//origin -= position;
				//position += origin;
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

				spriteBatch.Draw(_texture, position, new Rectangle(_characters[c].X, _characters[c].Y, _characters[c].Width, _characters[c].Height), color, rotation, origin + new Vector2(0, _characters[c].YOffset), scale, SpriteEffects.None, 1);

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
				return new Vector2(width+2, LineHeight+2);//maxYOffset + maxHeightMinusYOffset);
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
