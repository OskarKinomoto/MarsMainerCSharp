/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;

namespace MarsMiner
{
	class TriangleDistribution : Distribution
	{
		private float start;
		private float stop;
		private float p;

		public TriangleDistribution(float start, float stop, float p)
		{
			this.start = start;
			this.stop = stop;
			this.p = p;
		}

		public override float Propability(float deep)
		{
			if (deep < start || deep > stop)
				return 0;

			deep -= start;
			var len = stop - start;

			if (deep > len / 2)
				deep = -(deep - len / 2);

			return deep / (len / 2) * p;
		}
	}
}

