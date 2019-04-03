using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Owin;
using RestService.Identity;
using System.Configuration;

[assembly: OwinStartup(typeof(RestService.Startup))]

namespace RestService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            //config

            SimpleInjectorResolver.Init(config);

            Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(config);

            TicketFormatService tickerService = new TicketFormatService();

            var key = ConfigurationManager.AppSettings["AesDataProtectorKey"];

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                AccessTokenFormat = tickerService.GetAesTicketFormat(key),
                AuthenticationMode = AuthenticationMode.Active,
                Provider = new OAuthBearerAuthenticationProvider()
                {
                    OnRequestToken = OnRequestToken
                }
            });

            app.UseWebApi(config);
        }

        private static Task OnRequestToken(OAuthRequestTokenContext context)
        {
            if (context.Token == null)
            {
                context.Token = context.Request.Query.Get("bearer_token");
            }

            return Task.CompletedTask;
        }
    }
}