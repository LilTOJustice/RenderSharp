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
            FVec3 cameraDir = new FVec3(1, 0, 0) * camera.FocalLength; //.Rotate(scene.Camera.Rotation);
            return camera.Position + cameraDir + new FVec3(0, ly * screenPosNorm.Y, lx * screenPosNorm.X);
        }
    }
}
