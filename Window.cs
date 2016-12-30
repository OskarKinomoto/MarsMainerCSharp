using System;
using OpenTK;
using System.Collections.Generic;

namespace MarsMiner
{
	public class Window : PaintInterface, MouseInterface
	{
		private const int WindowBarWidth = 30;
		private const int WindowBarMArgin = 2;

		private int width;
		private int height;
		private bool isOpen = false;

		private string Title = "";
		private Textures TitleTex;

		public event Action onClose;

		private List<WindowButton> buttons = new List<WindowButton>();

		public void Add(WindowButton button)
		{
			buttons.Add(button);
		}

		public int Width {
			get {
				return this.width;
			}
		}

		public int Height {
			get {
				return this.height;
			}
		}

		public Window(int width, int height, string title)
		{
			this.width = width;
			this.height = height;
			SetTitle(title);

			WindowButton closeBtn = new WindowButton(new System.Drawing.Point(0, 0), WindowButton.Type.Close, this, WindowButton.Align.Right, Layer.StatusText);
			closeBtn.setOnClickEvent(Close);
			Add(closeBtn);
		}

		public void SetTitle(string title)
		{
			if (title == this.Title)
				return;
			
			this.Title = title;
			TitleTex = Textures.Text(title, Textures.FontName.Tahoma, 12);
		}

		public void Open()
		{
			isOpen = true;
		}

		public void Close()
		{
			isOpen = false;
			if (onClose != null)
				onClose();
		}

		public void Paint()
		{
			throw new MissingMethodException();
		}

		public void PaintOnScreen()
		{
			if (!isOpen)
				return;
			
			Vector2 position = new Vector2(-width / 2, -height / 2);
			Vector2 size = new Vector2(width, height);
			Painter.DisableTextures();
			Painter.Color(.8f, .6f, .8f);
			Painter.Square(position, size, 10);

			Painter.Color(.8f, .5f, .8f);
			Painter.Square(new Vector2(-width / 2, height / 2 - WindowBarWidth), new Vector2(width, WindowBarWidth), 10.1f);


			Painter.EnableTextures();
			TitleTex.Bind();
			Vector2 size2 = new Vector2(TitleTex.SquareSize.Width, TitleTex.SquareSize.Height);
			Painter.Square(new Vector2(-TitleTex.TextSize.Width / 2, height / 2 - 2 * WindowBarWidth - TitleTex.TextSize.Height + 10), size2, 10.11f);


			foreach (var btn in buttons)
				btn.PaintOnScreen();
		}

		public Vector2 windowToRealPos(Vector2 pos)
		{
			return new Vector2(pos.X - width / 2, -pos.Y + height / 2);
		}

		public Vector2 realToWindowPos(Vector2 pos)
		{
			return new Vector2(pos.X + width / 2, -pos.Y + height / 2);
		}

		public void Mouse(Vector2 position, Mouse.Action action)
		{
			Vector2 windowPosition = realToWindowPos(position);

			if (windowPosition.X < 0 || windowPosition.Y < 0 || windowPosition.X > width || windowPosition.Y > height)
				return;

			foreach (var btn in buttons)
				btn.Mouse(windowPosition, action);
		}
	}
}

