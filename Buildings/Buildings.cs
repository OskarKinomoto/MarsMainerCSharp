﻿/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using System.Collections.Generic;

namespace MarsMiner
{
	class Buildings
	{
		private Buildings()
		{
		}

		public static List<Building> NewBuildings(Action onClose, Money m, Robot r)
		{
			var ret = new List<Building>();

			ret.Add(new GasStation(m,r));

			ret.Add(new Shop(m,r));

			ret.Add(new RepairShop(m,r));

			ret.Add(new EnhancementShop(m,r));

			foreach (var b in ret) {
				b.onClose += onClose;
			}

			return ret;
		}
	}
}

