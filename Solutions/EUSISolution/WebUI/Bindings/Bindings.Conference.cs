using Base.Conference.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class ConferenceBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Conference.Initializer>();
            container.Register<IConferenceService, ConferenceService>();
            container.Register<IConferenceMessageService, ConferenceMessageService>();
            container.Register<IPrivateMessageService, PrivateMessageService>();
            container.Register<IPublicMessageService, PublicMessageService>();
        }
    }
}