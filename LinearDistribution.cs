/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

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

