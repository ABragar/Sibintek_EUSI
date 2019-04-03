using System.Collections.Generic;
using System.Threading.Tasks;
using Base.DAL;
using Base.Entities.Complex;

namespace Base.Service
{
    public interface IMailQueueService
    {
        void AddToQueue(IUnitOfWork uow, string sourse, string title, LinkBaseObject obj, string emailTo, bool save = true);
        Task<int> RemoveFromQueue(IUnitOfWork uow, IEnumerable<string> sourse);
        int SendQueue();
    }
}