using Base.Forum.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class ForumBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Forum.Initializer>();
            container.Register<IForumSectionService, ForumSectionService>();
            container.Register<IForumTopicService, ForumTopicService>();
            container.Register<IForumPostService, ForumPostService>();
        }
    }
}