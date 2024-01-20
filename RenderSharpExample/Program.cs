using MathSharp;
using RenderSharp;
using RenderSharp.Render2d;

namespace RenderSharpExample
{
    internal class Program
    {
        static int framerate = 30;
        static int duration = 3;
        static int resX = 1000;
        static int resY = 1000;

        static void Main(string[] args)
        {
            // Create scene
            Scene2d scene = new(framerate, duration, bgColor: new RGB(0, 0, 255));

            // Create renderer
            Renderer2d renderer = new(resX, resY, scene);

            // Create some actors
            Actor2d actor = new(new FVec2(100, 100)); // Actor created by size 10x10 at position (0, 0)
            Line2d line = new(10, actor.Position, new FVec2(20, 0), new HSV(0, 1, 1));

            // Register them with the scene
            scene.AddActor(actor, "Box");
            scene.AddActor(line, "line");

            // Add some shaders
            actor.Shader += ExampleShaders.Ghostly;
            line.Shader += ExampleShaders.Psychedelic;

            // Create the think function for the scene to run each frame
            scene.ThinkFunc += (Scene2dInstance scene, double time, double dt) =>
            {
                Actor2d box = scene["Box"];
                Line2d line = (Line2d)scene["line"];
                box.Position += new FVec2(0, 50) * dt;
                box.Rotation += 3 * dt;
                line.Start = box.Position;
            };

            // Finally render and output the video
            renderer.RenderMovie().Output("test");
        }
    }
}