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
		protected Layer layer;
		protected Point position;
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

		public float Height()
		{
			return size.Y;
		}

		virtual public void Mouse(Vector2 position, Mouse.Action action)
		{
		}
	}
}

