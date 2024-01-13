using RenderSharp.Math;
using RenderSharp.Render2d;
using RenderSharp.Scene;

namespace RenderSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Scene2d scene = new(30, 1);
            Renderer2d renderer = new(3840, 2160, scene);
            scene.Shader += ExampleShaders.Multibrot;
            renderer.RenderMovie().Output("test");
        }
    }
}