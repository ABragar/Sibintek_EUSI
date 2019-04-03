using System.Collections.Generic;
using Base;
using Base.Entities;
using Base.Links.Entities;
using Base.Reporting;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Settings;
using Base.Task.Entities;
using Base.UI;
using Base.UI.Service;
using Base.UI.ViewModal;
using Common.Data.Entities;
using Common.Data.Entities.Test;
using Data;
using Base.Security.Entities.Concrete;
using Base.Attributes;

namespace Common.Data
{
    public class CommonDataInitializer : IModuleInitializer
    {
        private readonly IPresetRegistorService _presetRegistorService;
        private readonly ISettingService<AppSetting> _appSettingService;
        private readonly ISettingService<ReportingSetting> _reportingSettingService;
        private readonly ILoginProvider _loginProvider;
        private readonly ILinkBuilder _linkBuilder;
        private readonly IImageSettingService _imageSettingService;
        public CommonDataInitializer(IPresetRegistorService presetRegistorService, ISettingService<AppSetting> appSettingService,
            ILoginProvider loginProvider, ILinkBuilder linkBuilder, 
            ISettingService<ReportingSetting> reportingSettingService, IImageSettingService imageSettingService)
        {
            _presetRegistorService = presetRegistorService;
            _appSettingService = appSettingService;
            _loginProvider = loginProvider;
            _linkBuilder = linkBuilder;
            _reportingSettingService = reportingSettingService;
            _imageSettingService = imageSettingService;
        }

        public void Init(IInitializerContext context)
        {
            LinksRegister.Reg(_linkBuilder);

            context.CreateVmConfig<TestBaseProfile>()
                .Service<ICrudProfileService<TestBaseProfile>>()
                .Title("Тестовый профиль");

            //context.ModifyVmConfig<User>("AccessUser").DetailView(dv => dv.Wizard("СoncreteAccessWizard"));            
            context.ModifyVmConfig<User>("AccessUser")
               .DetailView(dv => dv.Wizard("SibUserAccessWizard")
               .Editors(ed =>
               {
                   ed.Add(pr => pr.Image, ac => ac.Visible(false));
                   ed.Add(pr => pr.FullName, ac => ac.Visible(false));
                   ed.Add(pr => pr.SysName, ac => ac.Visible(true).DataType(PropertyDataType.Text).Title("Логин"));
                   ed.Add(pr => pr.IsActive, ac => ac.Visible(true).Title("Активный"));
                   ed.Add<ProfileInfo>("ProfileInfo", e => e.DataType(PropertyDataType.Custom).Title(" "));
                   ed.Add<AuthenticationInfo>("AuthenticationInfo", e => e.DataType(PropertyDataType.Custom).Title(" ").Visible(false));
               }))
              .ListView(x => x.Title("Пользователи")
              .Columns(col => col.Add(c => c.SysName, ac => ac.Visible(true).Order(0).Title("Логин"))
                               .Add(c => c.FullName, ac => ac.Visible(true).Order(1).Title("Ф.И.О."))
                               .Add(c => c.IsActive, ac => ac.Visible(true).Order(2).Title("Активный"))
                               .Add(c => c.Image, ac => ac.Visible(false))
              ))
              .LookupProperty(x => x.Text(e => e.SysName));



            context.DataInitializer("Data", "0.1", () =>
            {
                AppSettingsInitializer.Seed(context.UnitOfWork, _appSettingService);
                
                var presets = new Dictionary<string, ICollection<PresetRegistor>>();

                ReportingSettingsInitializer.Seed(context.UnitOfWork, _reportingSettingService);
                PresetsInitializer.Seed(context.UnitOfWork, _presetRegistorService, presets);
                UsersAndRolesInitializer.Seed(context.UnitOfWork, _loginProvider, presets);
            });
        }
    }
}