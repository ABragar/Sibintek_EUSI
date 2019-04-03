using System;
using System.Security.Claims;
using Base.Identity.OAuth2;
using Base.Security;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;

namespace Base.Identity
{
    public class TokenService
    {
        private readonly ISecureDataFormat<AuthenticationTicket> _format;
        private readonly ISystemClock _clock;
        private readonly string _identity_type = "token";
        public TokenService(TicketFormatService ticket_format_service)
        {

            _format = ticket_format_service.GetAesTicketFormat("l;sdl;sdkl;sdl;asdfkl;asasdada");
            _clock = ticket_format_service.SystemClock;
        }

        public string GetToken(ISecurityUser user)
        {
            var identity = IdentityHelper.CreateIdentity(user, _identity_type);

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());

            var date = _clock.UtcNow;

            ticket.Properties.IssuedUtc = date;
            ticket.Properties.ExpiresUtc = date.Add(TimeSpan.FromMinutes(20));

            return _format.Protect(ticket);
        }

    }
}