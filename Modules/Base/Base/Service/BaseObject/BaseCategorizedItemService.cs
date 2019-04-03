using System.Linq;
using Base.DAL;
using System.Linq.Dynamic;

namespace Base.Service
{
    public class BaseCategorizedItemService<T> : BaseObjectService<T>, IBaseCategorizedItemService<T>
        where T : BaseObject, ICategorizedItem
    {

        public BaseCategorizedItemService(IBaseObjectServiceFacade facade) : base(facade) { }

        public virtual IQueryable<T> GetCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = false)
        {
            var q = this.GetAll(unitOfWork, hidden);
            if (categoryID == 0)
                return q;
            else
                return q.Where(a => a.CategoryID == categoryID);
        }

        public virtual IQueryable<T> GetAllCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = false)
        {
            var strId = HCategory.IdToString(categoryID);

            return
                this.GetAll(unitOfWork, hidden)
                    .Where($"(it.Category_.sys_all_parents != null and it.Category_.sys_all_parents.Contains(\"{strId}\")) or it.Category_.ID = {categoryID}");
        }

        public virtual void ChangeCategory(IUnitOfWork unitOfWork, int id, int newCategoryID)
        {
            var obj = this.Get(unitOfWork, id);

            if (obj == null) return;

            obj.CategoryID = newCategoryID;

            this.Update(unitOfWork, obj);
        }
    }
}
