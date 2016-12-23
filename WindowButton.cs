using System;
using OpenTK;
using System.Diagnostics;

namespace MarsMiner
{
	public class WindowButton : PaintInterface, MouseInterface
	{
		public enum Position {
			Left,
			Center,
			Right,
		}

		public enum Type {
			Normal,
			Close,
		}

		private Position position;

		private Type type;

		public event Action onClickEvent;

		private Window parent;

		private Vector2 size = new Vector2(20,20);
		private float margin = 5;

		private Textures text;

		private int WindowX;
		private int WindowY;
		private bool focus = false;

		private Layer layer;

		public WindowButton(int windowX, int windowY, Type type, Window parent, Position position, Layer layer, string s = "")
		{
			this.layer = layer;
			this.WindowX = windowX;
			this.WindowY = windowY;
			this.position = position;
			this.type = type;
			this.parent = parent;

			if (type != Type.Close) {
				text = Textures.Text(s, Textures.FontName.Tahoma, 12);
				size = new Vector2(20 + text.TextSize.Width, 30);
			}
		}

		public void setWidth(float width)
		{
			size.X = 20 + width;
		}


		public void setOnClickEvent(Action onClickEvent)
		{
			this.onClickEvent = onClickEvent;
		}

		private Vector2 windowPosition() 
		{
			Vector2 ret = new Vector2(0, WindowY + size.Y + margin);

			switch (position) {
			case Position.Right:
				ret.X = parent.Width - WindowX - size.X - margin;
				break;
			case Position.Left:
				ret.X = WindowX + margin;
				break;
			case Position.Center:
				ret.X = (parent.Width - size.X) / 2;
				break;
			}

			return ret;
		}

		public void Paint()
		{
			throw new MissingMethodException();
		}

		public void PaintOnScreen()
		{
			Sprites.Bind();
			Painter.EnableTextures();
			Painter.StartQuads();

			var glPosition = parent.windowToRealPos(windowPosition());

			if (type == Type.Close) {
				Painter.Sprite(glPosition, size, focus ? Sprites.Name.CloseCircleFocus : Sprites.Name.CloseCircle, layer);
				Painter.Stop();
			} else {
				Painter.Sprite(glPosition + new Vector2(size.Y, size.Y), size - new Vector2(size.Y, size.Y) * 2, focus ? Sprites.Name.ButtonFocusCenter : Sprites.Name.ButtonCenter, layer);
				Painter.Sprite(glPosition, new Vector2(size.Y, size.Y), focus ? Sprites.Name.ButtonFocusLeft : Sprites.Name.ButtonLeft, layer);
				Painter.Sprite(glPosition + size - new Vector2(size.Y, size.Y), new Vector2(size.Y, size.Y), focus ? Sprites.Name.ButtonFocusRight : Sprites.Name.ButtonRight, layer);
				Painter.Stop();

				text.Bind();
				Vector2 TextSize = new Vector2(text.SquareSize.Width, text.SquareSize.Height);
				Painter.Square(glPosition - new Vector2(- (size.X - text.TextSize.Width) / 2, size.Y + (text.TextSize.Height) / 2.0f ), TextSize, layer + 0.01f);
			}
		}		

		private Vector2 buttonPosition(Vector2 position)
		{
			position -= windowPosition();
			position.Y += size.Y;
			return position;
		}

		private bool FocusTest(Vector2 btnPosition)
		{
			return 
				btnPosition.X >= 0 && btnPosition.X <= size.X
			&&
				btnPosition.Y >= 0 && btnPosition.Y <= size.Y;
		}

		public void Mouse(Vector2 position, Mouse.Action action)
		{
			focus = FocusTest(buttonPosition(position));
			
			if (focus && action == MarsMiner.Mouse.Action.LeftClick) {
				if (onClickEvent != null)
					onClickEvent();
			}
		}
	}
}

