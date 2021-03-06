﻿/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;
using System.Diagnostics;
using System.Drawing;

namespace MarsMiner
{
	class WindowButton : WindowObjectBase, MouseInterface
	{
		public enum Type {
			Normal,
			Close,
		}

		private Type type;

		public event Action onClickEvent;
		private float margin = 5;

		private Textures textTexture;

		private bool focus = false;

		public WindowButton(Point position, Type type, WindowObjectBase parent, Align align, Layer layer, string s = "")
			: base(parent, layer, position, align)
		{
			this.type = type;
			this.parent = parent;

			if (type != Type.Close) {
				textTexture = Textures.Text(s, Textures.FontName.Tahoma, 12);
				size = new Vector2(textTexture.TextSize.Width + 20, 30);
			} else {
				size = new Vector2(20);
			}
		}

		public void Width(float width)
		{
			size.X = 20 + width;
		}

		override public Vector2 ObjectToGlPosition(Vector2 pos)
		{
			Vector2 ret = new Vector2(0, position.Y + size.Y + margin);

			switch (align) {
			case Align.Right:
				ret.X = parent.Width() - position.X - size.X - margin;
				break;
			case Align.Left:
				ret.X = position.X + margin;
				break;
			case Align.Center:
				ret.X = (parent.Width() - size.X) / 2;
				break;
			}

			return parent.ObjectToGlPosition(ret);
		}

		public override void PaintOnScreen()
		{
			Sprites.Bind();
			Painter.EnableTextures();
			Painter.StartQuads();

			var glPosition = ObjectToGlPosition(InWindowPosition());

			if (type == Type.Close) {
				Painter.Sprite(glPosition, size, focus ? Sprites.Name.CloseCircleFocus : Sprites.Name.CloseCircle, layer + 0.5f);
				Painter.Stop();
			} else {
				Painter.Sprite(glPosition + new Vector2(size.Y, size.Y), size - new Vector2(size.Y, size.Y) * 2, focus ? Sprites.Name.ButtonFocusCenter : Sprites.Name.ButtonCenter, layer);
				Painter.Sprite(glPosition, new Vector2(size.Y, size.Y), focus ? Sprites.Name.ButtonFocusLeft : Sprites.Name.ButtonLeft, layer);
				Painter.Sprite(glPosition + size - new Vector2(size.Y, size.Y), new Vector2(size.Y, size.Y), focus ? Sprites.Name.ButtonFocusRight : Sprites.Name.ButtonRight, layer);
				Painter.Stop();

				textTexture.Bind();
				Vector2 TextSize = new Vector2(textTexture.SquareSize.Width, textTexture.SquareSize.Height);
				var textPos = glPosition - new Vector2(- (size.X - textTexture.TextSize.Width) / 2, size.Y + (textTexture.TextSize.Height) / 2.0f );
				Painter.Square(textPos, TextSize, layer + 0.03f);
			}
		}		

		private Vector2 InObjectPosition(Vector2 position)
		{
			position -= InWindowPosition();
			position.Y += size.Y;
			return position;
		}

		private bool FocusTest(Vector2 inObjectPosition)
		{
			return 
				inObjectPosition.X >= 0 && inObjectPosition.X <= size.X
			&&
				inObjectPosition.Y >= 0 && inObjectPosition.Y <= size.Y;
		}

		public override void Mouse(Vector2 position, Mouse.Action action)
		{
			focus = FocusTest(ParentToObjectPosition(position));
			
			if (focus && action == MarsMiner.Mouse.Action.LeftClick) {
				if (onClickEvent != null)
					onClickEvent();
			}
		}
	}
}
