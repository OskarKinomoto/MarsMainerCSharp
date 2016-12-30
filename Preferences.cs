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

		private static bool mineralsDev = true;

		public static bool MineralsDeveloper {
			get {
				return mineralsDev;
			}
			set {
				mineralsDev = value;
			}
		}
	}
}

