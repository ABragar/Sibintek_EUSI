using Base.Project.Entities;
using Base.Project.Service.Abstract;
using Base.Project.Service.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class ProjectsBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Project.Initializer>();
            container.Register<IProjectService, ProjectService>();
        }
    }
}