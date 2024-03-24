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
            throw new NotImplementedException();
        }

        internal override Actor Copy()
        {
            throw new NotImplementedException();
        }
    }
}
