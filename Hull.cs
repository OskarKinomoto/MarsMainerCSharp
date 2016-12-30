using System;
using System.Drawing;

namespace MarsMiner
{
	public class Hull : RobotStatusInterface
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

		public static float Endurance(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return 150;
			}
		}

		public static float HitMultiply(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return 1f;
			}
		}

		public Hull()
		{
			bar.Set(Percentage());
		}

		public int Percentage()
		{
			return (int)(current / Max(model) * 100);
		}

		public void Loose(float lifeAmount)
		{
			if (Preferences.GodMode)
				return;
			
			current -= lifeAmount;

			if (current <= 0)
				throw new GameOverException();

			bar.Set(Percentage());
		}

		public void LooseByVelocityChange(float begin, float end) 
		{
			var diff = begin - end - Endurance(model);

			if (diff > 0)
				Loose(diff * HitMultiply(model));
		}

		public void Paint()
		{
			bar.Paint(new OpenTK.Vector2(-Controler.WindowWidth / 2 + 10, Controler.WindowHeight / 2 - 22 - 30),
				new OpenTK.Vector2(100, 12), 10, Color.Red);
		}

		public float Missing()
		{
			return Max(model) - current;
		}

		public void Heal(float lifeAmount)
		{
			current += lifeAmount;

			if (current > Max(model))
				current = Max(model);

			bar.Set(Percentage());
		}
	}
}

