/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

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

		public override void Mouse(OpenTK.Vector2 position, MarsMiner.Mouse.Action action)
		{
			base.Mouse(position, action);
		}
	}
}

