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
            in Triangle triangle,
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader)
            : base(size, rotation, position, texture, fragShader)
        {
            this.triangle = new Triangle(
                position + triangle.v0.Rotate(rotation) * size,
                position + triangle.v1.Rotate(rotation) * size,
                position + triangle.v2.Rotate(rotation) * size);
        }

        internal override RGBA Sample(in FVec3 worldVec)
        {
            return triangle.Sample(worldVec);
        }

        internal override Actor Copy()
        {
            return new TriangleActor(
                triangle,
                BoundingBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader);
        }
    }
}
