using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Owin.Security.AesDataProtectorProvider;
using Owin.Security.AesDataProtectorProvider.CrypticProviders;

namespace Base.Identity.OAuth2
{
    public class TicketFormatService
    {
        public ISystemClock SystemClock { get; } = new SystemClock();

        public ISecureDataFormat<AuthenticationTicket> GetAesTicketFormat(string key)
        {
            return new TicketDataFormat(GetAesDataProtector(key));
        }

        public IDataProtector GetAesDataProtector(string key)
        {
            var provider = new AesDataProtectorProvider(new Sha512ManagedFactory(), new Sha256ManagedFactory(), new AesManagedFactory(), key);

            return provider.Create();
        }
    }
}