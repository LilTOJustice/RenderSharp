﻿using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// An object to be rendered from within a scene.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// World space bounding box size of the actor.
        /// The actor's center is at the origin of the bounding box, and the actor's rotation is about the center.
        /// The actor's rendered scale is determined by this value.
        /// </summary>
        public FVec3 Size { get; set; }
        
        /// <summary>
        /// World space width of the actor's bounding box.
        /// </summary>
        public double Width { get { return Size.X; } }

        /// <summary>
        /// World space height of the actor's bounding box.
        /// </summary>
        public double Height { get { return Size.Y; } }

        /// <summary>
        /// World space depth of the actor's bounding box.
        /// </summary>
        public double Depth { get { return Size.Z; } }
        
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
            Size = size;
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
        internal virtual bool Sample(in FVec3 worldVec, in FVec3 cameraPos, double minDepth, double time, out RGBA sample, out double depth)
        {
            depth = 0;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a deep copy of the actor.
        /// </summary>
        /// <returns>An actor with the same properties but non-referential to the original object.</returns>
        internal virtual Actor Copy()
        {
            return new Actor(
                Size,
                Rotation,
                Position,
                Texture,
                FragShader
            );
        }
    }
}
