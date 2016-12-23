using System;
using OpenTK;
using System.Drawing;

namespace MarsMiner
{
	public class ProgressBar
	{
		private static Vector3 BarBackgroundColor = new Vector3(.2f, .2f, .2f);

		private int _max;
		private int _current;

		public ProgressBar(int max, int current = 0)
		{
			_max = max;
			_current = current;
		}

		public void Set(int current)
		{
			_current = current;
		}

		public void Paint(Vector2 position, Vector2 size, float layer, Color color)
		{
			Painter.DisableTextures();
			Painter.Color(BarBackgroundColor);
			Painter.Square(position, size, layer);
			Painter.Color(color);
			Painter.Square(new Vector2(position.X + 2, position.Y + 2), new Vector2((size.X - 4) * ((float)_current / _max), size.Y - 4), layer + .1f);
		}
	}
}

