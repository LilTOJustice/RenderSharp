namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector4{T}"/> of type double. Values <see cref="R"/>, <see cref="G"/>,
    /// <see cref="B"/> and <see cref="A"/> all intended to lie within the range [0, 1].
    /// </summary>
    public class FRGBA : Vector4<double> 
    {
        /// <inheritdoc cref="FRGB.R"/>
        public double R { get { return this[0]; } set { this[0] = value; } }
        
        /// <inheritdoc cref="FRGB.G"/>
        public double G { get { return this[1]; } set { this[1] = value; } }
        
        /// <inheritdoc cref="FRGB.B"/>
        public double B { get { return this[2]; } set { this[2] = value; } }

        /// <summary>
        /// Alpha channel. Intended to range [0, 1].
        /// </summary>
        public double A { get { return this[3]; } set { this[3] = value; } }

        /// <summary>
        /// The red, green and blue components of the color. Reading from this creates a new color. Writing changes the existing values of the color.
        /// </summary>
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

        /// <inheritdoc cref="FRGB()"/>
        public FRGBA() { }

        /// <inheritdoc cref="FRGB(double[])"/>
        public FRGBA(double[] vec) : base(vec) { }

        /// <inheritdoc cref="FRGB(FRGB)"/>
        public FRGBA(FRGBA rgbaf) : base(rgbaf) { }

        /// <summary>
        /// Constructs a new color from the given color and alpha channel.
        /// </summary>
        /// <param name="rgbf">The red, green and blue channels of the new color.</param>
        /// <param name="a">The alpha channel of the new vector. Intended [0, 1].</param>
        public FRGBA(FRGB rgbf, double a) : base(rgbf, a) { }

        /// <summary>
        /// Constructs a new color from the given red, green, blue and alpha channels.
        /// </summary>
        /// <param name="r">The red channel of the new color. Intended [0, 1].</param>
        /// <param name="g">The green channel of the new color. Intended [0, 1].</param>
        /// <param name="b">The blue channel of the new color. Intended [0, 1].</param>
        /// <param name="a">The alpha channel of the new color. Intended [0, 1].</param>
        public FRGBA(double r, double g, double b, double a) : base(r, g, b, a) { }

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with values scaled by 255.</returns>
        public RGBA ToRGBA()
        {
            return new RGBA((RGB)this, (byte)(A * 255));
        }

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with hue [0, 360] and saturation, value and alpha [0 - 1].</returns>
        public HSVA ToHSVA()
        {
            return new HSVA((RGB)this, A);
        }

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with values scaled down by 255 and alpha truncated.</returns>
        public RGB ToRGB()
        {
            return RGB;
        }

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with the same channels, but alpha truncated.</returns>
        public FRGB ToFRGB()
        {
            return new FRGB(R, G, B);
        }

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1] and alpha truncated.</returns>
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
            return rgbaf.ToFRGB();
        }

        public static explicit operator HSV(FRGBA rgbaf)
        {
            return rgbaf.ToHSV();
        }

        /// <inheritdoc cref="FRGB.operator +(FRGB, FRGB)"/>
        public static FRGBA operator +(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator -(FRGB, FRGB)"/>
        public static FRGBA operator -(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, FRGB)"/>
        public static FRGBA operator *(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, double)"/>
        public static FRGBA operator *(FRGBA lhs, double scalar)
        {
            return new FRGBA(((Vector4<double>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, FRGB)"/>
        public static FRGBA operator /(FRGBA lhs, FRGBA rhs)
        {
            return new FRGBA(((Vector4<double>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, double)"/>
        public static FRGBA operator /(FRGBA lhs, double scalar)
        {
            return new FRGBA(((Vector4<double>)lhs / scalar).Components);
        }
    }
}
