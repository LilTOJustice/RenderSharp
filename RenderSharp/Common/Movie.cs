using MathSharp;
using System.Diagnostics;
using Xabe.FFmpeg.Downloader;

namespace RenderSharp
{
    /// <summary>
    /// A logical collection of frames that can be exported as a video.
    /// </summary>
    public class Movie
    {
        internal Vec2 Size { get; private set; }

        internal int Height { get { return Size.Y; } }

        internal int Width { get { return Size.X; } }

        internal double AspectRatio { get { return Width / Height; } }

        internal int Framerate { get; private set; }
        
        private int MovieID { get; set; }

        private string TempDir { get; set; }

        private static int _nextId = 0;

        internal Movie(int width, int height, int framerate)
        {
            Size = new Vec2(width, height);
            Framerate = framerate;
            MovieID = _nextId++;
            TempDir = $"{Directory.GetCurrentDirectory()}/temp_{MovieID}";
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
        /// If transparent is true, exports as webm.
        /// </summary>
        /// <param name="filename">Path to the exported video.</param>
        /// <param name="transparency">Whether to export with transparency. Will export in webm if true, otherwise mp4.</param>
        public void Output(string filename, bool transparency = false)
        {
            string fullName = filename;

            if (!File.Exists("ffmpeg.exe"))
            {
                Console.Write("FFmpeg not found, downloading... ");
                FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official).Wait();
                Console.WriteLine("Done.");
            }

            Console.WriteLine($"Exporting as {fullName}...");

            string cmd = $"-y -v -8 -framerate {Framerate} -f image2 -i temp_{MovieID}/%d.bmp "
                + (transparency ?
                $"-c:v vp9 -pix_fmt yuva420p -b:v 32768k {Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{fullName}.webm" :
                $"-c:v h264 -pix_fmt yuv420p -b:v 32768k {Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{fullName}.mp4");

            Console.WriteLine("ffmpeg " + cmd);

            if (Process.Start("./ffmpeg", cmd) == null)
            {
                Console.WriteLine("Error outputting file!");
                return;
            }

            Console.WriteLine("Done.");
        }

        internal void WriteFrame(Frame frame, int frameInd)
        {
            string filename = $"{TempDir}/{frameInd}";
            frame.Output(filename, "bmp");
        }

        /// <summary>
        /// Destructs the movie by deleting the <see cref="TempDir"/>.
        /// </summary>
        ~Movie() {
            Directory.Delete(TempDir, true);
        }
    }
}
