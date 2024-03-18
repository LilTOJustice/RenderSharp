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
            sphere = new Sphere(position, size);
        }

        internal override RGBA Sample(in FVec3 worldVec)
        {
            return sphere.Sample(worldVec);
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
