using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Base.WebApi.Models;

namespace Base.WebApi.Extensions
{
    public static class HttpRequestMessageExtensions
    {

        public static IHttpActionResult CreateContentResult<T>(this HttpRequestMessage request,
            HttpStatusCode statusCode,
            T content)
        {
            var context = request.GetRequestContext();

            
            return new NegotiatedContentResult<T>(statusCode, content, context.Configuration.Services.GetContentNegotiator(), request, context.Configuration.Formatters);
        }

        public static HttpResponseMessage CreateStandartResponseMessage(this HttpRequestMessage request,
            HttpStatusCode statusCode,
            string details,
            object extended)
        {
            return request.CreateResponse(statusCode, new StandartResult()
            {
                Details = details,
                Extended = extended,
            });

        }


    }
}