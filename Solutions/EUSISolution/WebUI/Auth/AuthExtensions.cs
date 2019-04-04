using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Duke.Owin.VkontakteMiddleware;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Base.Identity;
using Base.Identity.Esia;
using Base.Identity.OAuth2;
using ImageResizer.Configuration.Xml;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.OAuth;
using Base.Service;
using Base.Identity.Core;
using WebUI.Helpers;
using Base.Service.Log;

namespace WebUI.Auth
{
    public static class AuthExtensions
    {


        public static void UseAuth(this IAppBuilder app, OAuthService auth_service, TicketFormatService ticket_format_service, AuthSettingsSection auth_settings)
        {



            //TODO 
            app.UseCors(CorsOptions.AllowAll);

            
            app.UseCookieAuth();

            app.UseExternalAuthentication(auth_settings.ExternalAuth);

            var server = new OAuthAuthorizationServerOptions()
            {
                AccessTokenFormat = ticket_format_service.GetAesTicketFormat(auth_settings.AccessTokenKey.Local),
                Provider = auth_service.AuthorizationServerProvider,
                AccessTokenProvider = auth_service.AccessTokenProvider,
                RefreshTokenProvider = auth_service.RefreshTokenProvider,
                AuthorizationCodeProvider = auth_service.AuthorizationCodeProvider,
                TokenEndpointPath = new PathString("/account/token"),
                AuthorizeEndpointPath = new PathString("/account/authorize"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                AuthorizationCodeExpireTimeSpan = TimeSpan.FromMinutes(5),
                SystemClock = ticket_format_service.SystemClock,
                AllowInsecureHttp = true, //TODO
            };


            app.UseOAuthAuthorizationServer(server);


            //Local Bearer
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                Provider = new CustomOAuthBearerAuthenticationProvider(),
                AccessTokenProvider = server.AccessTokenProvider,
                AccessTokenFormat = server.AccessTokenFormat,
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = LocalBearer,
                SystemClock = server.SystemClock,
            });


