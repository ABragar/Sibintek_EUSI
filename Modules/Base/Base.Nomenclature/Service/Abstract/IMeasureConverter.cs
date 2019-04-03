using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Nomenclature.Entities;
using Base.Nomenclature.Entities.NomenclatureType;
using Base.Service;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Service.Abstract
{
    public interface IMeasureConverter
    {
        IQueryable<MeasureConvert> GetMultiply(IUnitOfWork uow, TmcNomenclature nomenclature);
    }
}
