using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="IVec3{TSelf, TBase, TFloat, TVFloat}"/> of type byte. Values <see cref="R"/>, <see cref="G"/> and <see cref="B"/>
    /// all intended to lie within the range [0, 255].
    /// </summary>
    public struct RGB : IVec3<RGB, byte, double, FVec3>, IEquatable<RGB>, ISwizzlable<RGB, byte>
    {
        /// <summary>
        /// Red channel. Intended to range [0, 255].
        /// </summary>
        public byte R { get { return X; } set { X = value; } }

        /// <summary>
        /// Green channel. Intended to range [0, 255].
        /// </summary>
        public byte G { get { return Y; } set { Y = value; } }

        /// <summary>
        /// Blue channel. Intended to range [0, 255].
        /// </summary>
        public byte B { get { return Z; } set { Z = value; } }

        /// <inheritdoc cref="IVec3{T, T, T, T}.X"/>
        public byte X { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Y"/>
        public byte Y { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Z"/>
        public byte Z { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Components"/>
        public byte[] Components => IVec3<RGB, byte, double, FVec3>.IComponents(this);

        /// <inheritdoc cref="ISwizzlable{T, T}.SwizzleMap"/>
        public static Dictionary<char, int> SwizzleMap => new Dictionary<char, int>
        {
            { 'r', 0 },
            { 'g', 1 },
            { 'b', 2 }
        };

        /// <inheritdoc cref="ISwizzlable{T, T}.this[string]"/>
        public byte[] this[string swizzle]
        {
            get => IVec3<RGB, byte, double, FVec3>.ISwizzleGet(this, swizzle);
            set => IVec3<RGB, byte, double, FVec3>.ISwizzleSet(ref this, swizzle, value);
        }

        /// <inheritdoc cref="IVec3{T, T, T, T}.this[int]"/>
        public byte this[int i]
        {
            get => IVec3<RGB, byte, double, FVec3>.IIndexerGet(this, i);
            set => IVec3<RGB, byte, double, FVec3>.IIndexerSet(ref this, i, value);
        }

        /// <summary>
        /// Constructs a new color from the given red, green and blue channels.
        /// </summary>
        /// <param name="r">The red channel of the new color. Intended [0, 255].</param>
        /// <param name="g">The green channel of the new color. Intended [0, 255].</param>
        /// <param name="b">The blue channel of the new color. Intended [0, 255].</param>
        public RGB(byte r, byte g, byte b) { R = r; G = g; B = b; }

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue scaled down by 255.</returns>
        public FRGB ToFRGB() => new FRGB(R / 255d, G / 255d, B / 255d);

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with the same channels, and an additional alpha 255.</returns>
        public RGBA ToRGBA() => new RGBA(this, 255);

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green and blue scaled down by 255, and an additional alpha 1.</returns>
        public FRGBA ToFRGBA() => new FRGBA(ToFRGB(), 1d);

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1].</returns>
        public HSV ToHSV()
        {
            double R = this.R, G = this.G, B = this.B;
            double M = Math.Max(Math.Max(R, G), B);
            double m = Math.Min(Math.Min(R, G), B);
            double V = M / 255;
            double S = (M > 0 ? 1 - m / M : 0);
            double H = Math.Acos(
                (R - .5 * G - .5 * B) / Math.Sqrt(R * R + G * G + B * B - R * G - R * B - G * B)
            ) * 180 / Math.PI;

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
        public HSVA ToHSVA() => new HSVA(ToHSV(), 1d);

        /// <inheritdoc cref="ToFRGB"/>
        public static implicit operator FRGB(in RGB rgb) => rgb.ToFRGB();

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(in RGB rgb) => rgb.ToRGBA();

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(in RGB rgb) => rgb.ToFRGBA();

        /// <inheritdoc cref="ToHSV"/>
        public static implicit operator HSV(in RGB rgb) => rgb.ToHSV();

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(in RGB rgb) => rgb.ToHSVA();

        /// <inheritdoc cref="IVec3{T, T, T, T}.Cross"/>
        public RGB Cross(in RGB rhs) => IVec3<RGB, byte, double, FVec3>.ICross(this, rhs);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Rotate"/>
        public FVec3 Rotate(in AVec3 angle) => IVec3<RGB, byte, double, FVec3>.IRotate(this, angle);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Mag2"/>
        public byte Mag2() => IVec3<RGB, byte, double, FVec3>.IMag2(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Mag"/>
        public double Mag() => IVec3<RGB, byte, double, FVec3>.IMag(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Dot"/>
        public byte Dot(in RGB other) => IVec3<RGB, byte, double, FVec3>.IDot(this, other);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Norm"/>
        public FVec3 Norm() => IVec3<RGB, byte, double, FVec3>.INorm(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IEquals(in T, in T)"/>
        public bool Equals(RGB other) => IVec3<RGB, byte, double, FVec3>.IEquals(this, other);

        /// <inheritdoc cref="FRGB.operator +"/>
        public static RGB operator +(in RGB lhs, in RGB rhs) => IVec3<RGB, byte, double, FVec3>.IAdd(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator -"/>
        public static RGB operator -(in RGB lhs, in RGB rhs) => IVec3<RGB, byte, double, FVec3>.ISub(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, in FRGB)"/>
        public static RGB operator *(in RGB lhs, in RGB rhs) => IVec3<RGB, byte, double, FVec3>.IMul(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, double)"/>
        public static FVec3 operator *(in RGB lhs, double scalar) => IVec3<RGB, byte, double, FVec3>.IFMul(lhs, scalar);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, in FRGB)"/>
        public static RGB operator /(in RGB lhs, in RGB rhs) => IVec3<RGB, byte, double, FVec3>.IDiv(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, double)"/>
        public static RGB operator /(in RGB lhs, byte scalar) => IVec3<RGB, byte, double, FVec3>.IDiv(lhs, scalar);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, double)"/>
        public static FVec3 operator /(in RGB lhs, double scalar) => IVec3<RGB, byte, double, FVec3>.IFDiv(lhs, scalar);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T"/>
        public static implicit operator RGB(byte[] swizzle) => IVec3<RGB, byte, double, FVec3>.ISwizzleToSelf(swizzle);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T[]"/>
        public static implicit operator byte[](in RGB swizzler) => IVec3<RGB, byte, double, FVec3>.ISelfToSwizzle(swizzler);

        /// <inheritdoc cref="IVec3{T, T, T, T}.ToString"/>
        public override string ToString() => IVec3<RGB, byte, double, FVec3>.IToString(this);
    }
}
