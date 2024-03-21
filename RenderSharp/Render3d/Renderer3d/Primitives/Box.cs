using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Box
    {
        private FVec3 position;
        private FVec3 size;
        private FVec3 size2;
        private RVec3 rotation;
        private RotorTransform rotorTransform;

        public Box(in FVec3 position, in FVec3 size, in RVec3 rotation)
        {
            this.position = position;
            this.size = size;
            size2 = size * size;
            this.rotation = rotation;
            rotorTransform = new RotorTransform(rotation);
        }

        private bool EpsilonCheck(double a, double b)
        {
            return Math.Abs(a - b) <= 0.00000000000001;
        }

        private bool TestX(in FVec3 s, out double t)
        {
            ref FVec3 p = ref position;
            ref RotorTransform rt = ref rotorTransform;
            double a = rt.D2 * s.X * s.X + rt.E2 * s.Y * s.Y + rt.F2 * s.Z * s.Z +
                2 * rt.DE * s.X * s.Y + 2 * rt.DF * s.X * s.Z + 2 * rt.EF * s.Y * s.Z;
            double b = -2 * (rt.D2 * s.X * p.X + rt.E2 * s.Y * p.Y + rt.F2 * s.Z * p.Z +
                rt.DE * (s.X * p.Y + s.Y * p.X) + rt.DF * (s.X * p.Z + s.Z * p.X) + rt.EF * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.D2 * p.X * p.X + rt.E2 * p.Y * p.Y + rt.F2 * p.Z * p.Z +
                2 * rt.DE * p.X * p.Y + 2 * rt.DF * p.X * p.Z + 2 * rt.EF * p.Y * p.Z - size2.X;
            return Transforms.SolveQuadratic(a, b, c, out t);
        }

        private bool TestY(in FVec3 s, out double t)
        {
            ref FVec3 p = ref position;
            ref RotorTransform rt = ref rotorTransform;
            double a = rt.G2 * s.X * s.X + rt.H2 * s.Y * s.Y + rt.I2 * s.Z * s.Z +
                2 * rt.GH * s.X * s.Y + 2 * rt.GI * s.X * s.Z + 2 * rt.HI * s.Y * s.Z;
            double b = -2 * (rt.G2 * s.X * p.X + rt.H2 * s.Y * p.Y + rt.I2 * s.Z * p.Z +
                rt.GH * (s.X * p.Y + s.Y * p.X) + rt.GI * (s.X * p.Z + s.Z * p.X) + rt.HI * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.G2 * p.X * p.X + rt.H2 * p.Y * p.Y + rt.I2 * p.Z * p.Z +
                2 * rt.GH * p.X * p.Y + 2 * rt.GI * p.X * p.Z + 2 * rt.HI * p.Y * p.Z - size2.Y;
            return Transforms.SolveQuadratic(a, b, c, out t);
        }

        private bool TestZ(in FVec3 s, out double t)
        {
            ref FVec3 p = ref position;
            ref RotorTransform rt = ref rotorTransform;
            double a = rt.J2 * s.X * s.X + rt.K2 * s.Y * s.Y + rt.L2 * s.Z * s.Z +
                2 * rt.JK * s.X * s.Y + 2 * rt.JL * s.X * s.Z + 2 * rt.KL * s.Y * s.Z;
            double b = -2 * (rt.J2 * s.X * p.X + rt.K2 * s.Y * p.Y + rt.L2 * s.Z * p.Z +
                rt.JK * (s.X * p.Y + s.Y * p.X) + rt.JL * (s.X * p.Z + s.Z * p.X) + rt.KL * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.J2 * p.X * p.X + rt.K2 * p.Y * p.Y + rt.L2 * p.Z * p.Z +
                2 * rt.JK * p.X * p.Y + 2 * rt.JL * p.X * p.Z + 2 * rt.KL * p.Y * p.Z - size2.Z;
            return Transforms.SolveQuadratic(a, b, c, out t);
        }

        private bool Intersects(in FVec3 test, out double depth)
        {
            if (TestX(test, out depth))
            {
                FVec3 rotated = (test * depth - position).Rotate(rotation);
                double resultX = Math.Abs(rotated.X) / size.X;
                if (EpsilonCheck(resultX, 1) && resultX > Math.Abs(rotated.Y) / size.Y
                    && resultX > Math.Abs(rotated.Z) / size.Z)
                {
                    return true;
                }
            }

            if (TestY(test, out depth))
            {
                FVec3 rotated = (test * depth - position).Rotate(rotation);
                double resultY = Math.Abs(rotated.Y) / size.Y;
                if (EpsilonCheck(resultY, 1) && resultY > Math.Abs(rotated.X) / size.X
                    && resultY > Math.Abs(rotated.Z) / size.Z)
                {
                    return true;
                }
            }
            
            if (TestZ(test, out depth))
            {
                FVec3 rotated = (test * depth - position).Rotate(rotation);
                double resultZ = Math.Abs(rotated.Z) / size.Z;
                if (EpsilonCheck(resultZ, 1) && resultZ > Math.Abs(rotated.X) / size.X
                    && resultZ > Math.Abs(rotated.Y) / size.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public RGBA Sample(in FVec3 worldVec, out double depth)
        {
            return Intersects(worldVec, out depth) ? new RGBA(255, 255, 255, 255) : new RGBA();
        }
    }
}
