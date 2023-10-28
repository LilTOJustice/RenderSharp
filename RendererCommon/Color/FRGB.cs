using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public class FRGB : Vector3<double>
    {
        public double R { get { return this[0]; } set { this[0] = value; } }

        public double G { get { return this[1]; } set { this[1] = value; } }

        public double B { get { return this[2]; } set { this[2] = value; } }

        public FRGB() { }

        public FRGB(double[] vec) : base(vec) { }

        public FRGB(FRGB rgbf) : base(rgbf) { }

        public FRGB(double r, double g, double b) : base(r, g, b) { }

        public RGB ToRGB()
        {
            return new RGB((byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }

        public RGBA ToRGBA()
        {
            return ToRGB();
        }

        public FRGBA ToRGBAFloat()
        {
            return ToRGB();
        }

        public HSV ToHSV()
        {
            return ToRGB();
        }

        public HSVA ToHSVA()
        {
            return ToRGB();
        }

        public static implicit operator RGB(FRGB rgbf)
        {
            return rgbf.ToRGB();
        }

        public static implicit operator RGBA(FRGB rgbf)
        {
            return rgbf.ToRGBA();
        }

        public static implicit operator FRGBA(FRGB rgbf)
        {
            return rgbf.ToRGBAFloat();
        }

        public static implicit operator HSV(FRGB rgbf)
        {
            return rgbf.ToHSV();
        }

        public static implicit operator HSVA(FRGB rgbf)
        {
            return rgbf.ToHSVA();
        }

        public FRGB Cross(FRGB rhs)
        {
            return new FRGB(Cross(this, rhs).Components);
        }

        public static FRGB operator +(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs + rhs).Components);
        }

        public static FRGB operator -(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs + rhs).Components);
        }

        public static FRGB operator *(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs * rhs).Components);
        }

        public static FRGB operator *(FRGB lhs, double scalar)
        {
            return new FRGB(((Vector3<double>)lhs * scalar).Components);
        }

        public static FRGB operator /(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs / rhs).Components);
        }

        public static FRGB operator /(FRGB lhs, double scalar)
        {
            return new FRGB(((Vector3<double>)lhs / scalar).Components);
        }
    }
}
