using Base;
using Base.DAL;
using CorpProp.Entities.Settings;
using EUSI.Entities.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EUSI.Entities.Accounting;
using CorpProp.Entities.Import;

namespace EUSI
{
    public class NotificationInitializer
    {
        public void Seed(IUnitOfWork uow)
        {
            var gr = uow.GetRepository<NotificationGroup>().Create(
                new NotificationGroup() { Name = "Заявка на регистрацию" });

            var gr2 = uow.GetRepository<NotificationGroup>().Create(
                new NotificationGroup() { Name = "Контроли по данным БУ" });

            var importGr = uow.GetRepository<NotificationGroup>().Find(x => x.Name == "Импорт");

            var UserNotificationTemplates = new List<UserNotificationTemplate>() {
                 new UserNotificationTemplate()
                  {
                      ObjectType = typeof(EstateRegistration).GetTypeName(),
                      Title = "Заявка выполнена",
                      Code = "EstateRegistration_Completed",
                      Subject = "Уведомление о выполнении заявки на регистрацию ОИ Заявка ЦДС: {Fields.NumberCDS} Заявка ЕУСИ: {Fields.Number}",
                      Message = "",
                      Description = "",
                      IsHTML = true,
                      HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("EUSI.Resources.Templates.EstateRegistration_Completed.html"),
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr
                  },
                  new UserNotificationTemplate()
                  {
                      ObjectType = typeof(EstateRegistration).GetTypeName(),
                      Title = "Заявка отклонена",
                      Code = "EstateRegistration_Rejected",
                      Subject = "Уведомление об отклонении заявки на регистрацию ОИ Заявка ЦДС: {Fields.NumberCDS} Заявка ЕУСИ: {Fields.Number}",
                      Message = "",
                      Description = "",
                      IsHTML = true,
                      HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("EUSI.Resources.Templates.EstateRegistration_Rejected.html"),
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr
                  },
                  new UserNotificationTemplate()
                  {
                      ObjectType = typeof(EstateRegistration).GetTypeName(),
                      Title = "Заявка на уточнении",
                      Code = "EstateRegistration_Redirected",
                      Subject = "Уведомление об уточнении заявки на регистрацию ОИ Заявка ЦДС: {Fields.NumberCDS} Заявка ЕУСИ: {Fields.Number}",
                      Message = "",
                      Description = "",
                      IsHTML = true,
                      HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("EUSI.Resources.Templates.EstateRegistration_Redirected.html"),
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr
                  },
                  new UserNotificationTemplate()
                  {
                      ObjectType = typeof(EstateRegistration).GetTypeName(),
                      Title = "Заявка создана ЦДС",
                      Code = "ER_GoodImportZDS",
                      Subject = "Уведомление о создании заявки на регистрацию ОИ Номер обращения в ЦДС: {Fields.NumberCDS} Заявка ЕУСИ: {Fields.Number}",
                      Message = "",
                      Description = "",
                      IsHTML = true,
                      HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("EUSI.Resources.Templates.EstateRegistration_Created.html"),
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr,
                      Recipient = "vnukovda@sibintek.ru"
                  },
                  new UserNotificationTemplate()
                  {
                      ObjectType = typeof(EstateRegistration).GetTypeName(),
                      Title = "Заявка ЦДС не пройдены КП",
                      Code = "ER_BadImportZDS",
                      Subject = "Уведомление об отклонении заявки на регистрацию ОИ Номер обращения в ЦДС: {Fields.NumberCDS}",
                      Message = "Шаблон в разработке",
                      Description = "",
                      IsHTML = false,
                      HtmlBody = null,
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr,
                      Recipient = "vnukovda@sibintek.ru"
                  },
                  new UserNotificationTemplate()
                  {
                      ObjectType = typeof(EstateRegistration).GetTypeName(),
                      Title = "Заявка ЦДС не пройден визуальный контроль",
                      Code = "ER_ImportVisualNotCheckZDS",
                      Subject = "Уведомление об отклонении заявки на регистрацию ОИ Номер обращения в ЦДС: {Fields.NumberCDS}",
                      Message = "Шаблон в разработке",
                      Description = "",
                      IsHTML = false,
                      HtmlBody = null,
                      BySystem = false,
                      ByEmail = true,
                      NotificationGroup = gr,
                      Recipient = "vnukovda@sibintek.ru"
                  },
                new UserNotificationTemplate()
                {
                    ObjectType = typeof(Entities.Accounting.DraftOS).GetTypeName(),
                    Title = "По данным карточкам ОС/НМА не были получены данные из БУС",
                    Code = "OS_DraftOSPassBuss_Originator",
                    Subject = "По данным карточкам ОС/НМА не были получены данные из БУС",
                    Message = "",
                    Description = "Уведомление для инициатора заявки",
                    IsHTML = true,
                    HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("EUSI.Resources.Templates.OS_DraftOSPassBuss.html"),
                    BySystem = false,
                    ByEmail = true,
                    NotificationGroup = gr2
                },
                new UserNotificationTemplate()
                {
                    ObjectType = typeof(Entities.Accounting.DraftOS).GetTypeName(),
                    Title = "По данным карточкам ОС/НМА не были получены данные из БУС",
                    Code = "OS_DraftOSPassBuss_BUS",
                    Subject = "По данным карточкам ОС/НМА не были получены данные из БУС",
                    Message = "",
                    Description = "Уведомление для ответственного БУС",
                    IsHTML = true,
                    HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("EUSI.Resources.Templates.OS_DraftOSPassBuss.html"),
                    BySystem = false,
                    ByEmail = true,
                    NotificationGroup = gr2
                },
                new UserNotificationTemplate()
                {
                    ObjectType = typeof(RentalOS).GetTypeName(),
                    Title = "Уведомление об успешном испорте ФСД(Аренда)",
                    Code = "RentalOSImportHistory_Success",
                    Subject = "Уведомление о результате импорта",
                    Message = "",
                    Description = "",
                    IsHTML = true,
                    HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("CorpProp.Resources.ImportHistory_Success.html"),
                    BySystem = false,
                    ByEmail = true,
                    NotificationGroup = importGr
                },
                new UserNotificationTemplate()
                {
                    ObjectType = typeof(RentalOS).GetTypeName(),
                    Title = "Уведомление об отмене импорта ФСД(Аренда)",
                    Code = "RentalOSImportHistory_Fail",
                    Subject = "Уведомление о результате импорта",
                    Message = "",
                    Description = "",
                    IsHTML = true,
                    HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("CorpProp.Resources.ImportHistory_Fail.html"),
                    BySystem = false,
                    ByEmail = true,
                    NotificationGroup = importGr
                },
                new UserNotificationTemplate()
                {
                    ObjectType = typeof(ImportHistory).GetTypeName(),
                    Title = "Уведомление об отмене импорта заявки на регистрацию. Номер заявки ЦДС {Fields.NumberCDS}",
                    Code = "EstateRegistrationImportHistory_Fail",
                    Subject = "Уведомление о результате импорта заявки на регистрацию. Номер заявки ЦДС {Fields.NumberCDS}",
                    Message = "",
                    Description = "",
                    IsHTML = true,
                    HtmlBody = CorpProp.DefaultData.DefaultDataHelper.ReadTextFromResource("CorpProp.Resources.ImportHistory_Fail.html"),
                    BySystem = false,
                    ByEmail = true,
                    NotificationGroup = importGr,
                    Recipient = "cds@rosneft.ru"
                }
            };
            uow.GetRepository<UserNotificationTemplate>().CreateCollection(UserNotificationTemplates);
        }
    }
}