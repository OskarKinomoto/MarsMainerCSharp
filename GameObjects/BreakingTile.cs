/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;

namespace MarsMiner
{
	class BreakingTile
	{
		public Tile tile;
		public bool destroyed = true;
		public Vector2 position;

		public BreakingTile() {}

		public BreakingTile(Tile tile = null)
		{
			this.tile = tile;
			this.destroyed = false;
			positionForRobot();
		}

		private void positionForRobot()
		{
			position = new Vector2(tile.PosX * Tile.Size + (Tile.Size - Robot.Size) / 2, (tile.PosY - 1) * Tile.Size + 1);
		}
		
	}
}

