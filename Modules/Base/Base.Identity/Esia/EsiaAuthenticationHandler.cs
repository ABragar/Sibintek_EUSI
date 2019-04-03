using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Esia.Model;
using Esia.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.Infrastructure;

namespace Base.Identity.Esia
{
    internal class EsiaAuthenticationHandler : AuthenticationHandler<EsiaAuthenticationOptions>
    {
        public override async Task<bool> InvokeAsync()
        {
            if (Options.CallbackPath != this.Request.Path)
                return false;

            var ticket = await AuthenticateAsync();






            if (ticket != null && ticket.Properties.RedirectUri != null)
            {
                if (ticket.Identity != null)
                    Context.Authentication.SignIn(ticket.Properties, ticket.Identity);


                Response.Redirect(ticket.Properties.RedirectUri);
            }





            return true;
        }


        private readonly EsiaRequest _request;
        private readonly PropertiesDataFormat _state_format;
        private readonly string _sign_in_type;

        
        public EsiaAuthenticationHandler(EsiaRequest request, PropertiesDataFormat state_format, string sign_in_type) 
        {
            _request = request;
            _state_format = state_format;
            _sign_in_type = sign_in_type;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {

            var form = await Request.ReadFormAsync();

            var response64 = form["SAMLResponse"];

            var relay_state = form["RelayState"];

            if (response64 == null && relay_state == null)
                return null;

            var properties = _state_format.Unprotect(relay_state);


            var response = EsiaResponse.Create(_request.Configuration, response64);

            var dictionary = response.GetAsDictionary();


            var identity = new ClaimsIdentity(dictionary.Where(x => x.Value != null).Select(x => CreateClaim("esia:" + x.Key, x.Value)), _sign_in_type);

            var esia_model = new EsiaAccountModel(dictionary);

            if (esia_model.UserId != null)
                identity.AddClaim(CreateClaim(ClaimTypes.NameIdentifier, esia_model.UserId));

            if (esia_model.PersonEmail != null)
                identity.AddClaim(CreateClaim(ClaimTypes.Email, esia_model.PersonEmail));

            if (esia_model.FirstName != null)
                identity.AddClaim(CreateClaim(ClaimTypes.Name, esia_model.FirstName));

            if (esia_model.LastName != null)
                identity.AddClaim(CreateClaim(ClaimTypes.Surname, esia_model.LastName));



            return new AuthenticationTicket(identity, properties);

        }


        private Claim CreateClaim(string key, string value)
        {
            return new Claim(key, value, ClaimValueTypes.String, this.Options.AuthenticationType);
        }

        protected override Task ApplyResponseChallengeAsync()
        {

            if (this.Response.StatusCode != 401)
                return Task.CompletedTask;

            AuthenticationResponseChallenge challenge = this.Helper.LookupChallenge(this.Options.AuthenticationType, this.Options.AuthenticationMode);
            if (challenge == null)
                return Task.CompletedTask;

            string callBackUrl = Request.Scheme + "://" + Request.Host + RequestPathBase + Options.CallbackPath;




            var relay_state = _state_format.Protect(challenge.Properties);


            var redirect_url = _request.CreateLoginUrl(callBackUrl, relay_state);

            Response.Redirect(redirect_url);

            return Task.CompletedTask;
        }
    }
}
