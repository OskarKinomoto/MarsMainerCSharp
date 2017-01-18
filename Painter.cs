/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace MarsMiner
{
	class Painter
	{
		public enum Horizontal
		{
			Left,
			Right,
			Center,
		}

		public const int Margin = 10;

		public static float InterfaceScale = 1f;
		public static float GameScale = 1.2f;

		private static bool InterfaceOrGame = true;

		private static float Scale {
			get {
				return InterfaceOrGame ? InterfaceScale : GameScale;
			}
		}

		public void Game()
		{
			InterfaceOrGame = false;
		}

		public void Interface()
		{
			InterfaceOrGame = true;
		}

		public static void Square(Vector2 pos, Vector2 size, float layer)
		{
			Painter.StartQuads();
			Square2(pos, size, layer);                        
			Painter.Stop();
		}

		public static void Square3(Vector2 pos, Vector2 size, float layer, float xTex, float yTex)
		{
			Painter.StartQuads();
			GL.TexCoord2(0.0, 0.0);
			GL.Vertex3(pos.X * Scale, (pos.Y + size.Y) * Scale, layer);
			GL.TexCoord2(xTex, 0.0);
			GL.Vertex3((pos.X + size.X) * Scale, (pos.Y + size.Y) * Scale, layer);
			GL.TexCoord2(xTex, yTex);
			GL.Vertex3((pos.X + size.X) * Scale, pos.Y * Scale, layer);
			GL.TexCoord2(0.0, yTex);
			GL.Vertex3(pos.X * Scale, pos.Y * Scale, layer);                      
			Painter.Stop();
		}

		public static void Square2(Vector2 pos, Vector2 size, float layer)
		{
			GL.TexCoord2(0.0, 0.0);
			GL.Vertex3(pos.X * Scale, (pos.Y + size.Y) * Scale, layer);
			GL.TexCoord2(1.0, 0.0);
			GL.Vertex3((pos.X + size.X) * Scale, (pos.Y + size.Y) * Scale, layer);
			GL.TexCoord2(1.0, 1.0);
			GL.Vertex3((pos.X + size.X) * Scale, pos.Y * Scale, layer);
			GL.TexCoord2(0.0, 1.0);
			GL.Vertex3(pos.X * Scale, pos.Y * Scale, layer);
		}

		public static void Sprite(Vector2 pos, Vector2 size, Sprites.Name sprite, float layer, int SpriteOffset = 0)
		{
			Sprites.TexCoord(sprite, 0, SpriteOffset);
			GL.Vertex3(pos.X * Scale, (pos.Y + size.Y) * Scale, layer);
			Sprites.TexCoord(sprite, 1, SpriteOffset);
			GL.Vertex3((pos.X + size.X) * Scale, (pos.Y + size.Y) * Scale, layer);
			Sprites.TexCoord(sprite, 2, SpriteOffset);
			GL.Vertex3((pos.X + size.X) * Scale, pos.Y * Scale, layer);
			Sprites.TexCoord(sprite, 3, SpriteOffset);
			GL.Vertex3(pos.X * Scale, pos.Y * Scale, layer);
		}

		public static void SpriteArray(Vector2 pos, Vector2 size, int x, int y, Sprites.Name sprite, float layer)
		{
			throw new NotImplementedException();
		}

		private static bool TexturesEnabled = false;

		public static void EnableTextures()
		{
			if (!TexturesEnabled) {
				GL.Enable(EnableCap.Texture2D);
				TexturesEnabled = true;
			}
		}

		public static void DisableTextures()
		{
			if (TexturesEnabled) {
				GL.Disable(EnableCap.Texture2D);
				TexturesEnabled = false;
			}
		}

		public static void StartQuads()
		{
			GL.Begin(PrimitiveType.Quads);
		}

		public static void Stop()
		{
			GL.End();
		}

		public static void Color(Vector3 color)
		{
			GL.Color3(color);
		}

		public static void Color(Color color)
		{
			GL.Color3(color);
		}

		public static void Color(float r, float g, float b)
		{
			GL.Color3(r, g, b);
		}

		public static void CameraMove(Camera camera)
		{
			GL.Translate(camera.position.X, camera.position.Y, 0);
		}

		public static void CameraMoveReverse(Camera camera)
		{
			GL.Translate(-camera.position.X, -camera.position.Y, 0);
		}

		public static void Gradient(
			Vector2 startPosition, Vector3 startColor,
			Vector2 endPosition, Vector3 endColor,
			Layer layer
		)
		{
			GL.Color3(startColor);
			GL.Vertex3(startPosition.X * Scale, startPosition.Y * Scale, layer);
			GL.Vertex3(endPosition.X * Scale, startPosition.Y * Scale, layer);

			GL.Color3(endColor);
			GL.Vertex3(endPosition.X * Scale, endPosition.Y * Scale, layer);
			GL.Vertex3(startPosition.X * Scale, endPosition.Y * Scale, layer);
		}

		public static void Text(Textures TextTexture, float WindowPosY, Horizontal position, Layer layer)
		{
			TextTexture.Bind();
			Painter.EnableTextures();

			Vector2 r = new Vector2(0, Controler.WindowHeight / 2 - TextTexture.TextSize.Height - WindowPosY - Margin);
			switch (position) {
			case Horizontal.Left:
				r.X = -Controler.WindowWidth / 2;
				break;
			case Horizontal.Center:
				r.X = -TextTexture.TextSize.Width / 2;
				break;
			case Horizontal.Right:
				r.X = Controler.WindowWidth / 2 - TextTexture.TextSize.Width;
				break;
			}

			Painter.Square3(r, new Vector2(TextTexture.TextSize.Width, TextTexture.TextSize.Height), layer, 
				TextTexture.TextSize.Width / TextTexture.SquareSize.Width, 
				TextTexture.TextSize.Height / TextTexture.SquareSize.Height
			);
		}
						
	}
}
