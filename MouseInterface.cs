using System;
using OpenTK;

namespace MarsMiner
{
	public interface MouseInterface
	{
		void Mouse(Vector2 position, Mouse.Action action);
	}
}

