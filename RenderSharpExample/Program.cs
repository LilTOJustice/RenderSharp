using MathSharp;
using RenderSharp;
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
                .WithActor("cube", new CubeActorBuilder()
                   .WithPosition(new FVec3(0, 0, 4))
                   .WithSize(new FVec3(1, 1, 1))
                   .WithColor(new RGBA(255, 0, 0, 128))
                   .WithShader(ExampleShaders.Psychedelic)
                )
                .WithActor("sphere", new SphereActorBuilder()
                    .WithPosition(new FVec3(0, 0, 4))
                    .WithSize(new FVec3(1, 1, 1))
                    .WithTexture(new Texture("../../../assets/gordon.jpg"))
                )
                .WithThink((SceneInstance scene, double time, double dt) =>
                {
                    scene["cube"].Rotation += new RVec3(0, 1 * dt, 2 * dt);
                    scene["sphere"].Rotation += new RVec3(0, 1 * dt, 2 * dt);
                })
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie(showDepth: false).Output("test");
        }
    }
}