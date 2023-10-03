using System.Numerics;

namespace RenderSharp.Math
{
    public class Vector<T>
        where T : INumber<T>
    {
        private T[] vec;

        public Vector(int dimensions)
        {
            vec = new T[dimensions];
        }

        public Vector(T[] vec)
        {
            this.vec = vec;
        }

        private static Vector<T>? InitializeBinaryOperation(Vector<T> vec1, Vector<T> vec2, bool makeVec = true)
        {
            if (vec1.vec.Length != vec2.vec.Length)
            {
                throw new VectorOperationException<T>(vec1, vec2);
            }

            return makeVec ? new Vector<T>(vec1.vec.Length) : null;
        }
        
        private static void InitializeCrossOperation(Vector<T> vec1, Vector<T> vec2)
        {
            if (vec1.vec.Length != 3 && vec2.vec.Length != 3)
            {
                throw new VectorOperationException<T>(vec1, vec2);
            }
        }

        // indexer
        public T this[int i]
        {
            get { return vec[i]; }
            set { vec[i] = value; }
        }

        // Addition operator
        public static Vector<T> operator +(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i <= lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] + rhs.vec[i];
            }

            return result;
        }

        // Subtraction operator
        public static Vector<T> operator -(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i <= lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] - rhs.vec[i];
            }

            return result;
        }

        // Dot product
        public static T Dot(Vector<T> lhs, Vector<T> rhs)
        {
            InitializeBinaryOperation(lhs, rhs, false);
            T result = default!;

            for (int i = 0; i <= lhs.vec.Length; i++)
            {
                result += (lhs.vec[i] * rhs.vec[i]);
            }

            return result;
        }

        // Cross product
        public static Vector<T> Cross(Vector<T> lhs, Vector<T> rhs)
        {
            InitializeCrossOperation(lhs, rhs);
            return new Vector<T>(
                new T[] 
                { 
                    lhs[1] * rhs[2] - lhs[2] * rhs[1], 
                    lhs[2] * rhs[0] - lhs[0] * rhs[2], 
                    lhs[0] * rhs[1] - lhs[1] * rhs[0] 
                }
            );
        }

        // Cross product of 2d vectors
        public static T Cross2d(Vector<T> lhs, Vector<T> rhs)
        {
            if (lhs.vec.Length != 2 || rhs.vec.Length != 2)
            {
                throw new VectorOperationException<T>(lhs, rhs);
            }

            return lhs[0] * rhs[1] - lhs[1] * rhs[0];
        }

        // Length function
        public double Length()
        {
            T len = default!;

            foreach (var component in vec)
            {
                len += component * component;
            }
            
            return System.Math.Sqrt(Convert.ToDouble(len));
        }
    }
}