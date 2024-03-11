using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="IVec4{TSelf, TBase, TFloat, TVFloat}"/> of type double. Values <see cref="R"/>, <see cref="G"/>,
    /// <see cref="B"/> and <see cref="A"/> all intended to lie within the range [0, 1].
    /// </summary>
    public struct FRGBA : IVec4<FRGBA, double, double, FRGBA>, IEquatable<FRGBA>, ISwizzlable<FRGBA, double>
    {
        /// <inheritdoc cref="FRGB.R"/>
        public double R { get { return X; } set { X = value; } }
        
        /// <inheritdoc cref="FRGB.G"/>
        public double G { get { return Y; } set { Y = value; } }
        
        /// <inheritdoc cref="FRGB.B"/>
        public double B { get { return Z; } set { Z = value; } }

        /// <summary>
        /// Alpha channel. Intended to range [0, 1].
        /// </summary>
        public double A { get { return W; } set { W = value; } }

        /// <inheritdoc cref="IVec4{T, T, T, T}.X"/>
        public double X { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Y"/>
        public double Y { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Z"/>
        public double Z { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.W"/>
        public double W { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Components"/>
        public double[] Components => IVec4<FRGBA, double, double, FRGBA>.IComponents(this);

        /// <inheritdoc cref="ISwizzlable{T, T}.SwizzleMap"/>
        public static Dictionary<char, int> SwizzleMap => new Dictionary<char, int>
        {
            { 'r', 0 },
            { 'g', 1 },
            { 'b', 2 },
            { 'a', 3 }
        };

        /// <inheritdoc cref="ISwizzlable{T, T}.this[string]"/>
        public double[] this[string swizzle] {
            get => IVec4<FRGBA, double, double, FRGBA>.ISwizzleGet(this, swizzle);
            set => IVec4<FRGBA, double, double, FRGBA>.ISwizzleSet(ref this, swizzle, value);
        }

        /// <inheritdoc cref="IVec4{T, T, T, T}.this[int]"/>
        public double this[int i]
        {
            get => IVec4<FRGBA, double, double, FRGBA>.IIndexerGet(this, i);
            set => IVec4<FRGBA, double, double, FRGBA>.IIndexerSet(ref this, i, value);
        }

        /// <summary>
        /// Constructs a new color from the given color and alpha channel.
        /// </summary>
        /// <param name="rgbf">The red, green and blue channels of the new color.</param>
        /// <param name="a">The alpha channel of the new vector. Intended [0, 1].</param>
        public FRGBA(in FRGB rgbf, double a) { R = rgbf.R; G = rgbf.G; B = rgbf.B; A = a; }

        /// <summary>
        /// Constructs a new color from the given red, green, blue and alpha channels.
        /// </summary>
        /// <param name="r">The red channel of the new color. Intended [0, 1].</param>
        /// <param name="g">The green channel of the new color. Intended [0, 1].</param>
        /// <param name="b">The blue channel of the new color. Intended [0, 1].</param>
        /// <param name="a">The alpha channel of the new color. Intended [0, 1].</param>
        public FRGBA(double r, double g, double b, double a) { R = r; G = g; B = b; A = a; }

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with values scaled by 255.</returns>
        public RGBA ToRGBA() => new RGBA((byte)(R * 255), (byte)(R * 255), (byte)(R * 255), (byte)(A * 255));

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with hue [0, 360] and saturation, value and alpha [0 - 1].</returns>
        public HSVA ToHSVA() => new HSVA(ToHSV(), A);

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with values scaled down by 255 and alpha truncated.</returns>
        public RGB ToRGB() => new RGB((byte)(R * 255), (byte)(G * 255), (byte)(B * 255));

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with the same channels, but alpha truncated.</returns>
        public FRGB ToFRGB() => new FRGB(R, G, B);

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1] and alpha truncated.</returns>
        public HSV ToHSV() => ColorFunctions.RGBToHSV(ToRGB());

        /// <inheritdoc cref="IVec4{T, T, T, T}.Mag2"/>
        public double Mag2() => IVec4<FRGBA, double, double, FRGBA>.IMag2(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Mag"/>
        public double Mag() => IVec4<FRGBA, double, double, FRGBA>.IMag(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Dot(in T)"/>
        public double Dot(in FRGBA other) => IVec4<FRGBA, double, double, FRGBA>.IDot(this, other);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Norm"/>
        public FRGBA Norm() => IVec4<FRGBA, double, double, FRGBA>.INorm(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.IEquals(in T, in T)"/>
        public bool Equals(FRGBA other) => IVec4<FRGBA, double, double, FRGBA>.IEquals(this, other);

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(in FRGBA rgbaf) => rgbaf.ToRGBA();

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(in FRGBA rgbaf) => rgbaf.ToHSVA();

        /// <inheritdoc cref="ToRGB"/>
        public static explicit operator RGB(in FRGBA rgbaf) => rgbaf.ToRGB();

        /// <inheritdoc cref="ToFRGB"/>
        public static explicit operator FRGB(in FRGBA rgbaf) => rgbaf.ToFRGB();

        /// <inheritdoc cref="ToHSV"/>
        public static explicit operator HSV(in FRGBA rgbaf) => rgbaf.ToHSV();

        /// <inheritdoc cref="FRGB.operator +"/>
        public static FRGBA operator +(in FRGBA lhs, in FRGBA rhs) => IVec4<FRGBA, double, double, FRGBA>.IAdd(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator -"/>
        public static FRGBA operator -(in FRGBA lhs, in FRGBA rhs) => IVec4<FRGBA, double, double, FRGBA>.ISub(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, in FRGB)"/>
        public static FRGBA operator *(in FRGBA lhs, in FRGBA rhs) => IVec4<FRGBA, double, double, FRGBA>.IMul(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, double)"/>
        public static FRGBA operator *(in FRGBA lhs, double scalar) => IVec4<FRGBA, double, double, FRGBA>.IMul(lhs, scalar);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, in FRGB)"/>
        public static FRGBA operator /(in FRGBA lhs, in FRGBA rhs) => IVec4<FRGBA, double, double, FRGBA>.IDiv(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, double)"/>
        public static FRGBA operator /(in FRGBA lhs, double scalar) => IVec4<FRGBA, double, double, FRGBA>.IDiv(lhs, scalar);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T"/>
        public static implicit operator FRGBA(double[] swizzle) => IVec4<FRGBA, double, double, FRGBA>.ISwizzleToSelf(swizzle);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T[]"/>
        public static implicit operator double[](in FRGBA swizzler) => IVec4<FRGBA, double, double, FRGBA>.ISelfToSwizzle(swizzler);

        /// <inheritdoc cref="IVec4{T, T, T, T}.ToString"/>
        public override string ToString() => IVec4<FRGBA, double, double, FRGBA>.IToString(this);
    }
}
