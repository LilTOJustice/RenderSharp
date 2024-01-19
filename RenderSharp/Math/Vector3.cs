using System.Numerics;

namespace RenderSharp.Math
{
    /// <summary>
    /// A mathematical vector of three dimensions and any <see cref="INumber{TSelf}"/> type.
    /// </summary>
    /// <typeparam name="T">The internal type of the vector. Must be <see cref="INumber{TSelf}"/>.</typeparam>
    public class Vector3<T> : Vector<T>
        where T : INumber<T>
    {
        /// <inheritdoc cref="Vector2{T}.X"/>
        public T X { get { return this[0]; } set { this[0] = value; } }

        /// <inheritdoc cref="Vector2{T}.Y"/>
        public T Y { get { return this[1]; } set { this[1] = value; } }

        /// <summary>
        /// The z component of the vector. Position 2 in the internal array <see cref="Vector{T}.vec"/>.
        /// </summary>
        public T Z { get { return this[2]; } set { this[2] = value; } }

        /// <summary>
        /// A new 2d vector containing the x and y components of the vector.
        /// </summary>
        public Vector2<T> XY
        {
            get
            {
                return new Vector2<T>(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// A new 2d vector containing the x and z components of the vector.
        /// </summary>
        public Vector2<T> XZ
        {
            get
            {
                return new Vector2<T>(X, Z);
            }
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// A new 2d vector containing the y and z components of the vector.
        /// </summary>
        public Vector2<T> YZ
        {
            get
            {
                return new Vector2<T>(Y, Z);
            }
            set
            {
                Y = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Constructs a 3d zero vector.
        /// </summary>
        public Vector3() : base(3) { }

        /// <inheritdoc cref="Vector{T}.Vector(T[])"/>
        public Vector3(T[] vec) : base(vec) { }

        /// <summary>
        /// Constructs a 3d vector from the given vector and z component.
        /// </summary>
        /// <param name="vec">The x and y components of the new vector.</param>
        /// <param name="z">The z component of the new vector.</param>
        public Vector3(Vector2<T> vec, T z)
            : base(vec, 3)
        {
            this.Z = z;
        }

        /// <inheritdoc cref="Vector{T}.Vector(Vector{T})"/>
        public Vector3(Vector3<T> other) : base(other) { }

        /// <summary>
        /// Constructs a 3d vector from the given components.
        /// </summary>
        /// <param name="x">The x component of the new vector.</param>
        /// <param name="y">The y component of the new vector.</param>
        /// <param name="z">The z component of the new vector.</param>
        public Vector3(T x, T y, T z)
            : base(3)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <inheritdoc cref="Vector2{T}.Cross(Vector2{T})"/>
        public static Vector3<T> Cross(Vector3<T> lhs, Vector3<T> rhs)
        {
            return new Vector3<T>
            (
                lhs[1] * rhs[2] - lhs[2] * rhs[1],
                lhs[2] * rhs[0] - lhs[0] * rhs[2],
                lhs[0] * rhs[1] - lhs[1] * rhs[0]
            );
        }

        /// <inheritdoc cref="Vector3{T}.Cross(Vector3{T}, Vector3{T})"/>
        public Vector3<T> Cross(Vector3<T> rhs)
        {
            return Cross(this, rhs);
        }

        /// <inheritdoc cref="Vector{T}.operator +(Vector{T}, Vector{T})"/>
        public static Vector3<T> operator +(Vector3<T> lhs, Vector3<T> rhs)
        {
            return new Vector3<T>(((Vector<T>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator -(Vector{T}, Vector{T})"/>
        public static Vector3<T> operator -(Vector3<T> lhs, Vector3<T> rhs)
        {
            return new Vector3<T>(((Vector<T>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator *(Vector{T}, Vector{T})"/>
        public static Vector3<T> operator *(Vector3<T> lhs, Vector3<T> rhs)
        {
            return new Vector3<T>(((Vector<T>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator *(Vector{T}, T)"/>
        public static Vector3<T> operator *(Vector3<T> lhs, T scalar)
        {
            return new Vector3<T>(((Vector<T>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator /(Vector{T}, Vector{T})"/>
        public static Vector3<T> operator /(Vector3<T> lhs, Vector3<T> rhs)
        {
            return new Vector3<T>(((Vector<T>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator /(Vector{T}, T)"/>
        public static Vector3<T> operator /(Vector3<T> lhs, T scalar)
        {
            return new Vector3<T>(((Vector<T>)lhs / scalar).Components);
        }
    }
}
