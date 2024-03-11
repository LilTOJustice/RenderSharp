using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// A <see cref="IVec3{TSelf, TBase, TFloat, TVFloat}"/> of type double. Values <see cref="R"/>, <see cref="G"/> and <see cref="B"/>.
    /// all intended to lie within the range [0, 1].
    /// </summary>
    public struct FRGB : IVec3<FRGB, double, double, FRGB>, IEquatable<FRGB>, ISwizzlable<FRGB, double>
    {
        /// <summary>
        /// Red channel. Intended to range [0, 1].
        /// </summary>
        public double R { get { return X; } set { X = value; } }

        /// <summary>
        /// Green channel. Intended to range [0, 1].
        /// </summary>
        public double G { get { return Y; } set { Y = value; } }

        /// <summary>
        /// Blue channel. Intended to range [0, 1].
        /// </summary>
        public double B { get { return Z; } set { Z = value; } }

        /// <inheritdoc cref="IVec3{T, T, T, T}.X"/>
        public double X { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Y"/>
        public double Y { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Z"/>
        public double Z { get; set; }

        /// <inheritdoc cref="IVec3{T, T, T, T}.Components"/>
        public double[] Components => IVec3<FRGB, double, double, FRGB>.IComponents(this);

        /// <inheritdoc cref="ISwizzlable{T, T}.SwizzleMap"/>
        public static Dictionary<char, int> SwizzleMap => new Dictionary<char, int>
        {
            { 'r', 0 },
            { 'g', 1 },
            { 'b', 2 }
        };

        /// <inheritdoc cref="ISwizzlable{T, T}.this[string]"/>
        public double[] this[string swizzle]
        {
            get => IVec3<FRGB, double, double, FRGB>.ISwizzleGet(this, swizzle);
            set => IVec3<FRGB, double, double, FRGB>.ISwizzleSet(ref this, swizzle, value);
        }

        /// <inheritdoc cref="IVec3{T, T, T, T}.this[int]"/>
        public double this[int i]
        {
            get => IVec3<FRGB, double, double, FRGB>.IIndexerGet(this, i);
            set => IVec3<FRGB, double, double, FRGB>.IIndexerSet(ref this, i, value);
        }

        /// <summary>
        /// Constructs a new color from the given r, g and b components.
        /// </summary>
        /// <param name="r">The red component of the new color. Intended [0, 1].</param>
        /// <param name="g">The green component of the new color. Intended [0, 1].</param>
        /// <param name="b">The blue component of the new color. Intended [0, 1].</param>
        public FRGB(double r, double g, double b) { R = r; G = g; B = b; }

        /// <summary>
        /// Returns the color expressed in RGB space.
        /// </summary>
        /// <returns>A new color with values scaled by 255.</returns>
        public RGB ToRGB() => new RGB((byte)(R * 255), (byte)(G * 255), (byte)(B * 255));

        /// <summary>
        /// Returns the color expressed in RGBA space.
        /// </summary>
        /// <returns>A new color with values scaled by 255, and an additional alpha 255.</returns>
        public RGBA ToRGBA() => new RGBA((byte)(R * 255), (byte)(G * 255), (byte)(B * 255), 255);

        /// <summary>
        /// Returns the color expressed in FRGBA space.
        /// </summary>
        /// <returns>A new color with the same values, and an additional alpha 1.</returns>
        public FRGBA ToFRGBA() => new FRGBA(R, G, B, 1d);

        /// <summary>
        /// Returns the color expressed in HSV space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1].</returns>
        public HSV ToHSV() => ColorFunctions.RGBToHSV(ToRGB());

        /// <summary>
        /// Returns the color expressed in HSVA space.
        /// </summary>
        /// <returns>A new color with hue [0, 360], saturation and value [0, 1], and an additional alpha 1.</returns>
        public HSVA ToHSVA() => new HSVA(ToHSV(), 1d);

        /// <inheritdoc cref="ToRGB"/>
        public static implicit operator RGB(in FRGB rgbf) => rgbf.ToRGB();

        /// <inheritdoc cref="ToRGBA"/>
        public static implicit operator RGBA(in FRGB rgbf) => rgbf.ToRGBA();

        /// <inheritdoc cref="ToFRGBA"/>
        public static implicit operator FRGBA(in FRGB rgbf) => rgbf.ToFRGBA();

        /// <inheritdoc cref="ToHSV"/>
        public static implicit operator HSV(in FRGB rgbf) => rgbf.ToHSV();

        /// <inheritdoc cref="ToHSVA"/>
        public static implicit operator HSVA(in FRGB rgbf) => rgbf.ToHSVA();

        /// <inheritdoc cref="IVec3{T, T, T, T}.Rotate"/>
        public FRGB Rotate(in AVec3 angle) => IVec3<FRGB, double, double, FRGB>.IRotate(this, angle);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Mag2"/>
        public double Mag2() => IVec3<FRGB, double, double, FRGB>.IMag2(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Mag"/>
        public double Mag() => IVec3<FRGB, double, double, FRGB>.IMag(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Dot"/>
        public double Dot(in FRGB other) => IVec3<FRGB, double, double, FRGB>.IDot(this, other);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Cross"/>
        public FRGB Cross(in FRGB other) => IVec3<FRGB, double, double, FRGB>.ICross(in this, in other);

        /// <inheritdoc cref="IVec3{T, T, T, T}.Norm"/>
        public FRGB Norm() => IVec3<FRGB, double, double, FRGB>.INorm(this);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IEquals(in T, in T)"/>
        public bool Equals(FRGB other) => IVec3<FRGB, double, double, FRGB>.IEquals(this, other);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IAdd"/>
        public static FRGB operator +(in FRGB lhs, in FRGB rhs) => IVec3<FRGB, double, double, FRGB>.IAdd(lhs, rhs);

        /// <inheritdoc cref="IVec3{T, T, T, T}.ISub"/>
        public static FRGB operator -(in FRGB lhs, in FRGB rhs) => IVec3<FRGB, double, double, FRGB>.ISub(lhs, rhs);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IMul(in T, in T)"/>
        public static FRGB operator *(in FRGB lhs, in FRGB rhs) => IVec3<FRGB, double, double, FRGB>.IMul(lhs, rhs);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IMul(in T, T)"/>
        public static FRGB operator *(in FRGB lhs, double scalar) => IVec3<FRGB, double, double, FRGB>.IMul(lhs, scalar);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IDiv(in T, in T)"/>
        public static FRGB operator /(in FRGB lhs, in FRGB rhs) => IVec3<FRGB, double, double, FRGB>.IDiv(lhs, rhs);

        /// <inheritdoc cref="IVec3{T, T, T, T}.IDiv(in T, T)"/>
        public static FRGB operator /(in FRGB lhs, double scalar) => IVec3<FRGB, double, double, FRGB>.IDiv(lhs, scalar);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T"/>
        public static implicit operator FRGB(double[] swizzle) => IVec3<FRGB, double, double, FRGB>.ISwizzleToSelf(swizzle);

        /// <inheritdoc cref="ISwizzlable{T, T}.implicit operator T[]"/>
        public static implicit operator double[](in FRGB self) => IVec3<FRGB, double, double, FRGB>.ISelfToSwizzle(self);

        /// <inheritdoc cref="IVec3{T, T, T, T}.ToString"/>
        public override string ToString() => IVec3<FRGB, double, double, FRGB>.IToString(this);
    }
}
