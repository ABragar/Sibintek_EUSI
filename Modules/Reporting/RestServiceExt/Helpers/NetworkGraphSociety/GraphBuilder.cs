using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using NLog;
using RestService.Helpers.NetworkGraphSociety.Style;
using Color = Microsoft.Msagl.Drawing.Color;

namespace RestService.Helpers.NetworkGraphSociety
{
    internal class GraphBuilder
    {
        
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        private static Object lockObject=new object();
        internal GraphBuilderOptions Options { get; set; }

        private GraphImage _graphImage;
        internal GraphImage Image
        {
            get { return _graphImage;}
            private set
            {
                IsReset = false;
                _graphImage = value;
            }
        }

        internal IGraphStyle GraphStyle { get; set; }
         
        internal int Id { get; set; } = -1;
        internal bool IsReset { get; private set; } = true;
         
        
        /// <summary>
        /// Reset state builder. Set default values to Image <see cref="GraphImage"/>, Id <see cref="int"/> 
        /// No reset builder Options.
        /// </summary>
        internal void Reset()
        {
            Id = -1;
            Image = null;
            IsReset = true;
        }
        
        internal  void Build()
        {
            if (Id < 0) throw new Exception("Society id not set but missing.");
            if (Options==null) throw new Exception("Graph options is null but missing.");
            
            if(GraphStyle==null) GraphStyle = new DefaultGraphStyle(Options);
            List<GraphBySociety> graphBySocieties =  GetEdges();
            Graph graph = CreateGraph(Options.Name, graphBySocieties);
            graph.ApplyStyle(GraphStyle);
            Image=new GraphImage()
            {
                Image = GraphToImageBytes(graph, Options.Width, Options.Height),
                FileName = Options.Name,
                Id = Id
            };
        }
        private  byte[] GraphToImageBytes(Graph graph, int width,int height)
        {
            Bitmap img = new Bitmap(width,height);
            GraphRenderer gr = new GraphRenderer(graph);
            lock (lockObject)
            {
                gr.Render(img);
            }
            MemoryStream memoryStream = new MemoryStream();
            img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return memoryStream.ToArray();
        }

        private  List<GraphBySociety> GetEdges()
        {
            using (var context = new ReportDbContext())
            {
                DbRawSqlQuery<GraphBySociety> rawSqlQuery = context.pGraphBySociety(Id);
                return rawSqlQuery.ToList();
            }
        }
        
        private Graph CreateGraph(string name, IList<GraphBySociety> edges)
        {
            Graph g = new Graph(name);
            foreach (GraphBySociety edge in edges)
            {
                if (edge.ShareMarket.HasValue)
                {
                    _log.Debug($"Shareholder:{edge.Shareholder},\tRecipient:{edge.Recipient},\tShareMarket:{Math.Round(edge.ShareMarket.Value,2)}");
                    g.AddEdge(edge.Shareholder, $"{(edge.ShareMarket.Value >(Math.Floor(edge.ShareMarket.Value*100000000)/100000000)?"≈ ":"")}{Math.Floor(edge.ShareMarket.Value*100000000)/100000000} %", edge.Recipient);
                }
                else
                {
                    g.AddEdge(edge.Shareholder, edge.Recipient);
                    _log.Debug($"Shareholder:{edge.Shareholder},\tRecipient:{edge.Recipient}");
                }
            }
            return g;
        }
    }
}