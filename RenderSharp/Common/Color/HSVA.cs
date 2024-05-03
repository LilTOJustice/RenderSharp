using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="IVec4{TSelf, TBase, TFloat, TVFloat}"/> of type double. Values <see cref="H"/> intended
    /// to lie within the range [0, 360] and <see cref="S"/>, <see cref="V"/> 
    /// and <see cref="A"/> intended to lie within the range [0, 1].
    /// </summary>
    public struct HSVA : IVec4<HSVA, double, double, HSVA>, IEquatable<HSVA>, ISwizzlable<HSVA, double>
    {
        /// <inheritdoc cref="HSV.H"/>
        public double H { get { return X; } set { X  = value; } }

        /// <inheritdoc cref="HSV.S"/>
        public double S { get { return Y; } set { Y  = value; } }
        
        /// <inheritdoc cref="HSV.V"/>
        public double V { get { return Z; } set { Z  = value; } }
        
        /// <inheritdoc cref="FRGBA.A"/>
        public double A { get { return W; } set { W  = value; } }

        /// <inheritdoc cref="IVec4{T, T, T, T}.X"/>
        public double X { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Y"/>
        public double Y { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Z"/>
        public double Z { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.W"/>
        public double W { get; set; }

        /// <inheritdoc cref="IVec4{T, T, T, T}.Components"/>
        public double[] Components => IVec4<HSVA, double, double, HSVA>.IComponents(this);

        /// <inheritdoc cref="ISwizzlable{T, T}.SwizzleMap"/>
        public static Dictionary<char, int> SwizzleMap => new Dictionary<char, int>
        {
            { 'h', 0 },
            { 's', 1 },
            { 'v', 2 },
            { 'a', 3 }
        };

        /// <inheritdoc cref="ISwizzlable{T, T}.this[string]"/>
        public double[] this[string swizzle]
        {
            get => IVec4<HSVA, double, double, HSVA>.ISwizzleGet(this, swizzle);
            set => IVec4<HSVA, double, double, HSVA>.ISwizzleSet(ref this, swizzle, value);
        }

        /// <inheritdoc cref="IVec4{T, T, T, T}.this[int]"/>
        public double this[int i]
        {
            get => IVec4<HSVA, double, double, HSVA>.IIndexerGet(this, i);
            set => IVec4<HSVA, double, double, HSVA>.IIndexerSet(ref this, i, value);
        }

        /// <summary>
        /// Constructs a new color from the given color and alpha channel.
        /// </summary>
        /// <param name="hsv">The hue, saturation and value channels of the new color.</param>
        /// <param name="a">The alpha channel of the new color. Intended [0, 1].</param>
        public HSVA(in HSV hsv, double a) { H = hsv.H; S = hsv.S; V = hsv.V; A = a; }

        /// <summary>
        /// Constructs a new color from the given hue, saturation, value and alpha channels.
        /// </summary>
        /// <param name="h">The hue channel of the new color. Intended [0, 1].</param>
        /// <param name="s">The saturation channel of the new color. Intended [0, 1].</param>
        /// <param name="v">The value channel of the new color. Intended [0, 1].</param>
        /// <param name="a">The alpha channel of the new color. Intended [0, 1].</param>
        public HSVA(double h, double s, double v, double a) { H = h; S = s; V = v; A = a; }

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with red, green, blue and alpha [0, 255].</returns>
        public RGBA ToRGBA() => new RGBA(ColorFunctions.HSVToRGB(ToHSV()), (byte)(A * 255));

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green, blue and alpha [0, 1].</returns>
        public FRGBA ToFRGBA() => new FRGBA(ToFRGB(), A);

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 255], and alpha truncated.</returns>
        public RGB ToRGB() => ColorFunctions.HSVToRGB(ToHSV());

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 1], and alpha truncated.</returns>
        public FRGB ToFRGB() => ColorFunctions.HSVToRGB(ToHSV()).ToFRGB();

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with the same channels, but alpha truncated.</returns>
        public HSV ToHSV() => new HSV(H, S, V);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Mag2"/>
        public double Mag2() => IVec4<HSVA, double, double, HSVA>.IMag2(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Mag"/>
        public double Mag() => IVec4<HSVA, double, double, HSVA>.IMag(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Dot"/>
        public double Dot(in HSVA other) => IVec4<HSVA, double, double, HSVA>.IDot(this, other);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Norm()"/>
        public HSVA Norm() => IVec4<HSVA, double, double, HSVA>.INorm(this);

        /// <inheritdoc cref="IVec4{T, T, T, T}.Norm()"/>
        public HSVA Norm(out double mag)
        {
            mag = Mag();
            return new HSVA(X / mag, Y / mag, Z / mag, W / mag);
        }

        /// <inheritdoc cref="IVec4{T, T, T, T}.IEquals(in T, in T)"/>
        public bool Equals(HSVA other) => IVec4<HSVA, double, double, HSVA>.IEquals(this, other);

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(in HSVA hsva) => hsva.ToRGBA();

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(in HSVA hsva) => hsva.ToFRGBA();

        /// <inheritdoc cref="ToRGB"/>
        public static explicit operator RGB(in HSVA hsva) => hsva.ToRGB();

        /// <inheritdoc cref="ToFRGB"/>
        public static explicit operator FRGB(in HSVA hsva) => hsva.ToFRGB();

        /// <inheritdoc cref="ToHSV"/>
        public static explicit operator HSV(in HSVA hsva) => hsva.ToHSV();

        /// <inheritdoc cref="FRGB.operator +"/>
        public static HSVA operator +(in HSVA lhs, in HSVA rhs) => IVec4<HSVA, double, double, HSVA>.IAdd(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator -"/>
        public static HSVA operator -(in HSVA lhs, in HSVA rhs) => IVec4<HSVA, double, double, HSVA>.ISub(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, in FRGB)"/>
        public static HSVA operator *(in HSVA lhs, in HSVA rhs) => IVec4<HSVA, double, double, HSVA>.IMul(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, double)"/>
        public static HSVA operator *(in HSVA lhs, double scalar) => IVec4<HSVA, double, double, HSVA>.IMul(lhs, scalar);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, in FRGB)"/>
        public static HSVA operator /(in HSVA lhs, in HSVA rhs) => IVec4<HSVA, double, double, HSVA>.IDiv(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, double)"/>
        public static HSVA operator /(in HSVA lhs, double scalar) => IVec4<HSVA, double, double, HSVA>.IDiv(lhs, scalar);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T"/>
        public static implicit operator HSVA(double[] swizzle) => IVec4<HSVA, double, double, HSVA>.ISwizzleToSelf(swizzle);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T[]"/>
        public static implicit operator double[](in HSVA swizzler) => IVec4<HSVA, double, double, HSVA>.ISelfToSwizzle(swizzler);

        /// <inheritdoc cref="IVec4{T, T, T, T}.ToString"/>
        public override string ToString() => IVec4<HSVA, double, double, HSVA>.IToString(this);
    }
}
