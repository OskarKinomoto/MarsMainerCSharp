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

		public static float DrillingSpeed(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return Tile.Size * 3;
			}
		}

		public float DrillingSpeed()
		{
			return DrillingSpeed(model);
		}


		public static float FuelUsage(Model m)
		{
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
	}
}

