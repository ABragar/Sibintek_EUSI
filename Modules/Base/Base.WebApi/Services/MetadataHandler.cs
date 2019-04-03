using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiProxy.Server;

namespace Base.WebApi.Services
{
    public class MetadataHandler : HttpMessageHandler
    {
        private readonly MetadataProvider _metadata_provider;

        public MetadataHandler(HttpConfiguration config)
        {
            _metadata_provider = new MetadataProvider(config);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var metadata = _metadata_provider.GetMetadata(request);

            var response = request.CreateResponse(HttpStatusCode.OK, metadata);


            return Task.FromResult(response);
        }
    }
}
