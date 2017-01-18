/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;

namespace MarsMiner
{
	class Engine
	{
		public enum Model 
		{
			Standard
		}

		private Model model = Model.Standard;

		public bool running = false;
		public float angle = 0;

		public static float UpForce(Model m)
		{
			if (Preferences.GodMode)
				return 500;
			
			switch (m) {
			case Model.Standard:
			default:
				return 180;
			}
		}

		public float UpForce() {
			return UpForce(model);
		}

		public static float DownForce(Model m)
		{
			if (Preferences.GodMode)
				return 500;
			
			switch (m) {
			case Model.Standard:
			default:
				return 100;
			}
		}

		public float DownForce() {
			return DownForce(model);
		}

		public static float HorizontalForce(Model m)
		{
			if (Preferences.GodMode)
				return 500;
			
			switch (m) {
			case Model.Standard:
			default:
				return 200;
			}
		}

		public float HorizontalForce() {
			return HorizontalForce(model);
		}

		public static float FuelUse(Model m)
		{			
			switch (m) {
			case Model.Standard:
			default:
				return 1;
			}
		}

		public float FuelUse() {
			return FuelUse(model);
		}

		public Vector2 Force() {
			Vector2 force = new Vector2(0, 0);

			if (running) {
				force.X = (float)Math.Cos(angle) * HorizontalForce();

				var y = (float)Math.Sin(angle);
				if (y > 0)
					force.Y = y * UpForce(); 
				else if(y < 0)
					force.Y = y * DownForce(); 
			}
			return force;
		}

		const float AngleMargin = (float)Math.PI / 12f;

		public Robot.Breaking Breaking() {
			if (-angle > -AngleMargin + Math.PI / 2 && -angle < AngleMargin + Math.PI / 2)
				return Robot.Breaking.Down;

			if (angle > -AngleMargin + Math.PI / 2 && angle < AngleMargin + Math.PI / 2)
				return Robot.Breaking.Up;

			if (angle < AngleMargin && angle > -AngleMargin)
				return Robot.Breaking.Right;

			if (angle < AngleMargin + Math.PI && angle > -AngleMargin + Math.PI)
				return Robot.Breaking.Left;

			return Robot.Breaking.None;
		}
	}
}

