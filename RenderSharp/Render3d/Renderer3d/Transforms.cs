using MathSharp;

namespace RenderSharp.Render3d
{
    internal class Transforms
    {
        public static FVec3 ScreenToWorldVec(in Vec2 screenPos, in Vec2 resolution, Camera camera)
        {
            FVec2 screenPosNorm = (FVec2)screenPos * 2 / resolution - new FVec2(1, 1);
            screenPosNorm.Y *= -1;
            double lx = camera.FocalLength == 0 ? 1
                : camera.FocalLength * Math.Tan(camera.Fov.X.Radians / 2);
            double ly = camera.FocalLength == 0 ? 1
                : camera.FocalLength * Math.Tan(camera.Fov.Y.Radians / 2);
            return new FVec3(
                lx * screenPosNorm.X,
                ly * screenPosNorm.Y,
                camera.FocalLength == 0 ? 1 : camera.FocalLength
            ).Rotate(camera.Rotation);
        }

        public static void GetValidIntersection(double a, double b, double c, double minDepth, out (double, double) closeFar)
        {
            double plusRoot, minusRoot;
            closeFar = (double.PositiveInfinity, double.PositiveInfinity);
            if (!Operations.SolveQuadratic(a, b, c, out plusRoot, out minusRoot))
            {
                return;
            }

            if (plusRoot < minDepth)
            {
                return;
            }
            
            closeFar.Item2 = plusRoot;

            if (minusRoot < minDepth)
            {
                return;
            }

            closeFar.Item1 = minusRoot;
        }

        public static Quaternion RotationToQuaternion(in RVec3 rotation)
        {
            double mag = Math.Sqrt((rotation.X * rotation.X + rotation.Y * rotation.Y + rotation.Z * rotation.Z).Radians);
            RVec3 norm = rotation / mag;
            return new Quaternion(new Radian(mag), new FVec3(norm.X.Radians, norm.Y.Radians, norm.Z.Radians));
        }
    }
}
