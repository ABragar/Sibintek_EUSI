using Base.DAL;
using Base.Map.Entities;
using Base.Service;

namespace Base.Map.Services
{
    public interface IMapLayerConfigService: IBaseCategoryService<MapLayerConfig>
    {
        MapLayerConfig GetByLayerId(IUnitOfWork unit_of_work,string layer_id);
    }
}