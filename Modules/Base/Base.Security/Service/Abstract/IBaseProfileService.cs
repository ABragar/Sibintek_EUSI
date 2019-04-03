using Base.DAL;
using Base.Service.Crud;

namespace Base.Security.Service
{
    public interface IBaseProfileService
    {
        BaseProfile CreateDefault(IUnitOfWork unitOfWork, string type);
        BaseProfile CreateEmptyProfile(IUnitOfWork unitOfWork, string type);
        BaseProfile CreateProfile(IUnitOfWork unitOfWork, BaseProfile profile, string type);
        BaseProfile UpdateProfile(IUnitOfWork unitOfWork, BaseProfile profile, string type);
        string GetProfileType(IUnitOfWork unitOfWork, int categoryId);
        IBaseObjectCrudService GetService(string type);
        void DeleteProfile(IUnitOfWork unitOfWork, BaseProfile profile, string type);
        bool ProfileComplite(IUnitOfWork unitOfWork, int id);
    }
}
