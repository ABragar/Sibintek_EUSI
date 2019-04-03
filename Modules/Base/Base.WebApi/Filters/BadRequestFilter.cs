using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Base.WebApi.Exceptions;
using Base.WebApi.Models;

namespace Base.WebApi.Filters
{
    public class BadRequestFilter : IExceptionFilter
    {
        public bool AllowMultiple => false;

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {

            var ex = actionExecutedContext.Exception as BadRequestException;

            if (ex != null)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, new StandartResult()
                {
                    Details = ex.Message,
                    Extended = ex.Extended
                });
            }

            return Task.CompletedTask;
        }

    }
}