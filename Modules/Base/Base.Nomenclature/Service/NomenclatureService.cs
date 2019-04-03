using System;
using Base.DAL;
using Base.Service;
using System.Linq;
using Base.Nomenclature.Entities.NomenclatureType;

namespace Base.Nomenclature.Service
{
    public interface INomenclatureService<T> : IBaseObjectService<T>
        where T : Entities.NomenclatureType.Nomenclature, new()
    {

    }

    public class NomenclatureService<T> : BaseObjectService<T>, INomenclatureService<T> where T : Entities.NomenclatureType.Nomenclature, new()
    {
        public NomenclatureService(IBaseObjectServiceFacade facade) : base(facade) { }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Image)
                .SaveOneToMany(x => x.Files, x => x.SaveOneObject(z => z.Object))
                .SaveOneObject(x => x.Measure);
        }
    }
}
