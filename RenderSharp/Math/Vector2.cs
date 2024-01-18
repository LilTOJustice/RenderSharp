using System.Numerics;

namespace RenderSharp.Math
{
    /// <summary>
    /// A mathematical vector of two dimensions and any <see cref="INumber{TSelf}"/> type.
    /// </summary>
    /// <typeparam name="T">The internal type of the vector. Must be <see cref="INumber{TSelf}"/>.</typeparam>
    public class Vector2<T> : Vector<T>
        where T : INumber<T>
    {
        /// <summary>
        /// The x component of the vector. Position 0 in the internal array <see cref="Vector{T}.vec"/>.
        /// </summary>
        public T X { get { return this[0]; } set { this[0] = value; } }

        /// <summary>
        /// The y component of the vector. Position 1 in the internal array <see cref="Vector{T}.vec"/>.
        /// </summary>
        public T Y { get { return this[1]; } set { this[1] = value; } }

        /// <summary>
        /// Constructs a 2d zero vector.
        /// </summary>
        public Vector2() : base(2) { }

        /// <inheritdoc cref="Vector{T}.Vector(T[])"/>
        public Vector2(T[] vec) : base(vec) { }

        /// <inheritdoc cref="Vector{T}.Vector(Vector{T})"/>
        public Vector2(Vector2<T> other) : base(other) { }

        /// <summary>
        /// Constructs a 2d vector with the given components.
        /// </summary>
        /// <param name="x">The x component of the new vector.</param>
        /// <param name="y">The y component of the new vector.</param>
        public Vector2(T x, T y)
            : base(2)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Computes the cross product between two vectors.
        /// </summary>
        /// <returns>A new 3d vector with the result of the cross product (<see href="https://en.wikipedia.org/wiki/Cross_product"/>).</returns>
        public static Vector3<T> Cross(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector3<T>(default!, default!, lhs[0] * rhs[1] - lhs[1] * rhs[0] );
        }

        /// <summary>
        /// Compute the 2d (scalar) cross product between two vectors.
        /// </summary>
        /// <returns>The magnitude of the result of a cross product between the two vectors. (<see href="https://en.wikipedia.org/wiki/Cross_product"/>).</returns>
        public static T Cross2d(Vector2<T> lhs, Vector2<T> rhs)
        {
            return lhs[0] * rhs[1] - lhs[1] * rhs[0];
        }

        /// <inheritdoc cref="Cross(Vector2{T}, Vector2{T})"/>
        public Vector3<T> Cross(Vector2<T> rhs)
        {
            return Cross(this, rhs);
        }

        /// <inheritdoc cref="Cross2d(Vector2{T}, Vector2{T})"/>
        public T Cross2d(Vector2<T> rhs)
        {
            return Cross2d(this, rhs);
        }

        /// <inheritdoc cref="Vector{T}.operator +(Vector{T}, Vector{T})"/>
        public static Vector2<T> operator +(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator +(Vector{T}, Vector{T})"/> 
        public static Vector2<T> operator -(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator *(Vector{T}, Vector{T})"/>
        public static Vector2<T> operator *(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator *(Vector{T}, T)"/>
        public static Vector2<T> operator *(Vector2<T> lhs, T scalar)
        {
            return new Vector2<T>(((Vector<T>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator /(Vector{T}, Vector{T})"/>
        public static Vector2<T> operator /(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator /(Vector{T}, T)"/>
        public static Vector2<T> operator /(Vector2<T> lhs, T scalar)
        {
            return new Vector2<T>(((Vector<T>)lhs / scalar).Components);
        }
    }
}
