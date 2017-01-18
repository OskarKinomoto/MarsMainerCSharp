/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;

namespace MarsMiner
{
	class Mouse
	{
		public enum Action {
			None,
			LeftClick,
		}

		private bool leftBtnClick = false;
		private bool leftBtn = false;
		private Vector2 position = new Vector2();
		private bool eventGen = false;

		public void Position(Vector2 position)
		{
			if (this.position != position) {
				this.position = position;

				eventGen = true;

				if (onMouseActive != null)
					onMouseActive();
			}
		}

		public Vector2 Position()
		{
			return position;
		}

		public void LeftBtn(bool state)
		{
			if (leftBtn && !state)
				leftBtnClick = true;

			leftBtn = state;
			eventGen = true;

			if (onMouseActive != null)
				onMouseActive();
		}

		public void ResetEvent()
		{
			eventGen = false;
			leftBtnClick = false;
		}

		public Action action()
		{
			return leftBtnClick ? Mouse.Action.LeftClick : Mouse.Action.None;
		}

		public bool Event()
		{
			return eventGen;
		}

		public event System.Action onMouseActive;
	}
}

