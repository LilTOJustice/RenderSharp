using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor with a model representation.
    /// </summary>
    public class ModelActor : Actor
    {
        private Model origModel, model;
        private FVec3 origSize, origPosition;
        private RVec3 origRotation;
        private FVec3 cameraRelPosition;

        internal ModelActor(
            FVec3 size,
            RVec3 rotation,
            FVec3 position,
            Texture texture,
            FragShader fragShader,
            Model model,
            FVec3? cameraPos = null,
            bool newModel = true)
            : base(size, rotation, position, texture, fragShader)
        {
            origSize = size;
            origPosition = position;
            origRotation = rotation;
            cameraRelPosition = position - cameraPos ?? new FVec3();
            origModel = model;
            this.model = newModel ? new Model(model, size, rotation, cameraRelPosition) : model;
        }

        internal override bool Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            return model.Sample(worldVec, minDepth, out sample, out depth);
        }

        internal override Actor Copy(in FVec3 cameraPos)
        {
            bool newModel = cameraPos != cameraRelPosition || Size != origSize || Rotation != origRotation || Position != origPosition;
            return new ModelActor(
               Size,
               Rotation,
               Position,
               Texture,
               FragShader,
               origModel,
               cameraPos,
               newModel);
        }
    }
}
