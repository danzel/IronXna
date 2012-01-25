using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace IronXna
{
	public class BorderedFont
	{
		internal SubFont Border, Inner;
		internal KerningDef Kerning;

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
			Texture2D _texture;
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

			public int CapitalHHeight
			{
				get { return _characters['H'].Height; }
			}

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

				SpaceWidth = int.Parse(split[idx]); idx++;
				LineHeight = int.Parse(split[idx]);
			}

			public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
			{
				for (int a = 0; a < text.Length; a++)
				{
					if (_kerning != null && a > 0)
					{
						position.X += _kerning.KerningFor(text[a - 1], text[a]);
					}

					RenderChar(spriteBatch, text[a], ref position, color);
				}
			}

			private void RenderChar(SpriteBatch spriteBatch, char c, ref Vector2 position, Color color)
			{

				if (c >= 256)
					return;

				if (c == ' ')
				{
					position.X += SpaceWidth;
					return;
				}

				if (_characters[c].Width == 0)
					return;

				if (_characters[c].XOffset != 0)
					position.X += _characters[c].XOffset;

				spriteBatch.Draw(_texture, position - new Vector2(0, _characters[c].YOffset), new Rectangle(_characters[c].X, _characters[c].Y, _characters[c].Width, _characters[c].Height), color);
				/*
				float texX = _characters[c].X * TextureWidthPercent / Width;
				float texX2 = texX + (_characters[c].Width * TextureWidthPercent / Width);
				float texY = _characters[c].Y * TextureHeightPercent / Height;
				float texY2 = texY + (_characters[c].Height * TextureHeightPercent / Height);

				Vertex2DArray[0].Set(0, -_characters[c].YOffset, texX, texY);

				Vertex2DArray[1].Set(0, _characters[c].Height - _characters[c].YOffset, texX, texY2);

				Vertex2DArray[2].Set(_characters[c].Width, _characters[c].Height - _characters[c].YOffset, texX2, texY2);

				Vertex2DArray[3].Set(_characters[c].Width, -_characters[c].YOffset, texX2, texY);

				GL.BindBuffer(BufferTarget.ArrayBuffer, VboBufferId);
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(4 * Vertex2D.Stride), Vertex2DArray, BufferUsageHint.StreamDraw);

				GL.VertexPointer(2, VertexPointerType.Float, Vertex2D.Stride, 0);
				GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex2D.Stride, Vector2.SizeInBytes);
				GL.DrawArrays(BeginMode.TriangleFan, 0, 4);
				 * */

				position.X += _characters[c].XAdvance;
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
