/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using System.Drawing;

namespace MarsMiner
{
	class GasStation : Building
	{
		Money m;
		Robot r;

		WindowButton fullRefill;
		WindowButton refill5;
		WindowButton refill10;
		WindowButton refill20;
		WindowButton refill50;

		public GasStation(Money m, Robot r) : base("Gas station", 3, 2, Sprites.Name.BuildingGasStation, -4)
		{
			this.m = m;
			this.r = r;

			Layer lastButtonLayer = Layer.StatusText;
			lastButtonLayer++;

			fullRefill = new WindowButton(new Point(0,300), WindowButton.Type.Normal, window, WindowButton.Align.Center, lastButtonLayer, "Full refill");
			fullRefill.Width(475);
			fullRefill.onClickEvent += () => {
				this.refill(1000);
			};
			window.Add(fullRefill);

			refill5 = new WindowButton(new Point(50,100), WindowButton.Type.Normal, window, WindowButton.Align.Left, lastButtonLayer, "5$ refill");
			refill5.Width(200);
			refill5.onClickEvent += () => {
				this.refill(5);
			};
			window.Add(refill5);

			refill10 = new WindowButton(new Point(50,100), WindowButton.Type.Normal, window, WindowButton.Align.Right, lastButtonLayer + 0.2f,  "10$ refill");
			refill10.Width(200);
			refill10.onClickEvent += () => {
				this.refill(10);
			};
			window.Add(refill10);

			refill20 = new WindowButton(new Point(50,200), WindowButton.Type.Normal, window, WindowButton.Align.Left, lastButtonLayer, "20$ refill");
			refill20.Width(200);
			refill20.onClickEvent += () => {
				this.refill(20);
			};
			window.Add(refill20);

			refill50 = new WindowButton(new Point(50,200), WindowButton.Type.Normal, window, WindowButton.Align.Right, lastButtonLayer + 0.2f,  "50$ refill");
			refill50.Width(200);
			refill50.onClickEvent += () => {
				this.refill(50);
			};
			window.Add(refill50);
		}

		private float pricePerUnit()
		{
			return .5f;
		}

		private void refill(int maxPrice)
		{
			if (maxPrice > m.Count())
				maxPrice = m.Count();
			
			var missing = r.FuelMissing();
			var priceToPayForAllMissing = pricePerUnit() * missing;
			if (priceToPayForAllMissing > maxPrice)
				priceToPayForAllMissing = maxPrice;

			r.FuelAdd(priceToPayForAllMissing / pricePerUnit());

			m.Use((int)Math.Ceiling(priceToPayForAllMissing));
		}
	}
}

