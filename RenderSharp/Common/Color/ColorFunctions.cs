namespace RenderSharp
{
    /// <summary>
    /// Contains miscellaneous functions that interact with color objects.
    /// </summary>
    public class ColorFunctions
    {
        /// <summary>
        /// Blends two colors together based on <see cref="FRGBA.A"/> (alpha) value.
        /// </summary>
        /// <param name="top">The color on "top" i.e. closer to the camera.</param>
        /// <param name="bottom">The color on "bottom" i.e. farther from the camera.</param>
        /// <returns>The resulting blended color (<see href="https://en.wikipedia.org/wiki/Alpha_compositing"/>).</returns>
        public static RGBA AlphaBlend(FRGBA top, FRGBA bottom)
        {
            return (FRGB)top * top.A + (FRGB)bottom * (1d - top.A);
        }
    }
}
