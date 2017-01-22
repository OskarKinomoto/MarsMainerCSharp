/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;

namespace MarsMiner
{
	interface MouseInterface
	{
		void Mouse(Vector2 position, Mouse.Action action);
	}
}

