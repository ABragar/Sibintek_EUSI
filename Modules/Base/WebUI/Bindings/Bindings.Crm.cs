using Base;
using Base.App;
using Base.CRM.Entities;
using Base.CRM.Services.Abstract;
using Base.CRM.Services.Concrete;
using Base.CRM.UI.Presets;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class CrmBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.CRM.Initializer>();
            container.Register<IDiscountService<DiscountBase<BaseObject>>, DiscountBaseService<DiscountBase<BaseObject>>>();

            container.Register<IPresetService<SalesFunnelPreset>, PresetService<SalesFunnelPreset>>(Lifestyle.Singleton);
            container.Register<IPresetFactory<SalesFunnelPreset>, DefaultPresetFactory<SalesFunnelPreset>>(Lifestyle.Singleton);
        }
    }
}