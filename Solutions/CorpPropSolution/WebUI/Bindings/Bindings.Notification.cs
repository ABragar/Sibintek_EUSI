using Base.Notification.Service.Abstract;
using Base.Notification.Service.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class NotificationBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Notification.Initializer>();
            container.Register<INotificationService, NotificationService>();
        }
    }
}