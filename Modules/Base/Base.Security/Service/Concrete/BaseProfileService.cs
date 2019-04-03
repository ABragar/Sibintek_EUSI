using Base.DAL;
using Base.Service.Crud;
using Base.UI;
using System;
using System.Linq;

namespace Base.Security.Service
{
    public class BaseProfileService : IBaseProfileService
    {
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly IAccessErrorDescriber _accessErrorDescriber;

        public BaseProfileService(IViewModelConfigService viewModelConfigService, IAccessErrorDescriber accessErrorDescriber)
        {
            _viewModelConfigService = viewModelConfigService;
            _accessErrorDescriber = accessErrorDescriber;
        }

        public BaseProfile CreateDefault(IUnitOfWork unitOfWork, string type)
        {
            var serv = GetService(type);

            var baseProfile = serv.CreateDefault(unitOfWork) as BaseProfile;

            return baseProfile;
        }

        public BaseProfile CreateEmptyProfile(IUnitOfWork unitOfWork, string type)
        {
            var serv = GetService(type);

            var baseProfile = serv.Create(unitOfWork, serv.CreateDefault(unitOfWork)) as BaseProfile;

            return baseProfile;
        }

        public BaseProfile CreateProfile(IUnitOfWork unitOfWork, BaseProfile profile, string type)
        {
            var serv = GetService(type);

            return serv.Create(unitOfWork, profile) as BaseProfile;
        }

        public BaseProfile UpdateProfile(IUnitOfWork unitOfWork, BaseProfile profile, string type)
        {
            var serv = GetService(type);

            return serv.Update(unitOfWork, profile) as BaseProfile;
        }

        public void DeleteProfile(IUnitOfWork unitOfWork, BaseProfile profile, string type)
        {
            var serv = GetService(type);
            serv.Delete(unitOfWork, profile);
        }

        public IBaseObjectCrudService GetService(string type)
        {
            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            var profileConfig = _viewModelConfigService.Get(type);

            if (profileConfig == null)
                throw new Exception($"configuration for profile [{type}] not found");

            var serv = profileConfig.GetService<IBaseObjectCrudService>();

            if (serv == null)
                throw new Exception($"service for config [{profileConfig.Title}] not found");

            return serv;
        }

        public string GetProfileType(IUnitOfWork unitOfWork, int categoryId)
        {
            string profileType = unitOfWork.GetRepository<UserCategory>().All()
                .Where(x => x.ID == categoryId)
                .Select(x => x.ProfileMnemonic).FirstOrDefault();

            if (string.IsNullOrEmpty(profileType))
                throw new Exception(_accessErrorDescriber.ProfileTypeIsEmpty(categoryId));

            return profileType;
        }

        public bool ProfileComplite(IUnitOfWork unitOfWork, int id)
        {
            if (!Ambient.AppContext.SecurityUser.IsAdmin && Ambient.AppContext.SecurityUser.ProfileInfo.ID != id)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            string exstraId = unitOfWork.GetRepository<BaseProfile>().All().Where(x => x.ID == id).Select(x => x.ExtraID).SingleOrDefault();

            var serv = GetService(exstraId);
            var profile = (BaseProfile)serv.Get(unitOfWork, id);
            profile.IsEmpty = false;
            //+validation
            serv.Update(unitOfWork, profile);
            return true;
        }
    }
}
