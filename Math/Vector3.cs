﻿using System.Numerics;

namespace RenderSharp.Math
{
    public class Vector3<T> : Vector<T>
        where T : INumber<T>
    {
        public T X { get { return this[0]; } set { this[0] = value; } }

        public T Y { get { return this[1]; } set { this[1] = value; } }

        public T Z { get { return this[2]; } set { this[2] = value; } }

        public Vector3()
            : base(3)
        {}

        public Vector3(T X, T Y, T Z)
            : base(3)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public static Vector3<T> Cross(Vector3<T> lhs, Vector3<T> rhs)
        {
            return new Vector3<T>
            (
                lhs[1] * rhs[2] - lhs[2] * rhs[1],
                lhs[2] * rhs[0] - lhs[0] * rhs[2],
                lhs[0] * rhs[1] - lhs[1] * rhs[0]
            );
        }

        public Vector3<T> Cross(Vector3<T> rhs)
        {
            return Cross(this, rhs);
        }
    }
}
