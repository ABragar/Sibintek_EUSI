using System.Collections.Generic;

namespace WebUI.Areas.Account.Models.Account
{
    public class ExternalProvidersViewModel
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public IReadOnlyCollection<ProviderInfo> Providers { get; set; }
    }
}
