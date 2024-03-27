using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor with a model representation.
    /// </summary>
    public class ModelActor : Actor
    {
        private Model origModel, model;
        private FVec3 cameraRelPosition;

        internal ModelActor(
            FVec3 size,
            RVec3 rotation,
            FVec3 position,
            Texture texture,
            FragShader fragShader,
            Model model,
            FVec3? cameraPos = null)
            : base(size, rotation, position, texture, fragShader)
        {
            cameraRelPosition = position - cameraPos ?? new FVec3();
            origModel = model;
            this.model = new Model(model, size, rotation, cameraRelPosition);
        }

        internal override bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            return model.Sample(worldVec, minDepth, time, out sample, out depth);
        }

        internal override Actor Copy(in FVec3 cameraPos)
        {
            return new ModelActor(
               Size,
               Rotation,
               Position,
               Texture,
               FragShader,
               origModel,
               cameraPos);
        }
    }
}
