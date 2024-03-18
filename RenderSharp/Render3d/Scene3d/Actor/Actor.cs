using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// An object to be rendered from within a scene.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// World space bounding box size of the actor.
        /// </summary>
        public FVec3 BoundingBoxSize { get; set; }
        
        /// <summary>
        /// World space width of the actor's collision box.
        /// </summary>
        public double Width { get { return BoundingBoxSize.X; } }

        /// <summary>
        /// World space height of the actor's collision box.
        /// </summary>
        public double Height { get { return BoundingBoxSize.Y; } }

        /// <summary>
        /// World space depth of the actor's collision box.
        /// </summary>
        public double Depth { get { return BoundingBoxSize.Z; } }
        
        /// <summary>
        /// World space position of the actor's center.
        /// </summary>
        public FVec3 Position { get; set; }
        
        /// <summary>
        /// Rotation of the actor about its center.
        /// </summary>
        public RVec3 Rotation { get; set; }
        
        /// <summary>
        /// Texture to render on the actor.
        /// </summary>
        public Texture Texture { get; set; }
        
        /// <summary>
        /// Shader to be applied to every rendered pixel of the actor.
        /// </summary>
        public FragShader FragShader { get; set; }

        internal Actor(
            in FVec3 size,
            in RVec3 rotation,
            in FVec3 position,
            Texture texture,
            FragShader fragShader)
        {
            Texture = texture;
            Position = position;
            BoundingBoxSize = size;
            Rotation = rotation;
            FragShader = fragShader;
        }

        /// <summary>
        /// Clears any active fragment shaders on the actor.
        /// </summary>
        public void ClearFragShaders()
        {
            FragShader = (FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; };
        }

        /// <summary>
        /// Clears any active shaders on the actor.
        /// </summary>
        public void ClearShaders()
        {
            ClearFragShaders();
        }

        // TODO: Implement
        internal virtual RGBA Sample(in FVec3 worldVec)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a deep copy of the actor.
        /// </summary>
        /// <returns>An actor with the same properties but non-referential to the original object.</returns>
        internal virtual Actor Copy()
        {
            return new Actor(
                BoundingBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader
            );
        }
    }
}
