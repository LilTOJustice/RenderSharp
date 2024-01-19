using MathSharp;

namespace RenderSharp.RendererCommon
{
    /// <summary>
    /// A <see cref="Vector3{T}"/> of type byte. Values <see cref="R"/>, <see cref="G"/> and <see cref="B"/>
    /// all intended to lie within the range [0, 255].
    /// </summary>
    public class RGB : Vector3<byte>
    {
        /// <summary>
        /// Red channel. Intended to range [0, 255].
        /// </summary>
        public byte R { get { return this[0]; } set { this[0] = value; } }

        /// <summary>
        /// Green channel. Intended to range [0, 255].
        /// </summary>
        public byte G { get { return this[1]; } set { this[1] = value; } }

        /// <summary>
        /// Blue channel. Intended to range [0, 255].
        /// </summary>
        public byte B { get { return this[2]; } set { this[2] = value; } }

        /// <inheritdoc cref="FRGB()"/>
        public RGB() { }

        /// <inheritdoc cref="FRGB(double[])"/>
        public RGB(byte[] vec) : base(vec) { }

        /// <inheritdoc cref="FRGB(FRGB)"/>
        public RGB(RGB rgb) : base(rgb) { }

        /// <summary>
        /// Constructs a new color from the given red, green and blue channels.
        /// </summary>
        /// <param name="r">The red channel of the new color. Intended [0, 255].</param>
        /// <param name="g">The green channel of the new color. Intended [0, 255].</param>
        /// <param name="b">The blue channel of the new color. Intended [0, 255].</param>
        public RGB(byte r, byte g, byte b) : base(r, g, b) { }

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue scaled down by 255.</returns>
        public FRGB ToFRGB()
        {
            return new FRGB(R / 255d, G / 255d, B / 255d);
        }

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with the same channels, and an additional alpha 255.</returns>
        public RGBA ToRGBA()
        {
            return new RGBA(this, 255);
        }

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green and blue scaled down by 255, and an additional alpha 1.</returns>
        public FRGBA ToFRGBA()
        {
            return new FRGBA(ToFRGB(), 1d);
        }

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1].</returns>
        public HSV ToHSV()
        {
            double R = this.R, G = this.G, B = this.B;
            double M = System.Math.Max(System.Math.Max(R, G), B);
            double m = System.Math.Min(System.Math.Min(R, G), B);
            double V = M / 255;
            double S = (M > 0 ? 1 - m / M : 0);
            double H = System.Math.Acos(
                (R - .5 * G - .5 * B) / System.Math.Sqrt(R * R + G * G + B * B - R * G - R * B - G * B)
            ) * Util.Constants.DEGPERPI;

            if (B > G)
            {
                H = 360 - H;
            }

            return new HSV(H, S, V);
        }

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1], and an additional alpha 1.</returns>
        public HSVA ToHSVA()
        {
            return new HSVA(ToHSV(), 1d);
        }

        /// <inheritdoc cref="ToFRGB"/>
        public static implicit operator FRGB(RGB rgb)
        {
            return rgb.ToFRGB();
        }

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(RGB rgb)
        {
            return rgb.ToRGBA();
        }

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(RGB rgb)
        {
            return rgb.ToFRGBA();
        }

        /// <inheritdoc cref="ToHSV"/>
        public static implicit operator HSV(RGB rgb)
        {
            return rgb.ToHSV();
        }

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(RGB rgb)
        {
            return rgb.ToHSVA();
        }

        /// <inheritdoc cref="FRGB.Cross(FRGB)"/>
        public RGB Cross(RGB rhs)
        {
            return new RGB(Cross(this, rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator +(FRGB, FRGB)"/>
        public static RGB operator +(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator -(FRGB, FRGB)"/>
        public static RGB operator -(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, FRGB)"/>
        public static RGB operator *(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator *(FRGB, double)"/>
        public static FVec3 operator *(RGB lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.R, lhs.G, lhs.B) * scalar).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, FRGB)"/>
        public static RGB operator /(RGB lhs, RGB rhs)
        {
            return new RGB(((Vector3<byte>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="FRGB.operator /(FRGB, double)"/>
        public static FVec3 operator /(RGB lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.R, lhs.G, lhs.B) / scalar).Components);
        }
    }
}
