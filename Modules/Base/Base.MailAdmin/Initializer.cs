using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.MailAdmin.Entities;
using Base.Security;
using Base.Settings;
using Base.UI;

namespace Base.MailAdmin
{
    public class Initializer: IModuleInitializer
    {
        private readonly ISettingService<MailAdminSettings> _setting_service;

        public Initializer(ISettingService<MailAdminSettings> setting_service)
        {
            _setting_service = setting_service;
        }

        public void Init(IInitializerContext context)
        {

            context.CreateVmConfig<MailAdminSettings>()
                .Service<ISettingService<MailAdminSettings>>();

            context.DataInitializer("MailAdmin", "0.1", () =>
            {
                _setting_service.Create(context.UnitOfWork, new MailAdminSettings
                {
                    Title = "Администрирование почтового сервера"
                });
            });


            context.CreateVmConfig<MailAccount>().Title("Почтовый аккаунт");

            context.ModifyVmConfig<User>("AccessUser")
                .DetailView(dv => dv.Editors(edts => edts.Add<MailAccount>("Почта", x => x.DataType(PropertyDataType.Custom).EditorTemplate("MailAccountInfo").Title(" "))));
        }
    }
}
