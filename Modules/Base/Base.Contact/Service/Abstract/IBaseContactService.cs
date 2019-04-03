using Base.Service;

namespace Base.Contact.Service.Abstract
{
    public interface IBaseContactService<T> : IBaseObjectService<T>
        where T : Entities.BaseContact
    {

    }
}
