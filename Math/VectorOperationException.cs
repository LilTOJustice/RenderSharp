namespace RenderSharp.Math
{
    public class VectorOperationException<T> : Exception
    {
        public VectorOperationException(Vector<T> vector)
            : base($"Operation not supported for vector {vector.ToString()}")
        {}

        public VectorOperationException(Vector<T> vec1, Vector<T> vec2)
            : base($"Operation not supported between vectors {vec1.ToString()} and {vec2.ToString()}")
        {}
    }
}
