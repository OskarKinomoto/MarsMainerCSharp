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

		Textures TextureEnterText;

		protected Window window;

		public event Action onClose;

		public Building(String s)
		{
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
			Vector2 position = new Vector2(LeftXPosition, 0);
			Vector2 size = new Vector2(-LeftXPosition + RightXPosition, 100);

			Painter.DisableTextures();
			Painter.Color(.8f, .8f, .8f);
			Painter.Square(position, size, 1);
		}

		public void PaintBuildingMenu()
		{
			window.Open();
			window.PaintOnScreen();
			
			const int margin = 20;
		}

		public bool CanEnter(Robot r)
		{
			return -1 < r.m_position.Y && r.m_position.Y < 2 &&
			LeftXPosition - Robot.Size / 2 < r.m_position.X &&
			r.m_position.X < RightXPosition - Robot.Size / 2;
		}

		public void Mouse(Vector2 position, Mouse.Action action)
		{
			window.Mouse(position, action);
		}

		public void Close()
		{
			if (onClose != null)
				onClose();
		}
	}
}

