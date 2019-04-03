using System;
using Owin;

namespace Base.Identity.Esia
{
    public static class EsiaAuthenticationExtensions
    {
        public static IAppBuilder UseEsiaAuthentication(this IAppBuilder app, EsiaAuthenticationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            app.Use((object)typeof(EsiaAuthenticationMiddleware), app, options);
            return app;
        }
    }
}