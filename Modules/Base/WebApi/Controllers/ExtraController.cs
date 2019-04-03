using System.Web.Http;
using WebApi.Attributes;

namespace WebApi.Controllers
{

    [CheckSecurityUser]
    public class ExtraController: ApiController
    {
        
    }
}