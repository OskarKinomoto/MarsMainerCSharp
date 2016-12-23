using System;
using OpenTK;

namespace MarsMiner
{
	public class Physics
	{
		private Physics(){}

		public const float DragForceConst = .0008f;
		public const float FrictionForceConst = 1f;
		public const float GravityForce2 = -80f;
		public static readonly Vector2 GravityForce = new Vector2(0, GravityForce2);

		public static Vector2 DragForce(Vector2 robotVelocity) 
		{
			return new Vector2(
				-robotVelocity.X * Math.Abs(robotVelocity.X) * DragForceConst,
				-robotVelocity.Y * Math.Abs(robotVelocity.Y) * DragForceConst
			);
		}
	}
}