            //External Bearer
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                AccessTokenProvider = server.AccessTokenProvider,
                AccessTokenFormat = ticket_format_service.GetAesTicketFormat(auth_settings.AccessTokenKey.External),
                AuthenticationMode = AuthenticationMode.Passive,
                AuthenticationType = ExternalBearer,
                SystemClock = server.SystemClock,
            });


            app.UseStageMarker(PipelineStage.Authorize);

        }

        public static string ScopeClaimType = "oauth2::bearer::scope";

        public static void UseCookieAuth(this IAppBuilder app)
        {
            // Cookie Authentication
            var options = new CookieAuthenticationOptions
            {
                CookieName = Constants.AppName + "Cookie",
                AuthenticationType = DefaultType,
                AuthenticationMode = AuthenticationMode.Active,
                LoginPath = new PathString("/account/login"),
                Provider = new CookieAuthenticationProvider()
                {
                    OnApplyRedirect = x =>
                    {
                        if (x.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                            x.Response.Redirect(x.RedirectUri);
                    }
                }
            };

            app.UseCookieAuthentication(options);

        }

        private static T With<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }


        internal static void UseExternalAuthentication(this IAppBuilder app, AuthSettingsSection.ExternalAuthElement settings)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            if (settings.Google.Enabled)
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = settings.Google.ClientId,
                    ClientSecret = settings.Google.ClientSecret
                }.With(x =>
                {
                    if (!string.IsNullOrEmpty(settings.Google.RedirectUrl))
                    {
                            x.CallbackPath = new PathString(settings.Google.RedirectUrl);
                    }
                }));
            }

            if (settings.Vkontakte.Enabled)
            {
                app.UseVkontakteAuthentication(new VkAuthenticationOptions
                {

                    AppId = settings.Vkontakte.AppId,
                    AppSecret = settings.Vkontakte.AppSecret,
                    Scope = "email" // For example "email,audio" More info here http://vk.com/dev/permissions
                }.With(x =>
                {
                    if (!string.IsNullOrEmpty(settings.Vkontakte.RedirectUrl))
                    {
                        x.CallbackPath = new PathString(settings.Vkontakte.RedirectUrl);
                    }
                }));



            }

            if (settings.Facebook.Enabled)
            {
                app.UseFacebookAuthentication(new FacebookAuthenticationOptions
                {
                    AppId = settings.Facebook.AppId,
                    AppSecret = settings.Facebook.AppSecret
                }.With(x =>
                {
                    if (!string.IsNullOrEmpty(settings.Facebook.RedirectUrl))
                    {
                        x.CallbackPath = new PathString(settings.Facebook.RedirectUrl);
                    }
                }));
            }

            if (settings.Twitter.Enabled)
            {
                app.UseTwitterAuthentication(new TwitterAuthenticationOptions
                {
                    ConsumerKey = settings.Twitter.ConsumerKey,
                    ConsumerSecret = settings.Twitter.ConsumerSecret
                }.With(x =>
                {
                    if (!string.IsNullOrEmpty(settings.Twitter.RedirectUrl))
                    {
                        x.CallbackPath = new PathString(settings.Twitter.RedirectUrl);
                    }
                }));
            }

            if (settings.Esia.Enabled)
            {
                app.UseEsiaAuthentication(new EsiaAuthenticationOptions()
                {
                    //test
                    IdpSSOTargetURL = "https://esia-portal1.test.gosuslugi.ru/idp/profile/SAML2/Redirect/SSO",

                    Issuer = settings.Esia.Issuer,
                    PFXFileName = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory")?.ToString(), settings.Esia.PFXFileName),
                    PFXPassword = settings.Esia.PFXPassword

                }.With(x =>
                {
                    if (!string.IsNullOrEmpty(settings.Esia.RedirectUrl))
                    {
                        x.CallbackPath = new PathString(settings.Esia.RedirectUrl);
                    }
                }));
            }
        }

        public const string LocalBearer = "LocalBearer";

        public const string ExternalBearer = "ExternalBearer";

        public const string DefaultType = CookieAuthenticationDefaults.AuthenticationType;

        public static IAuthenticationManager GetAuthentification(this HttpContextBase context)
        {
            return context.GetOwinContext().Authentication;
        }

        public static void SignIn(this HttpContextBase context, int user_id, string login, bool rememberMe = false)
        {
            context.GetAuthentification().SignIn(
                new AuthenticationProperties() { IsPersistent = rememberMe },
                IdentityHelper.CreateIdentity(user_id, login, DefaultType)
            );
        }

        public static int? GetUserId(this HttpContextBase context)
        {
            var identity = context.GetAuthentification().User?.Identity;
            return identity?.IsAuthenticated == true ? identity.GetUserId<int>() : (int?)null;
        }

        public static void SignOut(this HttpContextBase context)
        {
            context.GetAuthentification().SignOut(DefaultType);
        }




        static string[] Empty = new string[0];
        public static string[] GetScopes(string scopes)
        {
            if (string.IsNullOrWhiteSpace(scopes))
                return Empty;

            return scopes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


        }
    }

    public class CustomOAuthBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            if (!String.IsNullOrEmpty(context.Token))
            {
                context.OwinContext.Authentication.User = null;
            }
            return Task.CompletedTask;
        }



        public override Task ValidateIdentity(OAuthValidateIdentityContext context)
        {

            if (context.Ticket != null)
            {
                string scope;

                if (context.Ticket.Properties.Dictionary.TryGetValue("scope", out scope))
                {

                    context.Ticket.Identity.AddClaims(AuthExtensions.GetScopes(scope).Select(x => new Claim(AuthExtensions.ScopeClaimType, x)));

                }
                context.Validated();

            }


            return Task.CompletedTask;
        }


    }
}