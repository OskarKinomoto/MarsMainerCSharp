/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;

namespace MarsMiner
{
	abstract class Distribution
	{
		public Distribution()
		{
		}

		public abstract float Propability(float deep);
	}
}

