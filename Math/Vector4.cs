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

        public Vector4() : base(4) { }

        public Vector4(T[] vec) : base(vec) { }

        public Vector4(Vector2<T> vec, T Z, T W)
            : base(vec, 4)
        {
            this.Z = Z;
            this.W = W;
        }

        public Vector4(Vector3<T> vec, T W)
            : base(vec, 4)
        {
            this.W = W;
        }
        
        public Vector4(Vector4<T> vec) : base(vec) { }

        public Vector4(T X, T Y, T Z, T W)
            : base(4)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        public static Vector4<T> operator +(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs + rhs).Components);
        }

        public static Vector4<T> operator -(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs - rhs).Components);
        }

        public static Vector4<T> operator *(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs * rhs).Components);
        }

        public static Vector4<T> operator *(Vector4<T> lhs, T scalar)
        {
            return new Vector4<T>(((Vector<T>)lhs * scalar).Components);
        }

        public static Vector4<T> operator /(Vector4<T> lhs, Vector4<T> rhs)
        {
            return new Vector4<T>(((Vector<T>)lhs / rhs).Components);
        }

        public static Vector4<T> operator /(Vector4<T> lhs, T scalar)
        {
            return new Vector4<T>(((Vector<T>)lhs / scalar).Components);
        }
    }
}
