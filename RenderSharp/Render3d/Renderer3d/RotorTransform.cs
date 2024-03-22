using MathSharp;

namespace RenderSharp.Render3d
{
    internal struct RotorTransform
    {
        public readonly double cosX, sinX, cosY, sinY, cosZ, sinZ,
            D, E, F, G, H, I, J, K, L,
            D2, E2, F2, G2, H2, I2, J2, K2, L2,
            DE, DF, EF, GH, GI, HI, JK, JL, KL;

        public RotorTransform(in RVec3 rotation)
        {
            cosX = Math.Cos(rotation.X.Radians);
            sinX = Math.Sin(rotation.X.Radians);
            cosY = Math.Cos(rotation.Y.Radians);
            sinY = Math.Sin(rotation.Y.Radians);
            cosZ = Math.Cos(rotation.Z.Radians);
            sinZ = Math.Sin(rotation.Z.Radians);
            D = cosY * cosZ;
            E = sinX * sinY * cosZ - cosX * sinZ;
            F = cosX * sinY * cosZ + sinX * sinZ;
            G = cosY * sinZ;
            H = sinX * sinY * sinZ + cosX * cosZ;
            I = cosX * sinY * sinZ - sinX * cosZ;
            J = -sinY;
            K = sinX * cosY;
            L = cosX * cosY;
            D2 = D * D;
            E2 = E * E;
            F2 = F * F;
            G2 = G * G;
            H2 = H * H;
            I2 = I * I;
            J2 = J * J;
            K2 = K * K;
            L2 = L * L;
            DE = D * E;
            DF = D * F;
            EF = E * F;
            GH = G * H;
            GI = G * I;
            HI = H * I;
            JK = J * K;
            JL = J * L;
            KL = K * L;
        }
    }
}
