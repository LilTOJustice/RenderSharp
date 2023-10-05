using System.Numerics;

namespace RenderSharp.Math
{
    public class Vector4<T> : Vector<T>
        where T : INumber<T>
    {
        public T X { get { return this[0]; } set { this[0] = value; } }

        public T Y { get { return this[1]; } set { this[1] = value; } }

        public T Z { get { return this[2]; } set { this[2] = value; } }

        public T W { get { return this[3]; } set { this[3] = value; } }

        public Vector4()
            : base(4)
        {}

        public Vector4(T X, T Y, T Z, T W)
            : base(4)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }
    }
}
