using MathSharp;
using System.Diagnostics;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// Renderer for 2d scenes (<see cref="Scene2d"/>). Used for rendering scenes into <see cref="Frame"/>s or <see cref="Movie"/>s.
    /// </summary>
    public class Renderer2d
    {
        /// <summary>
        /// Target resolution for the renderer.
        /// </summary>
        public Vec2 Resolution { get; set; }

        /// <summary>
        /// Width from the renderer's target resolution (<see cref="Resolution"/>).
        /// </summary>
        public int Width { get { return Resolution.X; } set { Resolution.X = value; } }

        /// <summary>
        /// Height from the renderer's target resolution (<see cref="Resolution"/>).
        /// </summary>
        public int Height { get { return Resolution.Y; } set { Resolution.Y = value; } }

        /// <summary>
        /// Target scene for the render.
        /// </summary>
        public Scene2d Scene { get; set; }
        
        /// <summary>
        /// Final shader delegate run on every pixel of every frame of the render.
        /// </summary>
        public FragShader Shader { get; set; }

        /// <inheritdoc cref="Renderer2d"/>
        public Renderer2d(int resX, int resY, Scene2d scene, FragShader? shader = null)
        {
            if (resX < 1 || resY < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resX, resY);
            Scene = scene;
            Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
        }

        /// <inheritdoc cref="Renderer2d"/>
        public Renderer2d(Vec2 resolution, Scene2d scene, FragShader? shader = null)
        {
            if (resolution.X < 1 || resolution.Y < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resolution.Components);
            Scene = scene;
            Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
        }

        /// <summary>
        /// Renders a single frame given the index, or the only frame for a static scene. If the scene is dynamic,
        /// the scene will simulate up to the given frame index and the final frame will be rendered and returned.
        /// </summary>
        /// <param name="index">Which frame to render.</param>
        /// <returns>The desired rendered frame, based on the index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is negative or too large for the scene's duration.</exception>
        public Frame RenderFrame(int index = 0)
        {
            if (index < 0 || index >= Scene.TimeSeq.Count)
            {
                throw new ArgumentOutOfRangeException("Index argument should be >= 0 and <" +
                    " the length of the scene's time sequence.");
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.Write($"Simulating to frame index {index}...");
            Scene2dInstance sceneInstance = Scene.Simulate(index).Last();
            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.Elapsed}");
            return Render(sceneInstance, Scene.BgTexture, true);
        }

        /// <summary>
        /// Renders all frames from the <see cref="Scene"/>, and produces a <see cref="Movie"/> that can be exported.
        /// </summary>
        /// <returns>The rendered movie.</returns>
        /// <exception cref="Exception">Thrown if this method is called when the <see cref="Scene"/> is static (has a <see cref="Scene2d.TimeSeq"/> of length 0)</exception>
        public Movie RenderMovie()
        {
            if (Scene.TimeSeq.Count == 0)
            {
                throw new Exception("Attempted to render a movie on a static scene.");
            }

            Console.Write("Simulating... ");
            Stopwatch stopwatch = Stopwatch.StartNew();

            Movie movie = new(Width, Height, Scene.Framerate);
            List<Scene2dInstance> instances = Scene.Simulate();

            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.Elapsed}");

            int numThreads = Environment.ProcessorCount;
            List<Thread> threads = new(numThreads);
            int nextId = -1;
            int doneCount = 0;
            var threadRender = () =>
            {
                for (int i = Interlocked.Increment(ref nextId); i < instances.Count; i = Interlocked.Increment(ref nextId))
                {
                    var instance = instances[i];
                    movie.WriteFrame(Render(instance, Scene.BgTexture), instance.Index);
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
            Console.WriteLine($"Waiting for {threads.Count} threads...");

            while (doneCount <= instances.Count)
            {
                PrintBar(doneCount, instances.Count, timeElapsed: stopwatch.Elapsed.ToString());
                if (doneCount == instances.Count)
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

        /// <summary>
        /// Prints a loading bar to the screen given the progress and settings.
        /// </summary>
        /// <param name="frameIndex">Current progress.</param>
        /// <param name="numFrames">Target progress.</param>
        /// <param name="totalBars">Loading bar length in characters.</param>
        /// <param name="timeElapsed">Optional time to show next to the bar.</param>
        static void PrintBar(int frameIndex, int numFrames, int totalBars = 50, string timeElapsed = "")
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

        /// <summary>
        /// Rendering pipeline for a single frame.
        /// </summary>
        /// <param name="scene">Scene instance to render.</param>
        /// <param name="bgTexture">Optional background texture to render if no actors are intersected.</param>
        /// <param name="verbose">Whether to print status updates and time info.</param>
        /// <returns>The rendered frame.</returns>
        private Frame Render(Scene2dInstance scene, Texture bgTexture, bool verbose = false)
        {
            Frame output = new(Resolution);

            if (verbose)
            {
                Console.WriteLine($"Beginning actor render...");
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    FVec2 worldLoc = Util.Transforms.ScreenToWorld2(Resolution, new Vec2(x, y), scene.Camera.Center, scene.Camera.Zoom, scene.Camera.Rotation);
                    Vec2 bgTextureInd = Util.Transforms.WorldToBgTexture2(worldLoc, bgTexture.Size);
                    RGBA outColor = bgTexture[bgTextureInd.X, bgTextureInd.Y];

                    FRGBA fOut = outColor;
                    Scene.Shader(fOut, out fOut, bgTextureInd, bgTexture.Size, scene.Time);
                    outColor = fOut;

                    for (int i = scene.ActorIndex.Count - 1; i >= 0; i--)
                    {
                        foreach (var actor in scene.ActorIndex[i].Values)
                        {
                            FVec2? actorLoc = Util.Transforms.WorldToActor2(worldLoc, actor.Position, actor.Size, actor.Rotation);
                            if (actorLoc is null)
                            {
                                continue;
                            }

                            Vec2? textureInd = Util.Transforms.ActorToTexture2(actorLoc, actor.Size, actor.Texture.Size);
                            if (textureInd is null)
                            {
                                continue;
                            }

                            RGBA textureSample = actor.Texture[textureInd.X, textureInd.Y];

                            fOut = textureSample;
                            actor.Shader(fOut, out fOut, textureInd, actor.Texture.Size, scene.Time);
                            textureSample = fOut;

                            outColor = ColorFunctions.AlphaBlend(textureSample, outColor);
                        }
                    }

                    fOut = outColor;
                    Shader(fOut, out fOut, new Vec2(x, y), Resolution, scene.Time);
                    outColor = fOut;

                    output[x, y] = (RGB)outColor;
                }
            }

            stopwatch.Stop();

            if (verbose)
            {
                Console.WriteLine($"Done! ({stopwatch.Elapsed})\n\nRender complete.");
            }

            return output;
        }

        /// <summary>
        /// Clears all <see cref="Shader"/>s for the renderer.
        /// </summary>
        public void ClearShaders()
        {
            Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; }; 
        }
    }
}