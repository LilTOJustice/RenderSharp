using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public class RGBAFloat : Vector4<double> 
    {
        public double R { get { return this[0]; } set { this[0] = value; } }
        
        public double G { get { return this[1]; } set { this[1] = value; } }
        
        public double B { get { return this[2]; } set { this[2] = value; } }

        public double A { get { return this[3]; } set { this[3] = value; } }

        public RGBFloat RGB
        {
            get
            {
                return (RGBFloat)this;
            }
            set
            {
                R = value.R;
                G = value.G;
                B = value.B;
            }
        }

        public RGBAFloat() { }

        public RGBAFloat(double[] vec) : base(vec) { }

        public RGBAFloat(RGBAFloat rgbaf) : base(rgbaf) { }

        public RGBAFloat(RGBFloat rgbf, double a) : base(rgbf, a) { }

        public RGBAFloat(double r, double g, double b, double a) : base(r, g, b, a) { }

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

        public RGBFloat ToRGBFloat()
        {
            return new RGBFloat(R, G, B);
        }

        public HSV ToHSV()
        {
            return RGB;
        }

        public static implicit operator RGBA(RGBAFloat rgbaf)
        {
            return rgbaf.ToRGBA();
        }

        public static implicit operator HSVA(RGBAFloat rgbaf)
        {
            return rgbaf.ToHSVA();
        }

        public static explicit operator RGB(RGBAFloat rgbaf)
        {
            return rgbaf.ToRGB();
        }

        public static explicit operator RGBFloat(RGBAFloat rgbaf)
        {
            return rgbaf.ToRGBFloat();
        }

        public static explicit operator HSV(RGBAFloat rgbaf)
        {
            return rgbaf.ToHSV();
        }

        public static RGBAFloat operator +(RGBAFloat lhs, RGBAFloat rhs)
        {
            return new RGBAFloat(((Vector4<double>)lhs + (Vector4<double>)rhs).Components);
        }

        public static RGBAFloat operator -(RGBAFloat lhs, RGBAFloat rhs)
        {
            return new RGBAFloat(((Vector4<double>)lhs + (Vector4<double>)rhs).Components);
        }

        public static RGBAFloat operator *(RGBAFloat lhs, RGBAFloat rhs)
        {
            return new RGBAFloat(((Vector4<double>)lhs * (Vector4<double>)rhs).Components);
        }

        public static RGBAFloat operator /(RGBAFloat lhs, RGBAFloat rhs)
        {
            return new RGBAFloat(((Vector4<double>)lhs / (Vector4<double>)rhs).Components);
        }

        public static RGBAFloat operator /(RGBAFloat lhs, double scalar)
        {
            return new RGBAFloat(((Vector4<double>)lhs / scalar).Components);
        }
    }
}
