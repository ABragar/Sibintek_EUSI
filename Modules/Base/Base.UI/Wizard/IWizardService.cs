using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.UI.ViewModal;

namespace Base.UI.Wizard
{

    public interface IWizardService<T> : IService
        where T: IWizardObject
    {
        Task<T> StartAsync(IUnitOfWork unitOfWork, T wizardObj, ViewModelConfig config);
        Task<T> NextStepAsync(IUnitOfWork unitOfWork, T obj, ViewModelConfig config);
        Task<T> PrevStepAsync(IUnitOfWork unitOfWork, T obj, ViewModelConfig config);
        Task<IBaseObject> CompleteAsync(IUnitOfWork unitOfWork, T obj);
    }

}
