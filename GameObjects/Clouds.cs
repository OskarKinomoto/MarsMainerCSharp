/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;
using System.Drawing;

namespace MarsMiner
{
	public class Clouds : PaintInterface
	{
		private readonly float WindConstant = 70;
		private readonly float WindConstant2 = 40;

		Textures cloudsTexture;
		Textures cloudsLowTexture;
		bool loaded = false;

		private Vector2 size;
		private Vector2 size2;

		private float move_x = 0;
		private float positionY = 200;

		private float move_x2 = 100;
		private float positionY2 = 75;

		public Clouds()
		{
			if (Preferences.CloudsDeveloper) {
				WindConstant = 200;
				WindConstant2 = 150;
			}
		}

		public void Load()
		{
			loaded = true;
			cloudsTexture = new Textures("clouds.png");
			cloudsLowTexture = new Textures("clouds-low.png");
			size = new Vector2(cloudsTexture.SquareSize.Width, cloudsTexture.SquareSize.Height);
			size2 = new Vector2(cloudsLowTexture.SquareSize.Width / 2, cloudsLowTexture.SquareSize.Height / 2);
		}

		public void Paint()
		{
			if (!loaded)
				Load();

			Painter.EnableTextures();

			cloudsLowTexture.Bind();
			Painter.StartQuads();

			Painter.Square2(new Vector2(Model.MinTileX - size2.X + move_x2, positionY2), size2, -.1f);
			Painter.Square2(new Vector2(Model.MinTileX + move_x2, positionY2), size2, -.1f);
			Painter.Square2(new Vector2(Model.MinTileX + size2.X + move_x2, positionY2), size2, -.1f);

			Painter.Stop();
			
			cloudsTexture.Bind();
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
			move_x2 += tau * WindConstant2;

			if (move_x >= size.X)
				move_x -= size.X;

			if (move_x2 >= size2.X)
				move_x2 -= size2.X;
		}
	}
}

