/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MarsMiner
{
    class Sprites
    {
        public enum Name
        {
			None = -1,
            RobotLeft = 0,
			RobotRight = 1,
			TileFull = 2,
			Empty3 = 3,
			CloseCircle = 4,
			CloseCircleFocus = 5,
			ButtonLeft = 6,
			ButtonCenter = 7,
			ButtonRight = 8,
			ButtonFocusLeft = 9,
			ButtonFocusCenter = 10,
			ButtonFocusRight = 11,
			TileGold = 12,
			TileLapis = 13,
			TileSilver = 14,
			TileSaphire = 15,
			TilePlatinium = 16,
			TileCopper = 17,
			TileRubin = 18,
			TileNonBreakable1 = 19,
			TileNonBreakable2 = 20,
			TileNonBreakable3 = 21,
			TileNonBreakable4 = 22,
			TileSalt = 23,
			TileEmerald = 24,
			BuildingGasStation = 25, // – 30
			Shop = 31, // – 38
			RepairShop = 39, // – 44
			EnhancementShop = 45, // – 52
			TileNonBreakable5 = 53,
			TileNonBreakable6 = 54,
        }

        private const int size = 128;

		private static Textures spriteTextures;
		private static int x_range = 0;
		private static float x_size = 0;
		private static int y_range = 0;
		private static float y_size = 0;

        public static void Load()
        {
			spriteTextures = new Textures("sprite.png");
			x_range = spriteTextures.SquareSize.Width / size;
			y_range = spriteTextures.SquareSize.Height / size;
			x_size = 1.0f / x_range;
			y_size = 1.0f / y_range;
        }

        public static void Bind()
        {
            spriteTextures.Bind();
        }

		public static void TexCoord(Name sprite, int coord, int SpriteAdd = 0)
        {
			int i = ((int)sprite + SpriteAdd) % x_range;
			int j = ((int)sprite + SpriteAdd) / x_range;
            switch (coord)
            {
                case 0:
                    GL.TexCoord2(i * x_size, j * y_size);
                    break;
                case 1:
                    GL.TexCoord2((i + 1) * x_size, j * y_size);
                    break;
                case 2:
                    GL.TexCoord2((i + 1) * x_size, (j + 1) * y_size);
                    break;
                case 3:
                    GL.TexCoord2(i * x_size, (j + 1) * y_size);
                    break;
            }
        }
    }
}
