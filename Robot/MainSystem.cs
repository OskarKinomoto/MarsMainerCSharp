/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;

namespace MarsMiner
{
	public class MainSystem
	{
		public enum Model 
		{
			Standard
		}

		private Model model = Model.Standard;

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
	}
}

