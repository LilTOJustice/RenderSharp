namespace RenderSharp.Math
{
    public class HSV : Vector3<double>
    {
        public double H { get { return this[0]; } set { this[0] = value; } } 

        public double S { get { return this[1]; } set { this[1] = value; } } 
        
        public double V { get { return this[2]; } set { this[2] = value; } } 

        public HSV() { }

        public HSV(double[] vec) : base(vec) { }

        public HSV(HSV hsv) : base(hsv) { }
        
        public HSV(double h, double s, double v) : base(h, s, v) { }

        public RGB ToRGB()
        {
            double H = this.H, S = this.S, V = this.V;
            double M = 255 * V;
            double m = M * (1 - S);
            double z = (M - m) * (1 - System.Math.Abs(H / 60 % 2 - 1));
            byte R, G, B;

            if (H < 60)
            {
                R = (byte)M;
                G = (byte)(z + m);
                B = (byte)m;
            }
            else if (H < 120)
            {
                R = (byte)(z + m);
                G = (byte)M;
                B = (byte)m;
            }
            else if (H < 180)
            {
                R = (byte)m;
                G = (byte)M;
                B = (byte)(z + m);
            }
            else if (H < 240)
            {
                R = (byte)m;
                G = (byte)(z + m);
                B = (byte)M;
            }
            else if (H < 300)
            {
                R = (byte)(z + m);
                G = (byte)m;
                B = (byte)M;
            }
            else
            {
                R = (byte)M;
                G = (byte)m;
                B = (byte)(z + m);
            }

            return new RGB(R, G, B);
        }

        public FRGB ToRGBFloat()
        {
            return ToRGB();
        }

        public RGBA ToRGBA()
        {
            return ToRGB();
        }

        public FRGBA ToRGBAFloat()
        {
            return ToRGB();
        }

        public HSVA ToHSVA()
        {
            return new HSVA(this, 1d);
        }

        public static implicit operator RGB(HSV hsv)
        {
            return hsv.ToRGB();
        }

        public static implicit operator FRGB(HSV hsv)
        {
            return hsv.ToRGBFloat();
        }

        public static implicit operator RGBA(HSV hsv)
        {
            return hsv.ToRGBA();
        }

        public static implicit operator FRGBA(HSV hsv)
        {
            return hsv.ToRGBAFloat();
        }

        public static implicit operator HSVA(HSV hsv)
        { 
            return hsv.ToHSVA();
        }

        public HSV Cross(HSV rhs)
        {
            return new HSV(Cross(this, rhs).Components);
        }

        public static HSV operator +(HSV lhs, HSV rhs)
        {
            return new HSV(((Vector3<double>)lhs + rhs).Components);
        }

        public static HSV operator -(HSV lhs, HSV rhs)
        {
            return new HSV(((Vector3<double>)lhs + rhs).Components);
        }

        public static HSV operator *(HSV lhs, HSV rhs)
        {
            return new FRGB(((Vector3<double>)lhs * rhs).Components);
        }

        public static HSV operator *(HSV lhs, double scalar)
        {
            return new HSV(((Vector3<double>)lhs * scalar).Components);
        }

        public static HSV operator /(HSV lhs, HSV rhs)
        {
            return new HSV (((Vector3<double>)lhs / rhs).Components);
        }

        public static HSV operator /(HSV lhs, double scalar)
        {
            return new HSV(((Vector3<double>)lhs / scalar).Components);
        }
    }
}
