using System.Linq;
using Base.DAL;
using Base.Service;
using Base.Social.Entities.Components;
using Base.Social.Service.Abstract.Components;

namespace Base.Social.Service.Concrete.Components
{
    public class VoitingService : BaseObjectService<Voiting>, IVoitingService
    {
        public VoitingService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public void CreateOrUpdate(IUnitOfWork unitOfWork, Voiting model)
        {
            var voiting = GetAll(unitOfWork).FirstOrDefault(x => x.Type == model.Type && x.TypeId == model.TypeId && x.UserId == model.UserId);

            if (voiting != null)
            {
                voiting.Thumb = model.Thumb;
                Update(unitOfWork, voiting);
            }
            else
            {
                Create(unitOfWork, model);
            }
        }
    }
}
