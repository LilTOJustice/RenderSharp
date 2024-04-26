using MathSharp;

namespace RenderSharp.Render3d
{
    internal class Transforms
    {
        public static Ray ScreenToRay(in Vec2 screenPos, in Vec2 resolution, Camera camera)
        {
            double aspectRatio = (double)resolution.X / resolution.Y;
            FVec2 screenPosNorm = (FVec2)screenPos * 2 / resolution - new FVec2(1, 1);
            screenPosNorm.Y *= -1;
            double lx = camera.FocalLength == 0 ? 1
                : camera.FocalLength * Math.Tan(camera.Fov.X.Radians / 2);
            double ly = camera.FocalLength == 0 ? 1
                : camera.FocalLength * Math.Tan(camera.Fov.Y.Radians / 2);
            FVec3 cameraToScreen = new FVec3(
                lx * screenPosNorm.X * aspectRatio,
                ly * screenPosNorm.Y,
                camera.FocalLength == 0 ? 1 : camera.FocalLength).Rotate(camera.Rotation);
            return new Ray(camera.Position, cameraToScreen.Norm());
        }

        public static void GetValidIntersection(double a, double b, double c, out (double, double) closeFar)
        {
            double plusRoot, minusRoot;
            closeFar = (double.PositiveInfinity, double.PositiveInfinity);
            if (!Operations.SolveQuadratic(a, b, c, out plusRoot, out minusRoot))
            {
                return;
            }

            if (plusRoot < 0)
            {
                return;
            }
            
            closeFar.Item2 = plusRoot;

            if (minusRoot < 0)
            {
                return;
            }

            closeFar.Item1 = minusRoot;
        }
    }
}
