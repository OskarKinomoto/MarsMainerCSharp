using System;
using System.Drawing;
using OpenTK;

namespace MarsMiner
{
	class Fuel : RobotStatusInterface
	{
		public enum Model
		{
			Standard
		}

		private Model model = Model.Standard;
		private float current = 500;

		private ProgressBar bar = new ProgressBar(100, 0);

		public static float Max(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return 500;
			}
		}

		public int Percentage()
		{
			return (int)(current / Max(model) * 100);
		}

		public void Use(float fuelUse)
		{
			current -= fuelUse;

			if (current <= 0)
				throw new GameOverException();

			bar.Set(Percentage());
		}

		public void Paint()
		{
			bar.Paint(new OpenTK.Vector2(-Controler.WindowWidth / 2 + 10, Controler.WindowHeight / 2 - 22),
				new OpenTK.Vector2(100, 12), 10, Color.Orange);
		}

		public float Missing()
		{
			return Max(model) - current;
		}

		public void Add(float fuel)
		{
			current += fuel;

			if (current > Max(model))
				current = Max(model);

			bar.Set(Percentage());
		}
	}
}
