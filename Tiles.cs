using OpenTK;
using System;
using System.Collections.Generic;

namespace MarsMiner
{
	class Tiles
	{
		private Tile[,] tiles = new Tile[1, 1];

		public Tile this [int x, int y] {
			set {
				tiles[x, y] = value;
			}
			get {
				return tiles[x, y];
			}
		}

		public Tiles(List<Building> bs)
		{
			int XSize = Model.MaxX - Model.MinX;
			tiles = new Tile[XSize, Model.MaxYDepth];

			for (int x = 0; x < XSize; x++)
				for (int y = 0; y < Model.MaxYDepth; y++)
					tiles[x, y] = new Tile(Model.MinX + x, -y);

			// Nonbreakable under buildings :)
			foreach (var b in bs) {
				var left = (int)Math.Floor(b.left() / Tile.Size);
				var right = (int)Math.Floor(b.right() / Tile.Size);

				for (int x = left; x <= right; ++x)
					tiles[x - Model.MinX, 0].breakable = false;
			}
		}

		public void DrawTiles(Vector2 cam)
		{
			var yStart = (int)((-cam.Y - Controler.WindowHeight / 2) / Tile.Size - 1);
			var yEnd = (int)Math.Floor((-cam.Y + Controler.WindowHeight / 2) / Tile.Size + 1);
			var xStart = (int)((-cam.X - Controler.WindowWidth / 2) / Tile.Size - 1);
			var xEnd = (-cam.X + Controler.WindowWidth / 2) / Tile.Size + 1;

			// Tiles only underground
			if (yEnd > 0)
				yEnd = 0;

			if (yStart <= -Model.MaxYDepth)
				yStart = -Model.MaxYDepth - 1;

			// Tiles only betwin MinX – MaxX
			if (xEnd > Model.MaxX)
				xEnd = Model.MaxX;

			if (xStart < Model.MinX)
				xStart = Model.MinX;
			
			Painter.EnableTextures();
			Sprites.Bind();

			Painter.StartQuads();

			for (int x = xStart; x < xEnd; ++x)
				for (int y = yStart; y <= yEnd; ++y)
					tiles[x - Model.MinX, -y].drawTile();
			
			Painter.Stop();
		}

		private bool CheckTileCords(int xx, int yy)
		{
			return xx >= 0 &&
				yy >= 0 &&
				xx < tiles.GetLength(0) &&
				yy < tiles.GetLength(1);
		}

		private Vector2[] PossibleTilesOnRobot(Vector2 robotPosition)
		{
			var retList = new List<Vector2>();

			var xs = robotPosition.X / Tile.Size;
			var ys = robotPosition.Y / Tile.Size;

			var yy = (int)Math.Ceiling(ys * -1) - 1;
			var xx = (int)Math.Floor(xs) - Model.MinX;

			Action<int, int> Add = (x, y) => {
				retList.Add(new Vector2(x, y));
			};

			Add(xx, yy);
			Add(xx + 1, yy);
			Add(xx, yy - 1);
			Add(xx + 1, yy - 1);

			return retList.ToArray();
		}

		public Vector2[] TilesOnRobot(Robot r)
		{
			List<Vector2> ret = new List<Vector2>();

			var PossibleTiles = PossibleTilesOnRobot(r.m_position);
			var RobotVertices = r.GetVerticies();
			foreach (Vector2 tile in PossibleTiles) {
				Vector2 tileReal = new Vector2(tile.X + Model.MinX, -tile.Y - 1);

				bool isAnyVertexInTile = false;
				foreach (Vector2 RobotVertex in RobotVertices) {
					if (Tile.vertexInTile(tileReal, RobotVertex)) {
						isAnyVertexInTile = true;
						break;
					}
				}

				if (isAnyVertexInTile)
					ret.Add(tile);
			}

			return ret.ToArray();
		}

		public CollisionTile[] CollisionTiles(Vector2[] robotOnTiles)
		{
			List<CollisionTile> ret = new List<CollisionTile>();

			Action<int, int, CollisionTile.Position> Add = (int xx, int yy, CollisionTile.Position pos) => {
				if (CheckTileCords(xx, yy) && tiles[xx, yy].collision)
					ret.Add(new CollisionTile(tiles[xx, yy], pos));
			};

			foreach (Vector2 tile in robotOnTiles) {
				int xx = (int)tile.X;
				int yy = (int)tile.Y;

				// right
				Add(xx + 1, yy, CollisionTile.Position.Right);

				// left
				Add(xx - 1, yy, CollisionTile.Position.Left);

				// down
				Add(xx, yy + 1, CollisionTile.Position.Bottom);

				// up
				Add(xx, yy - 1, CollisionTile.Position.Top);
			}

			return ret.ToArray();

		}
	}
}
