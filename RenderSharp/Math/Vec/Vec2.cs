namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector2{T}"/> (2d vector) of type int.
    /// </summary>
    public class Vec2 : Vector2<int>
    {
        /// <inheritdoc cref="Vector2{T}.Vector2()"/>
        public Vec2() : base() { }

        /// <inheritdoc cref="Vector2{T}.Vector2(T[])"/>
        public Vec2(int[] vec) : base(vec) { }

        /// <inheritdoc cref="Vector2{T}.Vector2(Vector2{T})"/>
        public Vec2(Vec2 other) : base(other) { }

        /// <inheritdoc cref="Vector2{T}.Vector2(T, T)"/>
        public Vec2(int X, int Y) : base(X, Y) { }

        /// <inheritdoc cref="Vector2{T}.Cross(Vector2{T})"/>
        public Vec3 Cross(Vec2 rhs)
        {
            return new Vec3(Cross(this, rhs).Components);
        }

        public static implicit operator FVec2(Vec2 vec)
        {
            return new FVec2(vec.X, vec.Y);
        }

        /// <inheritdoc cref="Vector2{T}.operator +(Vector2{T}, Vector2{T})"/>
        public static Vec2 operator +(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator -(Vector2{T}, Vector2{T})"/>
        public static Vec2 operator -(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator *(Vector2{T}, Vector2{T})"/>
        public static Vec2 operator *(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator *(Vector2{T}, T)"/>
        public static Vec2 operator *(Vec2 lhs, int scalar)
        {
            return new Vec2(((Vector2<int>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator *(Vector2{T}, T)"/>
        public static FVec2 operator *(Vec2 lhs, double scalar)
        {
            return new FVec2((new Vector2<double>(lhs.X, lhs.Y) * scalar).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator /(Vector2{T}, Vector2{T})"/>
        public static Vec2 operator /(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator /(Vector2{T}, T)"/>
        public static Vec2 operator /(Vec2 lhs, int scalar)
        {
            return new Vec2(((Vector2<int>)lhs / scalar).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator /(Vector2{T}, T)"/>
        public static FVec2 operator /(Vec2 lhs, double scalar)
        {
            return new FVec2((new Vector2<double>(lhs.X, lhs.Y) / scalar).Components);
        }
    }
}
