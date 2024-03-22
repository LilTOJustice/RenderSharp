using MathSharp;
using RenderSharp.Render3d;

namespace RenderSharpExample
{
    internal class Program
    {
        static readonly int framerate = 60;
        static readonly int duration = 5;
        static readonly int resX = 1000;
        static readonly int resY = 1000;

        static void Main()
        {
            // Create scene
            Scene scene = SceneBuilder
                .MakeDynamic()
                .WithFramerate(framerate)
                .WithDuration(duration)
                .WithActor("box", new BoxActorBuilder()
                   .WithPosition(new FVec3(0, 0, 5))
                   .WithBoundingBoxSize(new FVec3(2, 1, 1))
                )
                .WithThink((SceneInstance scene, double time, double dt) =>
                {
                    scene["box"].Rotation += new RVec3(1.5 * dt, 1 * dt, 2 * dt);
                })
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}