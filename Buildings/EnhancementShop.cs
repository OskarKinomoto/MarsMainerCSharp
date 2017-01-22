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

		WindowButton btn;

		public EnhancementShop(Money m, Robot r) : base("Enhancement Shop", 4, 2, Sprites.Name.EnhancementShop, -11)
		{
			this.m = m;
			this.r = r;

			var layout = new WindowOneItemLayout(window, window.position + new System.Drawing.Size(0, Window.WindowBarWidth));
			window.Add(layout);

			btn = new WindowButton(new System.Drawing.Point(0,0), WindowButton.Type.Normal, layout, WindowObjectBase.Align.Center, layout.getLayer() + 0.2f, "Upgrade to better one");
			btn.onClickEvent += Upgrade;
			layout.Add(btn);
		}

		private void Upgrade()
		{
			if (m.Count() < 1000)
				return;

			r.UpgradeEngine();

			m.Use(1000);
		}
	}
}

