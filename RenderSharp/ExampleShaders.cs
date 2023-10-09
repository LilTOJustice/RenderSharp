using RenderSharp.Math;
using RenderSharp.RendererCommon;

namespace RenderSharp
{
    public class ExampleShaders
    {

        FragShader frag = (in RGBA fragIn, out RGBA fragOut, in Vec2 fragCoord, in Vec2 res, double time) =>
        {
            fragOut = new RGBA();
        };
    }
}
