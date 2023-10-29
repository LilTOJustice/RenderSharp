using RenderSharp.Math;
using RenderSharp.RendererCommon;

namespace RendererCommon.Color
{
    public class ColorFunctions
    {
        public static RGBA AlphaBlend(RGBA top, RGBA bottom)
        {
            //double alpha = top.A;
            //FVec3 blended = (top.RGB * alpha + bottom.RGB * (1d - alpha));
            return top; // TODO: FIX this function
        }
    }
}
