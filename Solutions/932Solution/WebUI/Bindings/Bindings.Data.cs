using Base;
using Base.DAL;
using Base.Security.Entities.Concrete;
using Base.Security.Service;
using Common.Data.Entities;
using Common.Data.Service.Concrete;
using Data;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class DataBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Data.Initializer>();


        }
    }
}