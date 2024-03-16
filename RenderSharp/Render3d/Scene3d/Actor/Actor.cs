using MathSharp;
using RenderSharp.Render3d.Renderer3d;

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

        internal Triangle[] Triangles
        {
            get
            {
                return new Triangle[0];
            }
        }

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
            BoundingBoxSize = size;
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
                BoundingBoxSize,
                Rotation,
                Position,
                Texture,
                FragShader,
                CoordShader
            );
        }
    }
}
