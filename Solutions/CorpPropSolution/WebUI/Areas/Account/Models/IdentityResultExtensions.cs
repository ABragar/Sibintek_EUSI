using System.Linq;
using Microsoft.AspNet.Identity;
using WebUI.Areas.Account.Models.Shared;

namespace WebUI.Areas.Account.Models
{
    public static class IdentityResultExtensions
    {
        public static ActionResultModel ToActionResult(this IdentityResult result, string go_back_url, params string[] succeed_messages)
        {
            return new ActionResultModel
            {
                GoBackUrl = go_back_url,
                Success = result.Succeeded,
                Messages = result.Succeeded ? succeed_messages : result.Errors.ToArray()
            };
        }

    }
}