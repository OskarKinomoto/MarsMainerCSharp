/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;

namespace MarsMiner
{
	class Keyboard
	{
		private bool m_up = false;
		private bool m_down = false;
		private bool m_left = false;
		private bool m_right = false;
		private bool m_enter = false;
		private bool m_esc = false;
		private bool m_e = false;

		public void UpdateArrows(bool Up, bool Down, bool Left, bool Right)
		{
			if ((Up || Down || Left || Right) & onKeyboardActivation != null)
				onKeyboardActivation();

			if (Up && !m_up && onUp != null)
				onUp();

			if (Down && !m_down && onDown != null)
				onDown();
			
			m_up = Up;
			m_down = Down;
			m_left = Left;
			m_right = Right;
		}

		public void UpdateRest(bool Enter, bool Esc, bool E)
		{
			if ((Enter || Esc || E) & onKeyboardActivation != null)
				onKeyboardActivation();

			if (Enter && !m_enter && onEnter != null)
				onEnter();

			if (E && !m_e && onE != null)
				onE();

			if (Esc && !m_esc && onEsc != null)
				onEsc();

			m_enter = Enter; 
			m_e = E;
			m_esc = Esc;
		}

		public Engine.State EngineState()
		{
			var KeyboardArrows = new Vector2();
			if (m_up)
				KeyboardArrows.Y += 1;
			if (m_down)
				KeyboardArrows.Y -= 1;
			if (m_left)
				KeyboardArrows.X -= 1;
			if (m_right)
				KeyboardArrows.X += 1;
			
			return new Engine.State(KeyboardArrows.LengthSquared > 0, (float)Math.Atan2(KeyboardArrows.Y, KeyboardArrows.X));
		}

		public event Action onKeyboardActivation;
		public event Action onEnter;
		public event Action onE;
		public event Action onEsc;

		public event Action onUp;
		public event Action onDown;
	}
}

