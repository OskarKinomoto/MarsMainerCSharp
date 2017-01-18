using System;
using System.Collections.Generic;
using System.Drawing;

namespace MarsMiner
{
	abstract class WindowLayoutBase : WindowObjectBase
	{
		protected List<WindowOneItemLayout> objects = new List<WindowOneItemLayout>();

		public WindowLayoutBase(WindowObjectBase parent) : base(parent, (parent.getLayer()) + 0.01f, new Point(0,0), Align.Left)
		{
		}

		abstract public void Add(WindowObjectBase obj);

		abstract public void Add(WindowObjectBase obj, int x, int y);
	}
}

