using Microsoft.Msagl.Drawing;

namespace RestService.Helpers.NetworkGraphSociety.Style
{
    class DefaultGraphStyle : IGraphStyle
    {
        private GraphBuilderOptions _options;

        public DefaultGraphStyle(GraphBuilderOptions options )
        {
            _options = options;
        }

        public void Apply(Graph graph)
        {
            foreach (Edge edge in graph.Edges)
            {
                Label label = edge.Label;
                if (label != null)
                {
                    label.FontSize = 12;
                }
            }

            foreach (Node gNode in graph.Nodes)
            {
                gNode.Attr.LabelMargin = 9;
                gNode.Attr.FillColor = _options.OtherColor;
                gNode.Attr.LineWidth = 0;
                gNode.Label.FontColor = Color.Black;
            }

            var firstNode = graph.FindNode(_options.Root);
            if (firstNode != null)
            {
                firstNode.Attr.FillColor = _options.RootColor;
                firstNode.Label.FontColor = Color.Black;
            }

            var lastNode = graph.FindNode(_options.Sheet);
            if (lastNode != null)
            {
                lastNode.Attr.FillColor = _options.SheetColor;
                lastNode.Label.FontColor = Color.Black;
            }
        }
    }
}