using Base.Security.Entities.Concrete;
using Base.UI.Wizard;

namespace Base.Security.Service
{
    public interface IAccessUserWizardService<T> : IWizardService<T>
        where T : AccessUserWizard
    {
    }
}
