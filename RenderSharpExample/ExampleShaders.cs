using MathSharp;
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

        public static void Mandelbrot(FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            FVec2 st = (FVec2)fragCoord / res;
            st.X *= 1d * res.X / res.Y;
            st = (st - new FVec2(0.4, -0.5)) / .4;
            Complex c = new(st.X, st.Y);
            double mandelOut = Mandel(c);
            if (mandelOut < 2)
            {
                fragOut = new FRGBA(0, 0, 0, 255);
            }
            else
            {
                fragOut = new HSV(mandelOut * 5, 1, 1);
            }
        }

        public static void Multibrot(FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            FVec2 st = (FVec2)fragCoord / res;
            st.X *= 1d * res.X / res.Y;
            st = (st - new FVec2(0.4, -0.5)) / .2;
            Complex c = new(st.X, st.Y);
            double exponent = 3 * Math.Cos(time) + 4;
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

        public static void Psychedelic(FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            HSVA hsv = (HSVA)fragIn;
            hsv.H = (time * 360) % 360;
            fragOut = hsv;
        }

        public static void TopLeftDebug(FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
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

        public static void Ghostly(FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            fragOut = fragIn;
            fragOut.A = 0.5 + Math.Sin(time * 2) * 0.5;
        }

        public static void CircleCut(FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time)
        {
            fragOut = fragIn;
            FVec2 st = (((FVec2)fragCoord) / res) - new FVec2(0.5, 0.5);
            if (st.Mag() > 0.5)
            {
                fragOut.A = 0;
            }
        }

        public static void WavyX(Vec2 coordIn, out Vec2 coordOut, Vec2 size, double time)
        {
            coordOut = coordIn + new Vec2((int)(20 * Math.Sin(coordIn.Y / 20.0 + time)), 0);
        }

        public static void WavyY(Vec2 coordIn, out Vec2 coordOut, Vec2 size, double time)
        {
            coordOut = coordIn + new Vec2(0, (int)(20 * Math.Sin(coordIn.Y / 20.0 + time)));
        }
    }
}
