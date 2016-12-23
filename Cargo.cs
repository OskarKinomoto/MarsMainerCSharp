using System;
using System.Collections.Generic;
using System.Drawing;

namespace MarsMiner
{
	class Cargo : RobotStatusInterface
	{
		List<Mineral> minerals = new List<Mineral>();

		public enum Model
		{
			Standard
		}

		private Model model = Model.Standard;

		private ProgressBar bar = new ProgressBar(100, 0);

		public static float max(Model m)
		{
			switch (m) {
			case Model.Standard:
			default:
				return 20;
			}
		}

		public int percentage()
		{
			return (int)(minerals.Count / max(model) * 100);
		}

		public bool Add(Mineral m)
		{
			if (minerals.Count == max(model))
				return false;

			minerals.Add(m);

			bar.Set(percentage());

			return true;
		}

		public void RemoveOne(Mineral mineral)
		{
			Mineral toremove = null;
			foreach (Mineral m in minerals) {
				if (m == mineral) {
					toremove = m;
					break;
				}
			}
			if (toremove != null)
				minerals.Remove(toremove);
			
			bar.Set(percentage());
		}

		public void Clean()
		{
			minerals.Clear();
			bar.Set(percentage());
		}

		public List<Mineral> Minerals()
		{
			return minerals;
		}

		public void Paint()
		{
			bar.Paint(new OpenTK.Vector2(Controler.WindowWidth / 2 - 10 - 100, Controler.WindowHeight / 2 - 22),
				new OpenTK.Vector2(100, 12), 10, Color.Aqua);
		}
	}
}

