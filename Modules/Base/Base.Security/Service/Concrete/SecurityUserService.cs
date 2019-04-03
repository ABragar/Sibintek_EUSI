using Base.DAL;
using Base.Events;
using Base.Service;
using Base.Utils.Common.Caching;
using System.Linq;
using Base.Security.Entities.Concrete;
using Base.Security.Service.Abstract;

namespace Base.Security.Service
{
    public class SecurityUserService : ISecurityUserService
    {
        private readonly IUserService<User> _userService;
        private readonly ILoginProvider _login_provider;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly ISimpleCacheWrapper _cacheWrapper;

        public SecurityUserService(IUnitOfWorkFactory unitOfWorkFactory, ISimpleCacheWrapper cacheWrapper, IUserService<User> userService, ILoginProvider login_provider)
        {

            _unitOfWorkFactory = unitOfWorkFactory;
            _cacheWrapper = cacheWrapper;
            _userService = userService;
            _login_provider = login_provider;
        }


        private static readonly CacheAccessor<ISecurityUser> UserGroupCache = new CacheAccessor<ISecurityUser>();

        public ISecurityUser FindSecurityUser(int id, string login)
        {
            _cacheWrapper.GetOrAdd(UserGroupCache, null, () => null);

            return _cacheWrapper.GetOrAdd(UserGroupCache, id.ToString(), () =>
               {
                   using (var uow = _unitOfWorkFactory.CreateSystem())
                   {
                       var repository = uow.GetRepository<User>();
                       var user = repository.All().FirstOrDefault(x => x.ID == id);

                       return user != null ? new SecurityUser(uow, user, login) : null;
                   }
               }, UserGroupCache.GetDependencyKey(null));

        }



        public void ClearAll()
        {
            _cacheWrapper.TryRemove(UserGroupCache, null);
        }

        public void Clear(int id)
        {
            _cacheWrapper.TryRemove(UserGroupCache, id.ToString());

        }

        public void Clear(ISecurityUser securityUser)
        {
            Clear(securityUser.ID);
        }

        public void OnEvent(IChangeObjectEvent<SimpleProfile> evnt)
        {
            this.Clear(_userService.GetUserIdByProfileId(evnt.UnitOfWork, evnt.Modified.ID).Result);
        }

        public void OnEvent(IChangeObjectEvent<User> evnt)
        {
            this.Clear(evnt.Modified.ID);
        }

        public void OnEvent(IChangeObjectEvent<UserCategory> evnt)
        {
            this.ClearAll();
        }

        public void OnEvent(IChangeObjectEvent<Role> evnt)
        {
            this.ClearAll();
        }
    }
}
