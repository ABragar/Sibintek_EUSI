using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Base.DAL;
using Base.Service.Log;
using Base.UI;
using CorpProp.Services.ScheduleStateRegistrationExport;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("Export")]
    internal class ExportController : BaseApiController
    {
        private IScheduleStateExportService _exportService;
        private readonly ILogService _logger;

        public ExportController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _exportService = new ScheduleStateExportService(unitOfWorkFactory);
        }

        [HttpGet]
        [Route("LndFormat/{mnemonic}/{id}")]
        public HttpResponseMessage LndFormat(string mnemonic, int id)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new PushStreamContent(
                (outputStream, httpContent, transportContext) =>
                {
                    try
                    {
                        
                        _exportService.Export(outputStream, mnemonic, id);
                    }
                    finally
                    {
                        outputStream.Dispose();
                    }

                });

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = $"{GetConfig().Title}.xlsx", FileNameStar = $"{GetConfig().Title}.xlsx" };

            return response;
        }
    }
}