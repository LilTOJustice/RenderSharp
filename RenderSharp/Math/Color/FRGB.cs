namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector3{T}"/> of type double. Values <see cref="R"/>, <see cref="G"/> and <see cref="B"/>
    /// all intended to lie within the range [0, 1].
    /// </summary>
    public class FRGB : Vector3<double>
    {
        /// <summary>
        /// Red channel. Intended to range [0, 1].
        /// </summary>
        public double R { get { return this[0]; } set { this[0] = value; } }

        /// <summary>
        /// Green channel. Intended to range [0, 1].
        /// </summary>
        public double G { get { return this[1]; } set { this[1] = value; } }

        /// <summary>
        /// Blue channel. Intended to range [0, 1].
        /// </summary>
        public double B { get { return this[2]; } set { this[2] = value; } }

        /// <summary>
        /// Constructs a 0-valued color.
        /// </summary>
        public FRGB() { }

        /// <summary>
        /// <c>Shallow</c> copies the input array into the new color.
        /// </summary>
        public FRGB(double[] vec) : base(vec) { }

        /// <summary>
        /// <c>Deep</c> copies the input color into the new color.
        /// </summary>
        public FRGB(FRGB rgbf) : base(rgbf) { }

        /// <summary>
        /// Constructs a new color from the given r, g and b components.
        /// </summary>
        /// <param name="r">The red component of the new color. Intended [0, 1].</param>
        /// <param name="g">The green component of the new color. Intended [0, 1].</param>
        /// <param name="b">The blue component of the new color. Intended [0, 1].</param>
        public FRGB(double r, double g, double b) : base(r, g, b) { }

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with values scaled by 255.</returns>
        public RGB ToRGB()
        {
            return new RGB((byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with values scaled by 255, and an additional alpha 255.</returns>
        public RGBA ToRGBA()
        {
            return ToRGB();
        }

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with the same values, and an additional alpha 1.</returns>
        public FRGBA ToFRGBA()
        {
            return ToRGB();
        }

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1].</returns>
        public HSV ToHSV()
        {
            return ToRGB();
        }

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1], and an additional alpha 1.</returns>
        public HSVA ToHSVA()
        {
            return ToRGB();
        }

        public static implicit operator RGB(FRGB rgbf)
        {
            return rgbf.ToRGB();
        }

        public static implicit operator RGBA(FRGB rgbf)
        {
            return rgbf.ToRGBA();
        }

        public static implicit operator FRGBA(FRGB rgbf)
        {
            return rgbf.ToFRGBA();
        }

        public static implicit operator HSV(FRGB rgbf)
        {
            return rgbf.ToHSV();
        }

        public static implicit operator HSVA(FRGB rgbf)
        {
            return rgbf.ToHSVA();
        }

        /// <summary>
        /// Computes the vector cross product between two colors.
        /// </summary>
        /// <returns>A new color with the result of the cross product.</returns>
        public FRGB Cross(FRGB rhs)
        {
            return new FRGB(Cross(this, rhs).Components);
        }

        /// <summary>
        /// Adds two colors together using standard vector addition.
        /// </summary>
        /// <returns>A new color with the result of the addition.</returns>
        public static FRGB operator +(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs + rhs).Components);
        }

        /// <summary>
        /// Subtracts two colors using standard vector subtraction.
        /// </summary>
        /// <returns>A new color with the result of the subtraction.</returns>
        public static FRGB operator -(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs + rhs).Components);
        }

        /// <summary>
        /// Performs inline multiplication between two colors.
        /// </summary>
        /// <returns>A new color with the result of the inline multiplication.</returns>
        public static FRGB operator *(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs * rhs).Components);
        }

        /// <summary>
        /// Performs scalar multiplication between a color and a scalar.
        /// </summary>
        /// <param name="lhs">The color to scale as a vector.</param>
        /// <param name="scalar">The scalar to scale by.</param>
        /// <returns>A new color with the result of the scalar multiplication.</returns>
        public static FRGB operator *(FRGB lhs, double scalar)
        {
            return new FRGB(((Vector3<double>)lhs * scalar).Components);
        }

        /// <summary>
        /// Performs inline division between two colors.
        /// </summary>
        /// <returns>A new color with the result of the inline division.</returns>
        public static FRGB operator /(FRGB lhs, FRGB rhs)
        {
            return new FRGB(((Vector3<double>)lhs / rhs).Components);
        }

        /// <summary>
        /// Performs scalar division between a color and a scalar.
        /// </summary>
        /// <param name="lhs">The color to scale as a vector.</param>
        /// <param name="scalar">The scalar to scale by.</param>
        /// <returns>A new color with the result of the scalar division.</returns>
        public static FRGB operator /(FRGB lhs, double scalar)
        {
            return new FRGB(((Vector3<double>)lhs / scalar).Components);
        }
    }
}
