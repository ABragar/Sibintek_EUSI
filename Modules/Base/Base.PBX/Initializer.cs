using Base.Attributes;
using Base.PBX.Entities;
using Base.PBX.Models;
using Base.PBX.Services.Abstract;
using Base.Security;
using Base.Service;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.PBX
{
    public class Initializer : IModuleInitializer
    {
        public Initializer()
        {

        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<PBXServer>()
                .Title("PBX Сервера")
                .Service<IBaseObjectService<PBXServer>>()
                .DetailView(d => d.Title("PBX Сервер")
                    .Toolbar(t =>
                        {
                            t.Add("GetPBXServerToolbar", "PBX", a => a.ListParams(p => p.Add("serverID", "[ID]")));
                        }
                    ))
                .ListView(l => l.Title("PBX Сервера"));

            context.CreateVmConfig<SIPAccount>()
                .Title("SIP Аккаунты")
                .Service<ISIPAccountService>()
                .LookupProperty(l => l.Text(p => p.User.FullName))
                .DetailView(d => d.Title("SIP Аккаунт"))
                .ListView(l => l.Title("SIP Аккаунты"));

            context.CreateVmConfig<PBXUser>()
                .Title("PBX Пользователь")
                .DetailView(d => d.Title("PBX Пользователь"))
                .ListView(l => l.Title("PBX Пользователи"));

            //context.CreateVmConfig<SIPUserWizard>()
            //    .Service<ISIPUserWizardService>()
            //    .Title("Мастер - создания пользователя")
            //    .WizzardDetailView(w => w.Steps(steps =>
            //    {
            //        steps.Add("access", s => s.StepProperties(prs => prs
            //            .Add(p => p.Image)
            //            .Add(p => p.LastName)
            //            .Add(p => p.FirstName)
            //            .Add(p => p.MiddleName)
            //            .Add(p => p.Gender)
            //            .Add(p => p.Email)
            //            .Add(p => p.Password)));
            //        steps.Add("wideprofile", s => s.StepProperties(prs => prs
            //            .Add(p => p.CreateSIPAccount)));
            //        steps.Add("sipserver", s => s.StepProperties(prs => prs
            //            .Add(p => p.PBXServer)).Rule(r => !r.CreateSIPAccount ? StepRuleResult.End : StepRuleResult.Ok));
            //        steps.Add("sipaccount", s => s.StepProperties(prs => prs
            //            .Add(p => p.extension)
            //            .Add(p => p.cidnumber)
            //            .Add(p => p.user_password)
            //            .Add(p => p.secret)
            //            .Add(p => p.vmsecret)
            //            .Add(p => p.auto_record)
            //            .Add(p => p.enable_webrtc)
            //            .Add(p => p.hasvoicemail)
            //            .Add(p => p.email_to_user)));
            //    }))
            //    .DetailView(x => x.Title("Пользователь"));

            context.ModifyVmConfig<User>("AccessUser")
                .DetailView(dv => dv.Editors(edts => edts.Add<SIPAccount>("SIPAccount", x=> x.DataType(PropertyDataType.Custom).EditorTemplate("SIPAccountInfo").Title(" "))));
        }
    }
}