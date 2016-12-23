using System;

namespace MarsMiner
{
	class CollisionTile
	{
		public enum Position {
			Left,
			Right,
			Top,
			Bottom,
		}

		public Tile tile;
		public Position position;

		public CollisionTile(Tile tile, Position position)
		{
			this.tile = tile;
			this.position = position;
		}
		

		public bool breakable {
			get {
				return tile.breakable;
			}
		}

		public float right {
			get {
				return tile.PosX * Tile.Size - Robot.Size;
			}
		}

		public float left {
			get {
				return (tile.PosX + 1) * Tile.Size;
			}
		}

		public float top {
			get {
				return (tile.PosY - 1) * Tile.Size - Robot.Size;
			}
		}

		public float bottom {
			get {
				return tile.PosY * Tile.Size;
			}
		}

	}
}

