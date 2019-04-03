using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Nomenclature.Entities.Category;
using Base.Nomenclature.Entities.NomenclatureType;
using Base.Nomenclature.Service.Abstract;
using Base.Service;

namespace Base.Nomenclature.Service.Concrete
{
    public interface ITmcReadCategoryService : IBaseCategoryService<TmcCategory>
    {
    }

    public class TmcReadCategoryService : CategoryService<TmcCategory>, ITmcReadCategoryService
    {
        private readonly ITmcCategoryService _tmcCategoryService;

        public TmcReadCategoryService(IBaseObjectServiceFacade facade, ITmcCategoryService tmcCategoryService) : base(facade)
        {
            _tmcCategoryService = tmcCategoryService;
        }

        public override IQueryable<TmcCategory> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            var taskCategories = unitOfWork.GetRepository<TmcNomenclature>().All()
                .Select(x => x.Category_).Distinct().Select(x => new
                {
                    x.ID,
                    x.ParentID,
                    Parents = x.sys_all_parents,
                });

            var ids = new List<int>();

            foreach (var category in taskCategories)
            {
                if (category.ParentID != null)
                    ids.AddRange(category.Parents.Split(HCategory.Seperator).Select(HCategory.IdToInt));

                ids.Add(category.ID);
            }

            return _tmcCategoryService.GetAll(unitOfWork, hidden).Where(x => ids.Contains(x.ID));

        }

    }
}
