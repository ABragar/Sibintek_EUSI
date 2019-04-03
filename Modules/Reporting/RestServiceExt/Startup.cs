using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Owin;
using System.Configuration;
using DAL.EF;
using RestService.Helpers;

[assembly: OwinStartup(typeof(RestService.Startup))]

namespace RestService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var config = new HttpConfiguration();

            config.Filters.Add(new ExceptionFilter());

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

            InitDb();

            app.UseWebApi(config);
        }

        private void InitDb()
        {
            using (var ctx=new ReportDbContext())
            {
                ctx.Database.Initialize(true);
            }
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