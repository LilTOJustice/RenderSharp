using RenderSharp.RendererCommon;
using RenderSharp.Math;
using RenderSharp.Scene;

namespace RenderSharp.Render2d
{
    public class Renderer2d
    {
        public Vec2 Resolution { get; set; }

        public int ResX { get { return Resolution.X; } set { Resolution.X = value; } }

        public int ResY { get { return Resolution.Y; } set { Resolution.Y = value; } }

        public Scene2d Scene { get; set; }

        public int NumThreads { get; set; }

        public Renderer2d(int resX, int resY, Scene2d scene, int numThreads)
        {
            if (resX < 1 || resY < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resX, resY);
            Scene = scene;
            NumThreads = numThreads;
        }

        public Renderer2d(Vec2 resolution, Scene2d scene, int numThreads)
        {
            if (resolution.X < 1 || resolution.Y < 1)
            {
                throw new ArgumentOutOfRangeException("Resolution must be >= 1 for both axes.");
            }

            Resolution = new Vec2(resolution.Components);
            Scene = scene;
            NumThreads = numThreads;
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

            return Render(Scene, Scene.TimeSeq[index], false);

        }

        public Movie RenderMovie()
        {
            if (Scene.TimeSeq.Count == 0)
            {
                throw new Exception("Attempted to render a movie on a static scene.");
            }

            
        }

        static int loadSeqInd = 0;
        const string loadSeq = "|/-\\";
        static void PrintBar(int frameIndex, int numFrames, int totalBars)
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

            Console.Write($"] {frameIndex} / {numFrames} ({(100d * frameIndex / numFrames).ToString(".3f")}%)" +
                $"{(frameIndex == numFrames ? ' ' : loadSeq[(loadSeqInd++) % loadSeq.Length])}";
            Console.Out.Flush();
        }


        Frame Render(Scene2d scene, double time, bool verbose = false)
        {
            Frame output = new Frame(Resolution);

            if (verbose)
            {
                Console.WriteLine($"\nBeginning actor render ({scene.Actors.Count} total)...");
            }

            var start = DateTime.Now;
            var end = start;

            int bgSpriteLeft = -scene.BgTexture?.Width / 2 ?? 0;
            int bgSpriteTop = scene.BgTexture?.Height / 2 ?? 0;
            
            for (int i = 0; i < ResY; i++)
            {
                for (int j = 0; j < ResX; j++)
                {
                    Vec2 worldLoc = scene.ScreenToWorld(Resolution, new Vec2(j, i));
                    RGBA outColor = scene.BgColor;

                    if (scene.BgTexture != null)
                    {
                        Vec2 ind = worldLoc - new Vec2(bgSpriteLeft, bgSpriteTop);
                        outColor = ColorFunctions.AlphaBlend(scene.BgTexture[ind.X % scene.BgTexture.Width, -ind.Y % scene.BgTexture.Height], outColor);
                    }

                    foreach (var actor in scene.Actors)
                    {
                        Vec2 actorLoc = Scene2d.WorldToActor(actor, worldLoc);
                        actorLoc -= new Vec2(-actor.Width / 2, actor.Height / 2);
                        Vec2 actorTlLoc = new Vec2(actorLoc.X, -actorLoc.Y);

                        if (actorTlLoc.X >= actor.Width || actorTlLoc.Y >= actor.Height)
                        {
                            continue;
                        }

                        Vec2 textureInd = (Vec2)(((FVec2)actorTlLoc) / actor.Size * actor.Texture.Size);
                        RGBA textureSample = new RGBA(actor.Texture[textureInd.X, textureInd.Y].Components);

                        actor.Shader(new FragShaderArgs(textureSample, textureSample, textureInd, actor.Texture.Size, time));

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