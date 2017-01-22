/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using System.Drawing;
using OpenTK;

namespace MarsMiner
{
	abstract class WindowObjectBase : PaintInterface, MouseInterface
	{
		// Types
		public enum Align {
			Left,
			Center,
			Right,
		}

		// Members
		protected WindowObjectBase parent;
		public Layer layer;
		public Point position;
		protected Align align;
		protected Vector2 size;

		// Ctor
		public WindowObjectBase(WindowObjectBase parent, Layer layer, Point position, Align align)
		{
			this.parent = parent;
			this.layer = layer;
			this.position = position;
			this.align = align;
		}

		// Methods
		public void Paint()
		{
			throw new MissingMethodException();
		}

		public abstract void PaintOnScreen();

		// Geters
		public Layer getLayer()
		{
			return layer;
		}

		public float Width()
		{
			return size.X;
		}

		virtual public Vector2 Size()
		{
			return size;
		}

		public float Height()
		{
			return size.Y;
		}

		protected Vector2 InWindowPosition() 
		{
			const int margin = 5;
			Vector2 ret = new Vector2(0, position.Y + 5);

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

			return ret;
		}

		public Vector2 ParentToObjectPosition(Vector2 pos)
		{
			return pos - InWindowPosition() + new Vector2(0, 0);
		}

		virtual public Vector2 ObjectToGlPosition(Vector2 pos)
		{
			return parent.ObjectToGlPosition(new Vector2(pos.X + position.X, pos.Y + position.Y));
		}

		virtual public void Mouse(Vector2 position, Mouse.Action action)
		{
		}
	}
}

