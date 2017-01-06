using System;
using System.Drawing;
using OpenTK;

namespace MarsMiner
{
	abstract class WindowObjectBase : PaintInterface
	{
		// Types
		public enum Align {
			Left,
			Center,
			Right,
		}

		// Members
		protected Window parent;
		protected Layer layer;
		protected Point position;
		protected Align align;
		protected Vector2 size;

		// Ctor
		public WindowObjectBase(Window parent, Layer layer, Point position, Align align)
		{
			this.parent = parent;
			this.layer = layer;
			this.position = position;
			this.align = align;
		}

		// Methods
		public abstract void Paint();
		public abstract void PaintOnScreen();
	}
}

