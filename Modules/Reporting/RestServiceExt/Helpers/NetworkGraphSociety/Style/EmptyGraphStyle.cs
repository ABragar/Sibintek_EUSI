using Microsoft.Msagl.Drawing;
using NLog;

namespace RestService.Helpers.NetworkGraphSociety.Style
{
    class EmptyGraphStyle : IGraphStyle
    {
        
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        public void Apply(Graph graph)
        {
            _log.Info("Apply empty style.");
        }
    }
}