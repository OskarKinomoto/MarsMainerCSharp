/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MarsMiner
{
    class Menu
    {
        private const int margin = 10;

        private int Position = 0;
        private int MenuElements = 0;

		private List<Tuple<Textures, Textures, Action>> MenuTextureElements = new List<Tuple<Textures, Textures, Action>>();
        private Textures MenuTitle;

        private float nextMenuItemPos;

        public Menu(String title)
        {
            MenuTitle = Textures.Text(title, Textures.FontName.Tahoma, 30);
        }

        public void up()
        {
            if (Position != 0)
                Position--;
        }

        public void down()
        {
            if (Position + 1 < MenuElements)
                Position++;
        }

        public void Enter()
        {
			MenuTextureElements[Position].Item3();
        }

        public void Reset()
        {
            Position = 0;
		}

		public void AddItem(String s, Action a)
		{
			MenuElements++;

			var tex = Textures.Text(s, Textures.FontName.Tahoma, 18);
			var tex2 = Textures.Text(s, Textures.FontName.Tahoma, 18, System.Drawing.Brushes.White);
			MenuTextureElements.Add(Tuple.Create(tex, tex2, a));
		}

        public void Paint()
        {
			Painter.EnableTextures();
            float layer = 10;

            // title
            var menuPosition = new Vector2(-MenuTitle.TextSize.Width / 2, Controler.WindowHeight / 2 - MenuTitle.SquareSize.Height - 50);

            MenuTitle.Bind();

            Painter.Square(menuPosition, new Vector2(MenuTitle.SquareSize.Width, MenuTitle.SquareSize.Height), layer);

            // elements
            nextMenuItemPos = 100 + MenuTitle.SquareSize.Height;

            for (int i = 0; i < MenuElements; ++i)
            {
                layer += 1e-2f;
                Textures tex;

                if (i == Position)
					tex = MenuTextureElements[i].Item2;
                else
					tex = MenuTextureElements[i].Item1;

                var elementPosition = new Vector2(-tex.TextSize.Width / 2,
                    Controler.WindowHeight / 2 - tex.SquareSize.Height - nextMenuItemPos);

                if (i == Position)
                {
					Painter.DisableTextures();

					Painter.Color(0.1f, 0.1f, 0.1f);

                    var elementPosition2 = new Vector2(-Controler.WindowWidth / 2,
						Controler.WindowHeight / 2 - tex.SquareSize.Height / 2 - nextMenuItemPos - (View.IsRunningOnMono() ? 0 : margin / 2));

                    Painter.Square(elementPosition2, new Vector2(Controler.WindowWidth, tex.TextSize.Height + margin), layer - 1e-3f);

					Painter.EnableTextures();
                }

                tex.Bind();
                Painter.Square(elementPosition, new Vector2(tex.SquareSize.Width, tex.SquareSize.Height), layer);
                nextMenuItemPos += tex.TextSize.Height + margin;
            }
        }
    }
}
