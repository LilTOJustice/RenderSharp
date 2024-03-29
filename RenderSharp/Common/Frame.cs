﻿using ImageMagick;
using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// Represents a single frame of a render that can be exported as an image.
    /// </summary>
    public class Frame
    {   
        private static readonly int channels = 4;

        internal Vec2 Size { get; private set; }

        internal int Height { get { return Size.Y; } }

        internal int Width { get { return Size.X; } }

        internal double AspectRatio { get { return Width / Height; } }

        private byte[] Image { get; set; }

        internal Frame(in Vec2 size)
        {
            Size = size;
            Image = new byte[Width * Height * channels];
        }

        internal unsafe RGBA this[int x, int y]
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
                Span<byte> color = new((byte*)&value, 4);
                color.CopyTo(new Span<byte>(Image, index, 4));
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
            new MagickImage(Image, settings).Write(fullname);
        }
    }
}   
