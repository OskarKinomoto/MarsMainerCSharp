﻿using System;
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

		private const int FONT_SIZE = 18;

		public static void Square(Vector2 pos, Vector2 size, float layer)
		{
			Painter.StartQuads();

			GL.TexCoord2(0.0, 0.0);
			GL.Vertex3(pos.X, pos.Y + size.Y, layer);
			GL.TexCoord2(1.0, 0.0);
			GL.Vertex3(pos.X + size.X, pos.Y + size.Y, layer);
			GL.TexCoord2(1.0, 1.0);
			GL.Vertex3(pos.X + size.X, pos.Y, layer);
			GL.TexCoord2(0.0, 1.0);
			GL.Vertex3(pos.X, pos.Y, layer);
                        
			Painter.Stop();
		}

		public static void Square2(Vector2 pos, Vector2 size, float layer)
		{
			GL.TexCoord2(0.0, 0.0);
			GL.Vertex3(pos.X, pos.Y + size.Y, layer);
			GL.TexCoord2(1.0, 0.0);
			GL.Vertex3(pos.X + size.X, pos.Y + size.Y, layer);
			GL.TexCoord2(1.0, 1.0);
			GL.Vertex3(pos.X + size.X, pos.Y, layer);
			GL.TexCoord2(0.0, 1.0);
			GL.Vertex3(pos.X, pos.Y, layer);
		}

		public static void Sprite(Vector2 pos, Vector2 size, Sprites.Name sprite, float layer)
		{
			Sprites.TexCoord(sprite, 0);
			GL.Vertex3(pos.X, pos.Y + size.Y, layer);
			Sprites.TexCoord(sprite, 1);
			GL.Vertex3(pos.X + size.X, pos.Y + size.Y, layer);
			Sprites.TexCoord(sprite, 2);
			GL.Vertex3(pos.X + size.X, pos.Y, layer);
			Sprites.TexCoord(sprite, 3);
			GL.Vertex3(pos.X, pos.Y, layer);
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

		public static void Color(Vector3 color) {
			GL.Color3(color);
		}

		public static void Color(Color color) {
			GL.Color3(color);
		}

		public static void Color(float r, float g, float b) {
			GL.Color3(r,g,b);
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
			GL.Vertex3(startPosition.X, startPosition.Y, layer);
			GL.Vertex3(endPosition.X, startPosition.Y, layer);

			GL.Color3(endColor);
			GL.Vertex3(endPosition.X, endPosition.Y, layer);
			GL.Vertex3(startPosition.X, endPosition.Y, layer);
		}

		public static void Text(Textures TextTexture, float WindowPosY, Horizontal position, Layer layer)
		{
			TextTexture.Bind();
			Painter.EnableTextures();

			Vector2 r = new Vector2(0, Controler.WindowHeight / 2 - TextTexture.SquareSize.Height - WindowPosY - Margin);
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

			Painter.Square(r, new Vector2(TextTexture.SquareSize.Width, TextTexture.SquareSize.Height), layer);
		}
						
	}
}