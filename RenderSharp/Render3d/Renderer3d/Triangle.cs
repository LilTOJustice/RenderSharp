using MathSharp;

namespace RenderSharp.Render3d.Renderer3d
{
    internal struct Triangle
    {
        FVec3 v0, v1, v2;

        private FVec3 Normal
        {
            get
            {
                FVec3 right = v2 - v0;
                FVec3 left = v1 - v0;
                return right.Cross(left);
            }
        }

        // v0 , v1, v2 are the vertices of the triangle, ordered counter-clockwise
        public Triangle(FVec3 v0, FVec3 v1, FVec3 v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }

        private bool Within(FVec3 test)
        {
            FVec3 normal = Normal;
            if (normal.Dot(test) == 0)
            {
                return false;
            }

            FVec3 unitNorm = Normal.Norm();
            double d = -unitNorm.Dot(v0);
            double t = - d / test.Dot(unitNorm);
            if (t < 0)
            {
                return false;
            }
            
            FVec3 intersection = test * t;

            double one = (v1 - v0).Cross(intersection - v0).Dot(unitNorm);
            double two = (v2 - v1).Cross(intersection - v1).Dot(unitNorm);
            double three = (v0 - v2).Cross(intersection - v2).Dot(unitNorm);

            return (v1 - v0).Cross(intersection - v0).Dot(unitNorm) <= 0 &&
                   (v2 - v1).Cross(intersection - v1).Dot(unitNorm) <= 0 &&
                   (v0 - v2).Cross(intersection - v2).Dot(unitNorm) <= 0;
        }

        public RGBA Sample(FVec3 planeNormal)
        {
            return Within(planeNormal) ? new RGBA(255, 255, 255, 255) : new RGBA(0, 0, 0, 255);
        }
    }
}
