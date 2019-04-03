using Base;
using Base.DAL;
using CorpProp.Entities.Import;
using CorpProp.Entities.Settings;

namespace CorpProp.Services.Settings
{
    public interface INotificationStrategy
    {
        UserNotificationTemplate GetNotificationTemplate(IUnitOfWork uow);

        string[] GetEmails(UserNotificationTemplate notificationTemplate, int? userID, string email);

        void Init(ImportHistory item);

        void Init(string templateCode);
    }
}