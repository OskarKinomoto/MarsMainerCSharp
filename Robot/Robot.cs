﻿/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

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

			public Breaking breaking = Breaking.None;

			public bool IsDown {
				get {
					return breaking == Breaking.Down && Down;
				}
			}

			public bool IsLeft {
				get {
					return breaking == Breaking.Left && LeftRight;
				}
			}


			public bool IsRight {
				get {
					return breaking == Breaking.Right && LeftRight;
				}
			}

			public bool All { set { LeftRight = Down = value; } }
		}


		public BreakingTile breakingTile = new BreakingTile();

		// geometry + painter
		public const float Size = Tile.Size * .8f;
		private Sprites.Name sprite = Sprites.Name.RobotRight;

		// physics
		public Vector2 m_position = new Vector2(0, 10);
		public Vector2 velocity = new Vector2(0, 0);
		public float mass = 1;

		// State
		private State state = State.UserControled;
		private PossibleBreaking possibleBreaking = new PossibleBreaking();
		private Mineral MineralToRecieve = null;

		// Robot subsystems
		private Fuel fuel = new Fuel();
		private Cargo cargo = new Cargo();
		private Hull hull = new Hull();

		private Drill drill = new Drill();
		private Engine engine = new Engine();
		private MainSystem mainsystem = new MainSystem();

		private List<RobotStatusInterface> subsystemInterface = new List<RobotStatusInterface>();

		// Constructor
		public Robot()
		{
			subsystemInterface.AddRange(new RobotStatusInterface[]{ fuel, cargo, hull });
		}

		// Interfaceses to subsystems
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

		public float HealthMissing()
		{
			return hull.Missing();
		}

		public void Repair(float FuelAmount)
		{
			hull.Heal(FuelAmount);
		}

		public List<Mineral> Minerals()
		{
			return cargo.Minerals();
		}

		public void MineralsClean()
		{
			cargo.Clean();
		}

		public void UpgradeEngine()
		{
			engine.Upgrade();
		}

		public void SetEngine(bool running, float angle = 0)
		{
			engine.angle = angle;
			engine.running = running;
		}

		public void SetEngine(Engine.State state)
		{
			engine.angle = state.angle;
			engine.running = state.running;
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
			foreach (var status in subsystemInterface)
				status.Paint();
		}
		// END of Paint Interface

		public void SetStateToUser()
		{
			state = State.UserControled;
		}

		public void SetStateToBreaking()
		{
			state = State.Breaking;
			StopMoving();
		}

		public void SetMineralToRecieve(Mineral m)
		{
			MineralToRecieve = m;
		}

		public bool IsBreaking()
		{
			return state == State.Breaking;
		}

		public bool IsMoving()
		{
			return velocity.LengthSquared != 0;
		}

		public bool IsMovingVerticaly()
		{
			return velocity.X != 0;
		}

		public bool IsMovingHorizontaly()
		{
			return velocity.Y != 0;
		}

		private void StopMoving()
		{
			velocity = new Vector2();
		}

		private void RecieveMineral()
		{
			if (MineralToRecieve != null)
				cargo.Add(MineralToRecieve);
			
			MineralToRecieve = null;
		}

		private void SetHorizontalPosition(float X)
		{
			m_position.X = X;
			velocity.X = 0;
		}

		private void SetVerticalPosition(float Y)
		{
			m_position.Y = Y;
			velocity.Y = 0;
		}

		// TICKS
		public void Tick(float tau, CollisionTile[] collisionTiles, out Breaking b)
		{
			b = Breaking.None;

			fuel.Use(mainsystem.FuelUse() * tau);

			switch (state) {

			case State.UserControled:
				TickUser(tau, collisionTiles, out b);
				break;

			case State.Breaking:
				TickBreaking(tau);
				break;
			}
		}

		private void TickBreaking(float tau)
		{
			fuel.Use(drill.FuelUsage() * tau);

			var tile = new Vector2(breakingTile.tile.PosX * Tile.Size + (Tile.Size - Size) / 2, (breakingTile.tile.PosY - 1) * Tile.Size + 1);

			var d = tile - m_position;
			var dist = d.Length;

			var move = drill.DrillingSpeed() * tau;

			if (dist <= move) {
				m_position = tile;
				breakingTile = new BreakingTile();

				RecieveMineral();
				SetStateToUser();
				drill.Heated();

			} else {
				m_position += d * move / dist;
			}
		}

		private void TickUser(float tau, CollisionTile[] collisionTiles, out Breaking isBreaking)
		{
			drill.Cool(tau);
			bool canBreak = drill.Coolded() && engine.running;

			isBreaking = Breaking.None;

			possibleBreaking.breaking = engine.Breaking();

			bool wasMovingAtBeginOfTick = velocity.LengthSquared > 0;

			if (engine.running) {
				switch (engine.Breaking()) {
				case Breaking.Left:
					sprite = Sprites.Name.RobotLeft;
					break;
				case Breaking.Right:
					sprite = Sprites.Name.RobotRight;
					break;
				}

				fuel.Use((float)(engine.FuelUse() * tau));
			}

			// Physics
			Vector2 forces = engine.Force() + Physics.Forces(engine.running, velocity, m_position.Y);

			// forces -> velocity
			velocity += forces * tau / mass;

			if (Math.Abs(velocity.Y) < 1e-3 && engine.Breaking() != Breaking.Down)
				velocity.Y = 0;

			if (Math.Abs(velocity.Y) > 1e-1)
				possibleBreaking.LeftRight = false;
			
			if (Math.Abs(velocity.X) > 5)
				possibleBreaking.Down = false;

			var oldPosition = m_position;

			// velocity -> position
			m_position += velocity * tau;
            
			// Stop at map endings
			if (m_position.X < Model.MinTileX)
				SetHorizontalPosition(Model.MinTileX);

			if (m_position.X + Robot.Size > Model.MaxTileX)
				SetHorizontalPosition(Model.MaxTileX - Robot.Size);

			float VelocityBeforeCollisions = wasMovingAtBeginOfTick ? velocity.Length : 0;

			// Magick algoithm – collisions and tile breaking
			foreach (CollisionTile tile in collisionTiles) {
				if (!tile.Colide(m_position))
					continue;

				switch (tile.position) {

				case CollisionTile.Position.Bottom:
					if (possibleBreaking.IsDown && tile.breakable && canBreak) {
						if (isBreaking == Breaking.None && velocity.Y != 0) {
							isBreaking = Breaking.Down;
							possibleBreaking.Down = false;
						}
					} else {
						possibleBreaking.All = true;
						SetVerticalPosition(tile.bottom);
					}
					break;

				case CollisionTile.Position.Left:
					if (possibleBreaking.IsLeft && tile.breakable && canBreak)
						isBreaking = Breaking.Left;
					else
						SetHorizontalPosition(tile.left);
					break;

				case CollisionTile.Position.Top:
					SetVerticalPosition(tile.top);
					break;

				case CollisionTile.Position.Right:
					if (possibleBreaking.IsRight && tile.breakable && canBreak)
						isBreaking = Breaking.Right;
					else
						SetHorizontalPosition(tile.right);
					break;
				}
			}

			if (isBreaking != Breaking.None)
				SetStateToBreaking();
			else if (Math.Abs(oldPosition.LengthSquared - m_position.LengthSquared) > Drill.MoveToHeatDrillSquared)
				drill.Heated();

			if (wasMovingAtBeginOfTick) {
				var VelocityAfterCollisions = velocity.Length;
				hull.LooseByVelocityChange(VelocityBeforeCollisions, VelocityAfterCollisions);
			}
		}
		// END TICKS

		// Geometry
		public Vector2 SelectBottomTile(Vector2[] tiles)
		{
			if (tiles.Length == 1)
				return tiles[0];

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
