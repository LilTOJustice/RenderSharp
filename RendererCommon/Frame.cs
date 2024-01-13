using ImageMagick;
using RenderSharp.Math;
using RenderSharp.RendererCommon;
using System.Diagnostics;

namespace RendererCommon
{
    public class Frame
    {   
        // properties
        public Vec2 Size { get; private set; }

        public int Height { get { return Size.Y; } private set { Size.Y = value; } }

        public int Width { get { return Size.X; } private set { Size.X = value; } }

        public double AspectRatio { get { return Width / Height; } }

        private byte[] Image { get; set; }

        public Frame(Vec2 size)
        {
            Size = size;
            Image = new byte[Width * Height * 4];
        }

        public Frame(int width, int height)
        {
            Size = new Vec2(width, height);
            Image = new byte[width * height * 4];
        }

        public RGBA this[int x, int y]
        {
            get
            {
                int index = (y * Width + x) * 4;
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
                int index = (y * Width + x) * 4;
                Image[index] = value.R;
                Image[index + 1] = value.G;
                Image[index + 2] = value.B;
                Image[index + 3] = value.A;
                    
            }
        }

        // output
        public void Output(string filename, string ext = "png")
        {
            string fullname = filename + "." + ext;
            Console.WriteLine("\nExporting frame as image: " + fullname + "...");
            var settings = new MagickReadSettings();
            settings.Format = MagickFormat.Rgba;
            settings.Width = Width;
            settings.Height = Height;
            settings.Depth = 8;
            var image = new MagickImage(Image, settings);
            image.Write(fullname);
            
        }
    }

    public class Movie
    {
        public Vec2 Size { get; private set; }

        public int Height { get { return Size.Y; } private set { Size.Y = value; } }

        public int Width { get { return Size.X; } private set { Size.X = value; } }

        public double AspectRatio { get { return Width / Height; } }

        public int Fps { get; private set; }
        public int Duration {  get; private set; }
        public int NumFrames { get { return Fps * Duration; } }
        private int MovieID { get; set; }
        private string TempDir { get; set; }

        private static int _nextId = 0;

        public Movie(int width, int height, int duration, int fps)
        {
            Size = new Vec2(width, height);
            Duration = duration;
            Fps = fps;
            MovieID = _nextId++;
            TempDir = $"{Directory.GetCurrentDirectory()}\\temp_{MovieID}";
            Directory.CreateDirectory(TempDir);
        }

        public void Output(string filename)
        {
            string fullname = filename + ".mp4";
            Console.WriteLine($"Exporting movie: {fullname}");
            string cmd = ($"-y -v -8 -framerate {Fps} -f image2 -i temp_{MovieID}/%d.bmp -c h264 " +
                $"-pix_fmt yuv420p -b:v 32768k {fullname}");
            Console.WriteLine(cmd + "\n");
            
            if (Process.Start("ffmpeg", cmd) == null)
            {
                Console.WriteLine("Error outputting file!");
                return;
            }

            Console.WriteLine("Done.");
        }

        public void WriteFrame(Frame frame, int frameInd)
        {
            if (frameInd >= NumFrames)
            {
                throw new Exception("Invalid frame index received!");
            }

            string filename = $"{TempDir}\\{frameInd}";
            frame.Output(filename, "bmp");
        }
    }
}   
