using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a sphere in 3D space.
    /// </summary>
    public class SphereActor : Actor
    {
        private Sphere sphere;
        FVec3 cameraRelPosition;

        internal SphereActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader,
            in FVec3? cameraPos = null)
            : base(size, rotation, position, texture, fragShader)
        {
            cameraRelPosition = position - cameraPos ?? new FVec3();
            sphere = new Sphere(cameraRelPosition, size, rotation);
        }

        internal override bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            if (sphere.Intersects(worldVec, minDepth, out depth))
            {
                FVec3 fromCenter = (worldVec * depth - cameraRelPosition).Rotate(Rotation);
                double theta1 = fromCenter.X == 0 ?
                    (fromCenter.Y < 0 ? -Math.PI / 2 : Math.PI / 2)
                    : Math.Atan(fromCenter.Y / fromCenter.X);
                double theta2 = fromCenter.X == 0 ?
                    (fromCenter.Z < 0 ? -Math.PI / 2 : Math.PI / 2)
                    : Math.Atan(fromCenter.Z / fromCenter.X);
                FVec2 uv = new FVec2(
                    (theta2 + Math.PI / 2) / Math.PI,
                    (theta1 + Math.PI / 2) / Math.PI);
                FRGBA fOut;
                FragShader(Texture[uv], out fOut, (Vec2)(uv * Texture.Size), Texture.Size, time);
                sample = fOut;
                return true;
            }

            sample = new RGBA();
            return false;
        }

        internal override Actor Copy(in FVec3 cameraPos)
        {
            return new SphereActor(
                Size,
                Rotation,
                Position,
                Texture,
                FragShader,
                cameraPos);
        }
    }
}
