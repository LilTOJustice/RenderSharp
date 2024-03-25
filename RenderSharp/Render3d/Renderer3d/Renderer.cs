using MathSharp;
using System.Diagnostics;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// NOT FULLY IMPLEMENTED YET.
    /// Renderer for 3d scenes (<see cref="Scene"/>). Used for rendering scenes into <see cref="Frame"/>s or <see cref="Movie"/>s.
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// Target resolution for the renderer.
        /// </summary>
        public Vec2 Resolution { get; set; }

        /// <summary>
        /// Width from the renderer's target resolution (<see cref="Resolution"/>).
        /// </summary>
        public int Width { get { return Resolution.X; } }

        /// <summary>
        /// Height from the renderer's target resolution (<see cref="Resolution"/>).
        /// </summary>
        public int Height { get { return Resolution.Y; } }

        /// <summary>
        /// Aspect ratio of the renderer's target resolution (<see cref="Resolution"/>).
        /// </summary>
        public double AspectRatio { get { return (double)Width / Height; } }

        /// <summary>
        /// Target scene for the render.
        /// </summary>
        public Scene Scene { get; }
        
        /// <summary>
        /// Final shader delegate run on every pixel of every frame of the render.
        /// </summary>
        public FragShader FragShader { get; set; }

        /// <summary>
        /// First shader run to transform screen space coordinates.
        /// </summary>
        public CoordShader CoordShader { get; set; }

        /// <inheritdoc cref="Renderer"/>
        public Renderer(int resX, int resY, Scene scene, FragShader? fragShader = null, CoordShader? coordShader = null)
        {
            if (resX < 1 || resY < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resX, resY);
            Scene = scene;
            FragShader = fragShader ?? ((FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            CoordShader = coordShader ?? ((Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; });
        }

        /// <inheritdoc cref="Renderer"/>
        public Renderer(in Vec2 resolution, Scene scene, FragShader? fragShader = null, CoordShader? coordShader = null)
        {
            if (resolution.X < 1 || resolution.Y < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = resolution;
            Scene = scene;
            FragShader = fragShader ?? ((FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            CoordShader = coordShader ?? ((Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; });
        }

        /// <summary>
        /// Renders a single frame given the index, or the only frame for a static scene. If the scene is dynamic,
        /// the scene will simulate up to the given frame index and the final frame will be rendered and returned.
        /// </summary>
        /// <param name="index">Which frame to render.</param>
        /// <param name="showDepth">Whether to output a depth map rather than color.</param>
        /// <returns>The desired rendered frame, based on the index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is negative or too large for the scene's duration.</exception>
        public Frame RenderFrame(int index = 0, bool showDepth = false)
        {
            if (index < 0 || (index >= Scene.TimeSeq.Count && Scene.TimeSeq.Count != 0))
            {
                throw new ArgumentOutOfRangeException("Index argument should be >= 0 and <" +
                    " the length of the scene's time sequence.");
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.Write($"Simulating to frame index {index}... ");
            SceneInstance sceneInstance = Scene.Simulate(index).Last();
            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.Elapsed}");
            return Render(sceneInstance, true, showDepth);
        }

        /// <summary>
        /// Renders all frames from the <see cref="Scene"/>, and produces a <see cref="Movie"/> that can be exported.
        /// </summary>
        /// <param name="showDepth">Whether to output a depth map rather than color.</param>
        /// <returns>The rendered movie.</returns>
        /// <exception cref="Exception">Thrown if this method is called when the <see cref="Scene"/> is static (has a <see cref="Scene.TimeSeq"/> of length 0)</exception>
        public Movie RenderMovie(bool showDepth = false)
        {
            if (Scene.TimeSeq.Count == 0)
            {
                throw new Exception("Attempted to render a movie on a static scene.");
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            Movie movie = new(Width, Height, Scene.Framerate);
            List<SceneInstance> instances = Scene.Simulate();

            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.Elapsed}");

            int numThreads = Environment.ProcessorCount;
            List<Thread> threads = new();
            int nextId = -1;
            int doneCount = 0;
            var threadRender = () =>
            {
                for (int i = Interlocked.Increment(ref nextId); i < instances.Count; i = Interlocked.Increment(ref nextId))
                {
                    var instance = instances[i];
                    movie.WriteFrame(Render(instance, false, showDepth), instance.Index);
                    Interlocked.Increment(ref doneCount);
                }
            };

            for (int i = 0; i < numThreads; i++)
            {
                Thread thread = new Thread(() => { threadRender(); });
                threads.Add(thread);
                thread.Start();
            }

            stopwatch = Stopwatch.StartNew();
            int instanceCount = instances.Count;
            Console.WriteLine($"Waiting for {threads.Count} threads to render {instanceCount} frames at {Width}x{Height}@{Scene.Framerate}...");

            while (doneCount <= instanceCount)
            {
                PrintBar(doneCount, instances.Count, timeElapsed: stopwatch.Elapsed.ToString());
                if (doneCount == instanceCount)
                {
                    stopwatch.Stop();
                    break;
                }

                Thread.Sleep(500);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine($"\nFinished in {stopwatch.Elapsed}");

            return movie;
        }

        static int loadSeqInd = 0;
        static string loadSeq = "|/-\\";

        internal static void PrintBar(int frameIndex, int numFrames, int totalBars = 50, string timeElapsed = "")
        {
            int numBars = (int)(1d * frameIndex / numFrames * totalBars);

            Console.Write("\r[");
            for (int i = 0; i < numBars; i++)
            {
                Console.Write('|');
            }

            for (int i = 0; i <  totalBars - numBars; i++)
            {
                Console.Write(' ');
            }

            Console.Write($"] {frameIndex} / {numFrames} ({string.Format("{0:0.00}", 100d * frameIndex / numFrames)}%)" +
                $" {(frameIndex == numFrames ? ' ' : loadSeq[(loadSeqInd++) % loadSeq.Length])} " + timeElapsed);
            Console.Out.Flush();
        }

        private Frame Render(SceneInstance scene, bool verbose, bool showDepth = false)
        {
            Frame output = new(Resolution);
            double[,] depthBuffer = new double[Width, Height];
            double maxDepth = 0;

            if (verbose)
            {
                Console.WriteLine($"Beginning render on {scene.Actors.Count} at {Width}x{Height}...");
                Console.WriteLine($"Rendering {scene.VertexCount} vertices ({scene.TriangleCount} triangles).");
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double depth;
                    output[x, y] = RenderPixel(scene, x, y, out depth);
                    maxDepth = Math.Max(maxDepth, depth);
                    depthBuffer[x, y] = depth;
                }
            }

            if (showDepth)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        double scaled = depthBuffer[x, y] == -1 || maxDepth == 0 ?
                            0 : 1 - depthBuffer[x, y] / maxDepth;
                        output[x, y] = new FRGB(scaled, scaled, scaled);
                    }
                }
            }

            stopwatch.Stop();

            if (verbose)
            {
                Console.WriteLine($"Done! ({stopwatch.Elapsed})\n\nRender complete.");
            }

            return output;
        }

        private RGBA RenderPixel(SceneInstance scene, int x, int y, out double depth)
        {
            Vec2 screenPos = new(x, y);
            CoordShader(screenPos, out screenPos, Resolution, scene.Time);
            FVec3 worldVec = Transforms.ScreenToWorldVec(screenPos, Resolution, scene.Camera);
            double minDepth = scene.Camera.FocalLength == 0 ? 0 : worldVec.Mag();
            FVec3 cameraPos = scene.Camera.Position;
            if (scene.Camera.FocalLength != 0)
            {
                worldVec = worldVec / minDepth;
            }
            else
            {
                worldVec.Z = 0;
                cameraPos += worldVec;
                worldVec = new FVec3(0, 0, 1).Rotate(scene.Camera.Rotation);
            }

            RGBA outColor = new();
            
            List<(RGBA, double)> renderQueue = new();
        
            foreach (Actor actor in scene.Actors.Values)
            {
                RGBA sample;
                double sampleDepth;
                if (actor.Sample(worldVec, cameraPos, minDepth, scene.Time, out sample, out sampleDepth))
                {
                    renderQueue.Add((sample, sampleDepth));
                }
            }

            renderQueue.Sort((a, b)  => b.Item2.CompareTo(a.Item2));
            depth = -1;

            foreach ((RGBA sample, double d) in renderQueue)
            {
                outColor = ColorFunctions.AlphaBlend(sample, outColor);
                depth = d;
            }

            return ScreenSpaceShaderPass(scene, screenPos, outColor);
        }

        private RGBA ScreenSpaceShaderPass(SceneInstance scene, Vec2 screenPos, RGBA inColor)
        {
            FRGBA fOut = inColor;
            FragShader(fOut, out fOut, screenPos, Resolution, scene.Time);
            return fOut;
        }

        /// <summary>
        /// Clears the fragment shader for the renderer.
        /// </summary>
        public void ClearFragShader()
        {
            FragShader = (FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
        }

        /// <summary>
        /// Clears the coordinate shader for the renderer.
        /// </summary>
        public void ClearCoordShader()
        {
            CoordShader = (Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; };
        }

        /// <summary>
        /// Clears all shaders for the renderer.
        /// </summary>
        public void ClearShaders()
        {
            ClearFragShader();
            ClearCoordShader();
        }
    }
}