using RenderSharp.Math;
using RenderSharp.Render2d;
using RenderSharp.RendererCommon;
using RenderSharp.Scene;

namespace RenderSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Scene2d scene = new(10, 2, bgcolor: new RGB(255, 0, 0));
            Renderer2d renderer = new(500, 500, scene);
            Scene2d.Actor actor = new(new Texture(100, 100));
            scene.AddActor(actor);
            scene.ThinkFunc += (Scene2dThinkFuncArgs args) =>
            {
                actor.Position += new Vec2(10, 10);
            };
            scene.Shader += ExampleShaders.Multibrot;
            renderer.RenderMovie().Output("test");
        }
    }
}