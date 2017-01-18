using OpenTK;
using System;
using System.Drawing;

namespace MarsMiner
{
	class Controler
	{
		private enum State
		{
			MainMenu,
			Loaded,
			InGame,
			InBuilding,
			InGameMenu,
			GameOver,
			Closing
		}

		Model m = new Model();

		Menu gameOverMenu;
		Menu mainMenu;
		Menu inGameMenu;
		Menu currentMenu;

		Building activeBuilding = null;

		private const float LAYER_BG = -10;
		private const float LAYER_TEXT = 10;

		private bool _up = false;
		private bool _down = false;
		private bool _left = false;
		private bool _right = false;
		private bool _enter = false;
		private bool _esc = false;
		private bool _e = false;

		private bool mouseActive = false;
		private bool mouseLeftClick = false;
		private bool mouseLeftButton = false;
		private Vector2 mousePosition = new Vector2(0, 0);
		private bool mouseEventGenerate = false;

		public static float WindowWidth = 0;
		public static float WindowHeight = 0;

		private Clouds clouds = new Clouds();

		private State state = State.MainMenu;

		public event Action<bool> mouseVisible;


		public static Vector2 WindowSize {
			get {
				return new Vector2(WindowWidth, WindowHeight);
			}
		}

		public Vector2 glToScreen(Vector2 glPoint)
		{
			return new Vector2(
				glPoint.X + WindowWidth / 2,
				glPoint.Y + WindowHeight / 2
			);
		}

		public Point glToScreen(Point glPoint)
		{
			return new Point(
				(int)(glPoint.X + WindowWidth / 2),
				(int)(-glPoint.Y - WindowHeight / 2)
			);
		}

		public Vector2 screenToGL(Vector2 screenPoint)
		{
			return new Vector2(
				screenPoint.X - WindowWidth / 2,
				screenPoint.Y - WindowHeight / 2
			);
		}

		public Vector2 screenToGL(Point screenPoint)
		{
			return new Vector2(
				(int)(screenPoint.X - WindowWidth / 2),
				(int)(-screenPoint.Y + WindowHeight / 2)
			);
		}

		public void StateToGame()
		{
			state = State.InGame;
			activeBuilding = null;
			currentMenu = null;
		}

		public bool isClosing()
		{
			return state == State.Closing;
		}

		public void Load()
		{
			MenuInitialize();
			Textures.loadAll();
			m.NewGame(StateToGame);
			Sprites.Load();

			foreach (Building b in m.GetBuildings)
				b.onClose += StateToGame;
		}

		private void MenuInitialize()
		{
			gameOverMenu = new Menu("Gameover");
			gameOverMenu.AddItem("New game", NewGame);
			gameOverMenu.AddItem("Exit", Exit);

			mainMenu = new Menu("MarsMiner");
			mainMenu.AddItem("New game", NewGame);
			mainMenu.AddItem("Exit", Exit);

			inGameMenu = new Menu("Pause");
			inGameMenu.AddItem("Resume", StateToGame);
			inGameMenu.AddItem("Exit", Exit);

			currentMenu = mainMenu;
		}

		private void Exit()
		{
			currentMenu = null;
			state = State.Closing;
		}

		private void NewGame()
		{
			m.NewGame(StateToGame);
			currentMenu = null;
			state = State.Loaded;
		}

		private bool isInMenu()
		{
			return state == State.GameOver || state == State.MainMenu || state == State.InGameMenu;
		}

		private bool isInGame()
		{
			return state == State.InGame || state == State.InBuilding;
		}

		public void Render(float width, float height)
		{
			WindowWidth = width;
			WindowHeight = height;

			if (mouseVisible != null)
				mouseVisible(mouseActive);

			if (isInGame()) {
				Painter.CameraMove(m.camera);
				// Paint in game coordinates
				RenderBackground();
				RenderTiles();

				clouds.Paint();

				if (m.robot.m_position.Y < height && m.robot.m_position.Y > -height)
					foreach (Building b in m.GetBuildings)
						b.Paint();

				m.robot.Paint();

				Painter.CameraMoveReverse(m.camera);
				// Paint in semiwindow coordinates
				m.Money.PaintOnScreen();
				m.robot.PaintOnScreen();

				if (activeBuilding != null) {
					activeBuilding.PaintBuildingMenu();
				} else if (-1 < m.robot.m_position.Y && m.robot.m_position.Y < 2) {
					foreach (Building b in m.GetBuildings) {
						if (b.CanEnter(m.robot)) {
							b.PaintEnter();
							break;
						}
					}
				}
			} else if (isInMenu()) {
				currentMenu.Paint();
			} else if (state == State.Loaded) {
				state = State.InGame;
			}

			// GC.Collect(); // otherwise it eats a lot of memory
		}

