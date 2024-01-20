using MathSharp;
using RenderSharp.Renderer;
using RenderSharp.RendererCommon;
using RenderSharp.Scene;

namespace RenderSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Scene2d scene = new(60, 6, bgColor: new RGB(0, 0, 255));
            Renderer2d renderer = new(400, 400, scene);
            Scene2d.Actor actor = new(new FVec2(10, 10));
            scene.AddActor(actor, "Box");
            actor.Shader += ExampleShaders.Ghostly;
            scene.ThinkFunc += (Scene2dInstance scene, double time, double dt) =>
            {
                scene.Actors["Box"].Position.X += 50 * dt;
                scene.Actors["Box"].Position.Y += 50 * dt;
            };
            renderer.RenderMovie().Output("test");
        }
    }
}