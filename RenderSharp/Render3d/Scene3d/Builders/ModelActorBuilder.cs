using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Builder for the <see cref="ModelActor"/> class.
    /// Used for <see cref="SceneBuilder.FinalStep.WithActor(string, ActorBuilder)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class ModelActorBuilder
    {
        /// <summary>
        /// Model used by the actor.
        /// </summary>
        public FinalStep WithModel(in Model model)
        {
            return new FinalStep(model);
        }
    }

    /// <inheritdoc cref="ActorBuilder"/>
    public class FinalStep : ActorBuilder
    {
        /// <inheritdoc cref="ModelActor.model"/>
        private Model model;

        internal FinalStep(Model model)
        {
            this.model = model;
        }

        /// <summary>
        /// <b>UNSUPPORTED.</b>
        /// </summary>
        public new ActorBuilder WithTexture(Texture texture) => throw new NotImplementedException("WithTexture not supported for ModelActor.");

        /// <summary>
        /// <b>UNSUPPORTED.</b>
        /// </summary>
        public new ActorBuilder WithColor(in RGBA color) => throw new NotImplementedException("WithTexture not supported for ModelActor.");

        internal override Actor Build()
        {
            size ??= new FVec3(1, 1, 1);
            rotation ??= new RVec3();
            texture ??= new Texture(1, 1, color);
            position ??= new FVec3();
            fragShader ??= ((FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            return new ModelActor((FVec3)size, (RVec3)rotation, (FVec3)position, texture, fragShader, model);
        }
    }
}
