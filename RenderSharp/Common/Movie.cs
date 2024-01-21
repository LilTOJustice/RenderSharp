﻿using MathSharp;
using System.Diagnostics;
using Xabe.FFmpeg.Downloader;

namespace RenderSharp
{
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
                $"-c:v vp9 -pix_fmt yuva420p -b:v 32768k {fullName}.webm" :
                $"-c:v h264 -pix_fmt yuv420p -b:v 32768k {fullName}.mp4");

            Console.WriteLine(cmd);

            if (Process.Start(".\\ffmpeg", cmd) == null)
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

        /// <summary>
        /// Destructs the movie by deleting the <see cref="TempDir"/>
        /// </summary>
        ~Movie() {
            Directory.Delete(TempDir, true);
        }
    }
}
