using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Sphere
    {
        private FVec3 position;
        private FVec3 radii2;
        private RotorTransform rotorTransform;

        public Sphere(in FVec3 position, in FVec3 radii, in RVec3 rotation)
        {
            this.position = position;
            radii2 = radii * radii;
            rotorTransform = new RotorTransform(rotation);
            ref RotorTransform rt = ref rotorTransform;
        }

        private bool Intersects(in FVec3 s, in FVec3 cameraPos, double minDepth, out double depth)
        {
            FVec3 p = position - cameraPos;
            ref RotorTransform rt = ref rotorTransform;
            double a =
                (rt.D2 * s.X * s.X + rt.E2 * s.Y * s.Y + rt.F2 * s.Z * s.Z + 2 * rt.DE * s.X * s.Y + 2 * rt.DF * s.X * s.Z + 2 * rt.EF * s.Y * s.Z) / radii2.X +
                (rt.G2 * s.X * s.X + rt.H2 * s.Y * s.Y + rt.I2 * s.Z * s.Z + 2 * rt.GH * s.X * s.Y + 2 * rt.GI * s.X * s.Z + 2 * rt.HI * s.Y * s.Z) / radii2.Y +
                (rt.J2 * s.X * s.X + rt.K2 * s.Y * s.Y + rt.L2 * s.Z * s.Z + 2 * rt.JK * s.X * s.Y + 2 * rt.JL * s.X * s.Z + 2 * rt.KL * s.Y * s.Z) / radii2.Z;
            double b = -2 * (
                (rt.D2 * s.X * p.X + rt.E2 * s.Y * p.Y + rt.F2 * s.Z * p.Z + rt.DE * (s.X * p.Y + s.Y * p.X) + rt.DF * (s.X * p.Z + s.Z * p.X) + rt.EF * (s.Y * p.Z + s.Z * p.Y)) / radii2.X +
                (rt.G2 * s.X * p.X + rt.H2 * s.Y * p.Y + rt.I2 * s.Z * p.Z + rt.GH * (s.X * p.Y + s.Y * p.X) + rt.GI * (s.X * p.Z + s.Z * p.X) + rt.HI * (s.Y * p.Z + s.Z * p.Y)) / radii2.Y +
                (rt.J2 * s.X * p.X + rt.K2 * s.Y * p.Y + rt.L2 * s.Z * p.Z + rt.JK * (s.X * p.Y + s.Y * p.X) + rt.JL * (s.X * p.Z + s.Z * p.X) + rt.KL * (s.Y * p.Z + s.Z * p.Y)) / radii2.Z);
            double c =
                (rt.D2 * p.X * p.X + rt.E2 * p.Y * p.Y + rt.F2 * p.Z * p.Z + 2 * rt.DE * p.X * p.Y + 2 * rt.DF * p.X * p.Z + 2 * rt.EF * p.Y * p.Z) / radii2.X +
                (rt.G2 * p.X * p.X + rt.H2 * p.Y * p.Y + rt.I2 * p.Z * p.Z + 2 * rt.GH * p.X * p.Y + 2 * rt.GI * p.X * p.Z + 2 * rt.HI * p.Y * p.Z) / radii2.Y +
                (rt.J2 * p.X * p.X + rt.K2 * p.Y * p.Y + rt.L2 * p.Z * p.Z + 2 * rt.JK * p.X * p.Y + 2 * rt.JL * p.X * p.Z + 2 * rt.KL * p.Y * p.Z) / radii2.Z - 1;

            return Transforms.GetValidIntersection(a, b, c, minDepth, out depth);
        }

        public RGBA Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, out double depth)
        {
            return Intersects(worldVec, cameraPos, minDepth, out depth) ? new RGBA(255, 0, 0, 255) : new RGBA();
        }
    }
}
