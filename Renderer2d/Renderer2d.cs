using RendererCommon.Color;
using RenderSharp.Math;
using RenderSharp.RendererCommon;

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
            if (index < 0 || index >= Scene.TimeSequence.Length)
            {
                throw new ArgumentOutOfRangeException("Index argument should be >= 0 and <" +
                    " the length of the scene's time sequence.");
            }

            SceneInstance instance = Scene.StartingInstance;

            for (int i = 0; i < index; i++)
            {
                instance = instance.Think(Scene.GetTimeSequence[i], Scene.DeltaTime);
            }

            return Render(instance, Scene.TimeSequence[index], false);
        }

        public Movie RenderMovie()
        {
            if (Scene.TimeSequence.Empty)
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


        Frame Render(SceneInstance sceneInstance, double time, bool verbose = false)
        {
            Frame output = new Frame(Resolution);

            if (verbose)
            {
                Console.WriteLine($"\nBeginning actor render ({sceneInstance.Actors.Length} total)...");
            }

            var start = DateTime.Now;
            var end = start;

            int bgSpriteLeft = -sceneInstance.BgTexture?.Width / 2 ?? 0;
            int bgSpriteTop = sceneInstance.BgTexture?.Height / 2 ?? -0;
            
            for (int i = 0; i < ResY; i++)
            {
                for (int j = 0; j < ResX; j++)
                {
                    Vec2 worldLoc = sceneInstance.ScreenToWorld(Resolution, new Vec2(j, i));

                    RGBA outColor = sceneInstance.BgColor;

                    if (sceneInstance.BgTexture != null)
                    {
                        Vec2 ind = worldLoc - new Vec2(bgSpriteLeft, bgSpriteTop);
                        Vec2 vInd = new Vec2(
                            ind.X % sceneInstance.BgSprite.Width,
                            -ind.Y % sceneInstance.BgSprite.Height);
                        outColor = ColorFunctions.AlphaBlend(sceneInstance.BgTexture[vInd], outColor);
                    }

                    foreach (Actor actor in sceneInstance.Actors)
                    {
                        Vec2 actorLoc = sceneInstance.WorldToActor(actor, worldLoc);
                        actorLoc -= new Vec2(-actor.Width / 2, actor.Height / 2);
                        Vec2 actorTlLoc = new Vec2(actorLoc.X, -actorLoc.Y);

                        if (actorTlLoc.X >= actor.Width || actorTlLoc.Y >= actor.Height)
                        {
                            continue;
                        }

                        Vec2 textureInd = ((FVec2)actorTlLoc) / actor.Size * actor.Texture.Size;
                        RGBA textureSample = new RGBA(actor.Texture[textureInd].Components);

                        foreach (FragShader frag in actor.Shaders)
                        {
                            frag(new FragShaderArgs(textureSample, textureSample, textureInd, actor.Texture.Size, time));
                        }

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