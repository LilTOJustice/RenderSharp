using RenderSharp.Render2d;
using RenderSharp.RendererCommon;
using RenderSharp.Scene;

namespace RenderSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Scene2d scene = new(30, 3, bgcolor: new RGB(0, 0, 255));
            Renderer2d renderer = new(2560, 1440, scene);
            scene.Shader += ExampleShaders.Multibrot;
            renderer.Shader += ExampleShaders.Psychedelic;
            renderer.RenderMovie().Output("test");
        }
    }
}