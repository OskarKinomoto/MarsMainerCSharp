/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

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

		public Keyboard keys = new Keyboard();

		public Mouse mouse = new Mouse();
		public event Action<bool> mouseVisible;

		public static float WindowWidth = 0;
		public static float WindowHeight = 0;

		private Clouds clouds = new Clouds();

		private State state = State.MainMenu;

		public static Vector2 WindowSize {
			get {
				return new Vector2(WindowWidth, WindowHeight);
			}
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

			keys.onEnter += () => {
				if (currentMenu != null)
					currentMenu.Enter();
			};

			keys.onUp += () => {
				if (currentMenu != null)
					currentMenu.up();
			};

			keys.onDown += () => {
				if (currentMenu != null)
					currentMenu.down();
			};

			keys.onE += enterBuilding;
			keys.onEsc += menuEsc;

			mouse.onMouseActive += () => {
				if (mouseVisible != null)
					mouseVisible(true);
			};
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
			if (mouse.Event() && activeBuilding != null) {
				activeBuilding.Mouse(mouse.Position(), mouse.action());
				mouse.ResetEvent();
			}

			if (state == State.InGame) {
				m.robot.SetEngine(keys.EngineState());
				
				m.tick(tau);

				if (m.testGameOver()) {
					state = State.GameOver;
					currentMenu = gameOverMenu;
				}
			}

			if (isInGame())
				clouds.Tick(tau);
		}

		private void menuEsc()
		{
			if (state == State.InGame) {
				state = State.InGameMenu;
				currentMenu = inGameMenu;
			} else if (state == State.InGameMenu || state == State.InBuilding) {
				StateToGame();
			}
		}

		private void enterBuilding()
		{
			if (!isInGame())
				return;
			
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

		public void MouseMove(Point pos)
		{
			mouse.Position(screenToGL(pos));
		}

		public void mouseClick(bool state)
		{
			mouse.LeftBtn(state);
		}
	}
}
