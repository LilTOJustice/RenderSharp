using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct BoundingBox
    {
        private Cube boundary;

        public BoundingBox(in FVec3 min, in FVec3 max, in RVec3 rotation)
        {
            boundary = new((max - min) / 2 + min, (max - min) / 2, rotation);
        }

        public bool Intersects(FVec3 test)
        {
            return boundary.Intersects(test, 0, out _, out _);
        }
    }
}
