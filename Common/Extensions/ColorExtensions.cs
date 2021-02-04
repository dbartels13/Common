using System;
using System.Drawing;
using System.Text;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Any Color extension methods
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Retrieves a contrast color for the given color
        /// </summary>
        /// <param name="color">The given color</param>
        /// <returns>The contrast color</returns>
	    public static Color ContrastColor(this Color color)
        {
            // Counting the perceptive luminance - human eye favors green color... 
            var a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255.0;
            return a < 0.5 ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
        }

        /// <summary>
        /// Converts this to a color representation needed in HTML
        /// </summary>
        /// <param name="color">The color object</param>
        /// <returns>The HTML representation (Eg. "#123456")</returns>
	    public static string ToHtml(this Color color)
        {
            var sb = new StringBuilder();
            sb.Append("#");
            if (color.R < 16)
                sb.Append("0");
            sb.Append(color.R.ToString("X"));
            if (color.G < 16)
                sb.Append("0");
            sb.Append(color.G.ToString("X"));
            if (color.B < 16)
                sb.Append("0");
            sb.Append(color.B.ToString("X"));
            return sb.ToString();
        }

        /// <summary>
        /// Compares 2 colors to see if they are visually similar (Delta-E 1976 is way to restrictive, so using this custom method)
        /// </summary>
        /// <param name="color1">The first color to compare</param>
        /// <param name="color2">The second color to compare</param>
        /// <returns></returns>
	    public static bool IsSimilar(this Color color1, Color color2)
        {
            // Going to allow total deviation of 50, adjusted for color code strength (R=0.3, G=0.59, B=0.11)
            var delta = Math.Abs(color1.R - color2.R) * 0.3;
            delta += Math.Abs(color1.G - color2.G) * 0.59;
            delta += Math.Abs(color1.B - color2.B) * 0.11;
            return delta < 50;
        }
    }
}
