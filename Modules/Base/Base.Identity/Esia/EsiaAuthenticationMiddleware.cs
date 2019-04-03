using System;
using System.IO;
using Esia.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;

namespace Base.Identity.Esia
{
    internal class EsiaAuthenticationMiddleware : AuthenticationMiddleware<EsiaAuthenticationOptions>
    {
        public EsiaAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, EsiaAuthenticationOptions options) : base(next, options)
        {
            _request = new EsiaRequest(new EsiaConfiguration()
            {
                IdpSSOTargetURL = options.IdpSSOTargetURL,
                Issuer = options.Issuer,
                PFXFileName = options.PFXFileName,
                PFXPassword = options.PFXPassword,
              //  LogFileName = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory")?.ToString(), "EsiaCert.txt")


            });

            _sign_in_type = app.GetDefaultSignInAsAuthenticationType();

            _state_format = new PropertiesDataFormat(app.CreateDataProtector("esia relay state"));
        }

        private readonly EsiaRequest _request;
        private readonly PropertiesDataFormat _state_format;
        private readonly string _sign_in_type;

        protected override AuthenticationHandler<EsiaAuthenticationOptions> CreateHandler()
        {
            return new EsiaAuthenticationHandler(_request, _state_format, _sign_in_type);
        }
    }
}