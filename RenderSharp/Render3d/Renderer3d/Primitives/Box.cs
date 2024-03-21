using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Box
    {
        private FVec3 position;
        private FVec3 size;
        private FVec3 size2;

        public Box(in FVec3 position, in FVec3 size)
        {
            this.position = position;
            this.size = size;
            size2 = size * size;
        }

        private bool TestX(in RotorTransform rt, in FVec3 s, out double t)
        {
            ref FVec3 p = ref position;
            double a = rt.D2 * s.X * s.X + rt.E2 * s.Y * s.Y + rt.F2 * s.Z * s.Z +
                2 * rt.DE * s.X * s.Y + 2 * rt.DF * s.X * s.Z + 2 * rt.EF * s.Y * s.Z;
            double b = -2 * (rt.D2 * s.X * p.X + rt.E2 * s.Y * p.Y + rt.F2 * s.Z * p.Z +
                rt.DE * (s.X * p.Y + s.Y * p.X) + rt.DF * (s.X * p.Z + s.Z * p.X) + rt.EF * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.D2 * p.X * p.X + rt.E2 * p.Y * p.Y + rt.F2 * p.Z * p.Z +
                2 * rt.DE * p.X * p.Y + 2 * rt.DF * p.X * p.Z + 2 * rt.EF * p.Y * p.Z - size2.X;
            return Transforms.SolveQuadratic(a, b, c, out t);
        }

        private bool TestY(in RotorTransform rt, in FVec3 s, out double t)
        {
            ref FVec3 p = ref position;
            double a = rt.G2 * s.X * s.X + rt.H2 * s.Y * s.Y + rt.I2 * s.Z * s.Z +
                2 * rt.GH * s.X * s.Y + 2 * rt.GI * s.X * s.Z + 2 * rt.HI * s.Y * s.Z;
            double b = -2 * (rt.G2 * s.X * p.X + rt.H2 * s.Y * p.Y + rt.I2 * s.Z * p.Z +
                rt.GH * (s.X * p.Y + s.Y * p.X) + rt.GI * (s.X * p.Z + s.Z * p.X) + rt.HI * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.G2 * p.X * p.X + rt.H2 * p.Y * p.Y + rt.I2 * p.Z * p.Z +
                2 * rt.GH * p.X * p.Y + 2 * rt.GI * p.X * p.Z + 2 * rt.HI * p.Y * p.Z - size2.Y;
            return Transforms.SolveQuadratic(a, b, c, out t);
        }

        private bool TestZ(in RotorTransform rt, in FVec3 s, out double t)
        {
            ref FVec3 p = ref position;
            double a = rt.J2 * s.X * s.X + rt.K2 * s.Y * s.Y + rt.L2 * s.Z * s.Z +
                2 * rt.JK * s.X * s.Y + 2 * rt.JL * s.X * s.Z + 2 * rt.KL * s.Y * s.Z;
            double b = -2 * (rt.J2 * s.X * p.X + rt.K2 * s.Y * p.Y + rt.L2 * s.Z * p.Z +
                rt.JK * (s.X * p.Y + s.Y * p.X) + rt.JL * (s.X * p.Z + s.Z * p.X) + rt.KL * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.J2 * p.X * p.X + rt.K2 * p.Y * p.Y + rt.L2 * p.Z * p.Z +
                2 * rt.JK * p.X * p.Y + 2 * rt.JL * p.X * p.Z + 2 * rt.KL * p.Y * p.Z - size2.Z;
            return Transforms.SolveQuadratic(a, b, c, out t);
        }

        private bool Intersects(in FVec3 test, in RotorTransform rt)
        {
            double t;
            if (TestX(rt, test, out t))
            {
                double resultX = Math.Abs(test.X * t - position.X) / size.X;
                if (resultX > Math.Abs(test.Y * t - position.Y) / size.Y
                    && resultX > Math.Abs(test.Z * t - position.Z) / size.Z)
                {
                    return true;
                }
            }

            if (TestY(rt, test, out t))
            {
                double resultY = Math.Abs(test.Y * t - position.Y) / size.Y;
                if (resultY > Math.Abs(test.X * t - position.X) / size.X
                    && resultY > Math.Abs(test.Z * t - position.Z) / size.Z)
                {
                    return true;
                }
            }
            
            if (TestZ(rt, test, out t))
            {
                double resultZ = Math.Abs(test.Z * t - position.Z) / size.Z;
                if (resultZ > Math.Abs(test.X * t - position.X) / size.X
                    && resultZ > Math.Abs(test.Y * t - position.Y) / size.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public RGBA Sample(in FVec3 worldVec, in RotorTransform rt)
        {
            return Intersects(worldVec, rt) ? new RGBA(255, 255, 255, 255) : new RGBA();
        }
    }
}
