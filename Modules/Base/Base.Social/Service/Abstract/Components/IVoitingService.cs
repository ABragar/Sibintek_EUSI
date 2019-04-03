using Base.DAL;
using Base.Service;
using Base.Social.Entities.Components;

namespace Base.Social.Service.Abstract.Components
{
    public interface IVoitingService: IBaseObjectService<Voiting>
    {
        void CreateOrUpdate(IUnitOfWork unitOfWork, Voiting model);
    }
}
