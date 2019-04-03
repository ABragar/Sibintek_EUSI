using Base.DAL;
using Base.Service;
using System.Linq;
using AppContext = Base.Ambient.AppContext;

namespace Base.Security.Service
{
    public class UserService : AccessUserService
    {
        private readonly IUserCategoryService _userCategoryService;

        public UserService(IBaseObjectServiceFacade facade,
            IBaseProfileService profileService,
            IUserCategoryService userCategoryService,
            IAutoMapperCloner autoMapperCloner,
            IAccessErrorDescriber accessErrorDescriber
            ) : base(facade, profileService, autoMapperCloner, accessErrorDescriber)
        {
            _userCategoryService = userCategoryService;
        }


        public override IQueryable<User> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            if (AppContext.SecurityUser == null || AppContext.SecurityUser.IsAdmin)
            {
                return base.GetAll(unitOfWork, hidden);
            }
            else
            {
                var catIds = _userCategoryService.GetAll(unitOfWork).Select(x => x.ID).ToArray();

                return base.GetAll(unitOfWork, hidden)
                    .Where(x => catIds.Contains(x.CategoryID));
            }
        }
    }
}
