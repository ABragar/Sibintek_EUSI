using Base.Service;
using Owin;

namespace WebApi
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseStandartApi(this IAppBuilder app_builder, IServiceLocator locator)
        {

            var builder = locator.GetService<HttpConfigurationBuilder>();

            var configuration = builder.Build();

            app_builder.Map("/api", x =>
            {
                x.Use(async (c, next) =>
                {
                    c.Request.Headers.Set("X-Requested-With", "XMLHttpRequest");
                    await next();
                });
                x.UseWebApi(configuration);
            });

            return app_builder;
        }

    }
}