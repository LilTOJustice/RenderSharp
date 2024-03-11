using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="IVec3{TSelf, TBase, TFloat, TVFloat}"/> of type double. Values <see cref="H"/> intended
    /// to lie within the range [0, 360] and <see cref="S"/> and <see cref="V"/>
    /// intended to lie within the range [0, 1].
    /// </summary>
    public struct HSV : IVec3<HSV, double, double, HSV>, IEquatable<HSV>, ISwizzlable<HSV, double>
    {
        /// <summary>
        /// Hue channel. Intended to range [0, 360].
        /// </summary>
        public double H { get { return X; } set { X = value; } } 

        /// <summary>
        /// Saturation channel. Intended to range [0, 1].
        /// </summary>
        public double S { get { return Y; } set { Y = value; } } 
        
        /// <summary>
        /// Value channel. Intended to range [0, 1].
        /// </summary>
        public double V { get { return Z; } set { Z = value; } }

        /// <inheritdoc cref="IVec3{T, T, T, T}.X"/>
        public double X { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Y"/>
        public double Y { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Z"/>
        public double Z { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Components"/>
        public double[] Components => IVec3<HSV, double, double, HSV>.IComponents(this);

        /// <inheritdoc cref="ISwizzlable{T, T}.SwizzleMap"/>
        public static Dictionary<char, int> SwizzleMap => new Dictionary<char, int>
        {
            { 'h', 0 },
            { 's', 1 },
            { 'v', 2 }
        };

        /// <inheritdoc cref="ISwizzlable{T, T}.this[string]"/>
        public double[] this[string swizzle]
        {
            get => IVec3<HSV, double, double, HSV>.ISwizzleGet(this, swizzle);
            set => IVec3<HSV, double, double, HSV>.ISwizzleSet(ref this, swizzle, value);
        }

        /// <inheritdoc cref="IVec3{T, T, T, T}.this[int]"/>
        public double this[int i]
        {
            get => IVec3<HSV, double, double, HSV>.IIndexerGet(this, i);
            set => IVec3<HSV, double, double, HSV>.IIndexerSet(ref this, i, value);
        }

        /// <summary>
        /// Constructs a new color from the given hue, saturation and value.
        /// </summary>
        /// <param name="h">The hue channel of the new color. Intended [0, 360].</param>
        /// <param name="s">The saturation channel of the new color. Intended [0, 1].</param>
        /// <param name="v">The value channel of the new color. Intended [0, 1].</param>
        public HSV(double h, double s, double v) { H = h; S = s; V = v; }

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 255].</returns>
        public RGB ToRGB() => ColorFunctions.HSVToRGB(this);

        /// <summary>
        /// Returns the color expressed in FRGB space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 1].</returns>
        public FRGB ToFRGB() => ColorFunctions.HSVToRGB(this).ToFRGB();

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 255], and an additional alpha 255.</returns>
        public RGBA ToRGBA() => new RGBA(ToRGB(), 255);

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with red, green and blue [0, 1], and an addition alpha 1.</returns>
        public FRGBA ToFRGBA() => new FRGBA(ToFRGB(), 1d);

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with the same values, and an addition alpha 1.</returns>
        public HSVA ToHSVA() => new HSVA(this, 1d);

        /// <inheritdoc cref="ToRGB"/>
        public static implicit operator RGB(in HSV hsv) => hsv.ToRGB();

        /// <inheritdoc cref="ToFRGB"/>
        public static implicit operator FRGB(in HSV hsv) => hsv.ToFRGB();

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(in HSV hsv) => hsv.ToRGBA();

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(in HSV hsv) => hsv.ToFRGBA();

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(in HSV hsv) => hsv.ToHSVA();

        /// <inheritdoc cref="IVec3{T, T, T, T}.ICross"/>
        public HSV Cross(in HSV rhs) => IVec3<HSV, double, double, HSV>.ICross(this, rhs);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Rotate"/>
        public HSV Rotate(in AVec3 angle) => IVec3<HSV, double, double, HSV>.IRotate(this, angle);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Mag2"/>
        public double Mag2() => IVec3<HSV, double, double, HSV>.IMag2(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Mag"/>
        public double Mag() => IVec3<HSV, double, double, HSV>.IMag(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Dot"/>
        public double Dot(in HSV other) => IVec3<HSV, double, double, HSV>.IDot(this, other);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Norm"/>
        public HSV Norm() => IVec3<HSV, double, double, HSV>.INorm(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IEquals(in T, in T)"/>
        public bool Equals(HSV other) => IVec3<HSV, double, double, HSV>.IEquals(this, other);

        /// <inheritdoc cref="FRGB.operator +"/>
        public static HSV operator +(in HSV lhs, in HSV rhs) => IVec3<HSV, double, double, HSV>.IAdd(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator -"/>
        public static HSV operator -(in HSV lhs, in HSV rhs) => IVec3<HSV, double, double, HSV>.ISub(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, in FRGB)"/>
        public static HSV operator *(in HSV lhs, in HSV rhs) => IVec3<HSV, double, double, HSV>.IMul(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator *(in FRGB, double)"/>
        public static HSV operator *(in HSV lhs, double scalar) => IVec3<HSV, double, double, HSV>.IMul(lhs, scalar);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, in FRGB)"/>
        public static HSV operator /(in HSV lhs, in HSV rhs) => IVec3<HSV, double, double, HSV>.IDiv(lhs, rhs);

        /// <inheritdoc cref="FRGB.operator /(in FRGB, double)"/>
        public static HSV operator /(in HSV lhs, double scalar) => IVec3<HSV, double, double, HSV>.IDiv(lhs, scalar);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T"/>
        public static implicit operator HSV(double[] swizzle) => IVec3<HSV, double, double, HSV>.ISwizzleToSelf(swizzle);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T[]"/>
        public static implicit operator double[](in HSV swizzler) => IVec3<HSV, double, double, HSV>.ISelfToSwizzle(swizzler);

        /// <inheritdoc cref="IVec3{T, T, T, T}.ToString"/>
        public override string ToString() => IVec3<HSV, double, double, HSV>.IToString(this);
    }
}
