using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct BoundingBox
    {
        private FVec3 min, max;
        private FVec3 diff;

        public BoundingBox(FVec3 min, FVec3 max)
        {
            this.min = min;
            this.max = max;
            diff = max - min;
        }

        // TODO: Implement
        public bool Intersects(FVec3 test)
        {
            FVec3 diffOverTest = diff / test;

            FVec3 scaled = test * diffOverTest.X;
            if (max.Y - scaled.Y >= min.Y || max.Z - scaled.Z >= min.Z)
            {
                return true;
            }

            scaled = test * diffOverTest.Y;
            if (max.X - scaled.X >= min.X || max.Z - scaled.Z >= min.Z)
            {
                return true;
            }

            scaled = test * diffOverTest.Z;
            if (max.X - scaled.X >= min.X || max.Y - scaled.Y >= min.Y)
            {
                return true;
            }

            return false;
        }
    }
}
