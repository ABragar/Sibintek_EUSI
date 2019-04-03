using Base.DAL;
using Base.Service;

namespace Base
{
    public interface IExportImportObject : IService
    {
        string Export(IUnitOfWork unitOfWork, int  objID);
        void Import(IUnitOfWork unitOfWork, string obj);
    }
}
