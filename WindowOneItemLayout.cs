/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;

namespace MarsMiner
{
	class WindowOneItemLayout : WindowLayoutBase
	{
		private WindowObjectBase item;

		public WindowOneItemLayout(WindowObjectBase parent) : base(parent)
		{
		}

		public override void Add(WindowObjectBase obj)
		{
			if (item != null)
				throw new Exception();

			item = obj;
		}

		public override void Add(WindowObjectBase obj, int x, int y)
		{
			throw new Exception();
		}

		public override void PaintOnScreen()
		{
			item.PaintOnScreen();
		}
	}
}

