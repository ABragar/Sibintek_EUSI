using bbase = Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.WindowsAuth.Entities;
using CorpProp.WindowsAuth.Services;
using Base.Security.Service;
using Base.UI;
using Base.UI.ViewModal;

namespace CorpProp.WindowsAuth
{
    public class Initializer : bbase.IModuleInitializer
    {
        public void Init(bbase.IInitializerContext context)
        {
            #region Active Directory User Import

            #region SelectActiveDirectoryUser
            context.CreateVmConfig<ADUser>("SelectActiveDirectoryUser")
                .Service<IADUserService>()
                .LookupProperty(x => x.Text(e => e.Login))
                .Title("Выбор пользователя из Active Directory")
                .IsReadOnly()
                .ListView(x => x.Title("Выбор пользователя из Active Directory")
                    .Columns(col => col
                        .Add(c => c.Login, ac => ac.Visible(true))
                        .Add(c => c.FullName, ac => ac.Visible(true))
                        )
                    .HiddenActions(new[] { LvAction.Create, LvAction.Edit, LvAction.Delete }));
            #endregion

            #region Import Active Directory User Wizard
            context.CreateVmConfig<ADUserWizard>()
                .Service<IAccessUserWizardService<ADUserWizard>>()
                .Title("Мастер - Импорт пользователя из Active Directory")
                .WizzardDetailView(w => w.Steps(steps => {
                    steps
                        .Add("first", s => s.StepProperties(sps => sps
                            .Add(sp => sp.ADUser)))
                        .Add("second", s => s.StepProperties(sps => sps
                           .Add(p => p.Login)
                           .Add(p => p.IsActive)
                           .Add(p => p.LastName)
                           .Add(p => p.FirstName)
                           .Add(p => p.MiddleName)
                           .Add(p => p.Society)))
                        .Add("third", s => s.StepProperties(sps => sps
                           .Add(p => p.Department)
                           .Add(p => p.Post)
                           .Add(p => p.Email)
                           .Add(p => p.Phone)
                           .Add(p => p.Mobile)
                           .Add(p => p.Description)
                           ));

                })
                .Editors(e => e
                    .Add(x => x.ADUser, ed => ed
                        .DataType(bbase.Attributes.PropertyDataType.Custom)
                        .Mnemonic("SelectActiveDirectoryUser"))));
            #endregion
            #endregion
        }
    }
}
