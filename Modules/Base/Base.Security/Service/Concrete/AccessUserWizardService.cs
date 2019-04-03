using Base.DAL;
using Base.Security.Entities.Concrete;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI.ViewModal;
using Base.UI.Wizard;

namespace Base.Security.Service
{
    public class AccessUserWizardService<T> : BaseWizardService<T, User>, IAccessUserWizardService<T>
        where T: AccessUserWizard
    {
        private readonly IAccessUserService _baseService;
        private readonly ILoginProvider _login_provider;

        public AccessUserWizardService(IAccessUserService baseService,
            IAccessService accessService,
            ILoginProvider loginProvider
            ) : base(baseService, accessService)
        {
            _baseService = baseService;
            _login_provider = loginProvider;
        }

        public override void OnBeforeStart(IUnitOfWork unitOfWork, ViewModelConfig config, T obj)
        {

        }

        public override void OnBeforeStepChange(IUnitOfWork unitOfWork, ViewModelConfig config, T obj)
        {

        }


        public override User Complete(IUnitOfWork unitOfWork, T obj)
        {
            var complite = obj.GetObject();

            var user = _baseService.Create(unitOfWork, complite);
            _login_provider.AttachPassword(unitOfWork, user.ID, obj.Email, obj.Password);

            return user;
        }
    }
}
