using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using NLog;

namespace RestService.Helpers
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // actionExcutedContext.Exception is the exception log it however you wish
            _log.Error(actionExecutedContext.Exception);

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new
            {
                actionExecutedContext.Exception.Message
            });
        }
    }
}