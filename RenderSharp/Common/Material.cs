using MathSharp;
using System.Runtime.CompilerServices;

namespace RenderSharp.Common
{
    /// <summary>
    /// Material for more advanced rendering.
    /// </summary>
    public class Material
    {
        Texture texture;

        /// <summary>
        /// Width of the underlying texture.
        /// </summary>
        public int Width => texture.Width;

        /// <summary>
        /// Height of the underlying texture.
        /// </summary>
        public int Height => texture.Height;

        /// <summary>
        /// Construct a new material.
        /// </summary>
        /// <param name="texture"></param>
        public Material(Texture texture)
        {
            this.texture = texture;
        }

        internal RGBA this[int x, int y] => texture[y, x];

        internal RGBA this[FVec2 uv] => texture[uv];

        /// <summary>
        /// Info about the material as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Texture: {texture.Size}\nDiffuse color: {texture[0, 0]}";
        }
    }
}
