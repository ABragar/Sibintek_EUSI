using Base.Ambient;
using Base.DAL;
using Base.Extensions;
using Base.Security.Service.Abstract;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Security.Service.Concrete
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IAppContextBootstrapper _appContextBootstrapper;
        private readonly IAccessUserService _accessUserService;
        private readonly IBaseProfileService _baseProfileService;

        public UserInfoService(IAppContextBootstrapper appContextBootstrapper,
            IAccessUserService accessUserService,
            IBaseProfileService baseProfileService)
        {
            _appContextBootstrapper = appContextBootstrapper;
            _accessUserService = accessUserService;
            _baseProfileService = baseProfileService;
        }

        public async Task<int> CreateUserAsync(IUnitOfWork unit_of_work, UserInfo info)
        {
            using (_appContextBootstrapper.LocalContextSecurity(new SystemUser()))
            {
                var user = _accessUserService.Create(unit_of_work, new User());

                await AddInfoAsync(unit_of_work, user, info);

                return user.ID;
            }
        }

        public async Task UpdateUserInfoAsync(IUnitOfWork unit_of_work, int user_id, UserInfo info)
        {
            using (_appContextBootstrapper.LocalContextSecurity(new SystemUser()))
            {
                if (info == null) return;

                var user =
                    await _accessUserService.GetAll(unit_of_work).Where(x => x.ID == user_id).SingleOrDefaultAsync();

                await AddInfoAsync(unit_of_work, user, info);
            }
        }

        public Task<bool> UserExistsAsync(IUnitOfWork unit_of_work, int user_id)
        {
            using (_appContextBootstrapper.LocalContextSecurity(new SystemUser()))
            {
                return _accessUserService.GetAll(unit_of_work).Where(x => x.ID == user_id).AnyAsync();
            }
        }

        private Task AddInfoAsync(IUnitOfWork unitOfWork, User user, UserInfo info)
        {
            var profile = user.Profile;

            if (info.Email != null && profile.Emails.All(x => x.Email != info.Email))
                profile.Emails.Add(new ProfileEmail() { Email = info.Email });

            if (profile.LastName == null && profile.FirstName == null)
            {
                profile.LastName = info.LastName;
                profile.FirstName = info.FirstName;
            }

            var profileType = _baseProfileService.GetProfileType(unitOfWork, user.CategoryID);
            _baseProfileService.UpdateProfile(unitOfWork, profile, profileType);

            return Task.CompletedTask;
        }
    }
}