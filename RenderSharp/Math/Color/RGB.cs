namespace RenderSharp.Math
{
    public class RGB : Vector3<byte>
    {
        public byte R { get { return this[0]; } set { this[0] = value; } }

        public byte G { get { return this[1]; } set { this[1] = value; } }

        public byte B { get { return this[2]; } set { this[2] = value; } }

        public RGB() { }

        public RGB(byte[] vec) : base(vec) { }

        public RGB(RGB rgb) : base(rgb) { }

        public RGB(byte r, byte g, byte b) : base(r, g, b) { }

        public FRGB ToRGBFloat()
        {
            return new FRGB(R / 255d, G / 255d, B / 255d);
        }

        public RGBA ToRGBA()
        {
            return new RGBA(this, 255);
        }

        public FRGBA ToRGBAFloat()
        {
            return new FRGBA(ToRGBFloat(), 1d);
        }

        public HSV ToHSV()
        {
            double R = this.R, G = this.G, B = this.B;
            double M = System.Math.Max(System.Math.Max(R, G), B);
            double m = System.Math.Min(System.Math.Min(R, G), B);
            double V = M / 255;
            double S = (M > 0 ? 1 - m / M : 0);
            double H = System.Math.Acos(
                (R - .5 * G - .5 * B) / System.Math.Sqrt(R * R + G * G + B * B - R * G - R * B - G * B)
            ) * Constants.DEGPERPI;

            if (B > G)
            {
                H = 360 - H;
            }

            return new HSV(H, S, V);
        }

        public HSVA ToHSVA()
        {
            return new HSVA(ToHSV(), 1d);
        }

        public static implicit operator FRGB(RGB rgb)
        {
            return rgb.ToRGBFloat();
        }

        public static implicit operator RGBA(RGB rgb)
        {
            return rgb.ToRGBA();
        }

        public static implicit operator FRGBA(RGB rgb)
        {
            return rgb.ToRGBAFloat();
        }

        public static implicit operator HSV(RGB rgb)
        {
            return rgb.ToHSV();
        }

        public static implicit operator HSVA(RGB rgb)
        {
            return rgb.ToHSVA();
        }

        public RGB Cross(RGB rhs)
        {
            return new RGB(Cross(this, rhs).Components);
        }

        public static RGB operator +(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs + rhs).Components);
        }

        public static RGB operator -(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs + rhs).Components);
        }

        public static RGB operator *(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs * rhs).Components);
        }

        public static RGB operator *(RGB lhs, byte scalar)
        {
            return new RGB(((Vector3<byte>)lhs * scalar).Components);
        }

        public static FVec3 operator *(RGB lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.R, lhs.G, lhs.B) * scalar).Components);
        }

        public static RGB operator /(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs / rhs).Components);
        }

        public static RGB operator /(RGB lhs, byte scalar)
        {
            return new RGB(((Vector3<byte>)lhs / scalar).Components);
        }

        public static FVec3 operator /(RGB lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.R, lhs.G, lhs.B) / scalar).Components);
        }
    }
}
