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

        internal override Sample Sample(in Ray ray, double time)
        {
            List<Model.ToRender> renderQueue;
            model.Sample(ray, out renderQueue);

            renderQueue.Sort((a, b) => b.distance.CompareTo(a.distance));
            Model.ToRender last = renderQueue.LastOrDefault(
                new Model.ToRender(default, default, default, default!, double.PositiveInfinity));
            
            Sample sample = new Sample(ray.origin + ray.direction * last.distance, last.normal, last.distance, new RGBA());

            foreach (Model.ToRender toRender in renderQueue)
            {
                FRGBA fOut;
                FragShader(
                    toRender.color,
                    out fOut,
                    (Vec2)(toRender.uv * toRender.material.Diffuse.Size),
                    toRender.material.Diffuse.Size,
                    time);
                sample.color = ColorFunctions.AlphaBlend(fOut, sample.color);
            }

            return sample;
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
