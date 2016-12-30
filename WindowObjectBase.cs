using System;
using System.Drawing;

namespace MarsMiner
{
	public abstract class WindowObjectBase : PaintInterface
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

