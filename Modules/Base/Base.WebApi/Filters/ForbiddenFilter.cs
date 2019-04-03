using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Base.WebApi.Extensions;
using Base.WebApi.Models;

namespace Base.WebApi.Filters
{
    public class ForbiddenFilter : IAuthenticationFilter
    {
        public bool AllowMultiple => false;


        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {

            var result = await context.Result.ExecuteAsync(cancellationToken);

            if (result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.Forbidden)
            {
                var auth = context.ActionContext.RequestContext.Principal?.Identity?.IsAuthenticated ?? false;

                if (auth)
                {
                    context.Result = context.Request.CreateContentResult(HttpStatusCode.Forbidden, new StandartResult
                    {
                        Details = "Не достаточно прав"
                    });

                }
                else
                {
                    context.Result = context.Request.CreateContentResult(HttpStatusCode.Unauthorized, new StandartResult
                    {
                        Details = "Требуется авторизация"
                    });
                }

                return;


            }

            context.Result = new ResponseMessageResult(result);
        }
    }
}