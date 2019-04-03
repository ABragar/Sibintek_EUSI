using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Base.Identity.Core;
using Base.Enums;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace Base.Identity.OAuth2
{
    public class OAuthService
    {
        private readonly IOAuthStore _store;


        public OAuthService(IOAuthStore store, IAccountManager accountManager)
        {
            _store = store;

            AuthorizationServerProvider = new InternalAuthorizationServerProvider(store, accountManager);
            AccessTokenProvider = new InternalAccessTokenProvider(store);
            AuthorizationCodeProvider = new InternalAuthorizationCodeProvider(store);
            RefreshTokenProvider = new InternalRefreshTokenProvider(store);
        }

        public IOAuthAuthorizationServerProvider AuthorizationServerProvider { get; }
        public IAuthenticationTokenProvider AccessTokenProvider { get; }
        public IAuthenticationTokenProvider AuthorizationCodeProvider { get; }
        public IAuthenticationTokenProvider RefreshTokenProvider { get; }

        private class InternalAuthorizationServerProvider : IOAuthAuthorizationServerProvider
        {
            private readonly IAccountManager _accountManager;

            private readonly IOAuthStore _store;

            public InternalAuthorizationServerProvider(IOAuthStore store, IAccountManager accountManager)
            {
                _store = store;
                _accountManager = accountManager;
            }

            public Task MatchEndpoint(OAuthMatchEndpointContext context)
            {
                return Task.CompletedTask;
            }

            public async Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
            {
                var client_id = context.ClientId;
                var redirect_uri = context.RedirectUri;

                if (!string.IsNullOrWhiteSpace(client_id) &&
                    !string.IsNullOrWhiteSpace(redirect_uri))
                {
                    var client = await _store.FindClientAsync(client_id);


                    if (client != null && (string.IsNullOrEmpty(client.RedirectUri) || string.Equals(client.RedirectUri,
                                               redirect_uri, StringComparison.OrdinalIgnoreCase)))
                    {
                        context.Validated(redirect_uri);
                        return;
                    }
                }
                context.Rejected();
            }

            public async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                string client_id;
                string client_secret;


                if (context.TryGetFormCredentials(out client_id, out client_secret) &&
                    !string.IsNullOrWhiteSpace(client_id) &&
                    !string.IsNullOrWhiteSpace(client_secret))
                {
                    var client = await _store.FindClientAsync(client_id);

                    if (client != null && client_secret == client.ClientSecret)
                    {
                        context.Validated(client_id);
                        return;
                    }
                }

                context.Rejected();
            }

            public Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
            {
                if (context.AuthorizeRequest.IsAuthorizationCodeGrantType)
                    context.Validated();
                else
                    context.Rejected();

                return Task.CompletedTask;
            }

            public Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
            {
                if (context.TokenRequest.IsAuthorizationCodeGrantType ||
                    context.TokenRequest.IsRefreshTokenGrantType ||
                    context.TokenRequest.IsResourceOwnerPasswordCredentialsGrantType)
                    context.Validated();
                else
                    context.Rejected();

                return Task.CompletedTask;
            }

            public async Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
            {
                await AddScopeAsync(context.Ticket);
                context.Validated();
            }


            public async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
            {
                await AddScopeAsync(context.Ticket);
                context.Validated(context.Ticket);
            }


            public async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var result = await _accountManager.AuthenticateByPasswordAsync(context.UserName, context.Password, false);

                if (result != null && result.Status == AuthStatus.Success)
                {
                    // create identity
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.UserIdValue.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                    ticket.Properties.Dictionary.Add("client_id", context.ClientId);
                    await AddScopeAsync(ticket);
                    context.Validated(ticket);
                    return;
                }

                context.Rejected();
            }

            public Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
            {
                context.Rejected();
                return Task.CompletedTask;
            }

            public Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
            {
                context.Rejected();
                return Task.CompletedTask;
            }

            public Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
            {
                return Task.CompletedTask;
            }

            public Task TokenEndpoint(OAuthTokenEndpointContext context)
            {
                return Task.CompletedTask;
            }

            public Task AuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context)
            {
                return Task.CompletedTask;
            }

            public Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
            {
                return Task.CompletedTask;
            }

            private async Task AddScopeAsync(AuthenticationTicket ticket)
            {
                var client_id = ticket.Properties.Dictionary["client_id"];

                var scopes = await _store.GetClientScopesAsync(client_id);

                ticket.Properties.Dictionary.Add("scope", string.Join(" ", scopes));
            }
        }

        private class InternalAccessTokenProvider : IAuthenticationTokenProvider
        {
            private IOAuthStore _store;

            public InternalAccessTokenProvider(IOAuthStore store)
            {
                _store = store;
            }

            public void Create(AuthenticationTokenCreateContext context)
            {
                throw new NotSupportedException("use CreateAsync");
            }

            public Task CreateAsync(AuthenticationTokenCreateContext context)
            {
                return Task.CompletedTask;
            }

            public void Receive(AuthenticationTokenReceiveContext context)
            {
                throw new NotSupportedException("use ReceiveAsync");
            }

            public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
            {
                return Task.CompletedTask;
            }
        }

        private class InternalAuthorizationCodeProvider : IAuthenticationTokenProvider
        {
            private readonly IOAuthStore _store;

            public InternalAuthorizationCodeProvider(IOAuthStore store)
            {
                _store = store;
            }

            public void Create(AuthenticationTokenCreateContext context)
            {
                throw new NotSupportedException("use CreateAsync");
            }

            public Task CreateAsync(AuthenticationTokenCreateContext context)
            {
                context.SetToken(context.SerializeTicket());
                return Task.CompletedTask;
            }

            public void Receive(AuthenticationTokenReceiveContext context)
            {
                throw new NotSupportedException("use ReceiveAsync");
            }

            public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
            {
                context.DeserializeTicket(context.Token);
                return Task.CompletedTask;
            }
        }

        private class InternalRefreshTokenProvider : IAuthenticationTokenProvider
        {
            private readonly IOAuthStore _store;

            public InternalRefreshTokenProvider(IOAuthStore store)
            {
                _store = store;
            }

            public void Create(AuthenticationTokenCreateContext context)
            {
                throw new NotSupportedException("use CreateAsync");
            }

            public Task CreateAsync(AuthenticationTokenCreateContext context)
            {
                context.Ticket.Properties.Dictionary.Remove("scope");
                context.Ticket.Properties.ExpiresUtc = null;
                context.SetToken(context.SerializeTicket());
                return Task.CompletedTask;
            }

            public void Receive(AuthenticationTokenReceiveContext context)
            {
                throw new NotSupportedException("use ReceiveAsync");
            }

            public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
            {
                context.DeserializeTicket(context.Token);
                context.Ticket.Properties.ExpiresUtc = DateTimeOffset.MaxValue;
                return Task.CompletedTask;
            }
        }
    }
}