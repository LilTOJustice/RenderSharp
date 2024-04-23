using MathSharp;

namespace RenderSharp.Render3d
{
    // Axis-aligned bounding box
    internal struct BoundingBox
    {
        public readonly FVec3 min, max;

        public BoundingBox(in FVec3 min, in FVec3 max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Intersects(in FVec3 test)
        {
            // Slab method
            double txMin = min.X / test.X;
            double txMax = max.X / test.X;

            double tmin = Math.Min(txMin, txMax);
            double tmax = Math.Max(txMin, txMax);

            double tyMin = min.Y / test.Y;
            double tyMax = max.Y / test.Y;

            tmin = Math.Max(tmin, Math.Min(tyMin, tyMax));
            tmax = Math.Min(tmax, Math.Max(tyMin, tyMax));

            double tzMin = min.Z / test.Z;
            double tzMax = max.Z / test.Z;

            tmin = Math.Max(tmin, Math.Min(tzMin, tzMax));
            tmax = Math.Min(tmax, Math.Max(tzMin, tzMax));

            return tmax > 0 && tmax >= tmin;
        }
    }
}
