using Base.DAL;
using Base.Events;
using Base.Extensions;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Security.Service
{
    public class AccessUserService : BaseCategorizedItemService<User>, IAccessUserService, IUserService<User>
    {
        private readonly IBaseProfileService _profileService;
        private readonly IAccessErrorDescriber _accessErrorDescriber;
        private readonly IAutoMapperCloner _autoMapperCloner;

        public AccessUserService(
            IBaseObjectServiceFacade facade,
            IBaseProfileService profileService,
            IAutoMapperCloner autoMapperCloner, IAccessErrorDescriber accessErrorDescriber)
            : base(facade)
        {
            _profileService = profileService;
            _autoMapperCloner = autoMapperCloner;
            _accessErrorDescriber = accessErrorDescriber;
        }


        protected override IObjectSaver<User> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<User> objectSaver)
        {
            var objSaver = base.GetForSave(unitOfWork, objectSaver);
            return objSaver;
        }

        public override User Create(IUnitOfWork unitOfWork, User obj)
        {
            if (!Ambient.AppContext.SecurityUser.IsAdmin)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            var repository = unitOfWork.GetRepository<User>();
            
                if (obj.CategoryID == 0)
                obj.CategoryID =
                    unitOfWork.GetRepository<UserCategory>()
                        .All()
                        .Where(x => x.SysName == "users")
                        .Select(x => x.ID)
                        .FirstOrDefault();

            if (obj.CategoryID == 0)
                throw new Exception("category not found");

            var user = repository.All().FirstOrDefault(u => !u.Hidden && u.SysName == obj.SysName && u.CategoryID == obj.CategoryID);

            if (user!= null)
            {
                var category = unitOfWork.GetRepository<UserCategory>()
                    .All().SingleOrDefault(c => c.ID == user.CategoryID);
                throw new Exception($"Данный польователь уже был добавлен в группу {category?.Name}!");
            }

            var neededProfileType = _profileService.GetProfileType(unitOfWork, obj.CategoryID);
            this.InitSortOrder(unitOfWork, obj);

            if (obj.Profile == null || neededProfileType != obj.Profile.GetType().Name)
            {
                var createdProfile = _profileService.CreateEmptyProfile(unitOfWork, neededProfileType);

                if (obj.Profile == null)
                {
                    createdProfile.IsEmpty = true;
                }
                else
                {
                    createdProfile.FirstName = obj.Profile.FirstName;
                    createdProfile.MiddleName = obj.Profile.MiddleName;
                    createdProfile.LastName = obj.Profile.LastName;
                    createdProfile.Image = obj.Profile.Image;
                    createdProfile.ImageID = obj.Profile.ImageID;
                    createdProfile.Gender = obj.Profile.Gender;
                    createdProfile.IsEmpty = false;
                }

                obj.Profile = createdProfile;
            }

            repository.Create(obj);
            unitOfWork.SaveChanges();

            OnCreate.Raise(() => new OnCreate<User>(obj, unitOfWork));
            return obj;
        }

        public override IReadOnlyCollection<User> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<User> collection)
        {
            throw new NotImplementedException();
        }

        public override User Update(IUnitOfWork unitOfWork, User obj)
        {
            if (!Ambient.AppContext.SecurityUser.IsAdmin)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            return base.Update(unitOfWork, obj);
        }

        public override IReadOnlyCollection<User> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<User> collection)
        {
            if (!Ambient.AppContext.SecurityUser.IsAdmin)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            return base.UpdateCollection(unitOfWork, collection);
        }

        public override void Delete(IUnitOfWork unitOfWork, User obj)
        {
            if (!Ambient.AppContext.SecurityUser.IsAdmin)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            base.Delete(unitOfWork, obj);
        }

        public override void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<User> collection)
        {
            if (!Ambient.AppContext.SecurityUser.IsAdmin)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            base.DeleteCollection(unitOfWork, collection);
        }

        public Task<User> GetAsync(IUnitOfWork unitOfWork, int id)
        {
            return GetAll(unitOfWork).Where(x => x.ID == id).SingleOrDefaultAsync();
        }

        public Task<List<User>> GetAllAsync(IUnitOfWork unitOfWork, IEnumerable<int> userIDs)
        {
            return GetAll(unitOfWork).Where(x => userIDs.Contains(x.ID)).ToListAsync();
        }

        public Task<int> GetUserIdByProfileId(IUnitOfWork unitOfWork, int profileId)
        {
            return
                GetAll(unitOfWork, hidden: null)
                    .Where(x => x.BaseProfileID == profileId)
                    .Select(x => x.ID)
                    .FirstOrDefaultAsync();
        }

        public BaseProfile ChangeCategory(IUnitOfWork unit_of_work, int user_id, int new_category_id, BaseProfile new_profile)
        {
            if (!Ambient.AppContext.SecurityUser.IsAdmin && Ambient.AppContext.SecurityUser.ID != user_id)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            if (!Ambient.AppContext.SecurityUser.IsAdmin)
            {
                if (!unit_of_work.GetRepository<UserCategory>().All()
                    .Any(x => x.ID == new_category_id && x.IsAccessible))
                {
                    throw new Exception(_accessErrorDescriber.AccessDenied());
                }
            }

            var repository = unit_of_work.GetRepository<User>();

            var user = repository.All().SingleOrDefault(x => x.ID == user_id);

            if (user == null)
                throw new Exception(_accessErrorDescriber.UserNotFound(user_id));

            var old_profile = unit_of_work.GetRepository<BaseProfile>().All().SingleOrDefault(x => x.ID == user.BaseProfileID);

            string old_profile_type = _profileService.GetProfileType(unit_of_work, user.CategoryID);
            string new_profile_type = _profileService.GetProfileType(unit_of_work, new_category_id);

            if (new_profile == null)
            {
                if (new_profile_type != old_profile_type)
                {
                    new_profile = _profileService.CreateDefault(unit_of_work, new_profile_type);

                    if (old_profile != null)
                    {
                        new_profile = (BaseProfile)_autoMapperCloner.Copy(old_profile, new_profile);
                        new_profile.ID = 0;
                        new_profile.RowVersion = null;
                        new_profile.Hidden = false;
                    }

                    new_profile.IsEmpty = false;
                }
            }

            user.CategoryID = new_category_id;

            if (new_profile != null)
            {
                user.Profile = _profileService.CreateProfile(unit_of_work, new_profile, new_profile_type);

                if (old_profile != null)
                {
                    _profileService.DeleteProfile(unit_of_work, old_profile, old_profile_type);
                }
            }
            else if (old_profile == null)
            {
                user.Profile = _profileService.CreateEmptyProfile(unit_of_work, new_profile_type);
            }

            repository.Update(user);

            unit_of_work.SaveChanges();

            OnUpdate.Raise(() => new OnUpdate<User>(user, user, unit_of_work));

            return user.Profile;

        }

        public override void ChangeCategory(IUnitOfWork unit_of_work, int id, int new_category_id)
        {
            ChangeCategory(unit_of_work, id, new_category_id, null);
        }
    }
}
