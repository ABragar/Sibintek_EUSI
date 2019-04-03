using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Nomenclature.Entities;
using Base.Nomenclature.Entities.NomenclatureType;
using Base.Nomenclature.Service.Abstract;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Service.Concrete
{
    public class MeasureConverter : IMeasureConverter
    {
        private readonly IMeasureConvertService _measureConvertService;

        public MeasureConverter(IMeasureConvertService measureConvertService)
        {
            _measureConvertService = measureConvertService;
        }

        public double GetMultiply(IUnitOfWork uow, Measure src, Measure dest)
        {
            return GetMultiply(uow, src.ID, dest.ID);
        }

        public double GetMultiply(IUnitOfWork uow, int srcID, int destID)
        {
            var conv = _measureConvertService.GetAll(uow).SingleOrDefault(x => x.SourceID == srcID && x.DestID == destID);

            if (conv != null)
            {
                return conv.Multiplier;
            }

            throw new Exception("Не удалось найти запись конвертации единицы изерения");
        }

        public IQueryable<MeasureConvert> GetMultiply(IUnitOfWork uow, TmcNomenclature nomenclature)
        {
            var conv = _measureConvertService.GetAll(uow).Where(x => x.TmcNomenclature.ID == nomenclature.ID && x.TmcNomenclature.MeasureID == x.SourceID);

            if (conv.Any())
            {
                return conv;
            }

            throw new Exception("Не удалось найти запись конвертации единицы изерения");
        }

    }
}
