using MathSharp;
using RenderSharp.Common;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Actor with a model representation.
    /// </summary>
    public class ModelActor : Actor
    {
        private Model origModel, model;
        
        /// <summary>
        /// UNIMPLEMENTED. Shader to be applied to every vertex of the actor's model.
        /// </summary>
        public VertexShader VertexShader { get; set; }

        internal ModelActor(
            FVec3 position,
            FVec3 size,
            RVec3 rotation,
            Texture texture,
            FragShader fragShader,
            VertexShader vertexShader,
            Model model)
            : base(position, size, rotation, texture, fragShader)
        {
            VertexShader = vertexShader;
            origModel = model;
            this.model = new Model(origModel, size, rotation, position);
        }

        internal override void Sample(in Ray ray, double minDepth, double time, out RGBA sample, out double depth)
        {
            List<(RGBA, FVec2, Material, double)> renderQueue;
            model.Sample(ray, minDepth, out renderQueue, out depth);
            sample = new RGBA();

            renderQueue.Sort((a, b) => b.Item4.CompareTo(a.Item4));
            depth = renderQueue.LastOrDefault((default, default, default!, double.PositiveInfinity)).Item4;

            foreach ((RGBA s, FVec2 uv, Material mat, _) in renderQueue)
            {
                FRGBA fOut;
                FragShader(s, out fOut, (Vec2)(uv * mat.Diffuse.Size), mat.Diffuse.Size, time);
                sample = ColorFunctions.AlphaBlend(fOut, sample);
            }
        }

    internal override Actor Copy()
    {
        return new ModelActor(
           Position,
           Size,
           Rotation,
           Texture,
           FragShader,
           VertexShader,
           origModel);
    }
}
}
