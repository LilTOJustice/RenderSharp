using ImageMagick;
using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A texture that can be assigned to objects within a scene.
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Internal texture buffer.
        /// </summary>
        private RGBA[,] _texture; 

        /// <summary>
        /// Dimensions of the texture.
        /// </summary>
        public Vec2 Size { get; set; } 

        /// <summary>
        /// Height component of the <see cref="Size"/>.
        /// </summary>
        public int Height { get { return Size.Y; } set { Size.Y = value; } }

        /// <summary>
        /// Width component of the <see cref="Size"/>.
        /// </summary>
        public int Width { get { return Size.X; } set { Size.X = value; } }

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

        /// <summary>
        /// Fills the texture with the given color.
        /// </summary>
        /// <param name="color">Color to fill the texture with.</param>
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

        /// <summary>
        /// Returns a pixel from the texture.
        /// </summary>
        /// <param name="x">Position within the texture relative to the width.</param>
        /// <param name="y">Position within the texture relative to the height.</param>
        /// <returns>A pixel in the texture.</returns>
        internal RGBA this[int x, int y]
        {
            get { return _texture[y, x]; }
        }
    }
}