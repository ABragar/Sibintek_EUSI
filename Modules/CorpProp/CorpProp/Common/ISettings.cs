using Base.DAL;
using Base.Service;
using CorpProp.Entities.Settings;
using System.Collections.Generic;

namespace CorpProp.Common
{
    public interface ISibNotification : IService
    {
        List<SibNotificationObject> PrepareLinkedObject(IUnitOfWork unitOfWork, SibNotification notification);
    }
}
