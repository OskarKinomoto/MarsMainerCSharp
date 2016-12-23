using System;

namespace MarsMiner
{
	class BreakingTile
	{
		public Tile tile;
		public bool destroyed = false;

		public BreakingTile(Tile tile)
		{
			this.tile = tile;
		}
		
	}
}

