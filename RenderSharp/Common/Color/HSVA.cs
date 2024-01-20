using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="Vector4{T}"/> of type double. Values <see cref="H"/> intended
    /// to lie within the range [0, 360] and <see cref="S"/>, <see cref="V"/> 
    /// and <see cref="A"/> intended to lie within the range [0, 1].
    /// </summary>
    public class HSVA : Vector4<double>
    {
        /// <inheritdoc cref="HSV.H"/>
        public double H { get { return this[0]; } set { this[0]  = value; } }

        /// <inheritdoc cref="HSV.S"/>
        public double S { get { return this[1]; } set { this[1]  = value; } }
        
        /// <inheritdoc cref="HSV.V"/>
        public double V { get { return this[2]; } set { this[2]  = value; } }
        
        /// <inheritdoc cref="FRGBA.A"/>
        public double A { get { return this[3]; } set { this[3]  = value; } }

        /// <summary>
        /// The hue, saturation and value components of the color. Reading from this creates a new color. Writing changes the existing values of the color.
        /// </summary>
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

        /// <inheritdoc cref="FRGB()"/>
        public HSVA() { }

        /// <inheritdoc cref="FRGB(double[])"/>
        public HSVA(double[] vec) : base(vec) { }

        /// <inheritdoc cref="FRGB(FRGB)"/>
        public HSVA(HSVA hsva) : base(hsva) { }

        /// <summary>
        /// Constructs a new color from the given color and alpha channel.
        /// </summary>
        /// <param name="hsv">The hue, saturation and value channels of the new color.</param>
        /// <param name="a">The alpha channel of the new color. Intended [0, 1].</param>
        public HSVA(HSV hsv, double a) : base(hsv, a) { }

        /// <summary>
        /// Constructs a new color from the given hue, saturation, value and alpha channels.
        /// </summary>
        /// <param name="h">The hue channel of the new color. Intended [0, 1].</param>
        /// <param name="s">The saturation channel of the new color. Intended [0, 1].</param>
        /// <param name="v">The value channel of the new color. Intended [0, 1].</param>
        /// <param name="a">The alpha channel of the new color. Intended [0, 1].</param>
        public HSVA(double h, double s, double v, double a) : base(h, s, v, a) { }

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with red, green, blue and alpha [0, 255].</returns>
        public RGBA ToRGBA()
        {
            return new RGBA(HSV, (byte)(A * 255));
        }

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green, blue and alpha [0, 1].</returns>
        public FRGBA ToFRGBA()
        {
            return ToRGBA();
        }

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 255], and alpha truncated.</returns>
        public RGB ToRGB()
        {
            return HSV;
        }

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 1], and alpha truncated.</returns>
        public FRGB ToFRGB()
        {
            return HSV;
        }

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with the same channels, but alpha truncated.</returns>
        public HSV ToHSV()
        {
            return new HSV(H, S, V);
        }

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(HSVA hsva)
        {
            return hsva.ToRGBA();
        }

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(HSVA hsva)
        {
            return hsva.ToFRGBA();
        }

        /// <inheritdoc cref="ToRGB"/>
        public static explicit operator RGB(HSVA hsva)
        {
            return hsva.ToRGB();
        }

        /// <inheritdoc cref="ToFRGB"/>
        public static explicit operator FRGB(HSVA hsva)
        {
            return hsva.ToFRGB();
        }

        /// <inheritdoc cref="ToHSV"/>
        public static explicit operator HSV(HSVA hsva)
        {
            return hsva.ToHSV();
        }

        /// <inheritdoc cref="FRGB.operator +(FRGB, FRGB)"/>
        public static HSVA operator +(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator -(FRGB, FRGB)"/>
        public static HSVA operator -(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, FRGB)"/>
        public static HSVA operator *(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, double)"/>
        public static HSVA operator *(HSVA lhs, double scalar)
        {
            return new HSVA(((Vector4<double>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, FRGB)"/>
        public static HSVA operator /(HSVA lhs, HSVA rhs)
        {
            return new HSVA(((Vector4<double>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, double)"/>
        public static HSVA operator /(HSVA lhs, double scalar)
        {
            return new HSVA(((Vector4<double>)lhs / scalar).Components);
        }
    }
}
