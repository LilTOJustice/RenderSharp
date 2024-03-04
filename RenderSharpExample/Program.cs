using MathSharp;
using RenderSharp;
using RenderSharp.Render2d;

namespace RenderSharpExample
{
    internal class Program
    {
        static readonly int framerate = 60;
        static readonly int duration = 3;
        static readonly int resX = 600;
        static readonly int resY = 600;

        static void Main()
        {
            // Create scene
            Scene scene = SceneBuilder
                .MakeDynamic()
                .WithFramerate(framerate)
                .WithDuration(duration)
                .WithThink(
                    (SceneInstance scene, double time, double dt) =>
                    {
                        Actor box = scene.GetActor("Box");
                        Line line = scene.GetLine("Line");
                        box.Position += new FVec2(0, 0.1) * dt;
                        line.Start = box.Position;
                    }
                )
                .WithActor(new ActorBuilder()
                    .WithSize(new FVec2(0.1, 0.05))
                    .WithTexture(new Texture(new Vec2(2, 2)))
                    .WithShader(ExampleShaders.TopLeftDebug)
                    .WithShader(ExampleShaders.Ghostly)
                    , "Box")
                .WithActor(new LineBuilder()
                    .WithThickness(0.001)
                    .WithEnd(new FVec2(0.8, 0))
                    .WithColor(new RGB(255, 0, 0))
                    .WithShader(ExampleShaders.Psychedelic)
                    , "Line")
                .WithBgColor(new RGB(0, 0, 255))
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}