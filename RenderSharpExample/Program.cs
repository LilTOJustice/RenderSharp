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
                        Line line = (Line)scene["line"];
                        box.Position += new FVec2(0, 50) * dt;
                        box.Rotation += 3 * dt;
                        line.Start = box.Position;
                    }
                )
                .WithActor(new ActorBuilder())
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Create some actors
            /*Actor2d actor = new(new FVec2(100, 100)); // Actor created by size 10x10 at position (0, 0)
            Line2d line = new(10, actor.Position, new FVec2(20, 0), new HSV(0, 1, 1));*/

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}