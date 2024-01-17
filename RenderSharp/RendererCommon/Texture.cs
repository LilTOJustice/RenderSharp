using ImageMagick;
using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public class Texture
    {
        private RGBA[,] _texture; 

        public Vec2 Size { get; set; } 

        public int Height { get { return Size.Y; } set { Size.Y = value; } }

        public int Width { get { return Size.X; } set { Size.X = value; } }

        public Texture(int width, int height, RGBA? color = null)
        {
            Size = new Vec2(width, height);
            _texture = new RGBA[Height, Width];
            Fill(color ?? new RGB(0, 0, 0));
        }

        public Texture(Vec2 size, RGBA? color = null)
        {
            Size = size;
            _texture = new RGBA[Height, Width];
            Fill(color ?? new RGB(0, 0, 0));
        }

        public Texture(string filename)
        {
            var image = new MagickImage(filename);
            var bmp = image.ToByteArray(MagickFormat.Rgba);

            Size = new Vec2 (image.Width, image.Height);
            _texture = new RGBA[Height, Width];

            for (int i = 0; i < image.Height; i++)
            {
                int heightStep = i * Width * 4;

                for (int j = 0; j < image.Width; j++)
                {
                    int widthStep = heightStep + j * 4;

                    _texture[i, j] = new RGBA
                        (
                            bmp[widthStep],
                            bmp[widthStep + 1],
                            bmp[widthStep + 2],
                            bmp[widthStep + 3]
                        );
                }
            }
        }

        public void Fill(RGBA color)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _texture[i, j] = color;
                }
            }
        }

        public RGBA this[int x, int y]
        {
            get { return _texture[y, x]; }
        }
    }
}