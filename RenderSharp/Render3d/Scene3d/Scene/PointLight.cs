using MathSharp;

namespace RenderSharp.Render3d
{
    /// <summary>
    /// Shadow caster for the scene.
    /// </summary>
    public class PointLight
    {
        /// <summary>
        /// Position of the point light.
        /// </summary>
        public FVec3 Position { get; set; }
        
        internal PointLight(in FVec3 position)
        {
            this.Position = position;
        }
    }
}