		private void RenderBackground()
		{
			Painter.DisableTextures();
			Painter.StartQuads();

			Painter.Gradient(
				new Vector2(Model.MinTileX, 0), new Vector3(.2f, .6f, .9f),
				new Vector2(Model.MaxTileX, 100 * Tile.Size), new Vector3(.0f, .0f, .1f),
				Layer.Background
			);

			Painter.Gradient(
				new Vector2(Model.MinTileX, 0), new Vector3(.37f, .22f, .07f),
				new Vector2(Model.MaxTileX, -50 * Tile.Size), new Vector3(.26f, .15f, .03f),
				Layer.Background
			);

			Painter.Gradient(
				new Vector2(Model.MaxTileX, -50 * Tile.Size), new Vector3(.26f, .15f, .03f),
				new Vector2(Model.MinTileX, -100 * Tile.Size), new Vector3(.15f, .08f, .01f),
				Layer.Background
			);

			Painter.Stop();
		}

		private void RenderTiles()
		{
			m.tiles.DrawTiles(m.camera.position);
		}

		public void Tick(float tau)
		{
			if (mouseEventGenerate && activeBuilding != null) {
				activeBuilding.Mouse(mousePosition, mouseLeftClick ? Mouse.Action.LeftClick : Mouse.Action.None);
				mouseLeftClick = false;
				mouseEventGenerate = false;
			}

			if (state == State.InGame) {
				var KeyboardArrows = new Vector2();
				if (_up)
					KeyboardArrows.Y += 1;
				if (_down)
					KeyboardArrows.Y -= 1;
				if (_left)
					KeyboardArrows.X -= 1;
				if (_right)
					KeyboardArrows.X += 1;

				m.robot.SetEngine(KeyboardArrows.LengthSquared > 0, (float)Math.Atan2(KeyboardArrows.Y, KeyboardArrows.X));
				
				m.tick(tau);

				if (m.testGameOver()) {
					state = State.GameOver;
					currentMenu = gameOverMenu;
				}
			}

			if (isInGame())
				clouds.Tick(tau);
		}

		private void menuEsc(bool keyDown)
		{
			if (keyDown && !_esc) {
				mouseActive = false;

				if (state == State.InGame) {
					state = State.InGameMenu;
					currentMenu = inGameMenu;
				} else if (state == State.InGameMenu) {
					StateToGame();
				} else if (state == State.InBuilding) {
					StateToGame();
				}
			}
		}

		private void menuUp(bool keyDown)
		{
			if (keyDown && !_up) {
				mouseActive = false;
				if (currentMenu != null)
					currentMenu.up();
			}
		}

		private void menuDown(bool keyDown)
		{
			if (keyDown && !_down) {
				mouseActive = false;
				if (currentMenu != null)
					currentMenu.down();
			}
		}

		private void enterBuilding(bool keyDown)
		{
			if (keyDown && !_e) {
				mouseActive = true;
				if (activeBuilding == null) {
					foreach (Building b in m.GetBuildings) {
						if (b.CanEnter(m.robot)) {
							activeBuilding = b;
							state = State.InBuilding;
							break;
						}
					}
				} else {
					activeBuilding.Close();
				}
			}
		}

		private void menuEnter(bool keyDown)
		{
			if (keyDown && !_enter) {
				mouseActive = false;
				if (currentMenu != null)
					currentMenu.Enter();				
			}
		}

		public void enter(bool state)
		{
			menuEnter(state);
			_enter = state;
		}

		public void esc(bool state)
		{
			menuEsc(state);
			_esc = state;
		}

		public void down(bool state)
		{
			menuDown(state);
			_down = state;
		}

		public void up(bool state)
		{
			menuUp(state);
			_up = state;
		}

		public void left(bool state)
		{
			if (state)
				mouseActive = false;
			_left = state;
		}

		public void right(bool state)
		{
			if (state)
				mouseActive = false;
			_right = state;
		}

		public void keyE(bool state)
		{
			enterBuilding(state);
			_e = state;
		}

		public void mouse(Point pos)
		{
			var newPos = screenToGL(pos);
			if (newPos != mousePosition) {
				mousePosition = newPos;

				mouseEventGenerate = true;
				mouseActive = true;
			}
		}

		public void mouseClick(bool state)
		{
			if (mouseLeftButton && !state) {
				mouseLeftClick = true;
			}

			mouseLeftButton = state;
			mouseEventGenerate = true;
			mouseActive = true;
		}
	}
}
