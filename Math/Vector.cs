﻿using System.Numerics;
using System.Text;

namespace RenderSharp.Math
{
    public class Vector<T>
        where T : INumber<T>
    {
        private T[] vec;

        public int Dimensions { get { return vec.Length; } }

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
        
        public T this[int i]
        {
            get { return vec[i]; }
            set { vec[i] = value; }
        }

        // Addition operator
        public static Vector<T> operator +(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] + rhs.vec[i];
            }

            return result;
        }

        public static Vector<T> operator -(Vector<T> lhs, Vector<T> rhs)
        {
            Vector<T> result = InitializeBinaryOperation(lhs, rhs)!;

            for (int i = 0; i < lhs.vec.Length; i++)
            {
                result[i] = lhs.vec[i] - rhs.vec[i];
            }

            return result;
        }


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

        public T Dot(Vector<T> rhs)
        {
            return Dot(this, rhs);
        }
    }
}