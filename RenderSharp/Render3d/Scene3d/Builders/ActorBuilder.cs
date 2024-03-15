using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Builder for the <see cref="Actor"/> class.
    /// Used for <see cref="OptionalsStep.WithActor(ActorBuilder, string)"/> within <see cref="SceneBuilder"/>.
    /// </summary>
    public class ActorBuilder
    {
        private FVec3? boundingBoxSize;
        private AVec3? rotation;
        private FVec3? position;
        private RGBA? color;
        private Texture? texture;
        private FragShader? fragShader;
        private CoordShader? coordShader;

        /// <inheritdoc cref="Actor.BoundingBoxSize"/>
        public ActorBuilder WithBoundingBoxSize(in FVec3 boundingBoxSize)
        {
            this.boundingBoxSize = boundingBoxSize;
            return this;
        }

        /// <inheritdoc cref="Actor.Rotation"/>
        public ActorBuilder WithRotation(AVec3 rotation)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
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

        /// <inheritdoc cref="Actor.CoordShader"/>
        public ActorBuilder WithShader(CoordShader shader)
        {
            coordShader += shader;
            return this;
        }

        internal Actor Build()
        {
            boundingBoxSize ??= new FVec3(1, 1, 1);
            rotation ??= new AVec3();
            texture ??= new Texture(1, 1, color);
            position ??= new FVec3();
            fragShader ??= ((FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
            coordShader ??= ((Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; });
            return new Actor((FVec3)boundingBoxSize, (AVec3)rotation, (FVec3)position, texture, fragShader, coordShader);
        }
    }
}
