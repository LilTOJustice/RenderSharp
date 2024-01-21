using MathSharp;
using RenderSharp;
using RenderSharp.Render2d;

namespace RenderSharpExample
{
    internal class Program
    {
        static int framerate = 60;
        static int duration = 3;
        static int resX = 300;
        static int resY = 300;

        static void Main(string[] args)
        {
            // Create scene
            Scene scene = new SceneBuilder()
                .AsDynamic()
                .WithFramerate(framerate)
                .WithDuration(duration)
                .WithThink(
                    (SceneInstance scene, double time, double dt) =>
                    {
                        Actor box = scene["Box"];
                        Line line = (Line)scene["Line"];
                        box.Position += new FVec2(0, 50) * dt;
                        box.Rotation += 3 * dt;
                        line.Start = box.Position;
                    }
                )
                .WithActor(new ActorBuilder()
                    .WithSize(new FVec2(100, 100))
                    .WithShader(ExampleShaders.Ghostly), "Box")
                .WithActor(new LineBuilder()
                    .WithThickness(10)
                    .WithEnd(new FVec2(20, 0))
                    .WithColor(new RGB(255, 0, 0))
                    .WithShader(ExampleShaders.Psychedelic), "Line")
                .WithBgColor(new RGB(0, 0, 255))
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}