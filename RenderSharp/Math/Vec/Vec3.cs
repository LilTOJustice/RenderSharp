namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector3{T}"/> (3d vector) of type int.
    /// </summary>
    public class Vec3 : Vector3<int>
    {
        /// <inheritdoc cref="Vector3{T}.Vector3()"/>
        public Vec3() : base() { }

        /// <inheritdoc cref="Vector3{T}.Vector3(T[])"/>
        public Vec3(int[] vec) : base(vec) { }

        /// <inheritdoc cref="Vector3{T}.Vector3(Vector3{T})"/>
        public Vec3(Vec3 other) : base(other) { }
        
        /// <inheritdoc cref="Vector3{T}.Vector3(T, T, T)"/>
        public Vec3(int X, int Y, int Z) : base(X, Y, Z) { }

        /// <inheritdoc cref="Vector3{T}.Vector3(Vector3{T})"/>
        public Vec3 Cross(Vec3 rhs)
        {
            return new Vec3(Cross((Vector3<int>)this, rhs).Components);
        }

        public static implicit operator FVec3(Vec3 vec)
        {
            return new FVec3(vec.X, vec.Y, vec.Z);
        }

        /// <inheritdoc cref="Vector3{T}.operator +(Vector3{T}, Vector3{T})"/>
        public static Vec3 operator +(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator -(Vector3{T}, Vector3{T})"/>
        public static Vec3 operator -(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator *(Vector3{T}, Vector3{T})"/>
        public static Vec3 operator *(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator *(Vector3{T}, T)"/>
        public static Vec3 operator *(Vec3 lhs, int scalar)
        {
            return new Vec3(((Vector3<int>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator *(Vector3{T}, T)"/>
        public static FVec3 operator *(Vec3 lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.X, lhs.Y, lhs.Z) * scalar).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator /(Vector3{T}, Vector3{T})"/>
        public static Vec3 operator /(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator /(Vector3{T}, T)"/>
        public static Vec3 operator /(Vec3 lhs, int scalar)
        {
            return new Vec3(((Vector3<int>)lhs / scalar).Components);
        }

        /// <inheritdoc cref="Vector3{T}.operator /(Vector3{T}, T)"/>
        public static FVec3 operator /(Vec3 lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.X, lhs.Y, lhs.Z) / scalar).Components);
        }
    }
}
