﻿using System;

namespace MarsMiner
{
	class Preferences
	{
		private Preferences() {}

		public static void Load()
		{

		}

		public static bool GodMode = true;

		private static bool mineralsDev = true;
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

