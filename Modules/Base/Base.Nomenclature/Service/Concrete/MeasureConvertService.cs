using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Nomenclature.Entities;
using Base.Nomenclature.Service.Abstract;
using Base.Service;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Service.Concrete
{
    public class MeasureConvertService : BaseObjectService<MeasureConvert>, IMeasureConvertService
    {
        public MeasureConvertService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<MeasureConvert> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<MeasureConvert> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Source)
                .SaveOneObject(x => x.Dest)
                .SaveOneObject(x => x.TmcNomenclature);
        }


    }
}
