using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Nomenclature.Entities.Category;
using Base.Service;

namespace Base.Nomenclature.Service
{
    public interface ITmcCategoryService : IBaseCategoryService<TmcCategory>
    {

    }

    public class TmcCategoryService : CategoryService<TmcCategory>, ITmcCategoryService
    {
        public TmcCategoryService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override void ChangePosition(IUnitOfWork unitOfWork, TmcCategory obj, int? posChangeID, string typePosChange)
        {            
            base.ChangePosition(unitOfWork, obj, posChangeID, typePosChange);
        }

        protected override IObjectSaver<TmcCategory> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<TmcCategory> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver);
        }
    }
}
