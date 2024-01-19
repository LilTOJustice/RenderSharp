namespace RenderSharp.Math
{
    /// <summary>
    /// A <see cref="Vector2{T}"/> (2d vector) of type double.
    /// </summary>
    public class FVec2 : Vector2<double>
    {
        /// <inheritdoc cref="Vector2{T}.Vector2()"/>
        public FVec2() : base() { }

        /// <inheritdoc cref="Vector2{T}.Vector2(T[])"/>
        public FVec2(double[] vec) : base(vec) { }

        /// <inheritdoc cref="Vector2{T}.Vector2(Vector2{T})"/>
        public FVec2(FVec2 other) : base(other) { }
        
        /// <inheritdoc cref="Vector2{T}.Vector2(T, T)"/>
        public FVec2(double X, double Y) : base(X, Y) { }

        /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T}, Vector3{T})"/>
        public static FVec3 Cross(FVec2 lhs, FVec2 rhs)
        {
            return new FVec3(Vector2<double>.Cross(lhs, rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.Cross(Vector2{T})"/>
        public FVec3 Cross(FVec2 rhs)
        {
            return Cross(this, rhs);
        }

        /// <summary>
        /// Computes the rotated vector by the given radians.
        /// </summary>
        /// <param name="radians">Number of radians to rotate by.</param>
        /// <returns>A new vector with the result of the rotation.</returns>
        public FVec2 Rotate(double radians)
        {
            return new FVec2(X * System.Math.Cos(radians) - Y * System.Math.Sin(radians), X * System.Math.Sin(radians) + Y * System.Math.Cos(radians));
        }

        public static explicit operator Vec2(FVec2 vec)
        {
            return new Vec2((int)vec.X, (int)vec.Y);
        }

        /// <inheritdoc cref="Vector2{T}.operator +(Vector2{T}, Vector2{T})"/>
        public static FVec2 operator +(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator -(Vector2{T}, Vector2{T})"/>
        public static FVec2 operator -(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator *(Vector2{T}, Vector2{T})"/>
        public static FVec2 operator *(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator *(Vector2{T}, T)"/>
        public static FVec2 operator *(FVec2 lhs, double scalar)
        {
            return new FVec2(((Vector2<double>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator /(Vector2{T}, Vector2{T})"/>
        public static FVec2 operator /(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector2{T}.operator /(Vector2{T}, T)"/>
        public static FVec2 operator /(FVec2 lhs, double scalar)
        {
            return new FVec2(((Vector2<double>)lhs / scalar).Components);
        }
    }
}
