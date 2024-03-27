using MathSharp;
using System.Runtime.CompilerServices;

namespace RenderSharp.Common
{
    /// <summary>
    /// Material for more advanced rendering.
    /// </summary>
    public class Material
    {
        /// <summary>
        /// The diffuse texture of the material.
        /// </summary>
        public Texture Diffuse { get; }

        /// <summary>
        /// Construct a new material.
        /// </summary>
        public Material(Texture diffuse)
        {
            this.Diffuse = diffuse;
        }

        /// <summary>
        /// Info about the material as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Diffuse: {Diffuse.Size}";
        }
    }
}
