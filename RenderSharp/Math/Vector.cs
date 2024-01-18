using System.Numerics;
using System.Text;

namespace RenderSharp.Math
{
    /// <summary>
    /// A mathematical vector of any dimension and any <see cref="INumber{TSelf}"/> type.
    /// </summary>
    /// <typeparam name="T">The internal type of the vector. Must be <see cref="INumber{TSelf}"/>.</typeparam>
    public class Vector<T> : IEquatable<Vector<T>>
        where T : INumber<T>
    {
        protected T[] vec;

        /// <summary>
        /// Number of dimensions the vector lies within.
        /// </summary>
        public int Dimensions { get { return vec.Length; } }

        /// <summary>
        /// Internal array representing the components of the vector.
        /// </summary>
        public T[] Components { get { return vec; } }

        /// <summary>
        /// Constructs a vector of <paramref name="dimensions"/> size.
        /// </summary>
        /// <param name="dimensions">Number of dimensions the vector is to lie within.</param>
        public Vector(int dimensions)
        {
            vec = new T[dimensions];
        }

        /// <summary>
        /// <c>Shallow</c> copies the array <paramref name="vec"/> to create the vector.
        /// </summary>
        /// <param name="vec">The array to <c>shallow</c></param>
        /// <seealso cref="Vector{T}.Vector(Vector{T})"/> for <c>deep</c> copy.
        public Vector(T[] vec)
        {
            this.vec = vec;
        }

        /// <summary>
        /// <c>Deep</c> copies <paramref name="other"/> to create the vector.
        /// </summary>
        /// <param name="other">The vector to <c>deep</c> copy from.</param>
        /// <seealso cref="Vector{T}.Vector(T[])"/> for <c>shallow</c> copy.
        public Vector(Vector<T> other)
        {
            vec = new T[other.Dimensions];
            other.Components.CopyTo(vec, 0);
        }

        /// <summary>
        /// <inheritdoc cref="Vector{T}.Vector(Vector{T})"/> Allows specification of new vector dimension.
        /// </summary>
        /// <param name="other">The vector to <c>deep</c> copy from.</param>
        /// <param name="dimensions">The final dimensions of the new vector.</param>
        public Vector(Vector<T> other, int dimensions)
        {
            vec = new T[dimensions];
            other.vec.CopyTo(vec, 0);
        }

        /// <summary>
        /// This helper performs checks essential for binary operations between <see cref="Vector{T}"/>s.
        /// </summary>
        /// <param name="vec1">The left hand side vector.</param>
        /// <param name="vec2">The right hand side vector.</param>
        /// <param name="makeVec">Whether to return a new vector, or null.</param>
        /// <returns>A new vector, of size <paramref name="vec1"/>'s dimensions</returns>
        /// <exception cref="VectorOperationException{T}">Thrown if vectors don't lie in the same number of dimensions.</exception>
        private static Vector<T>? InitializeBinaryOperation(Vector<T> vec1, Vector<T> vec2, bool makeVec = true)
        {
            if (vec1.vec.Length != vec2.vec.Length)
            {
                throw new VectorOperationException<T>(vec1, vec2);
            }

            return makeVec ? new Vector<T>(vec1.vec.Length) : null;
        }
        
        /// <summary>
        /// Accesses the <paramref name="i"/>th component of the vector.
        /// </summary>
        /// <param name="i">The index into the vector components from 0.</param>
        /// <returns>The component matching the given index.</returns>
        public T this[int i]
        {
            get { return vec[i]; }
            set { vec[i] = value; }
        }

        /// <summary>
        /// Adds two vectors together using standard vector addition.
        /// </summary>
        /// <returns>A new vector with the result of the addition.</returns>
        public static Vector<T> operator +(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] + rhs.vec[i];
            }

            return result;
        }

        /// <summary>
        /// Substracts two vectors together using standard vector subtraction.
        /// </summary>
        /// <returns>A new vector with the result of the subtraction.</returns>
        public static Vector<T> operator -(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] - rhs.vec[i];
            }

            return result;
        }

        /// <summary>
        /// Performs inline multiplication between two vectors.
        /// </summary>
        /// <returns>A new vector with the result of the inline multiplication.</returns>
        public static Vector<T> operator *(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] * rhs.vec[i];
            }

            return result;
        }

        /// <summary>
        /// Performs scalar multiplication between a vector and scalar.
        /// </summary>
        /// <param name="lhs">The vector to scale.</param>
        /// <param name="scalar">The factor to scale by.</param>
        /// <returns>A new scaled vector.</returns>
        public static Vector<T> operator *(Vector<T> lhs, T scalar)
        {
            Vector<T> result = new Vector<T>(lhs.vec.Length);
            
            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs[i] * scalar;
            }

            return result;
        }

        /// <summary>
        /// Performs inline division between two vectors.
        /// </summary>
        /// <returns>A new vector with the result of the inline division.</returns>
        public static Vector<T> operator /(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] / rhs.vec[i];
            }

            return result;
        }

        /// <summary>
        /// Performs scalar division between a vector and scalar.
        /// </summary>
        /// <param name="lhs">The vector to scale.</param>
        /// <param name="scalar">The factor to scale by.</param>
        /// <returns>A new scaled vector.</returns>
        public static Vector<T> operator /(Vector<T> lhs, T scalar)
        {
            Vector<T> result = new Vector<T>(lhs.vec.Length);
            
            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs[i] / scalar;
            }

            return result;
        }

        /// <summary>
        /// Computes the vectors length (magnitude).
        /// </summary>
        /// <returns>The magnitude of the vector by calculating its L^2 norm (<see href="https://en.wikipedia.org/wiki/Norm_(mathematics)#Euclidean_norm"/>).</returns>
        public double Length()
        {
            T len = default!;

            foreach (var component in vec)
            {
                len += component * component;
            }
            
            return System.Math.Sqrt(Convert.ToDouble(len));
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<");
            for (int i = 0; i < vec.Length; i++)
            {
                stringBuilder.Append(vec[i].ToString());
                if (i < vec.Length - 1)
                {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append(">");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Computes the dot product between two vectors.
        /// </summary>
        /// <returns>The dot product between two vectors as an integer.(<see href="https://en.wikipedia.org/wiki/Dot_product"/>)</returns>
        public static T Dot(Vector<T> lhs, Vector<T> rhs)
        {
            InitializeBinaryOperation(lhs, rhs);
            T result = default!;

            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result += lhs[i] * rhs[i];
            }

            return result;
        }

        /// <inheritdoc cref="Dot(Vector{T}, Vector{T})"/>
        public T Dot(Vector<T> rhs)
        {
            return Dot(this, rhs);
        }

        /// <summary>
        /// Returns whether two vectors are equivalent based on their component values.
        /// </summary>
        /// <returns>Whether the two vectors are equivalent.</returns>
        public static bool operator ==(Vector<T> lhs, Vector<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Returns whether two vectors are not equivalent based on their component values.
        /// </summary>
        /// <returns>Whether the two vectors are not equivalent.</returns>
        public static bool operator !=(Vector<T> lhs, Vector<T> rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <inheritdoc cref="operator ==(Vector{T}, Vector{T})"/>
        public bool Equals(Vector<T>? other)
        {
            if (other is null || Dimensions != other.Dimensions)
            {
                return false;
            }

            for (int i = 0; i < Dimensions; i++)
            {
                if (this[i] != other[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return ((object)this).GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Vector<T>);
        }
    }
}