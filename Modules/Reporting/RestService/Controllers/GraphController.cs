using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using NLog;
using ReportStorage.Service;
using RestService.EF;
using RestService.Entities;
using Color = Microsoft.Msagl.Drawing.Color;

namespace RestService.Controllers
{
    [RoutePrefix("Graph")]
    public class GraphController : ApiController
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();

        public GraphController()
        {
        }

//        [Obsolete("Оставлен для поддержки обратной совместимости")]
//        public GraphController(IReportStorageService reportStorageService, IFileService fileService)
//        {
//        }

        private async Task<List<GraphBySociety>> GetEdges(int id)
        {
            _log.Debug("Input {{id: {0} }}", id);
            using (var context = new DataContext())
            {
                DbRawSqlQuery<GraphBySociety> rawSqlQuery = context.pGraphBySociety(id);
                return await rawSqlQuery.ToListAsync();
            }
        }

        [HttpGet]
        [Route("Get/{width}/{hight}/{id}")]
        public async Task<HttpResponseMessage> GetGraph(int width, int hight, int id, [FromUri]string root, [FromUri]string sheet)
        {
            _log.Debug("Begin method: {0}/{1}/ , root:{2}, sheet:{3}", width, hight, root, sheet);
            var responce = Request.CreateResponse(HttpStatusCode.OK);
            List<GraphBySociety> edges = await GetEdges(id);
            Graph g = new Graph("test");
            foreach (GraphBySociety edge in edges)
            {
                if (edge.ShareMarket.HasValue)
                {
                    _log.Debug($"Shareholder:{edge.Shareholder},\tRecipient:{edge.Recipient},\tShareMarket:{Math.Round(edge.ShareMarket.Value,2)}");
                    g.AddEdge(edge.Shareholder, $"{Math.Round(edge.ShareMarket.Value,2)} %", edge.Recipient);
                }
                else
                {
                    g.AddEdge(edge.Shareholder, edge.Recipient);
                    _log.Debug($"Shareholder:{edge.Shareholder},\tRecipient:{edge.Recipient}");
                }
            }

            Styling(g, root, sheet, width, hight);

            responce.Content = GraphToContent(g, width, hight);
            responce.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return responce;
        }


        private void Styling(Graph graph, string root, string sheet, int width, int hight)
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
                gNode.Attr.FillColor = new Color(27, 119, 212);
                gNode.Attr.LineWidth = 0;
                gNode.Label.FontColor = Color.White;
            }

            var firstNode = graph.FindNode(root);
            if (firstNode != null)
            {
                firstNode.Attr.FillColor = new Color(249, 221, 22);
                firstNode.Label.FontColor = Color.Black;
            }

            var lastNode = graph.FindNode(sheet);
            if (lastNode != null)
            {
                lastNode.Attr.FillColor = new Color(63, 72, 76);
                lastNode.Label.FontColor = Color.White;
            }
        }

        private ByteArrayContent GraphToContent(Graph graph, int width, int hight)
        {
            Bitmap img = new Bitmap(width, hight);
            //graph.LayoutAlgorithmSettings=new RankingLayoutSettings();
            GraphRenderer gr = new GraphRenderer(graph);
            gr.Render(img);
            MemoryStream memoryStream = new MemoryStream();
            img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return new ByteArrayContent(memoryStream.ToArray());
        }
    }
}