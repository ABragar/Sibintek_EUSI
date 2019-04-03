using Base.Service;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;

namespace CorpProp.Services.NSI
{

    public interface ISocietyDeptHierarhyService : IBaseCategoryService<SocietyDept>
    {

    }

    public class SocietyDeptHierarhyService : BaseCategoryService<SocietyDept>, ISocietyDeptHierarhyService
    {
        public SocietyDeptHierarhyService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<SocietyDept> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SocietyDept> objectSaver)
        {
            if (objectSaver != null && objectSaver.Src != null)
            {
                objectSaver.Src.Title = objectSaver.Src.Code + " " + objectSaver.Src.Name;
                objectSaver.Dest.Title = objectSaver.Src.Title;
            }

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Society);
        }

    }
}
