using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Middlewares
{
    public class ADInternalAuthOptions : AuthenticationOptions
    {
        public ADInternalAuthOptions() : base("CorpProp.ADInternalAuth")
        {
            Description.Caption = "CorpProp.ADInternalAuth";
            AuthenticationMode = AuthenticationMode.Passive;
            ExpireTimeSpan = TimeSpan.FromDays(1);
            CookieName = "ADInternalAuth";
            LoginPath = new PathString("/ADInternalAuth/Login");
            ReturnUrlParameter = "ReturnUrl";
        }
        public string CookieName { get; internal set; }
        public TimeSpan ExpireTimeSpan { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> TicketDataFormat { get; set; }
        public ICookieManager CookieManager { get; set; }
        public PathString LoginPath { get; internal set; }
        public string ReturnUrlParameter { get; internal set; }
    }
}
