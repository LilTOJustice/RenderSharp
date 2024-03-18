using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Sphere
    {
        private FVec3 pos;
        private FVec3 radii2;
        private double c;

        public Sphere(in FVec3 pos, in FVec3 radii)
        {
            this.pos = pos;
            radii2 = radii * radii;
            c = pos.X * pos.X / radii2.X +
                     pos.Y * pos.Y / radii2.Y +
                     pos.Z * pos.Z / radii2.Z - 1;

        }

        private bool Intersects(in FVec3 test)
        {
            double a = test.X * test.X / radii2.X +
                     test.Y * test.Y / radii2.Y +
                     test.Z * test.Z / radii2.Z;

            if (a == 0)
            {
                return false;
            }

            double b = -2 * (test.X * pos.X / radii2.X +
                             test.Y * pos.Y / radii2.Y +
                             test.Z * pos.Z / radii2.Z);


            double sqrt = b * b - 4 * a * c;
            if (sqrt < 0)
            {
                return false;
            }

            double sqrtResult = Math.Sqrt(sqrt);
            double plust = (-b + sqrtResult) / (2 * a);
            double minust = (-b - sqrtResult) / (2 * a);
            if (plust < 0 && minust < 0)
            {
                return false;
            }

            return true;
        }

        public RGBA Sample(in FVec3 worldVec)
        {
            return Intersects(worldVec) ? new RGBA(255, 255, 255, 255) : new RGBA();
        }
    }
}
