using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Triangle
    {
        public readonly FVec3 v0, v1, v2, centroid, unitNorm;
        private FVec3 v01, v12, v20;

        public Triangle(in FVec3 v0, in FVec3 v1, in FVec3 v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            centroid = (v0 + v1 + v2) / 3;
            v01 = v1 - v0;
            v12 = v2 - v1;
            v20 = v0 - v2;
            unitNorm = (v2 - v0).Cross(v01).Norm();
        }

        public Triangle(in Triangle t, in FVec3 size, in RVec3 rotation, in FVec3 position)
            : this(
                t.v0.Rotate(rotation) * size + position,
                t.v1.Rotate(rotation) * size + position,
                t.v2.Rotate(rotation) * size + position)
        {}

        public bool Intersects(in Ray ray, out double depth, out FVec3 barycentric)
        {
            double dot = ray.direction.Dot(unitNorm);

            if (dot == 0)
            {
                depth = double.PositiveInfinity;
                barycentric = new FVec3();
                return false;
            }
            
            depth = unitNorm.Dot(v0 - ray.origin) / dot;

            if (depth < 0)
            {
                barycentric = new FVec3();
                return false;
            }
            
            FVec3 intersection = ray.origin + ray.direction * depth;
            FVec3 v = v01.Cross(intersection - v0);
            FVec3 w = v12.Cross(intersection - v1);
            FVec3 u = v20.Cross(intersection - v2);

            if (v.Dot(unitNorm) <= 0 &&
                   w.Dot(unitNorm) <= 0 &&
                   u.Dot(unitNorm) <= 0)
            {
                double areaV = v.Mag();
                double areaW = w.Mag();
                double areaU = u.Mag();
                double total = areaV + areaW + areaU;
                barycentric = new FVec3(
                    areaV / total,
                    areaW / total,
                    areaU / total
                );
                return true;
            }
            else
            {
                barycentric = new FVec3();
                return false;
            }
        }
    }
}
