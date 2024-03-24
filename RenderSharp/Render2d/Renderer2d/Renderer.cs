using MathSharp;
using System.Diagnostics;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// Renderer for 2d scenes (<see cref="Scene"/>). Used for rendering scenes into <see cref="Frame"/>s or <see cref="Movie"/>s.
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

        private Func<Vec2, FVec2, Vec2> BgTextureTransform { get; set; }

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
            BgTextureTransform = scene.BgTextureWorldSize.X == 0 || scene.BgTextureWorldSize.Y == 0 ?
                new Func<Vec2, FVec2, Vec2>(
                    (Vec2 screenPos, FVec2 worldLoc)
                    => Transforms.ScreenToStretchBgTexture(screenPos, Resolution, scene.BgTexture.Size)) :
                new Func<Vec2, FVec2, Vec2>(
                    (Vec2 screenPos, FVec2 worldLoc) => Transforms.WorldToBgTexture(worldLoc, scene.BgTexture.Size, scene.BgTextureWorldSize));
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
            BgTextureTransform = scene.BgTextureWorldSize.X == 0 || scene.BgTextureWorldSize.Y == 0 ?
                new Func<Vec2, FVec2, Vec2>(
                    (Vec2 screenPos, FVec2 worldLoc)
                    => Transforms.ScreenToStretchBgTexture(screenPos, Resolution, scene.BgTexture.Size)) :
                new Func<Vec2, FVec2, Vec2>(
                    (Vec2 screenPos, FVec2 worldLoc) => Transforms.WorldToBgTexture(worldLoc, scene.BgTexture.Size, scene.BgTextureWorldSize));
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
            Console.Write($"Simulating to frame index {index}... ");
            SceneInstance sceneInstance = Scene.Simulate(index).Last();
            stopwatch.Stop();
            Console.WriteLine($"Finished in {stopwatch.Elapsed}");
            return Render(sceneInstance, true);
        }

        /// <summary>
        /// Renders all frames from the <see cref="Scene"/>, and produces a <see cref="Movie"/> that can be exported.
        /// </summary>
        /// <returns>The rendered movie.</returns>
        /// <exception cref="Exception">Thrown if this method is called when the <see cref="Scene"/> is static (has a <see cref="Scene.TimeSeq"/> of length 0)</exception>
        public Movie RenderMovie()
        {
            if (Scene.TimeSeq.Count == 0)
            {
                throw new Exception("Attempted to render a movie on a static scene.");
            }

            Console.Write("Simulating... ");
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
                    movie.WriteFrame(Render(instance), instance.Index);
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

        private Frame Render(SceneInstance scene, bool verbose = false)
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
                    output[x, y] = RenderPixel(scene, x, y);
                }
            }

            stopwatch.Stop();

            if (verbose)
            {
                Console.WriteLine($"Done! ({stopwatch.Elapsed})\n\nRender complete.");
            }

            return output;
        }

        private RGBA RenderPixel(SceneInstance scene, int x, int y)
        {
            Vec2 screenPos = new(x, y);

            CoordShader(screenPos, out screenPos, Resolution, scene.Time);

            FVec2 worldLoc = Transforms.ScreenToWorld(
                Resolution,
                AspectRatio,
                screenPos,
                scene.Camera.Center,
                scene.Camera.Zoom,
                scene.Camera.Rotation
            );

            RGBA outColor = SampleFromBgTexture(scene, screenPos, worldLoc);

            for (int i = scene.ActorIndex.Count - 1; i >= 0; i--)
            {
                foreach (var actor in scene.ActorIndex[i].Values)
                {
                    outColor = SampleFromActor(scene, actor, outColor, worldLoc);
                }
            }

            return ScreenSpaceShaderPass(scene, screenPos, outColor);
        }

        private RGBA SampleFromBgTexture(SceneInstance scene, in Vec2 screenPos, in FVec2 worldLoc)
        {
            // 
            Vec2 bgTextureInd = BgTextureTransform(screenPos, worldLoc);
                //Transforms.ScreenToStretchBgTexture(screenPos, Resolution, scene.BgTexture.Size)
                //: Transforms.WorldToBgTexture2(worldLoc, scene.BgTexture.Size, scene.BgTextureWorldSize);
            Scene.BgCoordShader(bgTextureInd, out bgTextureInd, Resolution, scene.Time);
            RGBA outColor = scene.BgTexture[
                (int)Operations.Mod(bgTextureInd.X, scene.BgTexture.Width),
                (int)Operations.Mod(bgTextureInd.Y, scene.BgTexture.Height)];

            FRGBA fOut = outColor;
            Scene.BgFragShader(fOut, out fOut, bgTextureInd, scene.BgTexture.Size, scene.Time);
            return fOut.ToRGBA();
        }

        private RGBA SampleFromActor(SceneInstance scene, Actor actor, in RGBA inColor, in FVec2 worldLoc)
        {
            FVec2 actorLoc;
            if (!Transforms.WorldToActor(worldLoc, actor.Position, actor.Size, actor.Rotation, out actorLoc))
            {
                return inColor;
            }

            Vec2 textureInd;
            if (!Transforms.ActorToTexture(actorLoc, actor.Size, actor.Texture.Size, out textureInd))
            {
                return inColor;
            }

            actor.CoordShader(textureInd, out textureInd, actor.Texture.Size, scene.Time);
            RGBA textureSample = actor.Texture[textureInd.X, textureInd.Y];

            FRGBA fOut = textureSample;
            actor.FragShader(fOut, out fOut, textureInd, actor.Texture.Size, scene.Time);
            textureSample = fOut;

            return ColorFunctions.AlphaBlend(textureSample, inColor);
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