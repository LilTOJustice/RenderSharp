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
    }
}
