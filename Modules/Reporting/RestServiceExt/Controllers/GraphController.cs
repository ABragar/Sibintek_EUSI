using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using DAL.EF;
using DAL.Entities;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using NLog;
using RestService.Helpers;
using RestService.Helpers.NetworkGraphSociety;
using RestService.Helpers.NetworkGraphSociety.Style;
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

        [HttpGet]
        [Route("Get/{width}/{hight}/{id}")]
        public async Task<HttpResponseMessage> GetGraph(int width, int hight, int id, [FromUri]string root, [FromUri]string sheet)
        {
            _log.Debug("Begin method: {0}/{1}/ , root:{2}, sheet:{3}", width, hight, root, sheet);
            SocietyInfo rootSociety = await GetSocietyInfoAsync(1);
            SocietyInfo sheetSociety = await GetSocietyInfoAsync(id);
            GraphBuilderOptions options=new GraphBuilderOptions()
            {
                Name = $"{sheetSociety.IDEUP}_{DateTime.Now:yyyy.MM.dd}.jpg",
                Height = hight,
                Width = width,
                RootColor = new Color(249, 221, 22),
                SheetColor = new Color(63, 72, 76),
                OtherColor = new Color(27, 119, 212),
                Root = rootSociety.ShortName,
                Sheet = sheetSociety.ShortName
            };
            
            GraphBuilder builder=new GraphBuilder(){Options = options};
            GraphImageProvider imageProvider = new GraphImageProvider(builder);
            GraphImage graphImage = imageProvider.GetGraphImageBySocietyId(id);

            var responce = Request.CreateResponse(HttpStatusCode.OK);
            responce.Content = new ByteArrayContent( graphImage.Image);
            responce.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            return responce;
        }
        
        [HttpGet]
        [Route("GetZip")]
        public async Task<HttpResponseMessage> GetGraphImagesAsZip([FromUri]List<int> ids)
        {
            ZipObject zipObject=new ZipObject();
            MemoryStream stream = await zipObject.CreateAsync(ids);
            var responce = Request.CreateResponse(HttpStatusCode.OK);
            
            responce.Content = new StreamContent(stream);
            responce.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"SocietyNetworkGraphs_{DateTime.Now:ddMMyyyy}.zip"
                
            };
            responce.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return responce;
        }
        
        
        private async Task<SocietyInfo> GetSocietyInfoAsync(int id)
        {
            _log.Debug("Input {{id: {0} }}", id);
            using (var context = new ReportDbContext())
            {
                DbRawSqlQuery<SocietyInfo> rawSqlQuery = context.pGetSocietyInfo(id);
                SocietyInfo firstAsync = await rawSqlQuery.FirstOrDefaultAsync();
                if(firstAsync==null)throw new DataException($"Society with id {id} not found");
                return firstAsync;
            }
        }


    }
}