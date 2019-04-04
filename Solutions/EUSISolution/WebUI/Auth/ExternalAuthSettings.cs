using System;
using System.IO;

namespace WebUI.Auth
{
    public class ExternalAuthSettings
    {

        public bool GoogleEnabled { get; set; } = true;

        public string GoogleClientId { get; set; } = "695239549980-93q131rh7s2qechi8fjmgemgi1516jvs.apps.googleusercontent.com";

        public string GoogleClientSecret { get; set; } = "gc6RoDZ5-3SL88U0EGbDRr1D";


        public bool VkontakteEnabled { get; set; } = true;

        public string VkontakteAppId { get; set; } = "5200086";

        public string VkontakteAppSecret { get; set; } = "CZDEsWIpws2esSgkevEw";

        public bool FacebookEnabled { get; set; } = false;
        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }

        public bool TwitterEnabled { get; set; } = false;
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }


        public bool EsiaEnabled { get; set; } = false;
        public string EsiaIssuer { get; set; } = "fmbaros.ru";

        public string EsiaPFXFileName { get; set; } = "fmbaros.pfx";

        public string EsiaPFXPassword { get; set; } = "123";

    }
}