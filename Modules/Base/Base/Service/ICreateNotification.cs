using System.Threading.Tasks;
using Base.DAL;
using Base.Entities.Complex;
using Base.Security;

namespace Base.Service
{
    public interface ICreateNotification
    {
        void CreateNotification(IUnitOfWork unitOfWork, int[] userToId, LinkBaseObject obj, string title, string descr);
    }
}
