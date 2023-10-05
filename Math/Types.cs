namespace RenderSharp.Math
{
    public class Vec2 : Vector2<int>
    {
        public Vec2(int X, int Y) : base(X, Y) { }
    }

    public class Vec3 : Vector3<int>
    {
        public Vec3(int X, int Y, int Z) : base(X, Y, Z) { }
    }
    
    public class Vec4 : Vector4<int>
    {
        public Vec4(int X, int Y, int Z, int W) : base(X, Y, Z, W) { }
    }
    
    public class UVec2 : Vector2<ulong>
    {
        public UVec2(ulong X, ulong Y) : base(X, Y) { }
    }
    
    public class UVec3 : Vector3<ulong>
    {
        public UVec3(ulong X, ulong Y, ulong Z) : base(X, Y, Z) { }
    }
    
    public class UVec4 : Vector4<ulong>
    {
        public UVec4(ulong X, ulong Y, ulong Z, ulong W) : base(X, Y, Z, W) { }
    }
    
    public class FVec2 : Vector2<double>
    {
        public FVec2(double X, double Y) : base(X, Y) { }
    }
    
    public class FVec3 : Vector3<double>
    {
        public FVec3(double X, double Y, double Z) : base(X, Y, Z) { }
    }
    
    public class FVec4 : Vector4<double>
    {
        public FVec4(double X, double Y, double Z, double W) : base(X, Y, Z, W) { }
    }
}
