using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    internal struct FaceTriangle : IEquatable<FaceTriangle>
    {
        public readonly Triangle triangle;
        
        public readonly Material material;

        public readonly (FVec2, FVec2, FVec2) uv;

        private int id;

        public FaceTriangle(in Triangle triangle, Material material, in (FVec2, FVec2, FVec2) uv, int id)
        {
            this.triangle = triangle;
            this.material = material;
            this.uv = uv;
            this.id = id;
        }

        public FaceTriangle(in FaceTriangle t, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            triangle = new Triangle(t.triangle, size, rotation, position);
            material = t.material;
            uv = t.uv;
            id = t.id;
        }

        public bool Equals(FaceTriangle other) => id == other.id;

        public override bool Equals(object? obj) => obj is FaceTriangle other && Equals(other);

        public override int GetHashCode() => id;

        public bool Intersects(in FVec3 worldVec, double minDepth, out FVec2 uv, out double depth)
        {
            FVec3 barycentric;
            if (triangle.Intersects(worldVec, minDepth, out depth, out barycentric))
            {
                uv = this.uv.Item1 * barycentric.X + this.uv.Item2 * barycentric.Y + this.uv.Item3 * barycentric.Z;
                return true;
            }
            else
            {
                depth = -1;
                uv = new FVec2();
                return false;
            }
        }

        public override string ToString() => id.ToString();
    }
}
