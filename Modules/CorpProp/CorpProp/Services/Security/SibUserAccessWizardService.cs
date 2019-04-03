using Base.DAL;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI.ViewModal;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Security
{
    
    public class SibUserAccessWizardService : AccessUserWizardService<SibUserAccessWizard>
    {
        private readonly IPasswordService _passwordService;
        private readonly IAccessUserService _baseService;      
        private readonly ILoginProvider _login_provider;

        public SibUserAccessWizardService(IAccessUserService baseService,
            IAccessService accessService,
            ILoginProvider loginProvider,  
            IPasswordService passwordService
            )
            : base(baseService, accessService, loginProvider)
        {
            _baseService = baseService;
            _login_provider = loginProvider;
            _passwordService = passwordService;           
        }

        public override async Task<User> CompleteAsync(IUnitOfWork unitOfWork, SibUserAccessWizard obj)
        {           

            var user = await base.CompleteAsync(unitOfWork, obj); 
            return user;


        }

        public override async Task OnAfterStepChangeAsync(IUnitOfWork unitOfWork, ViewModelConfig config,
            string prevStepName, SibUserAccessWizard obj)
        {          

            await base.OnAfterStepChangeAsync(unitOfWork, config, prevStepName, obj);
        }
        public override User Complete(IUnitOfWork unitOfWork, SibUserAccessWizard obj)
        {
            //return base.Complete(unitOfWork, obj);
            var complite = obj.GetObject();
            if (complite.Profile != null && complite.Profile is SibUser && obj.Society != null)
            {
                int sid = obj.Society.ID;
                ((SibUser)(complite.Profile)).Society = unitOfWork.GetRepository<Society>().Filter(x=>x.ID == sid).FirstOrDefault();                
            }
            var profile = unitOfWork.GetRepository<SibUser>().Create(complite.Profile as SibUser);
            complite.Profile = profile;
            var user = _baseService.Create(unitOfWork, complite);
            _login_provider.AttachSystemPassword(unitOfWork, user.ID, obj.Login, obj.Password);
            
            return user;
        }
       
         
    }
}
