using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct Sphere
    {
        private FVec3 pos;
        private FVec3 radii2;
        private RVec3 rotation;

        public Sphere(in FVec3 pos, in FVec3 radii, in RVec3 rotation)
        {
            this.pos = pos;
            this.rotation = rotation;
            radii2 = radii * radii;
        }

        private FVec3 rotorTransform(in FVec3 s)
        {
            ref FVec3 p = ref pos;
            double cosX = Math.Cos(rotation.X.Radians);
            double sinX = Math.Sin(rotation.X.Radians);
            double cosY = Math.Cos(rotation.Y.Radians);
            double sinY = Math.Sin(rotation.Y.Radians);
            double cosZ = Math.Cos(rotation.Z.Radians);
            double sinZ = Math.Sin(rotation.Z.Radians);
            double D = cosY * cosZ;
            double E = sinX * sinY * cosZ - cosX * sinZ;
            double F = cosX * sinY * cosZ + sinX * sinZ;
            double G = cosY * sinZ;
            double H = sinX * sinY * sinZ + cosX * cosZ;
            double I = cosX * sinY * sinZ - sinX * cosZ;
            double J = -sinY;
            double K = sinX * cosY;
            double L = cosX * cosY;
            double D2 = D * D;
            double E2 = E * E;
            double F2 = F * F;
            double G2 = G * G;
            double H2 = H * H;
            double I2 = I * I;
            double J2 = J * J;
            double K2 = K * K;
            double L2 = L * L;
            double DE = D * E;
            double DF = D * F;
            double EF = E * F;
            double GH = G * H;
            double GI = G * I;
            double HI = H * I;
            double JK = J * K;
            double JL = J * L;
            double KL = K * L;
            double a =
                (D2 * s.X * s.X + E2 * s.Y * s.Y + F2 * s.Z * s.Z + 2 * DE * s.X * s.Y + 2 * DF * s.X * s.Z + 2 * EF * s.Y * s.Z) / radii2.X +
                (G2 * s.X * s.X + H2 * s.Y * s.Y + I2 * s.Z * s.Z + 2 * GH * s.X * s.Y + 2 * GI * s.X * s.Z + 2 * HI * s.Y * s.Z) / radii2.Y +
                (J2 * s.X * s.X + K2 * s.Y * s.Y + L2 * s.Z * s.Z + 2 * JK * s.X * s.Y + 2 * JL * s.X * s.Z + 2 * KL * s.Y * s.Z) / radii2.Z;
            double b =
                -2 * (D2 * s.X * p.X + E2 * s.Y * p.Y + F2 * s.Z * p.Z + DE * (s.X * p.Y + s.Y * p.X) + DF * (s.X * p.Z + s.Z * p.X) + EF * (s.Y * p.Z + s.Z * p.Y)) / radii2.X +
                -2 * (G2 * s.X * p.X + H2 * s.Y * p.Y + I2 * s.Z * p.Z + GH * (s.X * p.Y + s.Y * p.X) + GI * (s.X * p.Z + s.Z * p.X) + HI * (s.Y * p.Z + s.Z * p.Y)) / radii2.Y +
                -2 * (J2 * s.X * p.X + K2 * s.Y * p.Y + L2 * s.Z * p.Z + JK * (s.X * p.Y + s.Y * p.X) + JL * (s.X * p.Z + s.Z * p.X) + KL * (s.Y * p.Z + s.Z * p.Y)) / radii2.Z;
            double c =
                (D2 * p.X * p.X + E2 * p.Y * p.Y + F2 * p.Z * p.Z + 2 * DE * p.X * p.Y + 2 * DF * p.X * p.Z + 2 * EF * p.Y * p.Z) / radii2.X +
                (G2 * p.X * p.X + H2 * p.Y * p.Y + I2 * p.Z * p.Z + 2 * GH * p.X * p.Y + 2 * GI * p.X * p.Z + 2 * HI * p.Y * p.Z) / radii2.Y +
                (J2 * p.X * p.X + K2 * p.Y * p.Y + L2 * p.Z * p.Z + 2 * JK * p.X * p.Y + 2 * JL * p.X * p.Z + 2 * KL * p.Y * p.Z) / radii2.Z - 1;

            return new FVec3(a, b, c);
        }

        private bool Intersects(in FVec3 test)
        {
            FVec3 abc = rotorTransform(test);
            double a = abc.X;
            double b = abc.Y;
            double c = abc.Z;

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
