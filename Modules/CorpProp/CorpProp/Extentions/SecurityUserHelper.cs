using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.DAL;
using Base.Enums;
using Base.Extensions;
using Base.Security;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Security;

namespace CorpProp.Extentions
{
    public static class SecurityUserHelper
    {
        /// <summary>
        /// Возвращает профиль пользователя SibUser.
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static SibUser FindLinkedSibUser(this ISecurityUser securityUser, IUnitOfWork unitOfWork)
        {
            return unitOfWork
                             .GetRepository<SibUser>()
                             .All()
                             .Include(user => user.Society).FirstOrDefault(user => user.UserID == securityUser.ID);
        }

        /// <summary>
        /// Возвращает признак принадлежности пользователя к ЦАУК. 
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static bool IsFromCauk(this ISecurityUser securityUser, IUnitOfWork unitOfWork)
        {
            return IsFromCauk(FindLinkedSibUser(securityUser, unitOfWork));
        }

        public static bool IsFromCauk(this SibUser sibUser)
        {
            return sibUser?.Society?.IsCAUK ?? false;
        }

        /// <summary>
        /// Возвращает признак принадлежности пользователя к Сервису. 
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static bool IsFromService(this ISecurityUser securityUser, IUnitOfWork unitOfWork)
        {
            return IsFromService(FindLinkedSibUser(securityUser, unitOfWork));
        }

        public static bool IsFromService(this SibUser sibUser)
        {
            return sibUser?.Society?.IsService ?? false;
        }

        /// <summary>
        /// Вовзращает ИД ЕУП ОГ пользователя.
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static string GetUserIDEUP(this ISecurityUser securityUser, IUnitOfWork unitOfWork)
        {
            var su = unitOfWork.GetRepository<SibUser>()
                .FilterAsNoTracking(f=>f.UserID==securityUser.ID).Include(user => user.Society)?.FirstOrDefault();
            if (su != null && su.Society != null)
                return su.Society.IDEUP;
            return "";
        }

        /// <summary>
        /// Возвращает идентификатор профиля пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="uow">Сессия.</param>
        /// <returns>Идентификатор.</returns>
        public static int GetSibUserID(this ISecurityUser user, IUnitOfWork uow)
        {
            var su = uow.GetRepository<SibUser>()
                            .FilterAsNoTracking(f => !f.Hidden && f.UserID == user.ID)?.FirstOrDefault();
            if (su != null)
                return su.ID;
            return 0;


        }

        public static SibUser GetSibUser(this ISecurityUser user, IUnitOfWork uow)
        {
            return uow.GetRepository<SibUser>()
                            .Filter(f => !f.Hidden && f.UserID == user.ID)?
                            .FirstOrDefault();
        }


