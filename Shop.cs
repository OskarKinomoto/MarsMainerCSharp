/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using System.Drawing;

namespace MarsMiner
{
	class Shop : Building
	{
		Money money;
		Robot robot;
		
		WindowButton sellAll;

		public Shop(Money money, Robot robot) : base("Shop", 4, 2, Sprites.Name.Shop, 3)
		{
			this.money = money;
			this.robot = robot;

			Layer lastButtonLayer = Layer.StatusText;
			lastButtonLayer++;

			sellAll = new WindowButton(new Point(0,300), WindowButton.Type.Normal, window, WindowButton.Align.Center, lastButtonLayer, "Sell All");
			sellAll.Width(475);
			sellAll.onClickEvent += SellAll;
			window.Add(sellAll);
		}

		private void SellAll()
		{
			foreach (Mineral m in robot.Minerals())
				money.Add(m.BasePrice());

			robot.MineralsClean();
		}
	}
}

