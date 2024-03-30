using MathSharp;

namespace RenderSharp.Render3d
{
    // Axis-aligned bounding box
    internal struct BoundingBox
    {
        private Cube boundary;

        public BoundingBox(in FVec3 min, in FVec3 max)
        {
            boundary = new((max - min) / 2 + min, (max - min) / 2, new RVec3());
        }

        public bool Intersects(in FVec3 test)
        {
            return boundary.Intersects(test, 0, out _, out _);
        }
    }
}
