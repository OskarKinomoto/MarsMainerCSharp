using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MarsMiner
{
	class Model
	{
		// size constants
		private const int MapXSize = 40;
		public const int MinX = -MapXSize / 2;
		public const int MaxX = MapXSize / 2 - 1;
		public const int MaxYDepth = 1000;

		public const int MinTileX = MinX * Tile.Size;
		public const int MaxTileX = MaxX * Tile.Size;

		// members
		public Camera camera;
		private Money money;
		public Tiles tiles;
		public Vector2 workingForces;
		public Robot robot;
		public bool GameOver;
		private List<Building> buildings;

		public void NewGame(Action onClose)
		{
			camera = new Camera();
			robot = new Robot();
			workingForces = new Vector2(0, 0);
			money = new Money();
			GameOver = false;
			buildings = Buildings.NewBuildings(onClose, money, robot);
			tiles = new Tiles(buildings);
			camera.UpdatePosition(robot);
		}

		public void LoadGame()
		{
			// TODO
		}

		public void SaveGame()
		{
			// TODO
		}

		public Money Money {
			get {
				return money;
			}
		}

		public List<Building> GetBuildings {
			get {
				return buildings;
			}
		}

		public bool testGameOver()
		{
			return GameOver;
		}

		private Tile TileToBreak(Vector2[] tilesOnRobot, Robot.Breaking robotStatusBreking)
		{
			var robotTilePosition = tilesOnRobot[0];

			Func<Vector2, int, int, Tile> robotTilePosToTile = (vec, x, y) => {
				return tiles[(int)vec.X + x, (int)vec.Y + y];
			};

			Tile tileToBreak = new Tile(0, 0, false);
			switch (robotStatusBreking) {
			case Robot.Breaking.Left:
				tileToBreak = robotTilePosToTile(robotTilePosition, -1, 0);
				break;
			case Robot.Breaking.Right:
				tileToBreak = robotTilePosToTile(robotTilePosition, 1, 0);
				break;
			case Robot.Breaking.Down:
				robotTilePosition = robot.SelectBottomTile(tilesOnRobot);
				tileToBreak = robotTilePosToTile(robotTilePosition, 0, 1);
				break;
			}

			return tileToBreak;
		}

		private void ProcessBreakingTile(Vector2[] tilesOnRobot, Robot.Breaking robotStatusBreking, bool robotOldStatus)
		{
			Tile tileToBreak = TileToBreak(tilesOnRobot, robotStatusBreking);

			if (tileToBreak.exists) {
				if (tileToBreak.breakable) {
					robot.breakingTile = tileToBreak.setCollision();
					robot.SetMineralToRecieve(tileToBreak.GetMineral());
				} else {
					robot.SetStateToUser();
				}
			} else if (!robotOldStatus && robot.IsBreaking()) {
				robot.SetStateToUser();
			}
		}

		public void tick(float tau)
		{
			try {
				float angle = (float)Math.Atan2(workingForces.Y, workingForces.X);
				robot.SetEngine(workingForces.LengthSquared > 0, angle);

				// ---------- TILE BREAKING ------------

				var tilesOnRobot = tiles.TilesOnRobot(robot);
				var collisionTiles = tiles.CollisionTiles(tilesOnRobot);

				Robot.Breaking robotStatusBreaking = Robot.Breaking.None;
				var breakingTileOld = robot.breakingTile;

				var robotOldStatus = robot.IsBreaking();

				robot.Tick(tau, collisionTiles, out robotStatusBreaking);

				if (!breakingTileOld.destroyed && robot.breakingTile.destroyed)
					breakingTileOld.tile.destroy();

				ProcessBreakingTile(tilesOnRobot, robotStatusBreaking, robotOldStatus);

			} catch (GameOverException) {
				GameOver = true;
			}

			// ---------- CAMERA ------------
			camera.UpdatePosition(robot);
		}
	}
}
