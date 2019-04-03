using System;
using System.Linq;
using Base.Attributes;
using Base.Security.Entities.Concrete;
using Base.Security.Service;
using Base.Service;
using Base.UI;

namespace Base.Security
{
    public class Initializer : IModuleInitializer
    {


        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<User>()
                .Service<IUserService<User>>()
                .Title("Пользователи - Общие")
                .DetailView(x => x
                    .Title("Пользователь")
                    .HideToolbar(true)
                    .Width(550)
                    .Height(700))
                .ListView(x => x
                    .Title("Пользователи")
                    .ListViewCategorizedItem(f => f.HiddenTree(true)))
                .LookupProperty(x => x
                    .Text(e => e.FullName)
                    .Image(e => e.Image))
                .Preview(a => a.Select((uofw, user) =>
                {
                    var imageId = Guid.Empty;
                    string fullName = "";

                    if (user.BaseProfileID != null)
                    {
                        var profile =
                            uofw.GetRepository<BaseProfile>().All()
                                .Where(x => x.ID == user.BaseProfileID.Value)
                                .Select(x => new { x.FullName, ImageId = x.Image != null ? x.Image.FileID : Guid.Empty })
                                .Single();

                        imageId = profile.ImageId;
                        fullName = profile.FullName;
                    }

                    return new
                    {
                        user.ID,
                        FullName = fullName,
                        Image = new FileData()
                        {
                            FileID = imageId
                        },
                    };
                }).Fields(x => x.Add(u => u.ID).Add(u => u.Image).Add(u => u.FullName)));

            context.CreateVmConfig<User>("AccessUserForm")
                .Service<IAccessUserService>()
                .LookupProperty(x => x.Text(e => e.FullName))
                .Title("Пользователи - Безопасность")
                .DetailView(x => x
                    .Title("Пользователь")
                    .Wizard("AccessUserWizard").Editors(ed =>
                    {
                        ed.Add<ProfileInfo>("ProfileInfo", e => e.DataType(PropertyDataType.Custom).Title(" "));
                        ed.Add<AuthenticationInfo>("AuthenticationInfo", e => e.DataType(PropertyDataType.Custom).Title(" "));
                    }))
                .ListView(x => x
                    .Title("Пользователи"));

            context.CreateVmConfig<User>("AccessUserAD")
                .Service<IAccessUserService>()
                .LookupProperty(x => x.Text(e => e.FullName))
                .Title("Пользователи - Безопасность")
                .DetailView(x => x
                    .Title("Пользователь")
                    .Wizard("AccessUserWizard").Editors(ed =>
                    {
                        ed.Add<ProfileInfo>("ProfileInfo", e => e.DataType(PropertyDataType.Custom).Title(" "));
                        ed.Add<AuthenticationInfo>("AuthenticationInfo", e => e.DataType(PropertyDataType.Custom).Title(" "));
                    }))
                .ListView(x => x
                    .Title("Пользователи"));

            context.CreateVmConfig<AccessUserWizard>()
                .Service<IAccessUserWizardService<AccessUserWizard>>()
                .Title("Мастер - создания пользователя")
                .WizzardDetailView(w => w.Steps(steps =>
                {
                    steps.Add("first", s => s.StepProperties(prs => prs
                            .Add(p => p.Image)
                            .Add(p => p.LastName)
                            .Add(p => p.FirstName)
                            .Add(p => p.MiddleName)
                            .Add(p => p.Gender)
                            .Add(p => p.Email)
                            .Add(p => p.Password)
                    ));
                }).HasSummary(true))
                .DetailView(x => x.Title("Пользователь"));

            context.CreateVmConfig<UserCategory>()
                .Service<IUserCategoryService>()
                .Title("Пользователи - Группы")
                .DetailView(x => x.Title("Группа"))
                .ListView(x => x.Title("Группы"));

            context.CreateVmConfig<Role>()
                .Service<IRoleService>()
                .Title("Роли")
                .DetailView(x => x.Title("Роль"))
                .ListView(x => x.Title("Роли"));

            context.CreateVmConfig<BaseProfile>()
               .Title("Профиль - Базовый");

            context.CreateVmConfig<SimpleProfile>()
                .Service<ICrudProfileService<SimpleProfile>>()
                .Title("Профиль - Простой")
                .DetailView(x => x.Title("Пользователь"));


            context.CreateVmConfig<ProfilePhone>()
                .Title("Профиль - Телефоны")
                .DetailView(x => x.Title("Телефон"));

            context.CreateVmConfig<ProfileEmail>()
                .Title("Профиль - Электронная почта")
                .DetailView(x => x.Title("Электронная почта"));



            context.CreateVmConfig<PropertyPermission>()
                .Title("Разрешение на поле")
                .ListView(lv => lv.Title("Разрешения на поля"))
                .DetailView(dv => dv.Title("Разрешение на поле"));

        }
    }
}