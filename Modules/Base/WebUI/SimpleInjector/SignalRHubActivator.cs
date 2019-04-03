using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;

namespace WebUI.SimpleInjector
{

    public class SignalRHubActivator : IHubActivator
    {

        private readonly Container _container;

        public SignalRHubActivator(Container container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {

            return (IHub)_container.GetInstance(descriptor.HubType);
        }
    }
}