using RenderSharp.Math;
using System.Runtime.CompilerServices;

namespace RenderSharp.Renderer.Color
{
    public class RGBA : Vector4<byte>
    {
        public byte R { get { return this[0]; } set { this[0] = value; } }

        public byte G { get { return this[1]; } set { this[1] = value; } }
        
        public byte B { get { return this[2]; } set { this[2] = value; } }
        
        public byte A { get { return this[3]; } set { this[3] = value; } }

        public RGB RGB
        {
            get
            {
                return (RGB)this;
            }
            set
            {
                R = value.R;
                G = value.G;
                B = value.B;
            }
        }

        public RGBA() { }

        public RGBA(RGBA rgba) : base(rgba) { }

        public RGBA(RGB rgb, byte a) : base(rgb, a) { }

        public RGBA(byte r, byte g, byte b, byte a) : base(r, g, b, a) { }

        public RGBAFloat ToRGBAFloat()
        {
            return new RGBAFloat(ToRGBFloat(), A / 255d);
        }

        public HSVA ToHSVA()
        {
            return new HSVA(RGB, A * 255d);
        }

        public RGB ToRGB()
        {
            return new RGB(R, G, B);
        }

        public RGBFloat ToRGBFloat()
        {
            return new RGBFloat(R / 255d, G / 255d, B / 255d);
        }

        public HSV ToHSV()
        {
            return RGB;
        }

        public static implicit operator RGBAFloat(RGBA rgba)
        {
            return rgba.ToRGBAFloat();
        }

        public static implicit operator HSVA(RGBA rgba)
        {
            return rgba.ToHSVA();
        }

        public static explicit operator RGB(RGBA rgba)
        {
            return rgba.ToRGB();
        }

        public static explicit operator RGBFloat(RGBA rgba)
        {
            return rgba.ToRGBFloat();
        }

        public static explicit operator HSV(RGBA rgba)
        {
            return rgba.ToHSV();
        }
    }
}
