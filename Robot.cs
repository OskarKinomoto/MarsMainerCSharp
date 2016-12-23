using System;
using OpenTK;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace MarsMiner
{
	class Robot : PaintInterface
	{
		public enum Breaking
		{
			None,
			Left,
			Right,
			Down,
			Up,
		}

		private enum State
		{
			UserControled,
			Breaking,
		}

		private class PossibleBreaking
		{
			public bool LeftRight = false;
			public bool Down = false;

			public bool All { set { LeftRight = Down = value; } }
		}


		public Vector3 breakingTile = new Vector3(0, 0, 0);

		// geometry + painter
		public const float Size = Tile.Size * .8f;
		private Sprites.Name sprite = Sprites.Name.RobotRight;

		// physics
		public Vector2 m_position = new Vector2(0, 10);
		public Vector2 m_velocity = new Vector2(0, 0);
		public Vector2 forces = new Vector2(0, 0);
		public float mass = 1;

		// State
		private State state = State.UserControled;
		private PossibleBreaking possibleBreaking = new PossibleBreaking();
		private Mineral MineralToRecieve = null;

		// Robot "status"
		private Fuel fuel = new Fuel();
		private Cargo cargo = new Cargo();
		private Hull hull = new Hull();

		private Drill drill = new Drill();
		private Engine engine = new Engine();
		private MainSystem mainsystem = new MainSystem();

		private List<RobotStatusInterface> robotStatusInterface = new List<RobotStatusInterface>();

		// Constructor
		public Robot()
		{
			robotStatusInterface.AddRange(new RobotStatusInterface[]{ fuel, cargo, hull });
		}

		// Interfaceses to Status
		public int FuelPercentage()
		{
			return fuel.Percentage();
		}

		public float FuelMissing()
		{
			return fuel.Missing();
		}

		public void FuelAdd(float FuelAmount)
		{
			fuel.Add(FuelAmount);
		}

		public List<Mineral> Minerals()
		{
			return cargo.Minerals();
		}

		public void MineralsClean()
		{
			cargo.Clean();
		}
		// END of Interfaces to Status

		// Paint interface
		public void Paint()
		{
			Sprites.Bind();
			Painter.EnableTextures();
			Painter.StartQuads();
			Painter.Sprite(m_position, new Vector2(Size, Size), sprite, Layer.Robot);
			Painter.Stop();
		}

		public void PaintOnScreen()
		{
			foreach (var status in robotStatusInterface)
				status.Paint();
		}
		// END of Paint Interface

		public void SetStateToUser()
		{
			state = State.UserControled;
		}

		public void SetMineralToRecieve(Mineral m)
		{
			MineralToRecieve = m;
		}

		public bool IsBreaking()
		{
			return state == State.Breaking;
		}

		// TICKS
		public void Tick(float tau, Vector3[] collisionTiles, Breaking user, out Breaking b)
		{
			b = Breaking.None;

			fuel.Use(mainsystem.FuelUse() * tau);

			switch (state) {

			case State.UserControled:
				TickUser(tau, collisionTiles, out b, user);
				break;

			case State.Breaking:
				TickBreaking(tau);
				break;
			}
		}

		private void TickBreaking(float tau)
		{
			fuel.Use(drill.FuelUsage() * tau);

			var tileX = breakingTile.X * Tile.Size + (Tile.Size - Size) / 2;
			var tileY = (breakingTile.Y - 1) * Tile.Size;

			var d = new Vector2(tileX - m_position.X, tileY - m_position.Y);
			var dist = d.Length;

			var move = drill.DrillingSpeed() * tau;

			if (dist <= move) {
				m_position = new Vector2(tileX, tileY);
				state = State.UserControled;
				breakingTile = new Vector3(0, 0, 0);

				if (MineralToRecieve != null) {
					cargo.Add(MineralToRecieve);
					MineralToRecieve = null;
				}

			} else {
				m_position += d * move / dist;
			}
		}

		private void TickUser(float tau, Vector3[] collisionTiles, out Breaking isBreaking, Breaking userMovement)
		{
			isBreaking = Breaking.None;

			switch (userMovement) {
			case Breaking.Left:
				sprite = Sprites.Name.RobotLeft;
				break;
			case Breaking.Right:
				sprite = Sprites.Name.RobotRight;
				break;
			}

			// User move
			if (userMovement != Breaking.None) {
				fuel.Use((float)(engine.FuelUse() * tau));
			}

			bool wasMovingAtBeginOfTick = m_velocity.LengthSquared > 0;

			// forces -> velocity
			m_velocity += forces * tau / mass;

			if (Math.Abs(m_velocity.Y) < 1e-3 && userMovement != Breaking.Down)
				m_velocity.Y = 0;

			if (Math.Abs(m_velocity.Y) > 1e-1)
				possibleBreaking.LeftRight = false;
			
			if (Math.Abs(m_velocity.X) > 5)
				possibleBreaking.Down = false;

			// velocity -> position
			m_position += m_velocity * tau;
            
			// Stop at map endings
			if (m_position.X < Model.MinTileX) {
				m_position.X = Model.MinTileX;
				m_velocity.X = 0;
			}

			if (m_position.X + Robot.Size > Model.MaxTileX) {
				m_position.X = Model.MaxTileX - Robot.Size;
				m_velocity.X = 0;
			}

			float VelocityBeforeCollisions = m_velocity.Length;

			// Sort collision tiles so foreach magick algorithm will work as intended
			Array.Sort(collisionTiles, (Vector3 x, Vector3 y) => x.Z == y.Z ? 0 : (x.Z > y.Z ? 1 : -1));

			// Magick algoithm – collisions and tile breaking
			// TODO carefully refactor!! git etc to not break THE MAGICK!!
			foreach (Vector3 tile in collisionTiles) {
				// Test if bottom tile
				if (Math.Floor(tile.Z) == 0) {
					var down = tile.Y * Tile.Size;
					if (m_position.Y <= down + 1) {

						if (possibleBreaking.Down && userMovement == Breaking.Down && tile.Z == 0) {
							if (isBreaking == Breaking.None && m_velocity.Y != 0) {
								isBreaking = Breaking.Down;
								possibleBreaking.Down = false;
							}
						} else {
							possibleBreaking.All = true;
							m_position.Y = down + 1;
							m_velocity.Y = 0;
						}
					}
				}

				if (Math.Floor(tile.Z) == 1) { // left
					var left = (tile.X + 1) * Tile.Size;
					if (m_position.X <= left + 1) {
						if (possibleBreaking.LeftRight && userMovement == Breaking.Left && tile.Z == 1) {
							isBreaking = Breaking.Left;
						} else {
							m_position.X = left + 1;
							m_velocity.X = 0;
						}
					}
				}

				if (Math.Floor(tile.Z) == 2) { // up
					var top = (tile.Y - 1) * Tile.Size - Size;
					if (m_position.Y >= top - 1) {
						m_position.Y = top - 1;
						m_velocity.Y = 0;
					}
				}

				if (Math.Floor(tile.Z) == 3) { // right
					var right = (tile.X) * Tile.Size - Size;
					if (m_position.X >= right - 1) {
						if (possibleBreaking.LeftRight && userMovement == Breaking.Right && tile.Z == 3) {
							isBreaking = Breaking.Right;
						} else {
							m_position.X = right - 1;
							m_velocity.X = 0;
						}
					}
				}
			}

			if (isBreaking != Breaking.None) {
				state = State.Breaking;
				m_velocity = new Vector2();
			}

			if (wasMovingAtBeginOfTick)
				hull.LooseByVelocityChange(VelocityBeforeCollisions, m_velocity.Length);
		}

		// END TICKS

		// Geometry
		public Vector2 SelectBottomTile(Vector2[] tiles)
		{
			if (tiles.Length == 1) {
				return tiles[0];
			}

			var robotCenter = Center();
			var x01 = (tiles[0].X + Model.MinX) * Tile.Size;
			var x02 = (tiles[0].X + 1 + Model.MinX) * Tile.Size;
			if (x01 < robotCenter.X && robotCenter.X < x02) {
				return tiles[0];
			} else {
				return tiles[1];
			}
		}

		public Vector2[] GetVerticies()
		{
			Vector2[] ret = new Vector2[4];

			ret[0] = m_position;
			ret[1] = new Vector2(m_position.X, m_position.Y + Size);
			ret[2] = new Vector2(m_position.X + Size, m_position.Y);
			ret[3] = new Vector2(m_position.X + Size, m_position.Y + Size);

			return ret;
		}

		public Vector2 Center()
		{
			return new Vector2(m_position.X + Size / 2, m_position.Y + Size / 2);
		}
	}
}
