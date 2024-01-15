using RenderSharp.RendererCommon;
using RenderSharp.Math;
using RenderSharp.Scene;
using RendererCommon;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RenderSharp.Render2d
{
    public class Renderer2d
    {
        public Vec2 Resolution { get; set; }

        public int Width { get { return Resolution.X; } set { Resolution.X = value; } }

        public int Height { get { return Resolution.Y; } set { Resolution.Y = value; } }

        public Scene2d Scene { get; set; }
        
        public FragShader Shader { get; set; }

        public Renderer2d(int resX, int resY, Scene2d scene, FragShader? shader = null)
        {
            if (resX < 1 || resY < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resX, resY);
            Scene = scene;
            Shader = shader ?? ((in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
        }

        public Renderer2d(Vec2 resolution, Scene2d scene, FragShader? shader = null)
        {
            if (resolution.X < 1 || resolution.Y < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resolution.Components);
            Scene = scene;
            Shader = shader ?? ((in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
        }

        public Frame RenderFrame(int index = 0)
        {
            if (index < 0 || index >= Scene.TimeSeq.Count)
            {
                throw new ArgumentOutOfRangeException("Index argument should be >= 0 and <" +
                    " the length of the scene's time sequence.");
            }

            for (int i = 0; i < index; i++)
            {
                Scene.ThinkFunc(new Scene2dThinkFuncArgs(Scene.TimeSeq[i], Scene.DeltaTime));
            }

            return Render(new Scene2dInstance(Scene, Scene.TimeSeq[index], index), Scene.BgTexture, Scene.BgColor ?? (RGBA?)null, true);
        }

        public Movie RenderMovie()
        {
            if (Scene.TimeSeq.Count == 0)
            {
                throw new Exception("Attempted to render a movie on a static scene.");
            }

            Console.Write("Simulating... ");
            Stopwatch stopwatch = Stopwatch.StartNew();

            Movie movie = new(Width, Height, Scene.Framerate);
            List<Scene2dInstance> instances = new(Scene.TimeSeq.Count);
            for (int i = 0; i < Scene.TimeSeq.Count; i++)
            {
                double time = Scene.TimeSeq[i];
                Scene.ThinkFunc(new Scene2dThinkFuncArgs(time, Scene.DeltaTime));
                instances.Add(new Scene2dInstance(Scene, time, i));
            }

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
                    movie.WriteFrame(Render(instance, Scene.BgTexture, Scene.BgColor ?? (RGBA?)null), instance.Index);
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


        private Frame Render(Scene2dInstance scene, Texture? bgTexture = null, RGBA? bgColor = null, bool verbose = false)
        {
            Frame output = new(Resolution);

            if (verbose)
            {
                Console.WriteLine($"Beginning actor render ({scene.Actors.Count} total)...");
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            bgTexture ??= new Texture(Resolution, bgColor ?? new RGB());

            int bgSpriteLeft = -bgTexture!.Width / 2;
            int bgSpriteTop = bgTexture!.Height / 2;
            
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vec2 worldLoc = scene.ScreenToWorld(Resolution, new Vec2(x, y));

                    Vec2 ind = worldLoc - new Vec2(bgSpriteLeft, bgSpriteTop);
                    RGBA outColor = bgTexture[Util.Mod(ind.X, bgTexture.Width), Util.Mod(ind.Y, bgTexture.Height)];
                    Scene.Shader(outColor, out outColor, ind, bgTexture.Size, scene.Time);
                    outColor = ColorFunctions.AlphaBlend(outColor, outColor);

                    foreach (var actor in scene.Actors)
                    {
                        Vec2 actorLoc = Scene2dInstance.WorldToActor(actor, worldLoc);

                        if (actorLoc.X < -actor.Width / 2 || actorLoc.Y < -actor.Height / 2 || actorLoc.X >= actor.Width / 2 || actorLoc.Y >= actor.Height / 2)
                        {
                            continue;
                        }

                        Vec2 textureInd = actor.ActorToTexture(actorLoc);
                        RGBA textureSample = new (actor.Texture[textureInd.X, textureInd.Y].Components);

                        actor.Shader(textureSample, out textureSample, textureInd, actor.Texture.Size, scene.Time);

                        outColor = ColorFunctions.AlphaBlend(textureSample, outColor);
                    }

                    Shader(outColor, out outColor, new Vec2(x, y), Resolution, scene.Time);

                    output[x, y] = outColor;
                }
            }

            stopwatch.Stop();

            if (verbose)
            {
                Console.WriteLine($"Done! ({stopwatch.Elapsed})\n\nRender complete.");
            }

            return output;
        }

        public void ClearShaders()
        {
            Shader = (in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; }; 
        }
    }
}