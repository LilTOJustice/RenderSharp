namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector4{T}"/> (4d vector) of type double.
    /// </summary>
    public class FVec4 : Vector4<double>
    {
        /// <inheritdoc cref="Vector4{T}.Vector4()"/>
        public FVec4() : base() { }

        /// <inheritdoc cref="Vector4{T}.Vector4(T[])"/>
        public FVec4(double[] vec) : base(vec) { }

        /// <inheritdoc cref="Vector4{T}.Vector4(Vector4{T})"/>
        public FVec4(FVec4 other) : base(other) { }
        
        /// <inheritdoc cref="Vector4{T}.Vector4(T, T, T, T)"/>
        public FVec4(double X, double Y, double Z, double W) : base(X, Y, Z, W) { }

        public static explicit operator Vec4(FVec4 vec)
        {
            return new Vec4((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
        }

        /// <inheritdoc cref="Vector4{T}.operator +(Vector4{T}, Vector4{T})"/>
        public static FVec4 operator +(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator -(Vector4{T}, Vector4{T})"/>
        public static FVec4 operator -(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator *(Vector4{T}, Vector4{T})"/>
        public static FVec4 operator *(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator *(Vector4{T}, T)"/>
        public static FVec4 operator *(FVec4 lhs, double scalar)
        {
            return new FVec4(((Vector4<double>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator /(Vector4{T}, Vector4{T})"/>
        public static FVec4 operator /(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator /(Vector4{T}, T)"/>
        public static FVec4 operator /(FVec4 lhs, double scalar)
        {
            return new FVec4(((Vector4<double>)lhs / scalar).Components);
        }
    }
}
