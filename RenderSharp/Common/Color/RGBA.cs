using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="IVec4{TSelf, TBase, TFloat, TVFloat}"/> of type double. Values <see cref="R"/>, <see cref="G"/> and <see cref="B"/>
    /// all intended to lie within the range [0, 1].
    /// </summary>
    public struct RGBA : IVec4<RGBA, byte, double, FVec4>, IEquatable<RGBA>, ISwizzlable<RGBA, byte>
    {
        /// <inheritdoc cref="RGB.R"/>
        public byte R { get { return X; } set { X = value; } }

        /// <inheritdoc cref="RGB.G"/>
        public byte G { get { return Y; } set { Y = value; } }
        
        /// <inheritdoc cref="RGB.B"/>
        public byte B { get { return Z; } set { Z = value; } }
        
        /// <summary>
        /// Alpha channel. Intended to range [0, 255].
        /// </summary>
        public byte A { get { return W; } set { W = value; } }

        /// <inheritdoc cref="IVec4{T, T, T, T}.X"/>
        public byte X { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Y"/>
        public byte Y { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Z"/>
        public byte Z { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.W"/>
        public byte W { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Components"/>
        public byte[] Components => IVec4<RGBA, byte, double, FVec4>.IComponents(this);

        /// <inheritdoc cref="ISwizzlable{T, T}.SwizzleMap"/>
        public static Dictionary<char, int> SwizzleMap => new Dictionary<char, int>
        {
            { 'r', 0 },
            { 'g', 1 },
            { 'b', 2 },
            { 'a', 3 }
        };

        /// <inheritdoc cref="ISwizzlable{T, T}.this[string]"/>
        public byte[] this[string swizzle]
        {
            get => IVec4<RGBA, byte, double, FVec4>.ISwizzleGet(this, swizzle);
            set => IVec4<RGBA, byte, double, FVec4>.ISwizzleSet(ref this, swizzle, value);
        }

        /// <inheritdoc cref="IVec4{T, T, T, T}.this[int]"/>
        public byte this[int i]
        {
            get => IVec4<RGBA, byte, double, FVec4>.IIndexerGet(this, i);
            set => IVec4<RGBA, byte, double, FVec4>.IIndexerSet(ref this, i, value);
        }

        /// <summary>
        /// Constructs a new color from the given color vector and alpha channel.
        /// </summary>
        /// <param name="rgb">The red, green and blue channels of the new color.</param>
        /// <param name="a">The alpha channel of the new color.</param>
        public RGBA(in RGB rgb, byte a) { R = rgb.R; G = rgb.G; B = rgb.B; A = a; }

        /// <summary>
        /// Constructs a new color from the given red, green, blue and alpha channels.
        /// </summary>
        /// <param name="r">The red channel of the new color.</param>
        /// <param name="g">The green channel of the new color.</param>
        /// <param name="b">The blue channel of the new color.</param>
        /// <param name="a">The alpha channel of the new color.</param>
        public RGBA(byte r, byte g, byte b, byte a) { R = r; G = g; B = b; A = a; }

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green, blue and alpha scaled down by 255.</returns>
        public FRGBA ToFRGBA() => new FRGBA(ToFRGB(), A / 255d);

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], and saturation, value and alpha [0, 1].</returns>
        public HSVA ToHSVA() => new HSVA(ToRGB(), A / 255d);

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with the same channels, but alpha truncated.</returns>
        public RGB ToRGB() => new RGB(R, G, B);

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue scaled down by 255, and alpha truncated.</returns>
        public FRGB ToFRGB() => new FRGB(R / 255d, G / 255d, B / 255d);

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1], and alpha truncated.</returns>
        public HSV ToHSV() => ToRGB();

        /// <inheritdoc cref="IVec4{T, T, T, T}.Mag2"/>
        public byte Mag2() => IVec4<RGBA, byte, double, FVec4>.IMag2(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Mag"/>
        public double Mag() => IVec4<RGBA, byte, double, FVec4>.IMag(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Dot"/>
        public byte Dot(in RGBA other) => IVec4<RGBA, byte, double, FVec4>.IDot(this, other);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Norm"/>
        public FVec4 Norm() => IVec4<RGBA, byte, double, FVec4>.INorm(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.IEquals(in T, in T)"/>
        public bool Equals(RGBA other) => IVec4<RGBA, byte, double, FVec4>.IEquals(this, other);

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(in RGBA rgba) => rgba.ToFRGBA();

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(in RGBA rgba) => rgba.ToHSVA();

        /// <inheritdoc cref="ToRGB"/>
        public static explicit operator RGB(in RGBA rgba) => rgba.ToRGB();

        /// <inheritdoc cref="ToFRGB"/>
        public static explicit operator FRGB(in RGBA rgba) => rgba.ToFRGB();

        /// <inheritdoc cref="ToHSV"/>
        public static explicit operator HSV(in RGBA rgba) => rgba.ToHSV();

        /// <inheritdoc cref="FRGB.operator +"/>
        public static RGBA operator +(in RGBA lhs, in RGBA rhs) => IVec4<RGBA, byte, double, FVec4>.IAdd(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator -"/>
        public static RGBA operator -(in RGBA lhs, in RGBA rhs) => IVec4<RGBA, byte, double, FVec4>.ISub(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, in FRGB)"/>
        public static RGBA operator *(in RGBA lhs, in RGBA rhs) => IVec4<RGBA, byte, double, FVec4>.IMul(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, double)"/>
        public static FVec4 operator *(in RGBA lhs, double scalar) => IVec4<RGBA, byte, double, FVec4>.IFMul(lhs, scalar);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, in FRGB)"/>
        public static RGBA operator /(in RGBA lhs, in RGBA rhs) => IVec4<RGBA, byte, double, FVec4>.IDiv(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, double)"/>
        public static FVec4 operator /(in RGBA lhs, double scalar) => IVec4<RGBA, byte, double, FVec4>.IFDiv(lhs, scalar);

        /// <inheritdoc cref="IVec4{T, T, T, T}.ISwizzleToSelf"/>
        public static implicit operator RGBA(byte[] swizzle) => IVec4<RGBA, byte, double, FVec4>.ISwizzleToSelf(swizzle);

        /// <inheritdoc cref="IVec4{T, T, T, T}.ISelfToSwizzle"/>
        public static implicit operator byte[](in RGBA self) => IVec4<RGBA, byte, double, FVec4>.ISelfToSwizzle(self);

        /// <inheritdoc cref="IVec4{T, T, T, T}.ToString"/>
        public override string ToString() => IVec4<RGBA, byte, double, FVec4>.IToString(this);
    }
}
