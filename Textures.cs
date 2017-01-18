/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace MarsMiner
{
    class Textures
    {
        public enum Texture
        {
            None = -1,
        }

        private static int[] tex_map;
        private static Texture last_bind = Texture.None;

        public static void loadAll()
        {
            tex_map = new int[(int)Enum.GetValues(typeof(Texture)).Cast<Texture>().Max() + 1];
        }

        public static int Load(string filename)
        {
            Bitmap tmpBitmap = new Bitmap(filename);
            return Load(tmpBitmap);
        }

        private static int Load(Bitmap bitmap)
        {
            int target;
            System.Drawing.Imaging.BitmapData TextureData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

            GL.GenTextures(1, out target);
            GL.BindTexture(TextureTarget.Texture2D, target);

            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0,
				PixelFormat.Bgra, PixelType.UnsignedByte, TextureData.Scan0);

            bitmap.UnlockBits(TextureData);
            last_bind = Texture.None;
            return target;
        }

        public static void Bind(Texture t)
        {
            if (last_bind == t)
                return;

            last_bind = t;
            GL.BindTexture(TextureTarget.Texture2D, tex_map[(int)t]);
        }

        public static void Bind(int t)
        {
            last_bind = Texture.None;
            GL.BindTexture(TextureTarget.Texture2D, t);
        }

        public static void Delete(int t)
        {
            last_bind = Texture.None;
            GL.DeleteTexture(t);
        }

        public enum FontName
        {
            Tahoma
        }

        public static Textures Text(String str, FontName fontName, float fontSize)
        {
            return Text(str, fontName, fontSize, Brushes.Black);
        }

        public static Textures Text(String str, FontName fontName, float FontSize, Brush color)
        {
            const int height = 64; // must be power of 2
            const int width = 256; // must be power of 2
            Size SquareSize = new Size(width, height);

            var font = new Font(fontName.ToString(), FontSize);

            Bitmap bmp = new Bitmap(width, height);

            RectangleF rectf = new RectangleF(0, 0, width, height);

            Graphics g = Graphics.FromImage(bmp);
            SizeF TextSize = g.MeasureString(str, font);

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(str, font, color, rectf);

            g.Flush();

            int tex = Textures.Load(bmp);

            return new Textures(tex, SquareSize, TextSize);
        }

        private int _tex;
        private Size _SquareSize;
        private SizeF _TextSize;

        public Size SquareSize
        {
            get
            {
                return _SquareSize;
            }
        }

        public SizeF TextSize
        {
            get
            {
                return _TextSize;
            }
        }

        public Textures(int tex, Size squareSize, SizeF textSize)
        {
            _tex = tex;
            _SquareSize = squareSize;
            _TextSize = textSize;
        }

        public Textures(String path)
        {
            Bitmap tmpBitmap = new Bitmap(path);
            _SquareSize = tmpBitmap.Size;
            _tex = Load(tmpBitmap);
        }

        public void Bind()
        {
            Bind(_tex);
        }

        public void Delete()
        {
            Delete(_tex);
        }

    }
}
