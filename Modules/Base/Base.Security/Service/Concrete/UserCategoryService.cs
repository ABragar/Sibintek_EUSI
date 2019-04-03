using Base.DAL;
using Base.Enums;
using Base.Service;
using System;
using System.Linq;

namespace Base.Security.Service
{
    public class UserCategoryService : BaseUserCategoryService<UserCategory>, IUserCategoryService
    {
        private const string CurrentHasAdminRoleErrorMsg = "Группа с ролью \"Администратор\" не может быть доступна после регистрации";
        private const string ParentHasAdminRoleErrorMsg = "В родительской группе содержится роль \"Администратора\". Группа не может быть доступена после регистрации";
        private readonly IRoleService _roleService;


        public UserCategoryService(IBaseObjectServiceFacade facade,
            IRoleService roleService) : base(facade)
        {
            _roleService = roleService;
        }


        protected override IObjectSaver<UserCategory> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<UserCategory> objectSaver)
        {
            if (!objectSaver.Dest.IsAccessible)
                return base.GetForSave(unitOfWork, objectSaver);

            var srcRolesIds = objectSaver.Src.Roles.Select(x => x.ID);
            var roles = _roleService.GetAll(unitOfWork).Where(x => srcRolesIds.Contains(x.ID));
            if (roles.Any(x => x.SystemRole == SystemRole.Admin))
                throw new Exception(CurrentHasAdminRoleErrorMsg);

            if (objectSaver.Dest.ParentID != null)
            {
                var ids = objectSaver.Dest.sys_all_parents.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();

                var parentCategories = GetAll(unitOfWork).Where(x => ids.Contains(x.ID));

                foreach (var category in parentCategories)
                {
                    if (category.Roles.Any(x => x.SystemRole == SystemRole.Admin))
                        throw new Exception(ParentHasAdminRoleErrorMsg);
                }

            }

            return base.GetForSave(unitOfWork, objectSaver);
        }

        public IQueryable<UserCategory> GetAccessibleCategories(IUnitOfWork uofw)
        {
            return GetAll(uofw).Where(x => x.IsAccessible);
        }
    }
}
