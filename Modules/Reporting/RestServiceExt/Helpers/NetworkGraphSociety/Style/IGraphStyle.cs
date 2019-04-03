using Microsoft.Msagl.Drawing;

namespace RestService.Helpers.NetworkGraphSociety.Style
{
    internal interface IGraphStyle
    {
        void Apply(Graph graph);
    }
}