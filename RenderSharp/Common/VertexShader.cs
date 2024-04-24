using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// Delegate type for coordinate shaders. Which are shaders purely used to transform coordinates.
    /// </summary>
    /// <param name="vertIn">Input vertex for the shader.</param>
    /// <param name="vertOut">Output vertex for the shader.</param>
    /// <param name="time">Time elapsed.</param>
    public delegate void VertexShader(FVec3 vertIn, out FVec3 vertOut, double time);
}
