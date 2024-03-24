using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Triangle
    {
        private FVec3 v0, v1, v2;
        private FVec3 v01, v12, v20;
        private FVec3 normal;
        private FVec3 unitNorm;
        private double d;

        public Triangle(in FVec3 v0, in FVec3 v1, in FVec3 v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            v01 = v1 - v0;
            v12 = v2 - v1;
            v20 = v0 - v2;
            normal = (v2 - v0).Cross(v01);
            unitNorm = normal.Norm();
            d = -unitNorm.Dot(v0);
        }

        public bool Intersects(in FVec3 test, in FVec3 cameraPos, double minDepth, out double depth)
        {
            depth = -1;
            double dot = test.Dot(unitNorm);

            // Check if we are facing the triangle side-on
            if (dot == 0)
            {
                return false;
            }

            depth = - d / dot;

            // Check if the intersection is behind the near plane.
            if (depth < 0)
            {
                depth = -1;
                return false;
            }
            
            FVec3 intersection = test * depth;

            return v01.Cross(intersection - v0).Dot(unitNorm) <= 0 &&
                   v12.Cross(intersection - v1).Dot(unitNorm) <= 0 &&
                   v20.Cross(intersection - v2).Dot(unitNorm) <= 0;
        }
    }
}
