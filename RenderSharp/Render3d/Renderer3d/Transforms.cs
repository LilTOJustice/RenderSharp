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

        public static bool GetValidIntersection(double a, double b, double c, double minDepth, out double depth)
        {
            double minusRoot;
            if (!Operations.SolveQuadratic(a, b, c, out _, out minusRoot) || minusRoot < minDepth)
            {
                depth = -1;
                return false;
            }

            depth = minusRoot;
            return true;
        }
    }
}
