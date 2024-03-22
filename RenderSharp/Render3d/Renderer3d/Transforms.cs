using MathSharp;

namespace RenderSharp.Render3d
{
    internal class Transforms
    {
        public static FVec3 ScreenToWorldVec(in Vec2 screenPos, in Vec2 resolution, Camera camera)
        {
            FVec2 screenPosNorm = (FVec2)screenPos * 2 / resolution - new FVec2(1, 1);
            screenPosNorm.Y *= -1;
            double lx = camera.FocalLength * Math.Tan(camera.Fov.X.Radians / 2);
            double ly = camera.FocalLength * Math.Tan(camera.Fov.Y.Radians / 2);
            return new FVec3(lx * screenPosNorm.X, ly * screenPosNorm.Y, camera.FocalLength).Rotate(camera.Rotation);
        }

        public static bool GetValidIntersection(double a, double b, double c, double minDepth, out double depth)
        {
            double plusRoot, minusRoot;
            if (!Operations.SolveQuadratic(a, b, c, out plusRoot, out minusRoot) || (plusRoot < minDepth && minusRoot < minDepth))
            {
                depth = 0;
                return false;
            }

            depth = Math.Min(minusRoot, plusRoot);
            return true;
        }
    }
}
