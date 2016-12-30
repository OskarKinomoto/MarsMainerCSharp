using System;
using OpenTK;
using System.Diagnostics;
using System.Drawing;

namespace MarsMiner
{
	public class WindowButton : WindowObjectBase, MouseInterface
	{
		public enum Type {
			Normal,
			Close,
		}

		private Type type;

		public event Action onClickEvent;

		private Vector2 size = new Vector2(20,20);
		private float margin = 5;

		private Textures text;

		private bool focus = false;

		public WindowButton(Point position, Type type, Window parent, Align align, Layer layer, string s = "")
			: base(parent, layer, position, align)
		{
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
			Vector2 ret = new Vector2(0, position.Y + size.Y + margin);

			switch (align) {
			case Align.Right:
				ret.X = parent.Width - position.X - size.X - margin;
				break;
			case Align.Left:
				ret.X = position.X + margin;
				break;
			case Align.Center:
				ret.X = (parent.Width - size.X) / 2;
				break;
			}

			return ret;
		}

		public override void Paint()
		{
			throw new Exception();
		}

		public override void PaintOnScreen()
		{
			Sprites.Bind();
			Painter.EnableTextures();
			Painter.StartQuads();

			var glPosition = parent.windowToRealPos(windowPosition());

			if (type == Type.Close) {
				Painter.Sprite(glPosition, size, focus ? Sprites.Name.CloseCircleFocus : Sprites.Name.CloseCircle, layer + 0.5f);
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

