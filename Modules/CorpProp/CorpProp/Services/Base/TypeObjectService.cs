using Base;
using Base.DAL;
using Base.Enums;
using Base.Events;
using Base.Security;
using Base.Service;
using Base.Service.Log;
using Base.Utils.Common.Attributes;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.Security;
using CorpProp.Extentions;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Services.Base
{

    public interface ITypeObjectService<T> : IBaseObjectService<T>, IHistoryService<T>,
        IEventBusHandler<IOnCreate<T>>,
        IEventBusHandler<IOnDelete<T>>,
        IEventBusHandler<IOnUpdate<T>>
        where T : TypeObject
    {
       
    }

    

    [EnableFullTextSearch]
    public class TypeObjectService<T> : BaseObjectService<T>, ITypeObjectService<T> where T : TypeObject
    {
        private readonly ILogService _logger;
        public TypeObjectService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade)
        {
            _logger = logger;

        }
        /// <summary>
        /// Получает признак доступности кнопки выбора даты для историчного списка.
        /// </summary>
        public virtual bool HistoryActionVisible { get { return true;  } }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<T> objectSaver)
        {

            return base.GetForSave(unitOfWork, objectSaver);
        }

        public override T Get(IUnitOfWork unitOfWork, int id)
        {
            //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Read);
            //ThrowIfObjectAccessDenied(AppContext.SecurityUser, unitOfWork, id, TypePermission.Read);
            return base.Get(unitOfWork, id);
        }

        public override T Update(IUnitOfWork unitOfWork, T obj)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Write);
            ThrowIfObjectAccessDenied(AppContext.SecurityUser, unitOfWork, obj.ID, TypePermission.Write);
            return base.Update(unitOfWork, obj);
        }

        public override void Delete(IUnitOfWork unitOfWork, T obj)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), TypePermission.Delete);
            ThrowIfObjectAccessDenied(AppContext.SecurityUser, unitOfWork, obj.ID, TypePermission.Delete);
            base.Delete(unitOfWork, obj);
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return GetAllByPermissions(unitOfWork, TypePermission.Read);

        }

        public  IQueryable<T> GetAllByPermissions(
            IUnitOfWork unitOfWork
            , TypePermission typePerm
            , bool? hidden = false)
        {            
            var q = base.GetAll(unitOfWork, hidden);

            //есть разерешения на тип, проверим разрешения на объекты
            //TODO: убрать в шину доступа
            if (!AppContext.SecurityUser.IsAdmin)
            {
                var perms = AppContext.SecurityUser.GetPermissions(unitOfWork,typeof(T), typePerm);
                if (perms.Count > 0)
                    return q.AddPermCriteria(unitOfWork, perms);
            }

            return q;

        }


        public virtual void OnEvent(IOnDelete<T> evnt)
        {

        }
        public virtual void OnEvent(IOnCreate<T> evnt)
        {

        }
        public virtual void OnEvent(IOnUpdate<T> evnt)
        {

        }

        public virtual IQueryable<T> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            //return base.GetAll(uow, false);

            if (date == null)
                return this.GetAll(uow, false).Where(f => !f.IsHistory);
            else
                date = date.Value.Date;

            var q = this.GetAll(uow, false);
            var groups = q
                .Where(x => (x.ActualDate == null || (x.ActualDate != null && x.ActualDate.Value.Date <= date)))
                .GroupBy(gr => gr.Oid).Select(s => new { Oid = s.Key, MaxActualDate = s.Max(m => m.ActualDate) });

            var items = q.Join(groups, e => e.Oid, o => o.Oid, (e, o) => new { e, o })
                .Where(w => w.o.MaxActualDate == w.e.ActualDate)
                .Select(s => s.e);


            return items;

        }


        public int? GetObjIDByDate(IUnitOfWork unitOfWork, int id, DateTime? date)
        {
            try
            {
                if (date == null)
                    return id;
                else
                    date = date.Value.Date;

                var oid = this.Get(unitOfWork, id)?.Oid;
                var q = this.GetAll(unitOfWork, false).Where(f => f.Oid == oid);
                var groups = q
                    .Where(x => x.Oid == oid && (x.ActualDate == null || (x.ActualDate != null && x.ActualDate.Value.Date <= date)))
                    .GroupBy(gr => gr.Oid).Select(s => new { Oid = s.Key, MaxActualDate = s.Max(m => m.ActualDate) });

                var item = q.Join(groups, e => e.Oid, o => o.Oid, (e, o) => new { e, o })
                    .Where(w => w.o.MaxActualDate == w.e.ActualDate)
                    .Select(s => s.e.ID)
                    .FirstOrDefault();

                return item;
            }
            catch(Exception ex)
            { return null; }
           
        }

        public DateTime? GetMinDate(IUnitOfWork unitOfWork, int id)
        {
            try
            {  
                var oid = this.Get(unitOfWork, id)?.Oid;
                var q = this.GetAll(unitOfWork, false).Where(f => f.Oid == oid);
                var minDate = q
                    .Where(x => x.Oid == oid)
                    .GroupBy(gr => gr.Oid)
                    .Select(s => s.Min(m => m.ActualDate))
                    .FirstOrDefault();

                return minDate;
            }
            catch { return null; }

        }

        public bool HaveObjectPermission(
              ISecurityUser securityUser
            , IUnitOfWork uow
            , int id
            , TypePermission permType)
        {
            if (securityUser.IsAdmin) return true;

            //TODO: доработать
            return this.GetAllByPermissions(uow, permType).Where(f => f.ID == id).Any();
           
        }

        public void ThrowIfObjectAccessDenied(           
            ISecurityUser securityUser
           , IUnitOfWork uow
           , int id
           , TypePermission permType
           )
        {
            if (uow is ISystemUnitOfWork) return;

            if (!HaveObjectPermission(securityUser, uow, id, permType))
                throw new AccessDeniedException($"Отказано в доступе к объекту ИД:{id}");
        }

    }
}
