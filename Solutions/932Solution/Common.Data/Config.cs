using Base.DAL;
using Base.DAL.EF;

namespace Common.Data
{
    public class CommonDataConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
        
            BaseConfig.Init<TContext>(config);
            AppConfig.Init<TContext>(config);
            UiConfig.Init<TContext>(config);
            AuditConfig.Init<TContext>(config);
            BusinessProcessesConfig.Init<TContext>(config);
            ConferenceConfig.Init<TContext>(config);
            ContentConfig.Init<TContext>(config);
            ContractorConfig.Init<TContext>(config);
            CrmConfig.Init<TContext>(config);
            DocumentConfig.Init<TContext>(config);
            EventConfig.Init<TContext>(config);
            HelpConfig.Init<TContext>(config);
            MultimediaConfig.Init<TContext>(config);
            NotificationConfig.Init<TContext>(config);
            RegistersConfig.Init<TContext>(config);
            SupportConfig.Init<TContext>(config);
            TaskConfig.Init<TContext>(config);
            WordConfig.Init<TContext>(config);
            MailConfig.Init<TContext>(config);
            IdentityConfig.Init<TContext>(config);
            CensorshipConfig.Init<TContext>(config);
            DataTestConfig.Init<TContext>(config);
            ExportImportConfig.Init<TContext>(config);
            MapConfig.Init<TContext>(config);
            ReportingConfig.Init<TContext>(config);
            BaseCatalogConfig.Init<TContext>(config);
            NomenclatureConfig.Init<TContext>(config);
            LinksConfig.Init<TContext>(config);
            SocialConfig.Init<TContext>(config);
            ProjectsConfig.Init<TContext>(config);
            AnalyzeConfig.Init<TContext>(config);
//            CorpPropConfig.Init<TContext>(config);

            CorpProp.CorpPropConfig.Init<TContext>(config);
            CorpProp.RosReestr.CorpPropRosReestrConfig.Init<TContext>(config);
        }
    }
}