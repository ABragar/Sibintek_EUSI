using System.Linq;
using Base.DAL;
using Base.Map.Entities;
using Base.Service;

namespace Base.Map.Services
{
    public class MapLayerConfigService : BaseCategoryService<MapLayerConfig>,IMapLayerConfigService
    {
        public MapLayerConfigService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        public MapLayerConfig GetByLayerId(IUnitOfWork unit_of_work,string layer_id)
        {
            //TODO добавить быстрый поиск
            return GetAll(unit_of_work).Where(x => x.LayerId == layer_id).Single();
        }
    }
}