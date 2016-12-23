using System;

namespace MarsMiner
{
	class Engine
	{
		public enum Model 
		{
			Standard
		}

		private Model model = Model.Standard;


		public static float UpSpeed(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return 180;
			}
		}

		public float UpSpeed() {
			return UpSpeed(model);
		}

		public static float DownSpeed(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return 100;
			}
		}

		public float DownSpeed() {
			return DownSpeed(model);
		}

		public static float HorizontalSpeed(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return 200;
			}
		}

		public float HorizontalSpeed() {
			return HorizontalSpeed(model);
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
	}
}

