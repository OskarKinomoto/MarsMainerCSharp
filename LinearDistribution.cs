using System;

namespace MarsMiner
{
	class LinearDistribution : Distribution
	{
		private float p;

		public LinearDistribution(float p)
		{
			this.p = p;
		}

		public override float Propability(float deep)
		{
			return p;
		}
	}
}

