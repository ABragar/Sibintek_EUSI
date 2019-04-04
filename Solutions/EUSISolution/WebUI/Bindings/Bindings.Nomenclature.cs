using Base.Nomenclature.Entities.NomenclatureType;
using Base.Nomenclature.Service;
using Base.Nomenclature.Service.Abstract;
using Base.Nomenclature.Service.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class NomenclatureBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Nomenclature.Initializer>();
            container.Register<IOkpdHierarchyService, OkpdHierarchyService>();
            container.Register<ITmcNomenclatureService, TmcNomenclatureService>();
            container.Register<ITmcCategoryService, TmcCategoryService>();
            container.Register<INomenclatureService<ServicesNomenclature>, NomenclatureService<ServicesNomenclature>>();
            container.Register<INomenclatureService<Nomenclature>, NomenclatureService<Nomenclature>>();
            container.Register<IMeasureConvertService, MeasureConvertService>();
            container.Register<IMeasureConverter, MeasureConverter>();

            container.Register<ITmcReadCategoryService, TmcReadCategoryService>();
            container.Register<IOKPD2Service, OKPD2Service>();
        }
    }
}