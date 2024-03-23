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
            ref RotorTransform rt = ref rotorTransform;
        }

        private bool EpsilonCheck(double a, double b)
        {
            return Math.Abs(a - b) <= 0.001;
        }

        private bool TestX(in FVec3 s, in FVec3 cameraPos, double minDepth, out double depth)
        {
            FVec3 p = position - cameraPos;
            ref RotorTransform rt = ref rotorTransform;
            double a = rt.A2 * s.X * s.X + rt.B2 * s.Y * s.Y + rt.C2 * s.Z * s.Z +
                2 * rt.AB * s.X * s.Y + 2 * rt.AC * s.X * s.Z + 2 * rt.BC * s.Y * s.Z;
            double b = -2 * (rt.A2 * s.X * p.X + rt.B2 * s.Y * p.Y + rt.C2 * s.Z * p.Z +
                rt.AB * (s.X * p.Y + s.Y * p.X) + rt.AC * (s.X * p.Z + s.Z * p.X) + rt.BC * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.A2 * p.X * p.X + rt.B2 * p.Y * p.Y + rt.C2 * p.Z * p.Z +
                2 * rt.AB * p.X * p.Y + 2 * rt.AC * p.X * p.Z + 2 * rt.BC * p.Y * p.Z - size2.X;
            return Transforms.GetValidIntersection(a, b, c, minDepth, out depth);
        }

        private bool TestY(in FVec3 s, in FVec3 cameraPos, double minDepth, out double depth)
        {
            FVec3 p = position - cameraPos;
            ref RotorTransform rt = ref rotorTransform;
            double a = rt.D2 * s.X * s.X + rt.E2 * s.Y * s.Y + rt.F2 * s.Z * s.Z +
                2 * rt.DE * s.X * s.Y + 2 * rt.DF * s.X * s.Z + 2 * rt.EF * s.Y * s.Z;
            double b = -2 * (rt.D2 * s.X * p.X + rt.E2 * s.Y * p.Y + rt.F2 * s.Z * p.Z +
                rt.DE * (s.X * p.Y + s.Y * p.X) + rt.DF * (s.X * p.Z + s.Z * p.X) + rt.EF * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.D2 * p.X * p.X + rt.E2 * p.Y * p.Y + rt.F2 * p.Z * p.Z +
                2 * rt.DE * p.X * p.Y + 2 * rt.DF * p.X * p.Z + 2 * rt.EF * p.Y * p.Z - size2.Y;
            return Transforms.GetValidIntersection(a, b, c, minDepth, out depth);
        }

        private bool TestZ(in FVec3 s, in FVec3 cameraPos, double minDepth, out double depth)
        {
            FVec3 p = position - cameraPos;
            ref RotorTransform rt = ref rotorTransform;
            double a = rt.G2 * s.X * s.X + rt.H2 * s.Y * s.Y + rt.I2 * s.Z * s.Z +
                2 * rt.GH * s.X * s.Y + 2 * rt.GI * s.X * s.Z + 2 * rt.HI * s.Y * s.Z;
            double b = -2 * (rt.G2 * s.X * p.X + rt.H2 * s.Y * p.Y + rt.I2 * s.Z * p.Z +
                rt.GH * (s.X * p.Y + s.Y * p.X) + rt.GI * (s.X * p.Z + s.Z * p.X) + rt.HI * (s.Y * p.Z + s.Z * p.Y));
            double c = rt.G2 * p.X * p.X + rt.H2 * p.Y * p.Y + rt.I2 * p.Z * p.Z +
                2 * rt.GH * p.X * p.Y + 2 * rt.GI * p.X * p.Z + 2 * rt.HI * p.Y * p.Z - size2.Z;
            return Transforms.GetValidIntersection(a, b, c, minDepth, out depth);
        }

        public bool Intersects(in FVec3 test, in FVec3 cameraPos, double minDepth, out double depth)
        {
            FVec3 p = position - cameraPos;
            if (TestX(test, cameraPos, minDepth, out depth))
            {
                FVec3 rotated = (test * depth - p).Rotate(rotation);
                double resultX = Math.Abs(rotated.X) / size.X;
                if (EpsilonCheck(resultX, 1) && resultX > Math.Abs(rotated.Y) / size.Y
                    && resultX > Math.Abs(rotated.Z) / size.Z)
                {
                    return true;
                }
            }

            if (TestY(test, cameraPos, minDepth, out depth))
            {
                FVec3 rotated = (test * depth - p).Rotate(rotation);
                double resultY = Math.Abs(rotated.Y) / size.Y;
                if (EpsilonCheck(resultY, 1) && resultY > Math.Abs(rotated.X) / size.X
                    && resultY > Math.Abs(rotated.Z) / size.Z)
                {
                    return true;
                }
            }
            
            if (TestZ(test, cameraPos, minDepth, out depth))
            {
                FVec3 rotated = (test * depth - p).Rotate(rotation);
                double resultZ = Math.Abs(rotated.Z) / size.Z;
                if (EpsilonCheck(resultZ, 1) && resultZ > Math.Abs(rotated.X) / size.X
                    && resultZ > Math.Abs(rotated.Y) / size.Y)
                {
                    return true;
                }
            }

            depth = -1;
            return false;
        }
    }
}
