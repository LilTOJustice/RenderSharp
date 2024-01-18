namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector4{T}"/> (4d vector) of type int.
    /// </summary>
    public class Vec4 : Vector4<int>
    {
        /// <inheritdoc cref="Vector4{T}.Vector4()"/>
        public Vec4() : base() { }

        /// <inheritdoc cref="Vector4{T}.Vector4(T[])"/>
        public Vec4(int[] vec) : base(vec) { }
        
        /// <inheritdoc cref="Vector4{T}.Vector4(Vector4{T})"/>
        public Vec4(Vec4 other) : base(other) { }

        /// <inheritdoc cref="Vector4{T}.Vector4(T, T, T, T)"/>
        public Vec4(int X, int Y, int Z, int W) : base(X, Y, Z, W) { }

        public static implicit operator FVec4(Vec4 vec)
        {
            return new FVec4(vec.X, vec.Y, vec.Z, vec.W);
        }

        /// <inheritdoc cref="Vector4{T}.operator +(Vector4{T}, Vector4{T})"/>
        public static Vec4 operator +(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator -(Vector4{T}, Vector4{T})"/>
        public static Vec4 operator -(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator *(Vector4{T}, Vector4{T})"/>
        public static Vec4 operator *(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator *(Vector4{T}, T)"/>
        public static Vec4 operator *(Vec4 lhs, int scalar)
        {
            return new Vec4(((Vector4<int>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator *(Vector4{T}, T)"/>
        public static FVec4 operator *(Vec4 lhs, double scalar)
        {
            return new FVec4((new Vector4<double>(lhs.X, lhs.Y, lhs.Z, lhs.W) * scalar).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator /(Vector4{T}, Vector4{T})"/>
        public static Vec4 operator /(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator /(Vector4{T}, T)"/>
        public static Vec4 operator /(Vec4 lhs, int scalar)
        {
            return new Vec4(((Vector4<int>)lhs / scalar).Components);
        }

        /// <inheritdoc cref="Vector4{T}.operator /(Vector4{T}, T)"/>
        public static FVec4 operator /(Vec4 lhs, double scalar)
        {
            return new FVec4((new Vector4<double>(lhs.X, lhs.Y, lhs.Z, lhs.W) / scalar).Components);
        }
    }
}
