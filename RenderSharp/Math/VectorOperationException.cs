using System.Numerics;

namespace RenderSharp.Math
{
    /// <summary>
    /// Exception thrown when a vector operation is unable to occur.
    /// </summary>
    /// <typeparam name="T">The type of the vector(s). Must be a <see cref="INumber{TSelf}"/> type.</typeparam>
    public class VectorOperationException<T> : Exception
        where T : INumber<T>
    {
        /// <summary>
        /// Vector unary operation exception.
        /// </summary>
        /// <param name="vector">The offending vector.</param>
        public VectorOperationException(Vector<T> vector)
            : base($"Operation not supported for vector {vector.ToString()}")
        {}

        /// <summary>
        /// Vector binary operation exception.
        /// </summary>
        /// <param name="vec1">The left hand offending vector.</param>
        /// <param name="vec2">The right hand offending vector.</param>
        public VectorOperationException(Vector<T> vec1, Vector<T> vec2)
            : base($"Operation not supported between vectors {vec1.ToString()} and {vec2.ToString()}")
        {}
    }
}
