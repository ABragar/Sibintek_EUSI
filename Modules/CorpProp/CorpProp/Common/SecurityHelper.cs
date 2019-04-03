using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.DAL;
using Base.Security;

namespace CorpProp.Common
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Найти роли текущего пользователя
        /// </summary>
        /// <param name="work"></param>
        /// <param name="userId">Идентификатор пользователя <see cref="User.ID"/></param>
        /// <returns></returns>
        public static IEnumerable<Base.Security.Role> FindUserRoles(IUnitOfWork work, int? userId)
        {
            var user =
                 work.GetRepository<User>()
                     .Find(Base.Ambient.AppContext.SecurityUser.ID);
            //work.GetRepository<Role>().Filter(role => role.ID)
            var cat = work.GetRepository<UserCategory>().Find(user.CategoryID);

            if (cat == null) return null;

            ICollection<Role> GetCategoryRoles(IUnitOfWork uow, UserCategory category)
            {
                if (category == null)
                    throw new NullReferenceException("User category is null");

                var roles = category.Roles;

                if (category.ParentID != null)
                {
                    var parents = category.sys_all_parents.Split(HCategory.Seperator).Select(HCategory.IdToInt);
                    var r = uow.GetRepository<UserCategory>().All().Where(x1 => parents.Contains(x1.ID)).SelectMany(x2 => x2.Roles).Distinct().ToList();
                    roles = roles.Union(r).Distinct().ToList();
                }

                //TODO IQueryable<>
                return roles;
            }

            var currentUserRoles = GetCategoryRoles(work, cat);

            return currentUserRoles;
        }
    }
}
