/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace MarsMiner
{
    class View : GameWindow
    {
        // members
        private Controler c = new Controler();

		public View() : base(1280, 720, GraphicsMode.Default, "Miner")
        {
			VSync = VSyncMode.Adaptive;
			// TODO optimize?
			//c.mouseVisible += (bool visible) => {if (this.CursorVisible != visible) this.CursorVisible = visible;};
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Enable(EnableCap.DepthTest | EnableCap.Texture2D);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);

            c.Load();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection2 = Matrix4.CreateOrthographic(Width, Height, -11, 11);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection2);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var time = e.Time;

            if (time > 0.1)
                time = .1;
			
			c.Tick((float)time);


			// TODO create function recieving all keyboard state? 
            c.down(Keyboard[Key.Down]);
            c.up(Keyboard[Key.Up]);
            c.left(Keyboard[Key.Left]);
            c.right(Keyboard[Key.Right]);
			c.enter(Keyboard[Key.Enter] || Keyboard[Key.KeypadEnter]);
			c.esc(Keyboard[Key.Escape]);
			c.keyE(Keyboard[Key.E]);

            if (c.isClosing())
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.ClearColor(System.Drawing.Color.DimGray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            c.Render(Width, Height);

            SwapBuffers();
        }

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			base.OnMouseMove(e);
			c.mouse(e.Position);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
    	{
			if (e.Button == MouseButton.Left)
				c.mouseClick(true);
    	}

    	protected override void OnMouseUp(MouseButtonEventArgs e)
    	{
			if (e.Button == MouseButton.Left)
				c.mouseClick(false);
    	}

		public static bool IsRunningOnMono()
		{
			return Type.GetType ("Mono.Runtime") != null;
		}

        [STAThread]
        static void Main()
        {
            using (View game = new View())
            {
                game.Run(60.0, 0.0);
            }
        }
    }
}