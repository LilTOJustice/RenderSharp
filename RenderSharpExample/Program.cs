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
                   .WithBoundingBoxSize(new FVec3(1, 1, 1))
                )
                .WithActor("sphere", new SphereActorBuilder()
                    .WithPosition(new FVec3(0, 0, 5))
                    .WithBoundingBoxSize(new FVec3(2, 1, 1)))
                .WithThink((SceneInstance scene, double time, double dt) =>
                {
                    scene["box"].Rotation += new RVec3(0, 1 * dt, 0);
                    //scene["sphere"].Rotation += new RVec3(0, 1 * dt, 0);
                    //scene["box"].Position += new FVec3(0, 0, -2 * dt);
                    //scene.Camera.Position += new FVec3(0, 0, -1 * dt);
                })
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie(showDepth: false).Output("test");
        }
    }
}