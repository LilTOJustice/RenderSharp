using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a triangle in 3D space.
    /// </summary>
    public class TriangleActor : Actor
    {
        private Triangle triangle;

        internal TriangleActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader)
            : base(size, rotation, position, texture, fragShader)
        {
            FVec3 v0 = new FVec3(-0.5, -0.5, 0);
            FVec3 v1 = new FVec3(0.5, -0.5, 0);
            FVec3 v2 = new FVec3(0, 0.5, 0);
            triangle = new Triangle(
                position + v0.Rotate(rotation) * size,
                position + v1.Rotate(rotation) * size,
                position + v2.Rotate(rotation) * size);
        }

        internal override RGBA Sample(in FVec3 worldVec)
        {
            return triangle.Sample(worldVec);
        }

        internal override Actor Copy()
        {
            return new TriangleActor(
                BoundingBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader);
        }
    }
}
