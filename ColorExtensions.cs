﻿using Microsoft.Xna.Framework;

namespace IronXna
{
	public static class ColorExtensions
	{
		public static Color ApplyAlpha(this Color color, float opacity)
		{
#if UNITY
			return new Color(color, color.A * opacity / 255.0f);
#else
			return new Color((byte)(color.R * opacity), (byte)(color.G * opacity), (byte)(color.B * opacity), (byte)(color.A * opacity));
#endif
		}
	}
}
