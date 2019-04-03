using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Settings;
using CorpProp.Services.Settings;

namespace CorpProp.Model.Settings
{
    public static class SibNotificationModel
    {
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<SibNotification>()
                .Service<ISibNotificationService>()
                .Title("Уведомление")
                .DetailView_Default()
                .ListView_Default()
                .LookupProperty(lp => lp.Text(t => t.ID))
                ;

            context.CreateVmConfig<UserNotification>()               
               .Title("Уведомление")               
               .LookupProperty(lp => lp.Text(t => t.ID))
               ;

            context.CreateVmConfig<UserNotificationTemplate>()
              .Title("Шаблон уведомлений")
              .DetailView(dv => dv
               .Editors(eds => eds.Clear()
               .Add(ed => ed.NotificationGroup)
               .Add(ed => ed.ObjectType)
               .Add(ed => ed.Title)
               .Add(ed => ed.Code)
               .Add(ed => ed.BySystem)
               .Add(ed => ed.ByEmail)
               .Add(ed => ed.Recipient)
               .Add(ed => ed.Subject)
               .Add(ed => ed.Message)
               .Add(ed => ed.Report)
               .Add<Entities.Document.FileDB>(Guid.NewGuid().ToString(), ac => ac.EditorTemplate("Sib_FileDB").Title("Загрузить"))
               .Add(ed => ed.IsHTML)
               .Add(ed => ed.HtmlBody)
               .Add(ed => ed.Description)
               ))
               .ListView(lv => lv.Columns(cols => cols.Add(col=>col.NotificationGroup, ac => ac.Order(-200))))
              .LookupProperty(lp => lp.Text(t => t.ID))
              ;
            context.CreateVmConfig<NotificationGroup>()
             .Title("Группа уведомлений")
             .LookupProperty(lp => lp.Text(t => t.Name))
             ;
        }

        public static ViewModelConfigBuilder<SibNotification> DetailView_Default(this ViewModelConfigBuilder<SibNotification> conf)
        {
            return conf
                    .DetailView(dv => dv
                        .Title("Уведомление")
                        .Editors(edt => edt
                            .Add(a => a.Mnemonic)
                        )
                    )
                ;
        }

        public static ViewModelConfigBuilder<SibNotification> ListView_Default(this ViewModelConfigBuilder<SibNotification> conf)
        {
            return conf
                    .ListView(lv => lv
                        .Title("Уведомления")
                        .Type(ListViewType.Grid)
                        .DataSource(ds => ds
                            .Filter(f => f.ItemID == null && !f.Hidden)
                        )
                    )
                ;
        }
    }
}
