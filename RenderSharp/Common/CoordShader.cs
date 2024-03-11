using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// Delegate type for coordinate shaders. Which are shaders purely used to transform coordinates.
    /// </summary>
    /// <param name="coordIn">Input coordinates for the shader.</param>
    /// <param name="coordOut">Output coordinates for the shader.</param>
    /// <param name="size">Size of the space the shader is run on.</param>
    /// <param name="time">Time elapsed.</param>
    public delegate void CoordShader(Vec2 coordIn, out Vec2 coordOut, Vec2 size, double time);
}
