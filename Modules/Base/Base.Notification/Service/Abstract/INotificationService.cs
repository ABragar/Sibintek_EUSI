using System.Collections.Generic;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;

namespace Base.Notification.Service.Abstract
{
    public interface INotificationService : IBaseObjectService<Entities.Notification>, IReadOnly, ICreateNotification
    {
        Task<int> MarkAsRead(IEnumerable<int> ids);
    }
}
