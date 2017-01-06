using System;

namespace MarsMiner
{
	class Drill
	{
		public enum Model
		{
			Standard
		}

		private Model model = Model.Standard;

		float coolingState = 0;

		public static float DrillingSpeed(Model m)
		{
			if (Preferences.GodMode)
				return 5 * Tile.Size;
			
			switch (m) {
			case Model.Standard:
			default:
				return Tile.Size * 1.5f;
			}
		}

		public static float Cooling(Model m)
		{
			if (Preferences.GodMode)
				return 0;
			
			switch (m) {
			case Model.Standard:
			default:
				return .3f;
			}
		}

		public float DrillingSpeed()
		{
			return DrillingSpeed(model);
		}

		public float Cooling()
		{
			return Cooling(model);
		}

		public void Heated()
		{
			coolingState = Cooling();
		}

		public bool Coolded()
		{
			return coolingState == 0;
		}

		public void Cool(float tau)
		{
			coolingState -= tau;

			if (coolingState < 0)
				coolingState = 0;
		}


		public static float FuelUsage(Model m)
		{
			if (Preferences.GodMode)
				return 0;
			
			switch (m) {
			case Model.Standard:
			default:
				return 2f;
			}
		}

		public float FuelUsage()
		{
			return FuelUsage(model);
		}

		public const float MoveToHeatDrillSquared = 1f;
	}
}

