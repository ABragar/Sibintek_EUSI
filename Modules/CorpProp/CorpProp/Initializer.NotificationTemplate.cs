using Base;
using Base.DAL;
using CorpProp.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp
{
    public class NotificationInitializer
    {

        public void Seed(IUnitOfWork uow)
        {

            var gr = uow.GetRepository<NotificationGroup>().Create(
                new NotificationGroup() { Name = "Импорт" });

            var UserNotificationTemplates = new List<UserNotificationTemplate>() {
                  new UserNotificationTemplate()
                  {
                      ObjectType = typeof(Entities.Import.ImportHistory).GetTypeName(),
                      Title = "Уведомление об успешном испорте",
                      Code = "ImportHistory_Success",
                      Subject = "Уведомление о результате импорта",
                      Message = "",
                      Description = "",
                      IsHTML = true,
                      HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("CorpProp.Resources.ImportHistory_Success.html"),
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr
                  },
                  new UserNotificationTemplate()
                  {
                      ObjectType = typeof(Entities.Import.ImportHistory).GetTypeName(),
                      Title = "Уведомление об отмене импорта",
                      Code = "ImportHistory_Fail",
                      Subject = "Уведомление о результате импорта",
                      Message = "",
                      Description = "",
                      IsHTML = true,
                      HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("CorpProp.Resources.ImportHistory_Fail.html"),
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr
                  }

            };
            uow.GetRepository<UserNotificationTemplate>().CreateCollection(UserNotificationTemplates);
        }
        
        
    }
}
