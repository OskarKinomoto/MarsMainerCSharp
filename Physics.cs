/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;

namespace MarsMiner
{
	public class Physics
	{
		private Physics(){}

		public const float DragForceConst = .0008f;
		public const float FrictionForceConst = .7f;
		public const float GravityForce2 = -80f;
		public const float HeightForceConstant = .02f;

		public static readonly Vector2 GravityForce = new Vector2(0, GravityForce2);

		public static Vector2 DragForce(Vector2 robotVelocity) 
		{
			return new Vector2(
				-robotVelocity.X * Math.Abs(robotVelocity.X) * DragForceConst,
				-robotVelocity.Y * Math.Abs(robotVelocity.Y) * DragForceConst
			);
		}

		public static Vector2 HightForce(float height) 
		{
			if (height <= 0)
				return new Vector2();

			return new Vector2(0, -height * HeightForceConstant);
		}

		public static Vector2 FrictionForce(bool engineOn, Vector2 robotVelocity)
		{
			Vector2 frictionForce = new Vector2();
			if (engineOn && robotVelocity.Y == 0 && robotVelocity.X != 0) {
				frictionForce.X = -10 * Math.Sign(robotVelocity.X) *
					(float)Math.Pow(Math.Abs(robotVelocity.X), 1 / 2.0f)
					* Physics.FrictionForceConst * 2;
			}

			if (robotVelocity.Y == 0)
				frictionForce.X += -robotVelocity.X * Physics.FrictionForceConst; 
			
			return frictionForce;
		}

		public static Vector2 Forces(bool engineOn, Vector2 robotVelocity, float height)
		{
			return Physics.FrictionForce(engineOn, robotVelocity) + Physics.DragForce(robotVelocity) +
			Physics.HightForce(height) + Physics.GravityForce;
		}
	}
}

