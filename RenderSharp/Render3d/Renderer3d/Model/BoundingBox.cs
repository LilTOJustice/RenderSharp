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

        public bool Intersects(in Ray ray)
        {
            // Slab method
            double txMin = (min.X - ray.origin.X) * ray.inv.X;
            double txMax = (max.X - ray.origin.X) * ray.inv.X;

            double tmin = Math.Min(txMin, txMax);
            double tmax = Math.Max(txMin, txMax);

            double tyMin = (min.Y - ray.origin.Y) * ray.inv.Y;
            double tyMax = (max.Y - ray.origin.Y) * ray.inv.Y;

            tmin = Math.Max(tmin, Math.Min(tyMin, tyMax));
            tmax = Math.Min(tmax, Math.Max(tyMin, tyMax));

            double tzMin = (min.Z - ray.origin.Z) * ray.inv.Z;
            double tzMax = (max.Z - ray.origin.Z) * ray.inv.Z;

            tmin = Math.Max(tmin, Math.Min(tzMin, tzMax));
            tmax = Math.Min(tmax, Math.Max(tzMin, tzMax));

            return tmax > 0 && tmax >= tmin;
        }
    }
}
