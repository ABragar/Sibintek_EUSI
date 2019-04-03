using Base.Contact.Entities;
using Base.Service;

namespace Base.Contact.Service.Abstract
{
    public interface IBaseContactInterestService<T> : IBaseObjectService<T> where T: ContactInterest
    {
    }
}
