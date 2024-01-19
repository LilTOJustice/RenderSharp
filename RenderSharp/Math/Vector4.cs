using System.Numerics;

namespace RenderSharp.Math
{
    /// <summary>
    /// A mathematical vector of four dimensions and any <see cref="INumber{TSelf}"/> type.
    /// </summary>
    /// <typeparam name="T">The internal type of the vector. Must be <see cref="INumber{TSelf}"/>.</typeparam>
    public class Vector4<T> : Vector<T>
        where T : INumber<T>
    {
        /// <inheritdoc cref="Vector3{T}.X"/>
        public T X { get { return this[0]; } set { this[0] = value; } }

        /// <inheritdoc cref="Vector3{T}.Y"/>
        public T Y { get { return this[1]; } set { this[1] = value; } }

        /// <inheritdoc cref="Vector3{T}.Z"/>
        public T Z { get { return this[2]; } set { this[2] = value; } }

        /// <summary>
        /// The w component of the vector. Position 3 in the internal array <see cref="Vector{T}.vec"/>.
        /// </summary>
        public T W { get { return this[3]; } set { this[3] = value; } }

        /// <inheritdoc cref="Vector3{T}.XY"/>
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

        /// <inheritdoc cref="Vector3{T}.XZ"/>
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

        /// <inheritdoc cref="Vector3{T}.YZ"/>
        public Vector2<T> YZ
        {
            get
            {
                return new Vector2<T>(Y, Z);
            }
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// A new 3d vector containing the x, y and z components of the vector.
        /// </summary>
        public Vector3<T> XYZ
        {
            get
            {
                return new Vector3<T>(X, Y, Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// A new 3d vector containing the x, y, and w components of the vector.
        /// </summary>
        public Vector3<T> XYW
        {
            get
            {
                return new Vector3<T>(X, Y, W);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// A new 3d vector containing the x, z, and w components of the vector.
        /// </summary>
        public Vector3<T> XZW
        {
            get
            {
                return new Vector3<T>(X, Z, W);
            }
            set
            {
                X = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// A new 3d vector containing the y, z, and w components of the vector.
        /// </summary>
        public Vector3<T> YZW
        {
            get
            {
                return new Vector3<T>(Y, Z, W);
            }
            set
            {
                Y = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Constructs a 4d zero vector.
        /// </summary>
        public Vector4() : base(4) { }

        /// <inheritdoc cref="Vector{T}.Vector(T[])"/>
        public Vector4(T[] vec) : base(vec) { }

        /// <summary>
        /// Constructs a 4d vector from the given vector, z and w components.
        /// </summary>
        /// <param name="vec">The x and y components of the new vector.</param>
        /// <param name="Z">The z component of the new vector.</param>
        /// <param name="W">The w component of the new vector.</param>
        public Vector4(Vector2<T> vec, T Z, T W)
            : base(vec, 4)
        {
            this.Z = Z;
            this.W = W;
        }

        /// <summary>
        /// Constructs a 4d vector from the given vector and w component.
        /// </summary>
        /// <param name="vec">The x, y and z components of the new vector.</param>
        /// <param name="W">The w component of the new vector.</param>
        public Vector4(Vector3<T> vec, T W)
            : base(vec, 4)
        {
            this.W = W;
        }
        
        /// <inheritdoc cref="Vector{T}.Vector(Vector{T})"/>
        public Vector4(Vector4<T> vec) : base(vec) { }

        /// <summary>
        /// Constructs a 4d vector from the x, y, z and w components.
        /// </summary>
        /// <param name="X">The x component of the new vector.</param>
        /// <param name="Y">The y component of the new vector.</param>
        /// <param name="Z">The z component of the new vector.</param>
        /// <param name="W">The w component of the new vector.</param>
        public Vector4(T X, T Y, T Z, T W)
            : base(4)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        /// <inheritdoc cref="Vector{T}.operator +(Vector{T}, Vector{T})"/>
        public static Vector4<T> operator +(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs + rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator -(Vector{T}, Vector{T})"/>
        public static Vector4<T> operator -(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs - rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator *(Vector{T}, Vector{T})"/>
        public static Vector4<T> operator *(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs * rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator *(Vector{T}, T)"/>
        public static Vector4<T> operator *(Vector4<T> lhs, T scalar)
        {
            return new Vector4<T>(((Vector<T>)lhs * scalar).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator /(Vector{T}, Vector{T})"/>
        public static Vector4<T> operator /(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs / rhs).Components);
        }

        /// <inheritdoc cref="Vector{T}.operator /(Vector{T}, T)"/>
        public static Vector4<T> operator /(Vector4<T> lhs, T scalar)
        {
            return new Vector4<T>(((Vector<T>)lhs / scalar).Components);
        }
    }
}
