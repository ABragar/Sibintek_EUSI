using System.Collections.Generic;
using System.Linq;
using Base;
using Base.DAL;
using Base.Service;
using System.Linq.Dynamic;
using Base.Ambient;
using Base.UI.Extensions;
using CorpProp.Common;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Extentions;
using CorpProp.Services.Response.Fasade;

namespace CorpProp.Services.SibPermission
{
    public class BaseSibPermissionObjectService<T>: BaseObjectService<T>
        where T: BaseObject
    {
        public string Mnemonic;
        
        public BaseSibPermissionObjectService(IBaseObjectServiceFacade facade) : base(facade)
        {
            Mnemonic = typeof(T).Name;
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var query = base.GetAll(unitOfWork, hidden);
            if (AppContext.SecurityUser.IsAdmin)
                return query;
            var sibUser = AppContext.SecurityUser.FindLinkedSibUser(unitOfWork);
            var isCauk = sibUser.IsFromCauk();
            //работники ЦАУК могут просматривать без ограничений
            if (!isCauk)
            {
                if (sibUser?.SocietyID != null)
                {
                    ////работники ОГ могут просматривать объекты только своего ОГ
                    var filter = $"it.{nameof(ISocietal.SocietyID)} = {sibUser.SocietyID}";
                    var queryAccessBySocietys = query.Where(filter);

                    var sibPremissions = unitOfWork.GetRepository<Entities.Access.SibPermission>()
                                                   .Filter(
                                                           permission =>
                                                               permission.ObjectMnemonic == Mnemonic &&
                                                               permission.SocietyID == sibUser.SocietyID);
                    
                    var query2 = query.Join(
                                            sibPremissions,
                                            $"it.{nameof(BaseObject.ID)}",
                                            $"it.{nameof(Entities.Access.SibPermission.ObjectId)}",
                                            "outer",
                                            null).Cast<T>();
                    
                    query = queryAccessBySocietys.Union(query2).Distinct();
                }
                else
                {
                    //Если у обычного пользователя нет SocietyID, значит доступа нет
                    return new List<T>().AsQueryable();
                }
            }
            return query;
        }

        public override T Create(IUnitOfWork unitOfWork, T obj)
        {
            var societal = obj as ISocietal;
            if (societal != null)
            {
                societal.SocietyID = AppContext.SecurityUser.FindLinkedSibUser(unitOfWork)?.SocietyID;
            }
            return base.Create(unitOfWork, obj);
        }
    }
}
