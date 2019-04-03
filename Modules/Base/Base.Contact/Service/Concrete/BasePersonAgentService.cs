using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Service;

namespace Base.Contact.Service.Concrete
{
    public class BasePersonAgentService : BaseObjectService<BasePersonAgent>, IBasePersonAgentService
    {
        public BasePersonAgentService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<BasePersonAgent> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<BasePersonAgent> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Representative)
                .SaveOneObject(x => x.BaseEmployee);
        }
    }
}
