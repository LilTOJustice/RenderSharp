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
            double absolute = 0;
            while ((absolute = res.Magnitude) < 2 && its++ < MAXITS)
            {
                res = res * res + c;
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

        public static void Mandelbrot(FragShaderArgs args)
        {
            FVec2 st = (FVec2)args.fragCoord / args.res;
            st.X *= 1d * args.res.X / args.res.Y;
            st = (st - new FVec2(1.25, .5)) / .2;
            Complex c = new(st.X, st.Y);
            double mandelOut = Mandel(c);
            if (mandelOut < 2)
            {
                args.fragOut = new RGBA(0, 0, 0, 255);
            }
            else
            {
                args.fragOut = new HSV(mandelOut * 5, 1, 1);
            }
        }

        public static void Multibrot(FragShaderArgs args)
        {
            FVec2 st = (FVec2)args.fragCoord / args.res;
            st.X *= 1d * args.res.X / args.res.Y;
            st = (st - new FVec2(1.25, .5)) / .2;
            Complex c = new(st.X, st.Y);
            double exponent = 3 * System.Math.Cos(args.time) + 4;
            double multiOut = Multi(c, exponent);
            if (multiOut < 2)
            {
                args.fragOut = new RGBA(0, 0, 0, 255);
            }
            else
            {
                args.fragOut = new HSV(multiOut * 5, 1, 1);
            }
        }

        public static void Rainbow(FragShaderArgs args)
        {
            args.fragOut = (FRGBA)args.fragIn * ((FRGBA)new HSV(180 * args.time % 360, 1d, 1d) / 255d);
        }
    }
}
