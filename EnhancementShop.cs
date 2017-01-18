/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;

namespace MarsMiner
{
	class EnhancementShop : Building
	{
		Money m;
		Robot r;

		public EnhancementShop(Money m, Robot r) : base("Enhancement Shop", 4, 2, Sprites.Name.EnhancementShop, -11)
		{
			this.m = m;
			this.r = r;
		}
	}
}

