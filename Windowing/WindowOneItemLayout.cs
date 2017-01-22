/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;
using System.Drawing;

namespace MarsMiner
{
	class WindowOneItemLayout : WindowLayoutBase
	{
		private WindowObjectBase item;

		public WindowOneItemLayout(WindowObjectBase parent, Point position) : base(parent)
		{
			this.position = position;
		}

		public override void Add(WindowObjectBase obj)
		{
			if (item != null)
				throw new Exception();

			obj.layer = layer;
			item = obj;
		}

		public override void Add(WindowObjectBase obj, int x, int y)
		{
			throw new Exception();
		}

		public override void PaintOnScreen()
		{
			if (item == null)
				return;
			
			item.PaintOnScreen();
		}

		public override void Mouse(Vector2 position, MarsMiner.Mouse.Action action)
		{
			if (item == null)
				return;

			item.Mouse(ParentToObjectPosition(position), action);
		}
	}
}

