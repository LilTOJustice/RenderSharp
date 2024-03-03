﻿using MathSharp;

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
        public Texture Texture { get; set; }
        
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
            FVec2 size,
            double rotation,
            FVec2 position,
            Texture texture,
            FragShader shader)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Rotation = rotation;
            Shader = shader;
        }

        /// <summary>
        /// Clears any active shaders on the actor.
        /// </summary>
        public void ClearShaders()
        {
            Shader = (in FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time) => { fragOut = fragIn; }; 
        }

        /// <summary>
        /// Creates a deep copy of the actor.
        /// </summary>
        /// <returns>An actor with the same properties but non-referential to the original object.</returns>
        public virtual Actor Copy()
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
