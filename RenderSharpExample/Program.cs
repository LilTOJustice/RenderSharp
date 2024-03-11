using MathSharp;
using RenderSharp;
using RenderSharp.Render2d;

namespace RenderSharpExample
{
    internal class Program
    {
        static readonly int framerate = 60;
        static readonly int duration = 3;
        static readonly int resX = 1200;
        static readonly int resY = 1200;

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
                        scene.Camera.Zoom = 1 + Math.Sin(time);
                    }
                )
                .WithBgTexture(new Texture(resX, resY))
                .WithBgTextureWorldSize(new FVec2(1, 1))
                .WithBgShader(ExampleShaders.Multibrot)
                .WithBgShader(ExampleShaders.WavyX)
                .WithCamera("main", new FVec2(-0.1, 2))
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}