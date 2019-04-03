using Base.Macros;
using Base.Macros.Entities.Rules;
using Base.Macros.Service;
using Base.Rule;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class MacrosBindings
    {
        public static void Bind(Container container)
        {
            container.Register<IMacrosService, MacrosService>();
            container.Register<IRuleService<RuleForType>, RuleService<RuleForType>>(Lifestyle.Singleton);
            container.Register<IRuleService<RuleForMnemonic>, RuleService<RuleForMnemonic>>(Lifestyle.Singleton);
        }
    }
}