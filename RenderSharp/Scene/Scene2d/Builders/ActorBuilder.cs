using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// Builder for the <see cref="Actor"/> class.
    /// Used for <see cref="OptionalsStep.WithActor(ActorBuilder, string, int)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class ActorBuilder
    {
        private FVec2? size;
        private double rotation;
        private FVec2? position;
        private RGBA? color;
        private Texture? texture;
        private FragShader? fragShader;
        private CoordShader? coordShader;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public ActorBuilder WithColor(RGBA color)
        {
            this.color = new RGBA(color);
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

        /// <inheritdoc cref="Actor.CoordShader"/>
        public ActorBuilder WithShader(CoordShader shader)
        {
            coordShader += shader;
            return this;
        }

        internal Actor Build()
        {
            size ??= new FVec2(1, 1);
            texture ??= new Texture(1, 1, color);
            position ??= new FVec2();
            fragShader ??= ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            coordShader ??= ((in Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; });
            return new Actor(size, rotation, position, texture, fragShader, coordShader);
        }
    }
}
