using RenderSharp.Math;
using RenderSharp.RendererCommon;
using System.Numerics;

namespace RenderSharp
{
    public class ExampleShaders
    {
        static int MAXITS = 100;
        static double Mandel(Complex c)
        {
            int its = 0;
            Complex res = c;
            double absolute;
            while ((absolute = res.Magnitude) < 2 && its++ < MAXITS)
            {
                res = (res * res) + c;
            }

            return absolute;
        }

        static double Multi(Complex c, double exponent)
        {
            int its = 0;
            Complex res = c;
            double absolute = 0;
            while ((absolute = res.Magnitude) < 2 && its++ < MAXITS)
            {
                res = Complex.Pow(res, exponent) + c;
            }

            return absolute;
        }

        public static void Mandelbrot(in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            FVec2 st = (FVec2)fragCoord / res;
            st.X *= 1d * res.X / res.Y;
            st = (st - new FVec2(0.4, -0.5)) / .4;
            Complex c = new(st.X, st.Y);
            double mandelOut = Mandel(c);
            if (mandelOut < 2)
            {
                fragOut = new RGBA(0, 0, 0, 255);
            }
            else
            {
                fragOut = new HSV(mandelOut * 5, 1, 1);
            }
        }
        public static void Multibrot(in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            FVec2 st = (FVec2)fragCoord / res;
            st.X *= 1d * res.X / res.Y;
            st = (st - new FVec2(0.4, -0.5)) / .2;
            Complex c = new(st.X, st.Y);
            double exponent = 3 * System.Math.Cos(time) + 4;
            double multiOut = Multi(c, exponent);
            if (multiOut < 2)
            {
                fragOut = new RGBA(0, 0, 0, 255);
            }
            else
            {
                fragOut = new HSV(multiOut * 5, 1, 1);
            }
        }

        public static void Psychedelic(in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            HSV hsv = (HSV)fragIn;
            hsv.H += time * 360;
            fragOut = hsv;
        }

        public static void TopLeftDebug(in RGBA fragIn, out RGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            if (fragCoord == res / 4)
            {
                fragOut = new RGB(255, 0, 0);
            }
            else
            {
                fragOut = fragIn;
            }
        }
    }
}
