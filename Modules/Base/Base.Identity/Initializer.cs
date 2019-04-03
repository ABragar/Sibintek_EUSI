

using Base.Identity.Entities;
using Base.Settings;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Identity
{

    public class Initializer : IModuleInitializer
    {
        private readonly ISettingService<AuthSettings> _settings;

        public Initializer(ISettingService<AuthSettings> settings)
        {
            _settings = settings;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<AuthSettings>()
                .Service<ISettingService<AuthSettings>>()
                .Title("Настройки входа в систему")
                .DetailView(x => x.Title("Настройки входа в систему"))
                .ListView(x => x.Title("Настройки входа в систему").HiddenActions(new[] { LvAction.Create, LvAction.Delete }));

            //  context.CreateVmConfig<ExternalLoginInfo>().Service<IExternalLoginInfoService>();
            //  context.CreateVmConfig<PasswordLoginInfo>().Service<IPasswordLoginInfoService>();

            context.CreateVmConfig<OAuthClient>();
            context.CreateVmConfig<OAuthScope>();



            context.CreateVmConfig<UserToken>()
                .Title("Пользователи - токены для авторизации")
                .DetailView(d => d
                    .Title("Токен пользователя")
                    .DefaultSettings((uofw, token, editor) =>
                    {
                        if (token == null || token.ID == 0)
                        {
                            editor.ReadOnly(r => r.User, false);
                            editor.ReadOnly(r => r.EndDate, false);
                            editor.ReadOnly(r => r.Reason, false);
                        }
                        else
                        {
                            editor.Visible(v => v.StartDate, false);
                            editor.Visible(v => v.EndDate, false);
                        }
                    }))
                .ListView(l => l.Title("Токены"));

            context.DataInitializer("Identity", "0.1", () =>
            {
                _settings.Create(context.UnitOfWork, new AuthSettings()
                {
                    Title = "Настройки входа в систему"
                });
            });
        }
    }
}