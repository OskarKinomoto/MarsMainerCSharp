using System;

namespace MarsMiner
{
	class TrapezoidDistribution : Distribution
	{
		private float start;
		private float a;
		private float b;
		private float stop;
		private float p;

		public TrapezoidDistribution(float start, float a, float b, float stop, float p)
		{
			this.start = start;
			this.a = a;
			this.b = b;
			this.stop = stop;
			this.p = p;
		}

		public override float Propability(float deep)
		{
			if (deep < start || deep > stop)
				return 0;

			if (deep > a && deep < b)
				return p;

			deep -= start;
			var len = stop - start;

			if (deep > len / 2)
				deep = -(deep - len / 2);

			return deep / ((a - start) / 2) * p;
		}
	}
}

