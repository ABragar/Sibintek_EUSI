using Base.Document.Entities;
using Base.Service;

namespace Base.Document.Services.Abstract
{
    public interface IBaseDocumentService<T>: IBaseObjectService<T>
        where T: BaseDocument
    {   
    }
}