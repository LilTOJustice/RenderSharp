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

        internal override bool Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, out RGBA sample, out double depth)
        {
            if (sphere.Intersects(worldVec, cameraPos, minDepth, out depth))
            {
                sample = new RGBA(255, 0, 0, 255);
                return true;
            }

            sample = new RGBA();
            return false;
        }

        internal override Actor Copy()
        {
            return new SphereActor(
                BoundingBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader);
        }
    }
}
