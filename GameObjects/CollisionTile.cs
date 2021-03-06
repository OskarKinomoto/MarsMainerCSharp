﻿/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;

namespace MarsMiner
{
	class CollisionTile : IComparable<CollisionTile>
	{
		public enum Position {
			Bottom = 0,
			Left = 1,
			Top = 2,
			Right = 3,
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

		public bool Colide(Vector2 robot)
		{
			switch (position) {
			case Position.Bottom:
				return robot.Y <= bottom;
			case Position.Left:
				return robot.X <= left;
			case Position.Top:
				return robot.Y >= top;
			case Position.Right:
				return robot.X >= right;
			}

			throw new Exception();
		}

		public int CompareTo(CollisionTile other)
		{
			return (int)position - (int)other.position;
		}
	}
}

