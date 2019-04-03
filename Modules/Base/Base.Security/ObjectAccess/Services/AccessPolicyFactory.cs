using System;
using Base.Security.ObjectAccess.Policies;
using Base.Service;

namespace Base.Security.ObjectAccess.Services
{
    public class AccessPolicyFactory : IAccessPolicyFactory
    {
        private readonly IServiceLocator _locator;

        public AccessPolicyFactory(IServiceLocator locator)
        {
            _locator = locator;
        }

        public IAccessPolicy GetAccessPolicy(Type type)
        {
            if (!typeof(IAccessPolicy).IsAssignableFrom(type))
                throw new ArgumentException(nameof(type));

            return _locator.GetService(type) as IAccessPolicy;
        }
    }
}