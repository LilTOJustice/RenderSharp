using ImageMagick;
using RenderSharp.Math;
using System.Diagnostics;

namespace RenderSharp.RendererCommon
{
    public class Frame
    {   
        public Vec2 Size { get; private set; }

        public int Height { get { return Size.Y; } private set { Size.Y = value; } }

        public int Width { get { return Size.X; } private set { Size.X = value; } }

        public double AspectRatio { get { return Width / Height; } }

        private byte[] Image { get; set; }

        public Frame(Vec2 size)
        {
            Size = size;
            Image = new byte[Width * Height * 3];
        }

        public Frame(int width, int height)
        {
            Size = new Vec2(width, height);
            Image = new byte[width * height * 3];
        }

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

    public class Movie
    {
        public Vec2 Size { get; private set; }

        public int Height { get { return Size.Y; } private set { Size.Y = value; } }

        public int Width { get { return Size.X; } private set { Size.X = value; } }

        public double AspectRatio { get { return Width / Height; } }

        public int Framerate { get; private set; }
        
        private int MovieID { get; set; }

        private string TempDir { get; set; }

        private static int _nextId = 0;

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
