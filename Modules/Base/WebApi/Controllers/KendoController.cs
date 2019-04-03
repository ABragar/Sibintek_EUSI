using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Base.DAL;
using Base.Service.Log;
using Base.UI;

namespace WebApi.Controllers
{
    [RoutePrefix("kendo")]
    public class KendoController : BaseApiController
    {
        private readonly ILogService _logger;
        public KendoController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("export")]
        public HttpResponseMessage Export(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileContents)
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return result;
        }
    }
}