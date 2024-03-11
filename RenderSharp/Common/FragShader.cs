using MathSharp;

namespace RenderSharp
{
    /// <summary>
    /// Delegate type for fragment shaders.
    /// </summary>
    /// <param name="fragIn">Input pixel for the shader.</param>
    /// <param name="fragOut">Output pixel to write to for the shader.</param>
    /// <param name="fragCoord">Coordinate within the space of the rendered texture.</param>
    /// <param name="res">Size of the rendered texture.</param>
    /// <param name="time">Time elapsed.</param>
    public delegate void FragShader(FRGBA fragIn, out FRGBA fragOut, Vec2 fragCoord, Vec2 res, double time);
}
