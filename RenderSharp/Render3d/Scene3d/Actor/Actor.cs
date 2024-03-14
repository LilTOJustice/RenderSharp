using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// An object to be rendered from within a scene.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// World space collision box size of the actor.
        /// </summary>
        public FVec3 CollisionBoxSize { get; set; }
        
        /// <summary>
        /// World space width of the actor's collision box.
        /// </summary>
        public double Width { get { return CollisionBoxSize.X; } }

        /// <summary>
        /// World space height of the actor's collision box.
        /// </summary>
        public double Height { get { return CollisionBoxSize.Y; } }

        /// <summary>
        /// World space depth of the actor's collision box.
        /// </summary>
        public double Depth { get { return CollisionBoxSize.Z; } }
        
        /// <summary>
        /// World space position of the actor's center.
        /// </summary>
        public FVec3 Position { get; set; }
        
        /// <summary>
        /// Rotation of the actor about its center.
        /// </summary>
        public AVec3 Rotation { get; set; }
        
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

        internal Actor(
            in FVec3 size,
            in AVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader,
            CoordShader coordShader)
        {
            Texture = texture;
            Position = position;
            CollisionBoxSize = size;
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
                CollisionBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader,
                CoordShader
            );
        }
    }
}
