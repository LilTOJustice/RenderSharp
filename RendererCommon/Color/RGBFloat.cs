using RenderSharp.Math;

namespace RenderSharp.RendererCommon
{
    public class RGBFloat : Vector3<double>
    {
        public double R { get { return this[0]; } set { this[0] = value; } }

        public double G { get { return this[1]; } set { this[1] = value; } }

        public double B { get { return this[2]; } set { this[2] = value; } }

        public RGBFloat() { }

        public RGBFloat(double[] vec) : base(vec) { }

        public RGBFloat(RGBFloat rgbf) : base(rgbf) { }

        public RGBFloat(double r, double g, double b) : base(r, g, b) { }

        public RGB ToRGB()
        {
            return new RGB((byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }

        public RGBA ToRGBA()
        {
            return ToRGB();
        }

        public RGBAFloat ToRGBAFloat()
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

        public static implicit operator RGB(RGBFloat rgbf)
        {
            return rgbf.ToRGB();
        }

        public static implicit operator RGBA(RGBFloat rgbf)
        {
            return rgbf.ToRGBA();
        }

        public static implicit operator RGBAFloat(RGBFloat rgbf)
        {
            return rgbf.ToRGBAFloat();
        }

        public static implicit operator HSV(RGBFloat rgbf)
        {
            return rgbf.ToHSV();
        }

        public static implicit operator HSVA(RGBFloat rgbf)
        {
            return rgbf.ToHSVA();
        }

        public RGBFloat Cross(RGBFloat rhs)
        {
            return new RGBFloat(Cross((Vector3<double>)this, (Vector3<double>)rhs).Components);
        }

        public static RGBFloat operator +(RGBFloat lhs, RGBFloat rhs)
        {
            return new RGBFloat(((Vector3<double>)lhs + (Vector3<double>)rhs).Components);
        }

        public static RGBFloat operator -(RGBFloat lhs, RGBFloat rhs)
        {
            return new RGBFloat(((Vector3<double>)lhs + (Vector3<double>)rhs).Components);
        }

        public static RGBFloat operator *(RGBFloat lhs, RGBFloat rhs)
        {
            return new RGBFloat(((Vector3<double>)lhs * (Vector3<double>)rhs).Components);
        }

        public static RGBFloat operator *(RGBFloat lhs, double scalar)
        {
            return new RGBFloat(((Vector3<double>)lhs * scalar).Components);
        }

        public static RGBFloat operator /(RGBFloat lhs, RGBFloat rhs)
        {
            return new RGBFloat(((Vector3<double>)lhs / (Vector3<double>)rhs).Components);
        }

        public static RGBFloat operator /(RGBFloat lhs, double scalar)
        {
            return new RGBFloat(((Vector3<double>)lhs / scalar).Components);
        }
    }
}
