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
            FVec3 cameraDir = (new FVec3(0, 0, 1) * camera.FocalLength + new FVec3(lx * screenPosNorm.X, ly * screenPosNorm.Y, 0)).Rotate(camera.Rotation);
            return camera.Position + cameraDir;
        }

        public static bool SolveQuadratic(double a, double b, double c, out double root)
        {
            root = 0;
            double sqrt = b * b - 4 * a * c;
            if (sqrt < 0)
            {
                return false;
            }

            double sqrtResult = Math.Sqrt(sqrt);
            double plus = (-b + sqrtResult) / (2 * a);
            double minus = (-b - sqrtResult) / (2 * a);
            if (plus < 1 && minus < 1)
            {
                return false;
            }
            else if (plus < 1)
            {
                root = minus;
                return true;
            }
            else if (minus < 1)
            {
                root = plus;
                return true;
            }

            root = Math.Min(plus, minus);
            return true;
        }
    }
}
