using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// Builder for the <see cref="Actor"/> class.
    /// Used for <see cref="OptionalsStep.WithActor(ActorBuilder, string, int)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class ActorBuilder
    {
        FVec2? size = null;
        double rotation = 0;
        FVec2? position = null;
        Texture? texture = null;
        FragShader shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };

        /// <inheritdoc cref="Actor.Size"/>
        public ActorBuilder WithSize(FVec2 size)
        {
            this.size = new FVec2(size);
            return this;
        }

        /// <inheritdoc cref="Actor.Rotation"/>
        public ActorBuilder WithRotation(double rotation)
        {
            this.rotation = rotation;
            return this;
        }

        /// <inheritdoc cref="Actor.Position"/>
        public ActorBuilder WithPosition(FVec2 position)
        {
            this.position = new FVec2(position);
            return this;
        
        }

        /// <inheritdoc cref="Actor.Texture"/>
        public ActorBuilder WithTexture(Texture texture)
        {
            this.texture = texture;
            return this;
        }

        /// <inheritdoc cref="Actor.Shader"/>
        public ActorBuilder WithShader(FragShader shader)
        {
            this.shader += shader;
            return this;
        }

        /// <summary>
        /// Builds the actor.
        /// </summary>
        /// <returns>A constructed actor.</returns>
        internal Actor Build()
        {
            return new Actor(size, rotation, position, texture, shader);
        }
    }
}
