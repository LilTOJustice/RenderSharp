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
        private FVec3 origSize, origPosition;
        private RVec3 origRotation;
        private FVec3 cameraRelPosition;
        
        /// <summary>
        /// UNIMPLEMENTED. Shader to be applied to every vertex of the actor's model.
        /// </summary>
        public VertexShader VertexShader { get; set; }

        internal ModelActor(
            FVec3 size,
            RVec3 rotation,
            FVec3 position,
            Texture texture,
            FragShader fragShader,
            VertexShader vertexShader,
            Model model,
            FVec3? cameraPos = null,
            bool newModel = true)
            : base(size, rotation, position, texture, fragShader)
        {
            VertexShader = vertexShader;
            origSize = size;
            origPosition = position;
            origRotation = rotation;
            cameraRelPosition = position - cameraPos ?? new FVec3();
            origModel = model;
            this.model = newModel ? new Model(model, size, rotation, cameraRelPosition) : model;
        }

        internal override void Sample(in FVec3 worldVec, double minDepth, double time, out RGBA sample, out double depth)
        {
            List<(RGBA, FVec2, Material, double)> renderQueue;
            model.Sample(worldVec, minDepth, out renderQueue, out depth);
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

    internal override Actor Copy(in FVec3 cameraPos)
    {
        bool newModel = cameraPos != cameraRelPosition || Size != origSize || Rotation != origRotation || Position != origPosition;
        return new ModelActor(
           Size,
           Rotation,
           Position,
           Texture,
           FragShader,
           VertexShader,
           origModel,
           cameraPos,
           newModel);
    }
}
}
