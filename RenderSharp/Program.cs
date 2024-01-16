using RenderSharp.Render2d;
using RenderSharp.RendererCommon;
using RenderSharp.Scene;

namespace RenderSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Scene2d scene = new(60, 6, bgcolor: new RGB(0, 0, 255));
            Renderer2d renderer = new(500, 500, scene);
            Scene2d.Actor actor = new(new Texture(100, 100));
            scene.AddActor(actor);
            actor.Shader += ExampleShaders.Ghostly;
            scene.ThinkFunc += (Scene2dInstance scene, double time, double dt) =>
            {
                scene.Actors.First().Position.X += 50 * dt;
                Console.WriteLine(scene.Actors.First().Position.X);
            };
            renderer.RenderMovie().Output("test");
            Console.WriteLine(scene.Actors.First().Position);
        }
    }
}