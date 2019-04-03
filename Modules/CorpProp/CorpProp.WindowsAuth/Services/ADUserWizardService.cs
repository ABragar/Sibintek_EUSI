using Base.DAL;
using Base.Identity.Core;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI.ViewModal;
using Base.UI.Wizard;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.WindowsAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Services
{
    public class ADUserWizardService : AccessUserWizardService<ADUserWizard>
    {
        private readonly IADUserService _adUserService;
        private readonly IAccessUserService _baseService;
        private readonly IAccountManager _accountManager;

        public ADUserWizardService(IAccessUserService baseService,
            IAccessService accessService,
            ILoginProvider loginProvider,
            IADUserService adUserService,
            IAccountManager accountManager
            ) : base(baseService, accessService, loginProvider)
        {
            _adUserService = adUserService;
            _baseService = baseService;
            _accountManager = accountManager;
        }

        public override Task<User> CompleteAsync(IUnitOfWork unitOfWork, ADUserWizard obj)
        {
            return Task.Run(async () =>
            {
                var complete = obj.GetObject();
                if (complete.Profile != null && complete.Profile is SibUser && obj.Society != null)
                {
                    int sid = obj.Society.ID;
                    ((SibUser)(complete.Profile)).Society = unitOfWork.GetRepository<Society>().Filter(x => x.ID == sid).FirstOrDefault();
                }

                var user = _baseService.Create(unitOfWork, complete);
                await _accountManager.RegisterByUserIdAsync(user.ID, PrepareUserInfo(user));

                return user;
            });
        }

        private static UserInfo PrepareUserInfo(User user)
        {
            return new UserInfo() {
                Login = user.SysName,
                Email = user.Profile.GetPrimaryEmail(),
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName
            };
        }

        public override Task OnAfterStepChangeAsync(IUnitOfWork unitOfWork, ViewModelConfig config, string prevStepName, ADUserWizard obj)
        {
            return IsSelectedUserChangedAsync(unitOfWork, obj);            
        }

        private async Task IsSelectedUserChangedAsync(IUnitOfWork unitOfWork, ADUserWizard obj)
        {
            if (!string.Equals(obj.Login, obj.ADUser.Login, StringComparison.InvariantCultureIgnoreCase))
            {
                var adUser = await _adUserService.GetByLoginAsync(unitOfWork, obj.ADUser.Login);
                obj.CopyValuesToWizard(adUser);                
            }
        }
    }
}
