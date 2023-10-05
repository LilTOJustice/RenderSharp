using System.Numerics;

namespace RenderSharp.Math
{
    public class Vector2<T> : Vector<T>
        where T : INumber<T>
    {
        public T X { get { return this[0]; } set { this[0] = value; } }

        public T Y { get { return this[1]; } set { this[1] = value; } }

        public Vector2()
            : base(2)
        {}

        public Vector2(T X, T Y)
            : base(2)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
