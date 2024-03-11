using MathSharp;

namespace RenderSharp.Render2d
{
    /// <summary>
    /// An object to be rendered from within a scene.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// World space size of the actor.
        /// </summary>
        public FVec2 Size { get; set; }
        
        /// <summary>
        /// World space width of the actor.
        /// </summary>
        public double Width { get { return Size.X; } set { Size = new FVec2(Size.X, value); } }

        /// <summary>
        /// World space height of the actor.
        /// </summary>
        public double Height { get { return Size.Y; } set { Size = new FVec2(Size.X, value); } }
        
        /// <summary>
        /// World space position of the actor.
        /// </summary>
        public FVec2 Position { get; set; }
        
        /// <summary>
        /// Rotation of the actor about its center.
        /// </summary>
        public double Rotation { get; set; }
        
        /// <summary>
        /// Texture to render on the actor.
        /// </summary>
        public Texture Texture { get; set; }
        
        /// <summary>
        /// Shader to be applied to the actor's texture.
        /// </summary>
        public FragShader FragShader { get; set; }

        /// <summary>
        /// Shader applied to the actor space coordinates before being passed to the fragment shader.
        /// </summary>
        public CoordShader CoordShader { get; set; }

        internal Actor()
        {
            Size = new FVec2();
            Position = new FVec2();
            Texture = new Texture(0, 0);
            FragShader = (FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
            CoordShader = (Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; };
        }

        internal Actor(
            in FVec2 size,
            double rotation,
            in FVec2 position,
            Texture texture,
            FragShader fragShader,
            CoordShader coordShader)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Rotation = rotation;
            FragShader = fragShader;
            CoordShader = coordShader;
        }

        /// <summary>
        /// Clears any active fragment shaders on the actor.
        /// </summary>
        public void ClearFragShaders()
        {
            FragShader = (FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
        }

        /// <summary>
        /// Clears any active coordinate shaders on the actor.
        /// </summary>
        public void ClearCoordShaders()
        {
            CoordShader = (Vec2 vertIn, out Vec2 vertOut, Vec2 size, double time) => { vertOut = vertIn; };
        }

        /// <summary>
        /// Clears any active shaders on the actor.
        /// </summary>
        public void ClearShaders()
        {
            ClearFragShaders();
            ClearCoordShaders();
        }

        /// <summary>
        /// Creates a deep copy of the actor.
        /// </summary>
        /// <returns>An actor with the same properties but non-referential to the original object.</returns>
        public virtual Actor Copy()
        {
            return new Actor(
                Size,
                Rotation,
                Position,
                Texture,
                FragShader,
                CoordShader
            );
        }
    }
}
