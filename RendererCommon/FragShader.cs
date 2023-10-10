using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public class FragShaderArgs
    {
        public readonly RGBA fragIn;
        public RGBA fragOut;
        public readonly Vec2 fragCoord;
        public readonly Vec2 res;
        public double time;

        public FragShaderArgs(RGBA fragIn, RGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            this.fragIn = fragIn;
            this.fragOut = fragOut;
            this.fragCoord = fragCoord;
            this.res = res;
            this.time = time;
        }
    }

    public delegate void FragShader(FragShaderArgs args);
}
