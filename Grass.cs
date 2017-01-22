using System;
using OpenTK;

namespace MarsMiner
{
	class Grass : PaintInterface
	{
		private const int Len = 7;
		private static readonly Vector2 size = new Vector2(Tile.Size, Tile.Size);

		public void Paint()
		{
			Sprites.Bind();
			Painter.EnableTextures();
			Painter.StartQuads();

			int phase = 0;
			for (int i = Model.MinX; i < Model.MaxX; ++i) {
				phase++;
				Painter.Sprite(new Vector2(Tile.Size * i, 0), size, Sprites.Name.Grass, 2f, phase % Len);
			}

			Painter.Stop();
		}

		public void PaintOnScreen()
		{
			throw new Exception();
		}
	}
}

