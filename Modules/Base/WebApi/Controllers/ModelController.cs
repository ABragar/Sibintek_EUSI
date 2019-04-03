using System.Web.Http;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("model")]
    internal class ModelController: ApiController
    {
        
    }
}