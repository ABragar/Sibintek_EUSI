using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.Service;
using SimpleInjector;
using WebUI.SimpleInjector;

namespace WebUI.Bindings
{
    public class Bindings
    {
        private static readonly Lazy<Container> _container = new Lazy<Container>(CreateContainer);

        public static Container CreateContainer()
        {
            var container = new Container();

            var lifestyle = new HttpContextHybridScopeLifestyle();

            container.Options.DefaultScopedLifestyle = lifestyle;

            container.RegisterSingleton<IExecutionContextScopeManager>(new ExecutionContextScopeManager(container, lifestyle));

            Bind(container);
            
            container.Options.SuppressLifestyleMismatchVerification = true;
            return container;
        }


        public static Container GetContainer()
        {
            return _container.Value;
        }

        public static IReadOnlyCollection<IModuleInitializer> GetModuleInitializers(Container container)
        {
            return container.GetCurrentRegistrations().Where(x => typeof(IModuleInitializer).IsAssignableFrom(x.ServiceType))
                .Select(x => (IModuleInitializer)x.GetInstance())
                .ToArray();
        }

        private static void Bind(Container container)
        {
            BaseBindings.Bind(container);
            AccessBindings.Bind(container);
            AppBindings.Bind(container);
            SecurityBindings.Bind(container);
            UiBindings.Bind(container);
            WebUIBindings.Bind(container);
            UtilsBindings.Bind(container);
            WordBindings.Bind(container);
            FileStorageBindings.Bind(container);
            TaskBindings.Bind(container);
            BusinessProcessesBindings.Bind(container);
            NotificationBindings.Bind(container);
            ContentBindings.Bind(container);
            AuditBindings.Bind(container);
            EventBindings.Bind(container);
            HelpBindings.Bind(container);
            SupportBindings.Bind(container);
            CensorshipBindings.Bind(container);
            MailBindings.Bind(container);
            MultimediaBindings.Bind(container);
            ConferenceBindings.Bind(container);
            DocumentBindings.Bind(container);
            ContractorBindings.Bind(container);
            HangFireBindings.Bind(container);
            CrmBindings.Bind(container);
            IdentityBindings.Bind(container);
            MapBindings.Bind(container);
            LinksBindings.Bind(container);
            MacrosBindings.Bind(container);
            DataTestBindings.Bind(container);
            ExportImportBinding.Bind(container);
            ReportingBindings.Bind(container);

            CommonDataBindings.Bind(container);

            DataBindings.Bind(container);
            BaseCatalogBinding.Bind(container);
            NomenclatureBindings.Bind(container);
            SocialBindings.Bind(container);
            WebApiBindings.Bind(container);
            ProjectsBindings.Bind(container);
            CorpPropBindings.Bind(container);
            //доп.модули
            //ForumBindings.Bind(container);
            //NomenclatureBindings.Bind(container);
        }
    }
}