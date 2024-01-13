using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public delegate void FragShader(in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time);
}
