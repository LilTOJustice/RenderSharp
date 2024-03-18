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
                .WithCamera("main",
                new FVec3(0, 0, 0),
                fov: new DVec2(90, 90))
                .WithActor("triangle", new TriangleActorBuilder()
                    .WithPosition(new FVec3(0, 0, 2))
                ).WithThink((SceneInstance scene, double time, double dt) =>
                {
                    scene["triangle"].Rotation += new RVec3(1 * dt, 2 * dt, 3 * dt);
                })
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}