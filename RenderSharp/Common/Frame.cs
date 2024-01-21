using ImageMagick;
using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// Represents a single frame of a render that can be exported as an image.
    /// </summary>
    public class Frame
    {   
        private static readonly int channels = 4;

        /// <summary>
        /// Size of the frame.
        /// </summary>
        public Vec2 Size { get; private set; }

        /// <summary>
        /// Height component of the <see cref="Size"/>.
        /// </summary>
        public int Height { get { return Size.Y; } private set { Size.Y = value; } }

        /// <summary>
        /// Width component of the <see cref="Size"/>.
        /// </summary>
        public int Width { get { return Size.X; } private set { Size.X = value; } }

        /// <summary>
        /// Aspect ratio of the frame (Width / Height).
        /// </summary>
        public double AspectRatio { get { return Width / Height; } }

        /// <summary>
        /// Internal frame buffer.
        /// </summary>
        private byte[] Image { get; set; }

        /// <summary>
        /// Creates an empty black frame.
        /// </summary>
        /// <param name="size"></param>
        public Frame(Vec2 size)
        {
            Size = size;
            Image = new byte[Width * Height * channels];
        }

        /// <inheritdoc cref="Frame(Vec2)"/>
        public Frame(int width, int height)
        {
            Size = new Vec2(width, height);
            Image = new byte[width * height * channels];
        }

        /// <summary>
        /// Accesses the pixel at the given position.
        /// </summary>
        /// <param name="x">Position relative to the <see cref="Width"/> of the frame.</param>
        /// <param name="y">Position relative to the <see cref="Height"/> of the frame.</param>
        /// <returns>The pixel at the desired location.</returns>
        public RGBA this[int x, int y]
        {
            get
            {
                int index = (y * Width + x) * channels;
                return new RGBA
                    (
                        Image[index],
                        Image[index + 1],
                        Image[index + 2],
                        Image[index + 3]
                    );
            }
            set
            {
                int index = (y * Width + x) * channels;
                Image[index] = value.R;
                Image[index + 1] = value.G;
                Image[index + 2] = value.B;
                Image[index + 3] = value.A;
            }
        }

        /// <summary>
        /// Exports the frame with the given extension (if supported by <see href="https://imagemagick.org"/>).
        /// </summary>
        /// <param name="filename">Location of the exported file.</param>
        /// <param name="ext">Optional extension, must be supported by <see href="https://imagemagick.org"/>.</param>
        public void Output(string filename, string ext = "png")
        {
            string fullname = filename + "." + ext;
            var settings = new MagickReadSettings();
            settings.Format = MagickFormat.Rgba;
            settings.Width = Width;
            settings.Height = Height;
            settings.Depth = 8;
            var image = new MagickImage(Image, settings);
            image.Write(fullname);
            
        }
    }
}   
