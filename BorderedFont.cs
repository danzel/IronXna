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

		public BorderedFont(Texture2D borderedTexture, Texture2D innerTexture, string borderedDefStr, string innerDefStr, string kerning, bool isRetina)
		{
			Kerning = kerning == "" ? null : new KerningDef(kerning, isRetina);

			Border = new SubFont(borderedTexture, borderedDefStr, Kerning, isRetina);
			Inner = new SubFont(innerTexture, innerDefStr, Kerning, isRetina);
		}

		internal class KerningDef
		{
			private readonly Dictionary<int, float> _kerningDef = new Dictionary<int, float>();

			public KerningDef(string kerning, bool isRetina)
			{
				var split = kerning.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				foreach (var s in split)
				{
					char first = s[0];
					char second = s[1];
					float kern = int.Parse(s.Substring(2));
					if (isRetina)
						kern *= 0.5f;

					int index = first * 256 + second;
					_kerningDef[index] = kern;
				}
			}

			public float KerningFor(char first, char second)
			{
				int index = first * 256 + second;
				float res;
				if (_kerningDef.TryGetValue(index, out res))
					return res;
				return 0;
			}
		}


		internal class SubFont
		{
			private readonly KerningDef _kerning;
			internal readonly bool IsRetina;
			readonly Dictionary<char, CharDetail> _characters = new Dictionary<char, CharDetail>();

			/// <summary>
			/// Width of a space character
			/// </summary>
			public readonly float SpaceWidth;
			/// <summary>
			/// Height of a line in pixels
			/// </summary>
			public readonly float LineHeight;

			public readonly float AboveLineSize;

			public SubFont(Texture2D texture, string def, KerningDef kerning, bool isRetina)
			{
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
					float xOffset = int.Parse(split[idx]); idx++;
					float yOffset = int.Parse(split[idx]); idx++;
					float xAdvance = int.Parse(split[idx]); idx++;

					if (isRetina)
					{
						xOffset *= 0.5f;
						yOffset *= 0.5f;
						xAdvance *= 0.5f;
					}
					_characters[i] = new CharDetail
						{
							Texture = new SubTexture2D(texture, new Rectangle(x, y, width, height), isRetina),
							XOffset = xOffset,
							YOffset = yOffset,
							XAdvance = xAdvance
						};
				}

				AboveLineSize = _characters.Select(x => x.Value.YOffset).Max();

				SpaceWidth = int.Parse(split[idx]); idx++;
				LineHeight = int.Parse(split[idx]);

				if (IsRetina)
				{
					SpaceWidth *= 0.5f;
					LineHeight *= 0.5f;
				}
			}

			public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
			{
				var scaleVec = new Vector2(scale);

				for (int a = 0; a < text.Length; a++)
				{
					if (_kerning != null && a > 0)
						origin.X -= _kerning.KerningFor(text[a - 1], text[a]);

					RenderChar(spriteBatch, text[a], position, color, rotation, ref origin, scaleVec);
				}
			}

			private void RenderChar(SpriteBatch spriteBatch, char c, Vector2 position, Color color, float rotation, ref Vector2 origin, Vector2 scale)
			{
				if (c == ' ')
				{
					origin.X -= SpaceWidth;
					return;
				}
#if !DEBUG
				if (!_characters.ContainsKey(c)) //skip chars we dont know about
					return;
#endif

				var charDetail = _characters[c];

				if (charDetail.Texture.Width == 0)
					return;

				if (charDetail.XOffset != 0)
					origin.X -= charDetail.XOffset;

				spriteBatch.Draw(charDetail.Texture, position, color, rotation, origin + new Vector2(0, charDetail.YOffset), scale);

				origin.X -= charDetail.XAdvance;
			}

			public float GetCharWidth(char c)
			{
				if (c == ' ')
					return SpaceWidth;

				return _characters[c].XOffset + _characters[c].XAdvance;
			}

			public Vector2 MeasureString(string text)
			{
				float width = 0;
				float maxYOffset = 0, maxHeightMinusYOffset = 0, maxHeight = 0;

				for (int i = 0; i < text.Length; i++)
				{
					var c = text[i];

					if (c == ' ')
					{
						width += SpaceWidth;
						continue;
					}
#if !DEBUG
					if (!_characters.ContainsKey(c)) //skip chars we dont know about
						continue;
#endif
					if (_kerning != null && i > 0)
						width += _kerning.KerningFor(text[i - 1], text[i]);
					width += GetCharWidth(c);
					maxHeight = Math.Max(maxHeight, _characters[c].Texture.Height);
					maxYOffset = Math.Max(maxYOffset, _characters[c].YOffset);
					maxHeightMinusYOffset = Math.Max(maxHeightMinusYOffset, _characters[c].Texture.Height - _characters[c].YOffset);
				}
				return new Vector2(width+2, LineHeight+2);
			}
		}


		struct CharDetail
		{
			//Texture details
			public SubTexture2D Texture;

			//Character position details
			public float XOffset;
			public float YOffset;
			public float XAdvance;
		}
	}
}
