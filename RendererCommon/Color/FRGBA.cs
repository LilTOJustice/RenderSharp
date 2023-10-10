using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public class FRGBA : Vector4<double> 
    {
        public double R { get { return this[0]; } set { this[0] = value; } }
        
        public double G { get { return this[1]; } set { this[1] = value; } }
        
        public double B { get { return this[2]; } set { this[2] = value; } }

        public double A { get { return this[3]; } set { this[3] = value; } }

        public FRGB RGB
        {
            get
            {
                return (FRGB)this;
            }
            set
            {
                R = value.R;
                G = value.G;
                B = value.B;
            }
        }

        public FRGBA() { }

        public FRGBA(double[] vec) : base(vec) { }

        public FRGBA(FRGBA rgbaf) : base(rgbaf) { }

        public FRGBA(FRGB rgbf, double a) : base(rgbf, a) { }

        public FRGBA(double r, double g, double b, double a) : base(r, g, b, a) { }

        public RGBA ToRGBA()
        {
            return new RGBA((RGB)this, (byte)(A * 255));
        }

        public HSVA ToHSVA()
        {
            return new HSVA((RGB)this, A);
        }

        public RGB ToRGB()
        {
            return RGB;
        }

        public FRGB ToRGBFloat()
        {
            return new FRGB(R, G, B);
        }

        public HSV ToHSV()
        {
            return RGB;
        }

        public static implicit operator RGBA(FRGBA rgbaf)
        {
            return rgbaf.ToRGBA();
        }

        public static implicit operator HSVA(FRGBA rgbaf)
        {
            return rgbaf.ToHSVA();
        }

        public static explicit operator RGB(FRGBA rgbaf)
        {
            return rgbaf.ToRGB();
        }

        public static explicit operator FRGB(FRGBA rgbaf)
        {
            return rgbaf.ToRGBFloat();
        }

        public static explicit operator HSV(FRGBA rgbaf)
        {
            return rgbaf.ToHSV();
        }

        public static FRGBA operator +(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs + rhs).Components);
        }

        public static FRGBA operator -(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs + rhs).Components);
        }

        public static FRGBA operator *(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs * rhs).Components);
        }

        public static FRGBA operator *(FRGBA lhs, double scalar)
        {
            return new FRGBA(((Vector4<double>)lhs * scalar).Components);
        }

        public static FRGBA operator /(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs / rhs).Components);
        }

        public static FRGBA operator /(FRGBA lhs, double scalar)
        {
            return new FRGBA(((Vector4<double>)lhs / scalar).Components);
        }
    }
}
