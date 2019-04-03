using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using NLog;
using Owin.Security.AesDataProtectorProvider;
using Owin.Security.AesDataProtectorProvider.CrypticProviders;

namespace RestService
{
    public class TicketFormatService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public ISystemClock SystemClock { get; } = new SystemClock();

        public ISecureDataFormat<AuthenticationTicket> GetAesTicketFormat(string key)
        {
            _logger.Debug("GetAesTicketFormat");
            return new TicketDataFormat(GetAesDataProtector(key));
        }

        public IDataProtector GetAesDataProtector(string key)
        {
            _logger.Debug("GetAesDataProtector");
            var provider = new AesDataProtectorProvider(new Sha512ManagedFactory(), new Sha256ManagedFactory(), new AesManagedFactory(), key);
            return provider.Create();
        }
    }
}