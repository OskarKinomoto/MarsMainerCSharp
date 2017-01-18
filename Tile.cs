using OpenTK;
using System;

namespace MarsMiner
{
	class Tile
	{
		static Random rnd = new Random();

		// consts
		public const int Size = 60;

		// members
		public readonly int PosX;
		public readonly int PosY;
		public readonly Vector2 GamePosition;

		private enum State
		{
			None,
			Exists,
			Mineral,
			NonBreakable,
		}

		private Mineral mineral;

		private State state = State.Exists;

		private bool _collision = false;

		public bool collision {
			get {
				return _collision;
			}
		}

		public Mineral GetMineral()
		{
			return mineral;
		}

		public bool breakable {
			get {
				return state != State.NonBreakable;
			}
			set {
				if (state != State.None)
					state = value ? State.Exists : State.NonBreakable;
			}
		}

		public bool exists {
			get {
				return state != State.None;
			}
		}

		public BreakingTile setCollision()
		{
			_collision = false;
			return new BreakingTile(this);
		}

		public void destroy()
		{
			_collision = false;
			mineral = null;
			state = State.None;
		}

		public Tile(int x, int y, bool exists)
		{
			PosX = x;
			PosY = y;
			state = exists ? State.Exists : State.None;
			_collision = exists;
		}

		public Tile(int x, int y)
		{
			PosX = x;
			PosY = y;
			GamePosition = new Vector2(PosX * Size, (PosY - 1) * Size);


			if (y != 0) {
				state = genTile(x, y) ? State.Exists : State.None;

				if (state == State.Exists && rnd.Next(0, 10) == 0) {
						state = State.Mineral;
						mineral = Mineral.RandomByDepth(y);
				}
			} else {
				state = State.Exists;
			}

			_collision = exists;
		}

		private bool genTile(int x, int y)
		{
			return rnd.Next(0, 7) != 0;
		}

		public static bool VertexInTile(Vector2 tile, Vector2 point)
		{
			var x1 = Size * (0 + tile.X);
			var xp = point.X;
			var x2 = Size * (1 + tile.X);

			var y1 = Size * (0 + tile.Y);
			var yp = point.Y;
			var y2 = Size * (1 + tile.Y);

			return x1 <= xp && xp <= x2 && y1 <= yp && yp <= y2;
		}

		private Sprites.Name RockTile()
		{
			int i = ((PosX + 991) * (PosY + 293)) % 6;

			switch (i) {
			case 0:
				return Sprites.Name.TileNonBreakable1;
			case 1:
				return Sprites.Name.TileNonBreakable2;
			case 2:
				return Sprites.Name.TileNonBreakable3;
			case 3:
				return Sprites.Name.TileNonBreakable4;
			case 4:
				return Sprites.Name.TileNonBreakable5;
			case 5:
				return Sprites.Name.TileNonBreakable6;
			}

			throw new Exception();
		}

		public void drawTile()
		{
			if (state == State.None)
				return;
			
			Sprites.Name sprite = Sprites.Name.TileFull;

			Painter.Sprite(GamePosition, new Vector2(Size, Size), sprite, 0);				

			sprite = (Sprites.Name)(-1);
			switch (state) {
			case State.NonBreakable:
				sprite = RockTile();
				break;
			case State.Mineral:
				sprite = mineral.GetSprite();
				break;
			}
			if ((int)sprite != -1)
				Painter.Sprite(GamePosition, new Vector2(Size, Size), sprite, 0.01f);
		}


	}
}
