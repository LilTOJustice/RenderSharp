using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Builder for the <see cref="Actor"/> class.
    /// Used for <see cref="SceneBuilder.FinalStep.WithActor(string, ActorBuilder)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class ActorBuilder
    {
        /// <inheritdoc cref="Actor.Size"/>
        protected FVec3? size;

        /// <inheritdoc cref="Actor.Rotation"/>
        protected RVec3? rotation;
        
        /// <inheritdoc cref="Actor.Position"/>
        protected FVec3? position;

        /// <summary>
        /// Color to fill if no texture is given.
        /// </summary>
        protected RGBA? color;

        /// <inheritdoc cref="Actor.Texture"/>
        protected Texture? texture;

        /// <inheritdoc cref="Actor.FragShader"/>
        protected FragShader? fragShader;

        /// <inheritdoc cref="Actor.Size"/>
        public ActorBuilder WithSize(in FVec3 size)
        {
            this.size = size;
            return this;
        }

        /// <inheritdoc cref="Actor.Rotation"/>
        public ActorBuilder WithRotation(RVec3 rotation)
        {
            this.rotation = rotation;
            return this;
        }

        /// <inheritdoc cref="Actor.Position"/>
        public ActorBuilder WithPosition(in FVec3 position)
        {
            this.position = position;
            return this;
        }

        /// <inheritdoc cref="color"/>
        public ActorBuilder WithColor(in RGBA color)
        {
            this.color = color;
            return this;
        }

        /// <inheritdoc cref="Actor.Texture"/>
        public ActorBuilder WithTexture(Texture texture)
        {
            this.texture = texture;
            return this;
        }

        /// <inheritdoc cref="Actor.FragShader"/>
        public ActorBuilder WithShader(FragShader shader)
        {
            fragShader += shader;
            return this;
        }

        internal virtual Actor Build()
        {
            size ??= new FVec3(1, 1, 1);
            rotation ??= new RVec3();
            texture ??= new Texture(1, 1, color);
            position ??= new FVec3();
            fragShader ??= ((FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            return new Actor((FVec3)size, (RVec3)rotation, (FVec3)position, texture, fragShader);
        }
    }
}
