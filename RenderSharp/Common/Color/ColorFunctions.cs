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

        /// <summary>
        /// Converts an <see cref="RGB"/> color to an <see cref="HSV"/> color.
        /// </summary>
        public static HSV RGBToHSV(in RGB rgb)
        {
            double R = rgb.R, G = rgb.G, B = rgb.B;
            double M = Math.Max(Math.Max(R, G), B);
            double m = Math.Min(Math.Min(R, G), B);
            double V = M / 255;
            double S = (M > 0 ? 1 - m / M : 0);
            double H = Math.Acos(
                (R - .5 * G - .5 * B) / Math.Sqrt(R * R + G * G + B * B - R * G - R * B - G * B)
            ) * 180 / Math.PI;

            if (B > G)
            {
                H = 360 - H;
            }

            return new HSV(H, S, V);
        }

        /// <summary>
        /// Converts an <see cref="HSV"/> color to an <see cref="RGB"/> color.
        /// </summary>
        public static RGB HSVToRGB(in HSV hsv)
        {
            double H = hsv.H, S = hsv.S, V = hsv.V;
            double M = 255 * V;
            double m = M * (1 - S);
            double z = (M - m) * (1 - Math.Abs(H / 60 % 2 - 1));
            byte R, G, B;

            if (H < 60)
            {
                R = (byte)M;
                G = (byte)(z + m);
                B = (byte)m;
            }
            else if (H < 120)
            {
                R = (byte)(z + m);
                G = (byte)M;
                B = (byte)m;
            }
            else if (H < 180)
            {
                R = (byte)m;
                G = (byte)M;
                B = (byte)(z + m);
            }
            else if (H < 240)
            {
                R = (byte)m;
                G = (byte)(z + m);
                B = (byte)M;
            }
            else if (H < 300)
            {
                R = (byte)(z + m);
                G = (byte)m;
                B = (byte)M;
            }
            else
            {
                R = (byte)M;
                G = (byte)m;
                B = (byte)(z + m);
            }

            return new RGB(R, G, B);
        }
    }
}
