using Base.DAL;
using Base.Security;
using Base.UI;
using WebApi.Controllers;
using WebHttp = System.Web.Http;
using Base.BusinessProcesses.Services.Abstract;
using CorpProp.Services.Settings;
using Base.Service.Log;
using System.Web.Http;
using System;

namespace EUSI.WebApi.Controllers
{
    [WebHttp.RoutePrefix("eusi/error")]
    class ErrorController : BaseApiController
    {
        private ISecurityUser _securityUser = Base.Ambient.AppContext.SecurityUser;
        private readonly ILogService _logger;
        
        public ErrorController(
            IViewModelConfigService viewModelConfigService
            , IUnitOfWorkFactory unitOfWorkFactory
            , ILogService logger)
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Логгирование ошибки
        /// </summary>
        /// <param name="callstack">Колл-стек javascript</param>
        [HttpPost]
        [WebHttp.Route("logUndefinedError")]
        public void LogUndefinedError([FromBody] string callstack)
        {
            try
            {
                _logger.Log(callstack);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }
        }
    }
}
