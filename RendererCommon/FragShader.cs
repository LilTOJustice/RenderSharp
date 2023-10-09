using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public delegate void FragShader(in RGBA fragIn, out RGBA fragOut, in Vec2 fragCoord, in Vec2 res, double time);
}
