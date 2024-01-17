namespace RenderSharp.Math
{
    public class ColorFunctions
    {
        public static RGBA AlphaBlend(FRGBA top, FRGBA bottom)
        {
            double alpha = top.A;
            FRGB blended = top.RGB * alpha + bottom.RGB * (1d - alpha);
            return new FRGBA(blended, alpha);
        }
    }
}
