using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Base;
using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Access;
using CorpProp.Entities.Document;
using CorpProp.Extentions;
using CorpProp.Model;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Services.Security
{
    static class PermissionHelperSingletone
    {
        private static Dictionary<AccessModifier, int> _permissionIdByAccessModifier;

        public static int GetPermissionId(this AccessModifier accessModifier, IUnitOfWork uow)
        {
            try
            {
                if (_permissionIdByAccessModifier == null)
                    _permissionIdByAccessModifier = uow.GetRepository<FileCardPermission>()
                                                      .All()
                                                      .ToDictionary(permission => permission.AccessModifier, permission => permission.ID);
                return _permissionIdByAccessModifier[accessModifier];
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка. Обратитесь к администратору!");
            }
        }
    }

    public class SibAccessableObjectCategorizedItemService<T>: BaseCategorizedItemService<T>
        where T: BaseObject, ICategorizedItem, ISibAccessableObject
    {
        public SibAccessableObjectCategorizedItemService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }



        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var query = base.GetAll(unitOfWork, hidden);

            if (AppContext.SecurityUser.IsAdmin)
                return query;
            
            var sibAccess = typeof(ISibAccessableObject).IsAssignableFrom(query.ElementType);
            if (!sibAccess)
                return query;

            var sibUser = AppContext.SecurityUser.FindLinkedSibUser(unitOfWork);
            var isCauk = sibUser.IsFromCauk();
            //работники ЦАУК могут просматривать документы без ограничений
            if (!isCauk)
            {
                //разграничение прав доступа по ISibAccessableObject
                var everyoneCondition = $"{nameof(ISibAccessableObject.FileCardPermissionID)} = {AccessModifier.Everyone.GetPermissionId(unitOfWork)}";
                var authorOnlyCondition = $"{nameof(ISibAccessableObject.FileCardPermissionID)} = {AccessModifier.AuthorOnly.GetPermissionId(unitOfWork)}";
                var authorSocietyCondition = $"{nameof(ISibAccessableObject.FileCardPermissionID)} = {AccessModifier.AuthorSociety.GetPermissionId(unitOfWork)}";
                var currentUserIsAuthorCondition = sibUser?.ID != null ? $"{nameof(ISibAccessableObject.CreateUserID)} = {sibUser?.ID}" : "false";
                var currentUserSocietyIsAuthorSocietyContition = sibUser?.SocietyID != null ? $"{nameof(ISibAccessableObject.SocietyID)} = {sibUser?.SocietyID}" : "false";
                var filter = $"{everyoneCondition} or ({authorOnlyCondition} and {currentUserIsAuthorCondition}) or ({authorSocietyCondition} and {currentUserSocietyIsAuthorSocietyContition})";
                query = query.Where(filter);
            }
            return query;
        }

        public void SetSocietyFromCurrentUser(T item, IUnitOfWork unitOfWork)
        {
            var fileCard = item as FileCard;
            if (fileCard != null)
            {
                fileCard.SocietyID = AppContext.SecurityUser.FindLinkedSibUser(unitOfWork)?.SocietyID;
                //По умолчанию смотреть можно только автору
                fileCard.FileCardPermissionID = AccessModifier.AuthorOnly.GetPermissionId(unitOfWork);
            }
        }

        public override T Create(IUnitOfWork unitOfWork, T obj)
        {
            var item = base.Create(unitOfWork, obj);
            SetSocietyFromCurrentUser(item, unitOfWork);
            return item;
        }

        public override T CreateDefault(IUnitOfWork unitOfWork)
        {
            var item = base.CreateDefault(unitOfWork);
            SetSocietyFromCurrentUser(item, unitOfWork);
            return item;
        }
    }
}
