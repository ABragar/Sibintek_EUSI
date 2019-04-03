using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Nomenclature.Entities.Category;
using Base.Nomenclature.Entities.NomenclatureType;
using Base.Service;
using Base.Utils.Common.Maybe;


namespace Base.Nomenclature.Service
{
    public interface ITmcNomenclatureService : IBaseCategorizedItemService<TmcNomenclature>
    {
    }


    public class TmcNomenclatureService : BaseCategorizedItemService<TmcNomenclature>, ITmcNomenclatureService
    {
        public TmcNomenclatureService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<TmcNomenclature> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<TmcNomenclature> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Image)
                .SaveOneToMany(x => x.Files)
                .SaveOneObject(x => x.Measure)
                .SaveOneObject(x => x.GroupAccounting)
                .SaveOneObject(x => x.Okpd2);

        }

        public override TmcNomenclature Create(IUnitOfWork unitOfWork, TmcNomenclature obj)
        {
            try
            {
                return base.Create(unitOfWork, obj);
            }
            catch (Exception e)
            {
                throw new Exception("Номенклатурный номер не может повторяться");
            }
        }

        public override TmcNomenclature Update(IUnitOfWork unitOfWork, TmcNomenclature obj)
        {
            obj.ChangeDate = DateTime.Now;
            return base.Update(unitOfWork, obj);
        }

    }
}