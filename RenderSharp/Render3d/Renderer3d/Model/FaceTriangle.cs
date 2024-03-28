using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct FaceTriangle
    {
        public readonly Triangle triangle;

        public readonly (FVec2, FVec2, FVec2) uv;

        public FaceTriangle(in Triangle triangle, in (FVec2, FVec2, FVec2) uv)
        {
            this.triangle = triangle;
            this.uv = uv;
        }

        public FaceTriangle(in FaceTriangle t, in FVec3 size, in RVec3 rotation, in FVec3 position)
        {
            triangle = new Triangle(t.triangle, size, rotation, position);
            uv = t.uv;
        }

        public bool Sample(in FVec3 worldVec, double minDepth, out FVec2 uv, out double depth)
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
    }
}
