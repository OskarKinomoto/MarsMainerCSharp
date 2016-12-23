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
			ret[0].onClose += onClose;

			ret.Add(new Shop(m,r));
			ret[1].onClose += onClose;

			return ret;
		}
	}
}

