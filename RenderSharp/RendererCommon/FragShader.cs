using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public delegate void FragShader(in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time);
}
