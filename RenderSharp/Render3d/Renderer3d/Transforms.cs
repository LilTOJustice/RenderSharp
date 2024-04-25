using MathSharp;

namespace RenderSharp.Render3d
{
    internal class Transforms
    {
        public static Ray ScreenToRay(in Vec2 screenPos, in Vec2 resolution, Camera camera)
        {
            FVec2 screenPosNorm = (FVec2)screenPos * 2 / resolution - new FVec2(1, 1);
            screenPosNorm.Y *= -1;
            double lx = camera.FocalLength == 0 ? 1
                : camera.FocalLength * Math.Tan(camera.Fov.X.Radians / 2);
            double ly = camera.FocalLength == 0 ? 1
                : camera.FocalLength * Math.Tan(camera.Fov.Y.Radians / 2);
            return new Ray(camera.Position, new FVec3(
                lx * screenPosNorm.X,
                ly * screenPosNorm.Y,
                camera.FocalLength == 0 ? 1 : camera.FocalLength
            ).Rotate(camera.Rotation));
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
    }
}
