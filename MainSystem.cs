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

