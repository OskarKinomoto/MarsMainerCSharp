using System;
using OpenTK;
using System.Drawing;

namespace MarsMiner
{
	public class Clouds : PaintInterface
	{
		private readonly float WindConstant = 70;

		Textures cloudsTexture;
		bool loaded = false;
		private Vector2 size;

		private float move_x = 0;
		private float positionY = 200;

		public Clouds()
		{
			if (Preferences.GodMode)
				WindConstant = 200;
		}

		public void Load()
		{
			loaded = true;
			cloudsTexture = new Textures("clouds.png");
			size = new Vector2(cloudsTexture.SquareSize.Width, cloudsTexture.SquareSize.Height);
		}

		public void Paint()
		{
			if (!loaded)
				Load();

			cloudsTexture.Bind();

			Painter.EnableTextures();
			Painter.StartQuads();

			Painter.Square2(new Vector2(Model.MinTileX - size.X + move_x, positionY), size, 0);
			Painter.Square2(new Vector2(Model.MinTileX + move_x, positionY), size, 0);
			Painter.Square2(new Vector2(Model.MinTileX + size.X + move_x, positionY), size, 0);

			Painter.Stop();
		}

		public void PaintOnScreen()
		{
		}

		public void Tick(float tau)
		{
			move_x += tau * WindConstant;

			if (move_x >= size.X)
				move_x -= size.X;
		}
	}
}

