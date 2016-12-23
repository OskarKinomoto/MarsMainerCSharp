using OpenTK;
using System;
using System.Collections.Generic;

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


		public void tick(float tau)
		{
			try {
				Robot.Breaking userBreaking = Robot.Breaking.None;

				// ---------- PHYSICS ------------

				// TODO switch to by angle breaking recoginition
				if (workingForces.Y < 0)
					userBreaking = Robot.Breaking.Down;

				if (workingForces.Y > 0)
					userBreaking = Robot.Breaking.Up;

				if (workingForces.X > 0)
					userBreaking = Robot.Breaking.Right;
            
				if (workingForces.X < 0)
					userBreaking = Robot.Breaking.Left;


				Vector2 frictionForce = new Vector2();
				if (workingForces.X == 0 && robot.m_velocity.Y == 0 && robot.m_velocity.X != 0) {
					frictionForce.X = -10 * Math.Sign(robot.m_velocity.X) * 
						(float)Math.Pow(Math.Abs(robot.m_velocity.X), 1 / 2.0f) 
						* Physics.FrictionForceConst;
				}

				if (robot.m_velocity.Y == 0)
					frictionForce.X += -robot.m_velocity.X * Physics.FrictionForceConst; 

				robot.forces = workingForces + frictionForce + Physics.DragForce(robot.m_velocity) + Physics.GravityForce;

				// ---------- TILE BREAKING ------------

				var tilesOnRobot = tiles.TilesOnRobot(robot);
				var collisionTiles = tiles.CollisionTiles(tilesOnRobot);


				Robot.Breaking robotStatusBreking = Robot.Breaking.None;
				var breakingTileOld = robot.breakingTile;

				var robotOldStatus = robot.IsBreaking();

				robot.Tick(tau, collisionTiles, userBreaking, out robotStatusBreking);

				if (breakingTileOld.Z != 0 && robot.breakingTile.Z == 0) {
					tiles[(int)breakingTileOld.X - MinX, -(int)breakingTileOld.Y].destroy();
				}

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

				if (tileToBreak.exists) {
					if (tileToBreak.breakable) {
						tileToBreak.setCollision(ref robot.breakingTile);
						robot.SetMineralToRecieve(tileToBreak.GetMineral());
					} else {
						robot.SetStateToUser();
					}
				} else if (!robotOldStatus && robot.IsBreaking()) {
					robot.SetStateToUser();
				}

			} catch (GameOverException) {
				GameOver = true;
			}

			// ---------- CAMERA ------------
			camera.UpdatePosition(robot);
		}
	}
}
