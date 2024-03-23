using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct RotorTransform
    {
        public readonly double cosX, sinX, cosY, sinY, cosZ, sinZ,
            A, B, C, D, E, F, G, H, I,
            A2, B2, C2, D2, E2, F2, G2, H2, I2,
            AB, AC, BC, DE, DF, EF, GH, GI, HI;

        public RotorTransform(in RVec3 rotation)
        {
            cosX = Math.Cos(rotation.X.Radians);
            sinX = Math.Sin(rotation.X.Radians);
            cosY = Math.Cos(rotation.Y.Radians);
            sinY = Math.Sin(rotation.Y.Radians);
            cosZ = Math.Cos(rotation.Z.Radians);
            sinZ = Math.Sin(rotation.Z.Radians);
            A = cosY * cosZ;
            B = sinX * sinY * cosZ - cosX * sinZ;
            C = cosX * sinY * cosZ + sinX * sinZ;
            D = cosY * sinZ;
            E = sinX * sinY * sinZ + cosX * cosZ;
            F = cosX * sinY * sinZ - sinX * cosZ;
            G = -sinY;
            H = sinX * cosY;
            I = cosX * cosY;
            A2 = A * A;
            B2 = B * B;
            C2 = C * C;
            D2 = D * D;
            E2 = E * E;
            F2 = F * F;
            G2 = G * G;
            H2 = H * H;
            I2 = I * I;
            AB = A * B;
            AC = A * C;
            BC = B * C;
            DE = D * E;
            DF = D * F;
            EF = E * F;
            GH = G * H;
            GI = G * I;
            HI = H * I;
        }
    }
}
