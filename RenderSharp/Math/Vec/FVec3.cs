namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector3{T}"/> (3d vector) of type double.
    /// </summary>
    public class FVec3 : Vector3<double>
    {
        /// <inheritdoc cref="Vector3{T}.Vector3()"/>
        public FVec3() : base() { }

        /// <inheritdoc cref="Vector3{T}.Vector3(T[])"/>
        public FVec3(double[] vec) : base(vec) { }

        /// <inheritdoc cref="Vector3{T}.Vector3(Vector3{T})"/>
        public FVec3(FVec3 other) : base(other) { }

        /// <inheritdoc cref="Vector3{T}.Vector3(T, T, T)"/>
        public FVec3(double X, double Y, double Z) : base(X, Y, Z) { }

        /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T}, Vector3{T})"/>
        public static FVec3 Cross(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(Vector3<double>.Cross(lhs, rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T})"/>
        public FVec3 Cross(FVec3 rhs)
        {
            return Cross(this, rhs);
        }

        public static explicit operator Vec3(FVec3 vec)
        {
            return new Vec3((int)vec.X, (int)vec.Y, (int)vec.Z);
        }

        /// <inheritdoc cref="Vector3{T}.operator +(Vector3{T}, Vector3{T})"/>
        public static FVec3 operator +(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator -(Vector3{T}, Vector3{T})"/>
        public static FVec3 operator -(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator *(Vector3{T}, Vector3{T})"/>
        public static FVec3 operator *(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator *(Vector3{T}, T)"/>
        public static FVec3 operator *(FVec3 lhs, double scalar)
        {
            return new FVec3(((Vector3<double>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator /(Vector3{T}, Vector3{T})"/>
        public static FVec3 operator /(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator /(Vector3{T}, T)"/>
        public static FVec3 operator /(FVec3 lhs, double scalar)
        {
            return new FVec3(((Vector3<double>)lhs / scalar).Components);
        }
    }
}
