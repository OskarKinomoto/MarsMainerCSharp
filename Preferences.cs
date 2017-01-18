/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;

namespace MarsMiner
{
	class Preferences
	{
		private Preferences() {}

		public static void Load()
		{

		}

		public static bool GodMode = true;

		private static bool mineralsDev = false;
		private static bool cloudsDev = true;


		public static bool MineralsDeveloper {
			get {
				return mineralsDev;
			}
			set {
				mineralsDev = value;
			}
		}

		public static bool CloudsDeveloper {
			get {
				return cloudsDev;
			}
			set {
				cloudsDev = value;
			}
		}
	}
}

