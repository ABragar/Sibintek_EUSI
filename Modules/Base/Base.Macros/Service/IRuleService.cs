using Base.DAL;
using Base.Service;

namespace Base.Rule
{
    public interface IRuleService<T>: IBaseObjectService<T> where T: Macros.Entities.Rules.Rule
    {
    }

    public class RuleService<T> : BaseObjectService<T>, IRuleService<T> where T : Macros.Entities.Rules.Rule
    {
        public RuleService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.Rules);
        }
    }
}
