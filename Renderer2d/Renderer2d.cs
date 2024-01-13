using RenderSharp.RendererCommon;
using RenderSharp.Math;
using RenderSharp.Scene;
using RendererCommon;
using System.Diagnostics;

namespace RenderSharp.Render2d
{
    public class Renderer2d
    {
        public Vec2 Resolution { get; set; }

        public int Width { get { return Resolution.X; } set { Resolution.X = value; } }

        public int Height { get { return Resolution.Y; } set { Resolution.Y = value; } }

        public Scene2d Scene { get; set; }

        public Renderer2d(int resX, int resY, Scene2d scene)
        {
            if (resX < 1 || resY < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resX, resY);
            Scene = scene;
        }

        public Renderer2d(Vec2 resolution, Scene2d scene)
        {
            if (resolution.X < 1 || resolution.Y < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resolution.Components);
            Scene = scene;
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

            Movie movie = new(Width, Height, Scene.Framerate);
            List<Scene2dInstance> instances = new(Scene.TimeSeq.Count);
            for (int i = 0; i < Scene.TimeSeq.Count; i++)
            {
                double time = Scene.TimeSeq[i];
                Scene.ThinkFunc(new Scene2dThinkFuncArgs(time, Scene.DeltaTime));
                instances.Add(new Scene2dInstance(Scene, time, i));
            }

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

            Stopwatch stopwatch = Stopwatch.StartNew();
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


        Frame Render(Scene2dInstance scene, Texture? bgTexture = null, RGBA? bgColor = null, bool verbose = false)
        {
            Frame output = new(Resolution);

            if (verbose)
            {
                Console.WriteLine($"\nBeginning actor render ({scene.Actors.Count} total)...");
            }

            var start = DateTime.Now;
            var end = start;

            bgTexture ??= new Texture(Resolution);

            int bgSpriteLeft = -bgTexture!.Width / 2;
            int bgSpriteTop = bgTexture!.Height / 2;
            
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Vec2 worldLoc = scene.ScreenToWorld(Resolution, new Vec2(j, i));
                    RGBA outColor = bgColor ?? new RGBA(0, 0, 0, 255);

                    Vec2 ind = worldLoc - new Vec2(bgSpriteLeft, bgSpriteTop);
                    outColor = bgTexture[Util.Mod(ind.X, bgTexture.Width), Util.Mod(ind.Y, bgTexture.Height)];
                    Scene.Shader(outColor, out outColor, ind, bgTexture.Size, scene.Time);
                    outColor = ColorFunctions.AlphaBlend(outColor, outColor);

                    foreach (var actor in scene.Actors)
                    {
                        Vec2 actorLoc = Scene2dInstance.WorldToActor(actor, worldLoc);
                        actorLoc -= new Vec2(-actor.Width / 2, actor.Height / 2);
                        Vec2 actorTlLoc = new Vec2(actorLoc.X, -actorLoc.Y);

                        if (actorTlLoc.X >= actor.Width || actorTlLoc.Y >= actor.Height)
                        {
                            continue;
                        }

                        Vec2 textureInd = (Vec2)(((FVec2)actorTlLoc) / actor.Size * actor.Texture.Size);
                        RGBA textureSample = new RGBA(actor.Texture[textureInd.X, textureInd.Y].Components);

                        actor.Shader(textureSample, out textureSample, textureInd, actor.Texture.Size, scene.Time);

                        outColor = ColorFunctions.AlphaBlend(textureSample, outColor);
                    }

                    output[j, i] = outColor;
                }
            }

            if (verbose)
            {
                end = DateTime.Now;
                Console.WriteLine($"Done! ({(end - start).TotalSeconds}s)\n\nRender complete.");
            }

            return output;
        }

    }
}