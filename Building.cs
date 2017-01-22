/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using System.Diagnostics;

namespace MarsMiner
{
	class Building : MouseInterface
	{
		protected int LeftXPosition;
		protected int RightXPosition;

		protected int TextureXWidth;
		protected int TextureYHeight;
		protected Sprites.Name sprite;

		Textures TextureEnterText;

		protected Window window;

		public event Action onClose;

		public Building(String s, int TextureXWidth, int TextureYHeight, Sprites.Name sprite, int LeftX)
		{
			this.TextureXWidth = TextureXWidth;
			this.TextureYHeight = TextureYHeight;
			this.sprite = sprite;

			LeftXPosition = LeftX* Tile.Size;
			RightXPosition = LeftXPosition + TextureXWidth * Tile.Size;

			window = new Window(600, 400, s);
			window.onClose += Close;
			TextureEnterText = Textures.Text("Press E to enter " + s, Textures.FontName.Tahoma, 12);
		}

		public float left()
		{
			return LeftXPosition;
		}

		public float right()
		{
			return RightXPosition;
		}

		public void PaintEnter()
		{
			Vector2 position = new Vector2(-TextureEnterText.TextSize.Width / 2, Controler.WindowHeight / 2 - 100 - TextureEnterText.SquareSize.Height);
			Vector2 size = new Vector2(TextureEnterText.SquareSize.Width, TextureEnterText.SquareSize.Height);

			Painter.EnableTextures();
			TextureEnterText.Bind();
			Painter.Square(position, size, 1);
		}

		virtual public void Paint()
		{
			Vector2 position = new Vector2(LeftXPosition, (TextureYHeight - 1) * Tile.Size);
			Vector2 size = new Vector2(Tile.Size, Tile.Size);

			Sprites.Bind();
			Painter.EnableTextures();
			Painter.StartQuads();
			for (int i = 0; i < TextureYHeight; ++i)
				for (int j = 0; j < TextureXWidth; ++j)
					Painter.Sprite(position + new Vector2(Tile.Size * j, -i * Tile.Size), size, sprite, 3, i * TextureXWidth + j);

			Painter.Stop();			
		}

		public void PaintBuildingMenu()
		{
			window.Open();
			window.PaintOnScreen();
		}

		public bool CanEnter(Robot r)
		{
			return -1 < r.m_position.Y && r.m_position.Y < 2 &&
			LeftXPosition - Robot.Size / 2 < r.m_position.X &&
			r.m_position.X < RightXPosition - Robot.Size / 2;
		}

		public void Mouse(Vector2 position, Mouse.Action action)
		{
			window.Mouse(window.realToWindowPos(position), action);
		}

		public void Close()
		{
			if (onClose != null)
				onClose();
		}
	}
}

