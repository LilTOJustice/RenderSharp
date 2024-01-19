using ImageMagick;
using RenderSharp.Math;
using System.Diagnostics;

namespace RenderSharp.RendererCommon
{
    /// <summary>
    /// Represents a single frame of a render that can be exported as an image.
    /// </summary>
    public class Frame
    {   
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
            Image = new byte[Width * Height * 3];
        }

        /// <inheritdoc cref="Frame(Vec2)"/>
        public Frame(int width, int height)
        {
            Size = new Vec2(width, height);
            Image = new byte[width * height * 3];
        }

        /// <summary>
        /// Accesses the pixel at the given position.
        /// </summary>
        /// <param name="x">Position relative to the <see cref="Width"/> of the frame.</param>
        /// <param name="y">Position relative to the <see cref="Height"/> of the frame.</param>
        /// <returns>The pixel at the desired location.</returns>
        public RGB this[int x, int y]
        {
            get
            {
                int index = (y * Width + x) * 3;
                return new RGB
                    (
                        Image[index],
                        Image[index + 1],
                        Image[index + 2]
                    );
            }
            set
            {
                int index = (y * Width + x) * 3;
                Image[index] = value.R;
                Image[index + 1] = value.G;
                Image[index + 2] = value.B;
                    
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
            settings.Format = MagickFormat.Rgb;
            settings.Width = Width;
            settings.Height = Height;
            settings.Depth = 8;
            var image = new MagickImage(Image, settings);
            image.Write(fullname);
            
        }
    }

    /// <summary>
    /// A logical collection of frames that can be exported as a video.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Size of the individual frames of the movie.
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
        /// Aspect ratio of the movie (Width / Height).
        /// </summary>
        public double AspectRatio { get { return Width / Height; } }

        /// <summary>
        /// Framerate of the movie.
        /// </summary>
        public int Framerate { get; private set; }
        
        /// <summary>
        /// Index of the movie. Acts as a unique identifier for the movie if multiple renders occur in one run.
        /// </summary>
        private int MovieID { get; set; }

        /// <summary>
        /// Location of the individual frames of the movie.
        /// </summary>
        private string TempDir { get; set; }

        private static int _nextId = 0;

        /// <summary>
        /// Constructs a logically empty movie.
        /// </summary>
        /// <param name="width">Width of each frame.</param>
        /// <param name="height">Height of each frame.</param>
        /// <param name="framerate">Framerate of the movie.</param>
        public Movie(int width, int height, int framerate)
        {
            Size = new Vec2(width, height);
            Framerate = framerate;
            MovieID = _nextId++;
            TempDir = $"{Directory.GetCurrentDirectory()}\\temp_{MovieID}";
            try
            {
                Directory.Delete(TempDir, true);
            }
            catch
            { }
            Directory.CreateDirectory(TempDir);
        }

        /// <summary>
        /// Exports the movie as an mp4 using ffmpeg.
        /// </summary>
        /// <param name="filename">Path to the exported video.</param>
        public void Output(string filename)
        {
            string fullname = filename + ".mp4";
            Console.WriteLine($"Exporting movie: {fullname}");
            string cmd = ($"-y -v -8 -framerate {Framerate} -f image2 -i temp_{MovieID}/%d.bmp -c h264 " +
                $"-pix_fmt yuv420p -b:v 32768k {fullname}");
            Console.WriteLine(cmd + "\n");
            
            if (Process.Start("ffmpeg", cmd) == null)
            {
                Console.WriteLine("Error outputting file!");
                return;
            }

            Console.WriteLine("Done.");
        }

        /// <summary>
        /// Stores a single frame in the <see cref="TempDir"/>.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="frameInd"></param>
        public void WriteFrame(Frame frame, int frameInd)
        {
            string filename = $"{TempDir}\\{frameInd}";
            frame.Output(filename, "bmp");
        }

        ~Movie() {
            Directory.Delete(TempDir, true);
        }
    }
}   
