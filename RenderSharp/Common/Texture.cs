using ImageMagick;
using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A texture that can be assigned to objects within a scene.
    /// </summary>
    public class Texture
    {
        private RGBA[,] _texture; 

        internal Vec2 Size { get; set; } 

        internal int Height { get { return Size.Y; } } //set { Size = new(value, Size.Y); } }

        internal int Width { get { return Size.X; } } //{ Size = new(value, Size.Y); } }

        /// <summary>
        /// Constructs an alpha opaque texture with the given dimensions and color.
        /// </summary>
        /// <param name="width">Desired width of the texture in pixels.</param>
        /// <param name="height">Desired height of the texture in pixels.</param>
        /// <param name="color">Optional color to fill the texture. Defaults to black.</param>
        public Texture(int width, int height, RGBA? color = null)
        {
            Size = new Vec2(width, height);
            _texture = new RGBA[Height, Width];
            Fill(color ?? new RGB(0, 0, 0));
        }

        /// <summary>
        /// Constructs a texture with the given dimensions and color.
        /// </summary>
        /// <param name="size">Desired width and height of the texture in pixels.</param>
        /// <param name="color">Optional color to fill the texture. Defaults to alpha-opaque black.</param>
        public Texture(Vec2 size, RGBA? color = null)
        {
            Size = size;
            _texture = new RGBA[Height, Width];
            Fill(color ?? new RGB(0, 0, 0));
        }

        /// <summary>
        /// Constructs a texture using an image on disk supported by <see href="https://imagemagick.org"/>.
        /// </summary>
        /// <param name="filename">Path to the image.</param>
        public Texture(string filename)
        {
            var image = new MagickImage(filename);
            var bmp = image.ToByteArray(MagickFormat.Rgba);

            Size = new Vec2(image.Width, image.Height);
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

        private void Fill(RGBA color)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _texture[i, j] = color;
                }
            }
        }

        internal RGBA this[int x, int y]
        {
            get { return y < Height && x < Width ? _texture[y, x] : new RGBA(); }
        }
    }
}