using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor with a model representation.
    /// </summary>
    public class ModelActor : Actor
    {
        private Model model;

        internal ModelActor(FVec3 size, RVec3 rotation, FVec3 position, Texture texture, FragShader fragShader, Model model)
            : base(size, rotation, position, texture, fragShader)
        {
            this.model = model;
        }

        internal override bool Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, double time, out RGBA sample, out double depth)
        {
            return model.Sample(worldVec, cameraPos, minDepth, time, out sample, out depth);
        }

        internal override Actor Copy()
        {
            return new ModelActor(
               Size,
               Rotation,
               Position,
               Texture,
               FragShader,
               model);
        }
    }
}
