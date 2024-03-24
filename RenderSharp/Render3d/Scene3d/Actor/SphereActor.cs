using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a sphere in 3D space.
    /// </summary>
    public class SphereActor : Actor
    {
        private Sphere sphere;

        internal SphereActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader)
            : base(size, rotation, position, texture, fragShader)
        {
            sphere = new Sphere(position, size, rotation);
        }

        internal override bool Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, double time, out RGBA sample, out double depth)
        {
            if (sphere.Intersects(worldVec, cameraPos, minDepth, out depth))
            {
                FVec3 fromCenter = (worldVec * depth - (Position - cameraPos)).Rotate(Rotation);
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

        internal override Actor Copy()
        {
            return new SphereActor(
                Size,
                Rotation,
                Position,
                Texture,
                FragShader);
        }
    }
}
