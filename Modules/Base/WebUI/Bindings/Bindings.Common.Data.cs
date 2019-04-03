using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base.ComplexKeyObjects;
using Base.DAL.EF;
using Base.Security.Service;
using Common.Data;
using Common.Data.Entities;
using Common.Data.Service.Concrete;
using Data;
using Data.EF;
using SimpleInjector;

namespace WebUI.Bindings
{
	public class CommonDataBindings
	{
        public static void Bind(Container container)
        {
            container.Register<CommonDataInitializer>();

            container.Register(Config.BuildConfig, Lifestyle.Singleton);

            container.Register<IEntityTypeResolver,EFContextEntityTypeResolver<DataContext>>();

            container.Register(AutoMapperConfig.BuildConfig, Lifestyle.Singleton);
 
        }
    }
}