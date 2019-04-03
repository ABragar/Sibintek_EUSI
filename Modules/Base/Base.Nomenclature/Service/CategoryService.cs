using Base.DAL;
using Base.Service;
using System.Linq;
using System;
using Base.Nomenclature.Entities.Category;

namespace Base.Nomenclature.Service
{
    public class CategoryService<T> : BaseCategoryService<T> where T : BaseNomCategory, new()
    {
        public CategoryService(IBaseObjectServiceFacade facade) : base(facade) { }
    }
}
