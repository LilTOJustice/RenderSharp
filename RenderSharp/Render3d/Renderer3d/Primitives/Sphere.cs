using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Sphere
    {
        public FVec3 position;
        private FVec3 radii2;
        private RotorTransform rotorTransform;

        public Sphere(in FVec3 position, in FVec3 radii, in RVec3 rotation)
        {
            this.position = position;
            radii2 = radii * radii;
            rotorTransform = new RotorTransform(rotation);
            ref RotorTransform rt = ref rotorTransform;
            ref FVec3 p = ref this.position;
        }

        public bool Intersects(in Ray ray, double minDepth, out (double, double) depthCloseFar)
        {
            FVec3 p = position - ray.origin;
            FVec3 s = ray.direction;
            ref RotorTransform rt = ref rotorTransform;
            double a =
                (rt.A2 * s.X * s.X + rt.B2 * s.Y * s.Y + rt.C2 * s.Z * s.Z + 2 * rt.AB * s.X * s.Y + 2 * rt.AC * s.X * s.Z + 2 * rt.BC * s.Y * s.Z) / radii2.X +
                (rt.D2 * s.X * s.X + rt.E2 * s.Y * s.Y + rt.F2 * s.Z * s.Z + 2 * rt.DE * s.X * s.Y + 2 * rt.DF * s.X * s.Z + 2 * rt.EF * s.Y * s.Z) / radii2.Y +
                (rt.G2 * s.X * s.X + rt.H2 * s.Y * s.Y + rt.I2 * s.Z * s.Z + 2 * rt.GH * s.X * s.Y + 2 * rt.GI * s.X * s.Z + 2 * rt.HI * s.Y * s.Z) / radii2.Z;
            double b = -2 * (
                (rt.A2 * s.X * p.X + rt.B2 * s.Y * p.Y + rt.C2 * s.Z * p.Z + rt.AB * (s.X * p.Y + s.Y * p.X) + rt.AC * (s.X * p.Z + s.Z * p.X) + rt.BC * (s.Y * p.Z + s.Z * p.Y)) / radii2.X +
                (rt.D2 * s.X * p.X + rt.E2 * s.Y * p.Y + rt.F2 * s.Z * p.Z + rt.DE * (s.X * p.Y + s.Y * p.X) + rt.DF * (s.X * p.Z + s.Z * p.X) + rt.EF * (s.Y * p.Z + s.Z * p.Y)) / radii2.Y +
                (rt.G2 * s.X * p.X + rt.H2 * s.Y * p.Y + rt.I2 * s.Z * p.Z + rt.GH * (s.X * p.Y + s.Y * p.X) + rt.GI * (s.X * p.Z + s.Z * p.X) + rt.HI * (s.Y * p.Z + s.Z * p.Y)) / radii2.Z);
            double c =
                (rt.A2 * p.X * p.X + rt.B2 * p.Y * p.Y + rt.C2 * p.Z * p.Z + 2 * rt.AB * p.X * p.Y + 2 * rt.AC * p.X * p.Z + 2 * rt.BC * p.Y * p.Z) / radii2.X +
                (rt.D2 * p.X * p.X + rt.E2 * p.Y * p.Y + rt.F2 * p.Z * p.Z + 2 * rt.DE * p.X * p.Y + 2 * rt.DF * p.X * p.Z + 2 * rt.EF * p.Y * p.Z) / radii2.Y +
                (rt.G2 * p.X * p.X + rt.H2 * p.Y * p.Y + rt.I2 * p.Z * p.Z + 2 * rt.GH * p.X * p.Y + 2 * rt.GI * p.X * p.Z + 2 * rt.HI * p.Y * p.Z) / radii2.Z - 1;

            Transforms.GetValidIntersection(a, b, c, minDepth, out depthCloseFar);
            return depthCloseFar.Item2 != double.PositiveInfinity;
        }
    }
}
