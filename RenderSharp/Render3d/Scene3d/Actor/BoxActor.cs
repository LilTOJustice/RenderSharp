using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor representing a box in 3D space.
    /// </summary>
    public class BoxActor : Actor
    {
        private Box box;

        internal BoxActor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader)
            : base(size, rotation, position, texture, fragShader)
        {
            box = new Box(position, size);
        }

        internal override RGBA Sample(in FVec3 worldVec)
        {
            return box.Sample(worldVec);
        }

        internal override Actor Copy()
        {
            return new BoxActor(
                BoundingBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader);
        }
    }
}