        /// <summary>
        /// Возвращает список ИД ЕУП ОГ, доступных пользователю.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public static List<string> GetTerritorys(this ISecurityUser user, IUnitOfWork uow)
        {
            List<string> list = new List<string>();
            try
            {
                var sibUserID = user.GetSibUserID(uow);

                return uow.GetRepository<SibUserTerritory>()
                    .FilterAsNoTracking(f => !f.Hidden && f.ObjLeftId == sibUserID && f.ObjRigth != null
                    && f.ObjRigth.IDEUP != null && f.ObjRigth.IDEUP != "")
                    .Select(s => s.ObjRigth.IDEUP).ToList();
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// Возвращает список ИД ЕУП ОГ, у кого ОГ агент.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public static List<string> GetUserAgents(this ISecurityUser user, IUnitOfWork uow)
        {
            List<string> list = new List<string>();
            try
            {
                var userIDEUP = user.GetUserIDEUP(uow);

                var ag = uow.GetRepository<SocietyAgents>()
                    .FilterAsNoTracking(f => !f.Hidden && f.ObjRigth != null && f.ObjRigth.IDEUP == userIDEUP)
                    .Select(s => s.ObjLeft.IDEUP).ToList();

                list.AddRange(ag);
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// Возвращает список ИД ЕУП ОГ, управляемых.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public static List<string> GetUserBelows(this ISecurityUser user, IUnitOfWork uow)
        {
            List<string> list = new List<string>();
            try
            {
                var userIDEUP = user.GetUserIDEUP(uow);

                var ch = uow.GetRepository<SocietySubsidiaries>()
                    .FilterAsNoTracking(f => !f.Hidden && f.ObjLeft != null && f.ObjLeft.IDEUP == userIDEUP && f.ObjRigth.IDEUP != null && f.ObjRigth.IDEUP != "")
                    .Select(s => s.ObjRigth.IDEUP).ToList();

                list.AddRange(ch);
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        /// <summary>
        /// Возвращает коллекцию разрешений на объекты для указанного пользователя.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<ObjectPermission> GetObjectsPermissions(this ISecurityUser securityUser, IUnitOfWork uow)
        {
            List<ObjectPermission> list = new List<ObjectPermission>();
            try
            {

                var userID = securityUser.ID;
                var user = uow.GetRepository<User>().Find(userID);
                var category = uow.GetRepository<UserCategory>().Find(user.CategoryID);
                if (category != null)
                {
                    var roles = category.Roles;

                    if (category.ParentID != null)
                    {
                        var parents = category.sys_all_parents.Split(HCategory.Seperator).Select(HCategory.IdToInt);
                        var r = uow.GetRepository<UserCategory>().FilterAsNoTracking(f=> parents.Contains(f.ID))
                            .SelectMany(x => x.Roles).Distinct().ToList();
                        roles = roles.Union(r).Distinct().ToList();
                    }

                    var objPermissions = roles
                        .Join(uow.GetRepository<Permission>()
                        .FilterAsNoTracking(f => !f.Hidden), e => e.ID, o => o.RoleID, (e, o) => o)
                        .Join(uow.GetRepository<ObjectPermission>()
                        .FilterAsNoTracking(f => !f.Hidden), e => e.ID, o => o.TypePermissionID, (e, o) => o);

                    if (objPermissions != null)
                        list = objPermissions.ToList<ObjectPermission>();

                }
            }
            catch (Exception ex)
            {
                //TODO: обработать
            }

            return list;
        }

        /// <summary>
        /// Возвращает для текущего пользователя разрешения на чтение экземпляров объектов указанного типа.
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="uow"></param>
        /// <param name="ttype"></param>
        /// <returns></returns>
        public static List<ObjectPermission> GetReadObjectPermissions(
            this ISecurityUser securityUser
            , IUnitOfWork uow
            , Type ttype)
        {
            List<ObjectPermission> list = new List<ObjectPermission>();
            try
            {
                var typeName = ttype.GetTypeName();
                return securityUser.GetObjectsPermissions(uow)
                    .Where(f => f.TypePermission != null && f.TypePermission.FullName == typeName && f.AllowRead)
                    .ToList();
            }
            catch (Exception ex)
            {
                //TODO: обработать
            }

            return list;
        }

        public static List<ObjectPermission> GetPermissions(
           this ISecurityUser securityUser
           , IUnitOfWork uow
           , Type ttype
           , TypePermission typePerm
           )
        {
            List<ObjectPermission> list = new List<ObjectPermission>();
            try
            {
                var typeName = ttype.GetTypeName();
                //TODO: скорректировать условие
                return securityUser.GetObjectsPermissions(uow)
                    .Where(f => f.TypePermission != null
                    && f.TypePermission.FullName == typeName
                    &&
                    (f.AllowRead == typePerm.HasFlag(TypePermission.Read)
                    || f.AllowWrite == typePerm.HasFlag(TypePermission.Write)
                    || f.AllowDelete == typePerm.HasFlag(TypePermission.Delete))
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                //TODO: обработать
            }

            return list;
        }

        /// <summary>
        /// Возвращает для текущего пользователя разрешения на редактирование экземпляров объектов указанного типа.
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="uow"></param>
        /// <param name="ttype"></param>
        /// <returns></returns>
        public static List<ObjectPermission> GetWriteObjectPermissions(
            this ISecurityUser securityUser
            , IUnitOfWork uow
            , Type ttype)
        {
            List<ObjectPermission> list = new List<ObjectPermission>();
            try
            {
                var typeName = ttype.GetTypeName();
                return securityUser.GetObjectsPermissions(uow)
                    .Where(f => f.TypePermission != null && f.TypePermission.FullName == typeName && f.AllowWrite && f.AllowRead)
                    .ToList();
            }
            catch (Exception ex)
            {
                //TODO: обработать
            }

            return list;
        }

        /// <summary>
        /// Возвращает для текущего пользователя разрешения на редактирование экземпляров объектов указанного типа.
        /// </summary>
        /// <param name="securityUser"></param>
        /// <param name="uow"></param>
        /// <param name="ttype"></param>
        /// <returns></returns>
        public static List<ObjectPermission> GetDeleteObjectPermissions(
            this ISecurityUser securityUser
            , IUnitOfWork uow
            , Type ttype)
        {
            List<ObjectPermission> list = new List<ObjectPermission>();
            try
            {
                var typeName = ttype.GetTypeName();
                return securityUser.GetObjectsPermissions(uow)
                    .Where(f => f.TypePermission != null && f.TypePermission.FullName == typeName && f.AllowDelete && f.AllowRead)
                    .ToList();
            }
            catch (Exception ex)
            {
                //TODO: обработать
            }

            return list;
        }




    }
}
