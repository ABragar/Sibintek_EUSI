using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class BaseCatalogBinding
    {
        public static void Bind(Container container)
        {
            container.Register<BaseCatalog.Initializer>();
        }
    }
}