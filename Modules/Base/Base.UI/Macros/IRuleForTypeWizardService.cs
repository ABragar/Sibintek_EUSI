using Base.App.Macros;
using Base.Macros.Entities.Rules;
using Base.Rule;
using Base.Service;
using Base.UI.Wizard;

namespace Base.UI.Macros
{
    public interface IRuleForTypeWizardService : IWizardService<RuleForTypeWizard>
    {
    }

    public class RuleForTypeWizardService : BaseWizardService<RuleForTypeWizard, RuleForType>, IRuleForTypeWizardService
    {
        public RuleForTypeWizardService(IRuleService<RuleForType> baseService, IAccessService accessService) : base(baseService, accessService)
        {
        }
    }
}
