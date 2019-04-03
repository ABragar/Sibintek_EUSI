using System.Drawing;
using System.IO;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using RestService.Helpers.NetworkGraphSociety.Style;

namespace RestService.Helpers
{
    internal static class Extensions
    {
        internal static void ApplyStyle(this Graph graph, IGraphStyle style)
        {
            style.Apply(graph);
        }
        
        /// <summary>
        /// Create image with given width and height
        /// </summary>
        /// <param name="graph">Target object <see cref="Graph"/></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>Byte array of jpeg image</returns>
        internal static byte[] GraphToImageBytes(this Graph graph, int width,int height)
        {
            Bitmap img = new Bitmap(width,height);
            GraphRenderer gr = new GraphRenderer(graph);
            gr.Render(img);
            MemoryStream memoryStream = new MemoryStream();
            img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Find char from array and replace fined chars on input value. 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="characters">Characters to search</param>
        /// <param name="value">Value to replace</param>
        /// <returns></returns>
        internal static string Replace(this string str, char[] characters, char value)
        {
            foreach (char character in characters)
            {
                str = str.Replace(character, value);
            }
            return str;
        }
    }
}