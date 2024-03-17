using MathSharp;
using RenderSharp.Render3d;

namespace RenderSharpExample
{
    internal class Program
    {
        static readonly int framerate = 60;
        static readonly int duration = 3;
        static readonly int resX = 1000;
        static readonly int resY = 1000;

        static void Main()
        {
            // Create scene
            Scene scene = SceneBuilder
                .MakeStatic()
                .WithCamera("main",
                new FVec3(0, 0, 0),
                fov: new DVec2(90, 90))
                .WithActor("triangle", new TriangleActorBuilder()
                    .WithTriangle(new Triangle(
                        new FVec3(2, -Math.Sqrt(3) / 4, -0.5),
                        new FVec3(2, -Math.Sqrt(3) / 4, 0.5),
                        new FVec3(2, Math.Sqrt(3) / 4, 0))))
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderFrame().Output("test");
        }
    }
}