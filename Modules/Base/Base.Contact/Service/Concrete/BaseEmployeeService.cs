using System.Linq;
using Base.Ambient;
using Base.Contact.Entities;
using Base.DAL;
using Base.Security;
using Base.Service;

namespace Base.Contact.Service.Concrete
{
    public interface IBaseEmployeeService<T> : IBaseCategorizedItemService<T> where T : BaseEmployee, new()
    {

    }

    public class BaseEmployeeService<T> : BaseCategorizedItemService<T>, IBaseEmployeeService<T> 
        where T : BaseEmployee, new()
    {
        public BaseEmployeeService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override IQueryable<T> GetAllCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = null)
        {
            var strId = HCategory.IdToString(categoryID);
            return GetAll(unitOfWork, hidden).
                Where(x => (x.Department.sys_all_parents != null && x.Department.sys_all_parents.Contains(strId)) || x.Department.ID == categoryID);
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            if (objectSaver.Src.Responsible == null)
                objectSaver.Src.Responsible =
                    unitOfWork.GetRepository<User>().Find(u => u.ID == AppContext.SecurityUser.ID);

            return base.GetForSave(unitOfWork, objectSaver)                
                .SaveOneObject(x => x.Image)
                .SaveOneObject(x => x.Responsible)
                //.SaveManyToMany(x => x.ContactInterests)
                .SaveOneToMany(x => x.Phones)
                .SaveOneToMany(x => x.Emails)
                .SaveOneObject(x => x.Department)
                .SaveOneObject(x => x.Post)
                .SaveOneToMany(x => x.Family, x => x.SaveOneObject(z => z.Relative))
                .SaveOneToMany(x => x.Agents, x => x.SaveOneObject(o => o.Representative))
                ;
        }
        
    }
}