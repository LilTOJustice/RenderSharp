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
                        box.Position += new FVec2(0, 0.1) * dt;
                    }
                )
                .WithActor(new ActorBuilder()
                    .WithSize(new FVec2(0.1, 0.05))
                    .WithColor(new RGBA(0, 0, 0, 0))
                    , "Box")
                .WithBgTexture(new Texture("C:\\Users\\muian\\OneDrive\\Pictures\\Profile Pic\\gordon.jpg"), new FVec2(1, 1))
                //.WithBgColor(new RGB(0, 0, 255))
                .Build();

            // Create renderer
            Renderer renderer = new(resX, resY, scene);
            renderer.CoordShader = ExampleShaders.Wavy;

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}