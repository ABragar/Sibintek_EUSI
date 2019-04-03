using Base;
using Base.Ambient;
using Base.Security;
using Base.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Request.ResponseCells;
using Data;
using WebUI.Service;

namespace WebUI.Concrete
{
    public class Initializer
    {
        public static void Run(IServiceLocator locator, IReadOnlyCollection<IModuleInitializer> initializers)
        {
            var appContextBootstrapper = locator.GetService<IAppContextBootstrapper>();

            using (appContextBootstrapper.LocalContextSecurity(new SystemUser()))
            {
                using (var appInitializer = locator.GetService<IApplicationInitializer>())
                {
                    appInitializer.Init(initializers);                    
                    //locator.GetService<IAggregateColumnsByConfig>().AggregateAll(AggregateType.Sum, type => typeof(CorpProp.Initializer).Assembly.GetType(type.FullName)!=null);

                    new WarmUp(locator).WarmUpRepoByTypes(new []
                                                         {
                                                             typeof(CorpProp.Entities.Request.Response),
                                                             typeof(CorpProp.Entities.Request.Request),
                                                             //typeof(CorpProp.Entities.NSI.RequestStatus),
                                                             typeof(Base.Notification.Entities.Notification)
                                                         }, WarmUp.WarmUpType.Find);
                }
            }
        }
    }
}