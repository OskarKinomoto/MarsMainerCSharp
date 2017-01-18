using System;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;

namespace MarsMiner
{
	class Window : WindowObjectBase
	{
		private const int WindowBarWidth = 30;
		private const int WindowBarMArgin = 2;
		private bool isOpen = false;

		private string Title = "";
		private Textures TitleTex;

		public event Action onClose;

		private List<WindowObjectBase> buttons = new List<WindowObjectBase>();

		public void Add(WindowButton button)
		{
			buttons.Add(button);
		}

		public Window(int width, int height, string title) : base(null, Layer.Window, new Point(0,0), Align.Center)
		{
			size = new Vector2(width, height);
			SetTitle(title);

			WindowButton closeBtn = new WindowButton(new System.Drawing.Point(0, 0), WindowButton.Type.Close, this, WindowButton.Align.Right, Layer.StatusText);
			closeBtn.onClickEvent += Close;
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

		override public void PaintOnScreen()
		{
			if (!isOpen)
				return;
			
			Vector2 position = new Vector2(-Width() / 2, -Height() / 2);
			Vector2 size = new Vector2(Width(), Height());
			Painter.DisableTextures();
			Painter.Color(.8f, .6f, .8f);
			Painter.Square(position, size, 10);

			Painter.Color(.8f, .5f, .8f);
			Painter.Square(new Vector2(-Width() / 2, Height() / 2 - WindowBarWidth), new Vector2(Width(), WindowBarWidth), 10.1f);

			Painter.EnableTextures();
			TitleTex.Bind();
			Vector2 size2 = new Vector2(TitleTex.SquareSize.Width, TitleTex.SquareSize.Height);
			Painter.Square(new Vector2(-TitleTex.TextSize.Width / 2, Height() / 2 - 2 * WindowBarWidth - TitleTex.TextSize.Height + 10), size2, 10.11f);


			foreach (var btn in buttons)
				btn.PaintOnScreen();
		}

		public Vector2 windowToRealPos(Vector2 pos)
		{
			return new Vector2(pos.X - Width() / 2, -pos.Y + Height() / 2);
		}

		public Vector2 realToWindowPos(Vector2 pos)
		{
			return new Vector2(pos.X + Width() / 2, -pos.Y + Height() / 2);
		}

		public void Mouse(Vector2 position, Mouse.Action action)
		{
			if (position.X < 0 || position.Y < 0 || position.X > Width() || position.Y > Height())
				return;

			foreach (var btn in buttons)
				btn.Mouse(position, action);
		}
	}
}

