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
        public double Width { get { return Size.X; } set { Size.X = value; } }

        /// <summary>
        /// World space height of the actor.
        /// </summary>
        public double Height { get { return Size.Y; } set { Size.Y = value; } }
        
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
        public Texture Texture { get; protected set; }
        
        /// <summary>
        /// Shader to be applied to the actor's texture.
        /// </summary>
        public FragShader Shader { get; set; }

        /// <summary>
        /// Constructs an actor.
        /// </summary>
        /// <param name="texture">Texture to be rendered on the actor.</param>
        /// <param name="position">World space position of the actor.</param>
        /// <param name="size">World space size of the actor.</param>
        /// <param name="rotation">Rotation of the actor about its center (radians).</param>
        /// <param name="shader">Shader to be applied to the actor's texture.</param>
        internal Actor(
            FVec2? size = null,
            double rotation = 0,
            FVec2? position = null,
            Texture? texture = null,
            FragShader? shader = null)
        {
            Texture = texture ?? new Texture((Vec2)(size ?? new Vec2()));
            Position = position ?? new FVec2();
            Size = size ?? new FVec2();
            Rotation = rotation;
            Shader = shader ?? ((in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; });
        }

        /// <summary>
        /// Clears any active shaders on the actor.
        /// </summary>
        public void ClearShaders()
        {
            Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; }; 
        }

        internal virtual Actor Reconstruct()
        {
            return new Actor(
                    new FVec2(Size),
                    Rotation,
                    new FVec2(Position),
                    Texture,
                    Shader
                );
        }
    }
}
