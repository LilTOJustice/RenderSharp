using MathSharp;

namespace RenderSharp.RendererCommon
{
    /// <summary>
    /// A <see cref="Vector3{T}"/> of type double. Values <see cref="H"/> intended
    /// to lie within the range [0, 360] and <see cref="S"/> and <see cref="V"/>
    /// intended to lie within the range [0, 1].
    /// </summary>
    public class HSV : Vector3<double>
    {
        /// <summary>
        /// Hue channel. Intended to range [0, 360].
        /// </summary>
        public double H { get { return this[0]; } set { this[0] = value; } } 

        /// <summary>
        /// Saturation channel. Intended to range [0, 1].
        /// </summary>
        public double S { get { return this[1]; } set { this[1] = value; } } 
        
        /// <summary>
        /// Value channel. Intended to range [0, 1].
        /// </summary>
        public double V { get { return this[2]; } set { this[2] = value; } } 

        /// <inheritdoc cref="FRGB()"/>
        public HSV() { }

        /// <inheritdoc cref="FRGB(double[])"/>
        public HSV(double[] vec) : base(vec) { }

        /// <inheritdoc cref="FRGB(FRGB)"/>
        public HSV(HSV hsv) : base(hsv) { }
        
        /// <summary>
        /// Constructs a new color from the given hue, saturation and value.
        /// </summary>
        /// <param name="h">The hue channel of the new color. Intended [0, 360].</param>
        /// <param name="s">The saturation channel of the new color. Intended [0, 1].</param>
        /// <param name="v">The value channel of the new color. Intended [0, 1].</param>
        public HSV(double h, double s, double v) : base(h, s, v) { }

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 255].</returns>
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

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 1].</returns>
        public FRGB ToFRGB()
        {
            return ToRGB();
        }

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 255], and an additional alpha 255.</returns>
        public RGBA ToRGBA()
        {
            return ToRGB();
        }

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 1], and an addition alpha 1.</returns>
        public FRGBA ToFRGBA()
        {
            return ToRGB();
        }

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with the same values, and an addition alpha 1.</returns>
        public HSVA ToHSVA()
        {
            return new HSVA(this, 1d);
        }

        /// <inheritdoc cref="ToRGB"/>
        public static implicit operator RGB(HSV hsv)
        {
            return hsv.ToRGB();
        }

        /// <inheritdoc cref="ToFRGB"/>
        public static implicit operator FRGB(HSV hsv)
        {
            return hsv.ToFRGB();
        }

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(HSV hsv)
        {
            return hsv.ToRGBA();
        }

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(HSV hsv)
        {
            return hsv.ToFRGBA();
        }

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(HSV hsv)
        { 
            return hsv.ToHSVA();
        }

        /// <inheritdoc cref="FRGB.Cross(FRGB)"/>
        public HSV Cross(HSV rhs)
        {
            return new HSV(Cross(this, rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator +(FRGB, FRGB)"/>
        public static HSV operator +(HSV lhs, HSV rhs)
        {
            return new HSV(((Vector3<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator -(FRGB, FRGB)"/>
        public static HSV operator -(HSV lhs, HSV rhs)
        {
            return new HSV(((Vector3<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, FRGB)"/>
        public static HSV operator *(HSV lhs, HSV rhs)
        {
            return new FRGB(((Vector3<double>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, double)"/>
        public static HSV operator *(HSV lhs, double scalar)
        {
            return new HSV(((Vector3<double>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, FRGB)"/>
        public static HSV operator /(HSV lhs, HSV rhs)
        {
            return new HSV (((Vector3<double>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, double)"/>
        public static HSV operator /(HSV lhs, double scalar)
        {
            return new HSV(((Vector3<double>)lhs / scalar).Components);
        }
    }
}
