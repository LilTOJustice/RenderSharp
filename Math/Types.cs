using System.Numerics;

namespace RenderSharp.Math
{
    public class Vec2 : Vector2<int>
    {
        public Vec2() : base() { }

        public Vec2(int[] vec) : base(vec) { }

        public Vec2(int X, int Y) : base(X, Y) { }

        public Vec3 Cross(Vec2 rhs)
        {
            return new Vec3(Cross((Vector2<int>)this, (Vector2<int>)rhs).Components);
        }

        public static implicit operator FVec2(Vec2 vec)
        {
            return new FVec2(vec.X, vec.Y);
        }

        public static explicit operator UVec2(Vec2 vec)
        {
            return new UVec2((ulong)vec.X, (ulong)vec.Y);
        }

        public static Vec2 operator +(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs + (Vector2<int>)rhs).Components);
        }

        public static Vec2 operator -(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs + (Vector2<int>)rhs).Components);
        }

        public static Vec2 operator *(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs * (Vector2<int>)rhs).Components);
        }

        public static Vec2 operator /(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(((Vector2<int>)lhs / (Vector2<int>)rhs).Components);
        }

        public static Vec2 operator /(Vec2 lhs, int scalar)
        {
            return new Vec2(((Vector2<int>)lhs / scalar).Components);
        }

        public static FVec2 operator /(Vec2 lhs, double scalar)
        {
            return new FVec2((new Vector2<double>(lhs.X, lhs.Y) / scalar).Components);
        }
    }

    public class Vec3 : Vector3<int>
    {
        public Vec3() : base() { }

        public Vec3(int[] vec) : base(vec) { }
        
        public Vec3(int X, int Y, int Z) : base(X, Y, Z) { }

        public Vec3 Cross(Vec3 rhs)
        {
            return new Vec3(Cross((Vector3<int>)this, (Vector3<int>)rhs).Components);
        }

        public static implicit operator FVec3(Vec3 vec)
        {
            return new FVec3(vec.X, vec.Y, vec.Z);
        }

        public static explicit operator UVec3(Vec3 vec)
        {
            return new UVec3((ulong)vec.X, (ulong)vec.Y, (ulong)vec.Z);
        }

        public static Vec3 operator +(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs + (Vector3<int>)rhs).Components);
        }

        public static Vec3 operator -(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs + (Vector3<int>)rhs).Components);
        }

        public static Vec3 operator *(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs * (Vector3<int>)rhs).Components);
        }

        public static Vec3 operator /(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(((Vector3<int>)lhs / (Vector3<int>)rhs).Components);
        }

        public static Vec3 operator /(Vec3 lhs, int scalar)
        {
            return new Vec3(((Vector3<int>)lhs / scalar).Components);
        }

        public static FVec3 operator /(Vec3 lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.X, lhs.Y, lhs.Z) / scalar).Components);
        }
    }
    
    public class Vec4 : Vector4<int>
    {
        public Vec4() : base() { }

        public Vec4(int[] vec) : base(vec) { }
        
        public Vec4(int X, int Y, int Z, int W) : base(X, Y, Z, W) { }

        public static implicit operator FVec4(Vec4 vec)
        {
            return new FVec4(vec.X, vec.Y, vec.Z, vec.W);
        }

        public static explicit operator UVec4(Vec4 vec)
        {
            return new UVec4((ulong)vec.X, (ulong)vec.Y, (ulong)vec.Z, (ulong)vec.W);
        }

        public static Vec4 operator +(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs + (Vector4<int>)rhs).Components);
        }

        public static Vec4 operator -(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs + (Vector4<int>)rhs).Components);
        }

        public static Vec4 operator *(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs * (Vector4<int>)rhs).Components);
        }

        public static Vec4 operator /(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(((Vector4<int>)lhs / (Vector4<int>)rhs).Components);
        }

        public static Vec4 operator /(Vec4 lhs, int scalar)
        {
            return new Vec4(((Vector4<int>)lhs / scalar).Components);
        }

        public static FVec4 operator /(Vec4 lhs, double scalar)
        {
            return new FVec4((new Vector4<double>(lhs.X, lhs.Y, lhs.Z, lhs.W) / scalar).Components);
        }
    }
    
    public class UVec2 : Vector2<ulong>
    {
        public UVec2() : base() { }

        public UVec2(ulong[] vec) : base(vec) { }
        
        public UVec2(ulong X, ulong Y) : base(X, Y) { }

        public UVec3 Cross(UVec2 rhs)
        {
            return new UVec3(Cross((Vector2<ulong>)this, (Vector2<ulong>)rhs).Components);
        }

        public static implicit operator FVec2(UVec2 vec)
        {
            return new FVec2(vec.X, vec.Y);
        }

        public static explicit operator Vec2(UVec2 vec)
        {
            return new Vec2((int)vec.X, (int)vec.Y);
        }

        public static UVec2 operator +(UVec2 lhs, UVec2 rhs)
        {
            return new UVec2(((Vector2<ulong>)lhs + (Vector2<ulong>)rhs).Components);
        }

        public static UVec2 operator -(UVec2 lhs, UVec2 rhs)
        {
            return new UVec2(((Vector2<ulong>)lhs + (Vector2<ulong>)rhs).Components);
        }

        public static UVec2 operator *(UVec2 lhs, UVec2 rhs)
        {
            return new UVec2(((Vector2<ulong>)lhs * (Vector2<ulong>)rhs).Components);
        }

        public static UVec2 operator /(UVec2 lhs, UVec2 rhs)
        {
            return new UVec2(((Vector2<ulong>)lhs / (Vector2<ulong>)rhs).Components);
        }

        public static UVec2 operator /(UVec2 lhs, ulong scalar)
        {
            return new UVec2(((Vector2<ulong>)lhs / scalar).Components);
        }

        public static FVec2 operator /(UVec2 lhs, double scalar)
        {
            return new FVec2((new Vector2<double>(lhs.X, lhs.Y) / scalar).Components);
        }
    }
    
    public class UVec3 : Vector3<ulong>
    {
        public UVec3() : base() { }

        public UVec3(ulong[] vec) : base(vec) { }
        
        public UVec3(ulong X, ulong Y, ulong Z) : base(X, Y, Z) { }

        public UVec3 Cross(UVec3 rhs)
        {
            return new UVec3(Cross((Vector3<ulong>)this, (Vector3<ulong>)rhs).Components);
        }

        public static implicit operator FVec3(UVec3 vec)
        {
            return new FVec3(vec.X, vec.Y, vec.Z);
        }

        public static explicit operator Vec3(UVec3 vec)
        {
            return new Vec3((int)vec.X, (int)vec.Y, (int)vec.Z);
        }

        public static UVec3 operator +(UVec3 lhs, UVec3 rhs)
        {
            return new UVec3(((Vector3<ulong>)lhs + (Vector3<ulong>)rhs).Components);
        }

        public static UVec3 operator -(UVec3 lhs, UVec3 rhs)
        {
            return new UVec3(((Vector3<ulong>)lhs + (Vector3<ulong>)rhs).Components);
        }

        public static UVec3 operator *(UVec3 lhs, UVec3 rhs)
        {
            return new UVec3(((Vector3<ulong>)lhs * (Vector3<ulong>)rhs).Components);
        }

        public static UVec3 operator /(UVec3 lhs, UVec3 rhs)
        {
            return new UVec3(((Vector3<ulong>)lhs / (Vector3<ulong>)rhs).Components);
        }

        public static UVec3 operator /(UVec3 lhs, ulong scalar)
        {
            return new UVec3(((Vector3<ulong>)lhs / scalar).Components);
        }

        public static FVec3 operator /(UVec3 lhs, double scalar)
        {
            return new FVec3((new Vector3<double>(lhs.X, lhs.Y, lhs.Z) / scalar).Components);
        }
    }
    
    public class UVec4 : Vector4<ulong>
    {
        public UVec4() : base() { }

        public UVec4(ulong[] vec) : base(vec) { }
        
        public UVec4(ulong X, ulong Y, ulong Z, ulong W) : base(X, Y, Z, W) { }

        public static implicit operator FVec4(UVec4 vec)
        {
            return new FVec4(vec.X, vec.Y, vec.Z, vec.W);
        }

        public static explicit operator Vec4(UVec4 vec)
        {
            return new Vec4((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
        }

        public static UVec4 operator +(UVec4 lhs, UVec4 rhs)
        {
            return new UVec4(((Vector4<ulong>)lhs + (Vector4<ulong>)rhs).Components);
        }

        public static UVec4 operator -(UVec4 lhs, UVec4 rhs)
        {
            return new UVec4(((Vector4<ulong>)lhs + (Vector4<ulong>)rhs).Components);
        }

        public static UVec4 operator *(UVec4 lhs, UVec4 rhs)
        {
            return new UVec4(((Vector4<ulong>)lhs * (Vector4<ulong>)rhs).Components);
        }

        public static UVec4 operator /(UVec4 lhs, UVec4 rhs)
        {
            return new UVec4(((Vector4<ulong>)lhs / (Vector4<ulong>)rhs).Components);
        }

        public static UVec4 operator /(UVec4 lhs, ulong scalar)
        {
            return new UVec4(((Vector4<ulong>)lhs / scalar).Components);
        }

        public static FVec4 operator /(UVec4 lhs, double scalar)
        {
            return new FVec4((new Vector4<double>(lhs.X, lhs.Y, lhs.Z, lhs.W) / scalar).Components);
        }
    }
    
    public class FVec2 : Vector2<double>
    {
        public FVec2() : base() { }

        public FVec2(double[] vec) : base(vec) { }
        
        public FVec2(double X, double Y) : base(X, Y) { }

        public FVec3 Cross(FVec2 rhs)
        {
            return new FVec3(Cross((Vector2<double>)this, (Vector2<double>)rhs).Components);
        }

        public static explicit operator Vec2(FVec2 vec)
        {
            return new Vec2((int)vec.X, (int)vec.Y);
        }

        public static explicit operator UVec2(FVec2 vec)
        {
            return new UVec2((ulong)vec.X, (ulong)vec.Y);
        }

        public static FVec2 operator +(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs + (Vector2<double>)rhs).Components);
        }

        public static FVec2 operator -(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs + (Vector2<double>)rhs).Components);
        }

        public static FVec2 operator *(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs * (Vector2<double>)rhs).Components);
        }

        public static FVec2 operator /(FVec2 lhs, FVec2 rhs)
        {
            return new FVec2(((Vector2<double>)lhs / (Vector2<double>)rhs).Components);
        }

        public static FVec2 operator /(FVec2 lhs, double scalar)
        {
            return new FVec2(((Vector2<double>)lhs / scalar).Components);
        }
    }
    
    public class FVec3 : Vector3<double>
    {
        public FVec3() : base() { }
        
        public FVec3(double[] vec) : base(vec) { }

        public FVec3(double X, double Y, double Z) : base(X, Y, Z) { }

        public FVec3 Cross(FVec3 rhs)
        {
            return new FVec3(Cross((Vector3<double>)this, (Vector3<double>)rhs).Components);
        }

        public static explicit operator Vec3(FVec3 vec)
        {
            return new Vec3((int)vec.X, (int)vec.Y, (int)vec.Z);
        }

        public static explicit operator UVec3(FVec3 vec)
        {
            return new UVec3((ulong)vec.X, (ulong)vec.Y, (ulong)vec.Z);
        }

        public static FVec3 operator +(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs + (Vector3<double>)rhs).Components);
        }

        public static FVec3 operator -(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs + (Vector3<double>)rhs).Components);
        }

        public static FVec3 operator *(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs * (Vector3<double>)rhs).Components);
        }

        public static FVec3 operator /(FVec3 lhs, FVec3 rhs)
        {
            return new FVec3(((Vector3<double>)lhs / (Vector3<double>)rhs).Components);
        }

        public static FVec3 operator /(FVec3 lhs, double scalar)
        {
            return new FVec3(((Vector3<double>)lhs / scalar).Components);
        }
    }
    
    public class FVec4 : Vector4<double>
    {
        public FVec4() : base() { }

        public FVec4(double[] vec) : base(vec) { }
        
        public FVec4(double X, double Y, double Z, double W) : base(X, Y, Z, W) { }

        public static explicit operator Vec4(FVec4 vec)
        {
            return new Vec4((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
        }

        public static explicit operator UVec4(FVec4 vec)
        {
            return new UVec4((ulong)vec.X, (ulong)vec.Y, (ulong)vec.Z, (ulong)vec.W);
        }

        public static FVec4 operator +(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs + (Vector4<double>)rhs).Components);
        }

        public static FVec4 operator -(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs + (Vector4<double>)rhs).Components);
        }

        public static FVec4 operator *(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs * (Vector4<double>)rhs).Components);
        }

        public static FVec4 operator /(FVec4 lhs, FVec4 rhs)
        {
            return new FVec4(((Vector4<double>)lhs / (Vector4<double>)rhs).Components);
        }

        public static FVec4 operator /(FVec4 lhs, double scalar)
        {
            return new FVec4(((Vector4<double>)lhs / scalar).Components);
        }
    }

    public class Constants
    {
        public const double DEGPERPI = 180 / System.Math.PI;
    }
}
