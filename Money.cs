/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;

namespace MarsMiner
{
	public class Money : PaintInterface
	{
		private int money = 100;
		private Textures Texture;

		public int Count() {
			return money;
		}

		public void Use(int money)
		{
			if (money < 0)
				throw new ArgumentException();
			
			this.money -= money;
			TextureUpdate();
		}

		public void Add(int money)
		{
			if (money < 0)
				throw new ArgumentException();
			
			this.money += money;
			TextureUpdate();
		}

		private void TextureUpdate() {
			if (Texture != null)
				Texture.Delete();
			
			Texture = Textures.Text(money + "$", Textures.FontName.Tahoma, 12);
		}

		public void Paint()
		{
			throw new MissingMethodException();
		}

		public void PaintOnScreen()
		{
			if (Texture == null)
				TextureUpdate();

			Painter.Text(Texture, 0, Painter.Horizontal.Center, Layer.StatusText);
		}
	}
}

