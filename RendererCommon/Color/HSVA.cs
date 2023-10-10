using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public class HSVA : Vector4<double>
    {
        public double H { get { return this[0]; } set { this[0]  = value; } }

        public double S { get { return this[1]; } set { this[1]  = value; } }
        
        public double V { get { return this[2]; } set { this[2]  = value; } }
        
        public double A { get { return this[3]; } set { this[3]  = value; } }

        public HSV HSV
        {
            get
            {
                return (HSV)this;
            }
            set
            {
                H = value.H;
                S = value.S;
                V = value.V;
            }
        }

        public HSVA() { }

        public HSVA(double[] vec) : base(vec) { }

        public HSVA(HSVA hsva) : base(hsva) { }

        public HSVA(HSV hsv, double a) : base(hsv, a) { }

        public HSVA(double h, double s, double v, double a) : base(h, s, v, a) { }

        public RGBA ToRGBA()
        {
            return new RGBA(HSV, (byte)(A * 255));
        }

        public RGBAFloat ToRGBAFloat()
        {
            return ToRGBA();
        }

        public RGB ToRGB()
        {
            return HSV;
        }

        public RGBFloat ToRGBFloat()
        {
            return HSV;
        }

        public HSV ToHSV()
        {
            return new HSV(H, S, V);
        }

        public static implicit operator RGBA(HSVA hsva)
        {
            return hsva.ToRGBA();
        }

        public static implicit operator RGBAFloat(HSVA hsva)
        {
            return hsva.ToRGBAFloat();
        }

        public static explicit operator RGB(HSVA hsva)
        {
            return hsva.ToRGB();
        }

        public static explicit operator RGBFloat(HSVA hsva)
        {
            return hsva.ToRGBFloat();
        }

        public static explicit operator HSV(HSVA hsva)
        {
            return hsva.ToHSV();
        }

        public static HSVA operator +(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs + (Vector4<double>)rhs).Components);
        }

        public static HSVA operator -(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs + (Vector4<double>)rhs).Components);
        }

        public static HSVA operator *(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs * (Vector4<double>)rhs).Components);
        }

        public static HSVA operator /(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs / (Vector4<double>)rhs).Components);
        }

        public static HSVA operator /(HSVA lhs, double scalar)
        {
            return new HSVA(((Vector4<double>)lhs / scalar).Components);
        }
    }
}
