using Base.Security;
using System;

namespace Base.Ambient
{
    public interface IAppContextBootstrapper
    {
        ISecurityUser GetSecurityUser();        
        IDisposable LocalContextSecurity(Func<ISecurityUser> securityUserfunc);

        IDisposable LocalContextSecurity(ISecurityUser user);
        IDateTimeProvider GetDateTimeProvider();        
    }
}