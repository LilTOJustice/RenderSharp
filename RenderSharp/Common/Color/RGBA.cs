using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="Vector4{T}"/> of type double. Values <see cref="R"/>, <see cref="G"/> and <see cref="B"/>
    /// all intended to lie within the range [0, 1].
    /// </summary>
    public class RGBA : Vector4<byte>
    {
        /// <inheritdoc cref="RGB.R"/>
        public byte R { get { return this[0]; } set { this[0] = value; } }

        /// <inheritdoc cref="RGB.G"/>
        public byte G { get { return this[1]; } set { this[1] = value; } }
        
        /// <inheritdoc cref="RGB.B"/>
        public byte B { get { return this[2]; } set { this[2] = value; } }
        
        /// <summary>
        /// Alpha channel. Intended to range [0, 255].
        /// </summary>
        public byte A { get { return this[3]; } set { this[3] = value; } }

        /// <summary>
        /// The red, green and blue components of the color. Reading from this creates a new color. Writing changes the existing values of the color.
        /// </summary>
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

        /// <inheritdoc cref="FRGB()"/>
        public RGBA() { }

        /// <inheritdoc cref="FRGB(double[])"/>
        public RGBA(byte[] vec) : base(vec) { }

        /// <inheritdoc cref="FRGB(FRGB)"/>
        public RGBA(RGBA rgba) : base(rgba) { }

        /// <summary>
        /// Constructs a new color from the given color vector and alpha channel.
        /// </summary>
        /// <param name="rgb">The red, green and blue channels of the new color.</param>
        /// <param name="a">The alpha channel of the new color.</param>
        public RGBA(RGB rgb, byte a) : base(rgb, a) { }

        /// <summary>
        /// Constructs a new color from the given red, green, blue and alpha channels.
        /// </summary>
        /// <param name="r">The red channel of the new color.</param>
        /// <param name="g">The green channel of the new color.</param>
        /// <param name="b">The blue channel of the new color.</param>
        /// <param name="a">The alpha channel of the new color.</param>
        public RGBA(byte r, byte g, byte b, byte a) : base(r, g, b, a) { }

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green, blue and alpha scaled down by 255.</returns>
        public FRGBA ToFRGBA()
        {
            return new FRGBA(ToFRGB(), A / 255d);
        }

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], and saturation, value and alpha [0, 1].</returns>
        public HSVA ToHSVA()
        {
            return new HSVA(RGB, A / 255d);
        }

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with the same channels, but alpha truncated.</returns>
        public RGB ToRGB()
        {
            return new RGB(R, G, B);
        }

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue scaled down by 255, and alpha truncated.</returns>
        public FRGB ToFRGB()
        {
            return new FRGB(R / 255d, G / 255d, B / 255d);
        }

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1], and alpha truncated.</returns>
        public HSV ToHSV()
        {
            return RGB;
        }

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(RGBA rgba)
        {
            return rgba.ToFRGBA();
        }

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(RGBA rgba)
        {
            return rgba.ToHSVA();
        }

        /// <inheritdoc cref="ToRGB"/>
        public static explicit operator RGB(RGBA rgba)
        {
            return rgba.ToRGB();
        }

        /// <inheritdoc cref="ToFRGB"/>
        public static explicit operator FRGB(RGBA rgba)
        {
            return rgba.ToFRGB();
        }

        /// <inheritdoc cref="ToHSV"/>
        public static explicit operator HSV(RGBA rgba)
        {
            return rgba.ToHSV();
        }

        /// <inheritdoc cref="FRGB.operator +(FRGB, FRGB)"/>
        public static RGBA operator +(RGBA lhs, RGBA rhs)
        {
            return new RGBA(((Vector4<byte>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator -(FRGB, FRGB)"/>
        public static RGBA operator -(RGBA lhs, RGBA rhs)
        {
            return new RGBA(((Vector4<byte>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, FRGB)"/>
        public static RGBA operator *(RGBA lhs, RGBA rhs)
        {
            return new RGBA(((Vector4<byte>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, double)"/>
        public static FVec4 operator *(RGBA lhs, double scalar)
        {
            return new FVec4((new Vector4<double>(lhs.R, lhs.G, lhs.B, lhs.A) * scalar).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, FRGB)"/>
        public static RGBA operator /(RGBA lhs, RGBA rhs)
        {
            return new RGBA(((Vector4<byte>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, double)"/>
        public static FVec4 operator /(RGBA lhs, double scalar)
        {
            return new FVec4((new Vector4<double>(lhs.R, lhs.G, lhs.B, lhs.A) / scalar).Components);
        }
    }
}
