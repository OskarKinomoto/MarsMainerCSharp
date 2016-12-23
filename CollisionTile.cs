﻿using System;

namespace MarsMiner
{
	class CollisionTile : IComparable<CollisionTile>
	{
		public enum Position {
			Left = 1,
			Right = 3,
			Top = 2,
			Bottom = 0,
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
				return tile.PosX * Tile.Size - Robot.Size - 1;
			}
		}

		public float left {
			get {
				return (tile.PosX + 1) * Tile.Size + 1;
			}
		}

		public float top {
			get {
				return (tile.PosY - 1) * Tile.Size - Robot.Size - 1;
			}
		}

		public float bottom {
			get {
				return tile.PosY * Tile.Size + 1;
			}
		}

		public int CompareTo(CollisionTile other)
		{
			return position == other.position ? 0 : ((int)position > (int)other.position ? 1 : -1);
		}
	}
}

