using System.Numerics;

namespace RenderSharp.Math
{
    public class Vector2<T> : Vector<T>
        where T : INumber<T>
    {
        public T X { get { return this[0]; } set { this[0] = value; } }

        public T Y { get { return this[1]; } set { this[1] = value; } }

        public Vector2() : base(2) { }

        public Vector2(T[] vec) : base(vec) { }

        public Vector2(Vector2<T> vec) : base(vec) { }

        public Vector2(T X, T Y) 
            : base(2)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector3<T> Cross(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector3<T>(default!, default!, lhs[0] * rhs[1] - lhs[1] * rhs[0] );
        }

        public static T Cross2d(Vector2<T> lhs, Vector2<T> rhs)
        {
            return lhs[0] * rhs[1] - lhs[1] * rhs[0];
        }

        public Vector3<T> Cross(Vector2<T> rhs)
        {
            return Cross(this, rhs);
        }

        public T Cross2d(Vector2<T> rhs)
        {
            return Cross2d(this, rhs);
        }

        public static Vector2<T> operator +(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs + (Vector<T>)rhs).Components);
        }

        public static Vector2<T> operator -(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs - (Vector<T>)rhs).Components);
        }

        public static Vector2<T> operator *(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs * (Vector<T>)rhs).Components);
        }

        public static Vector2<T> operator *(Vector2<T> lhs, T scalar)
        {
            return new Vector2<T>(((Vector<T>)lhs * scalar).Components);
        }

        public static Vector2<T> operator /(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(((Vector<T>)lhs / (Vector<T>)rhs).Components);
        }

        public static Vector2<T> operator /(Vector2<T> lhs, T scalar)
        {
            return new Vector2<T>(((Vector<T>)lhs / scalar).Components);
        }
    }
}
