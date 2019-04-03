using Base;
using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Services.Base
{

    public interface IDictHistoryService<T> : IDictObjectService<T>, IHistoryService<T> where T : DictObject
    {

    }


    [EnableFullTextSearch]
    public class DictHistoryService<T> : BaseObjectService<T>, IDictHistoryService<T> where T : DictObject
    {


        public DictHistoryService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        /// <summary>
        /// Получает признак доступности кнопки выбора даты для историчного списка.
        /// </summary>
        public virtual bool HistoryActionVisible { get { return true; } }


        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<T> objectSaver)
        {
            var obj =
                base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.DictObjectStatus)
                .SaveOneObject(x => x.DictObjectState)
                ;

            if (obj != null)
            {
                DictObjectStatus doStatus = obj.Original?.DictObjectStatus;
                obj.Dest.ChangeState(unitOfWork, doStatus?.Code);
            }
            return obj;
            
        }

        protected override void OnSave(T obj)
        {
            obj.TrimCode();

            base.OnSave(obj);
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, hidden);
        }

        public IQueryable<T> GetAllByDate(IUnitOfWork uow, DateTime? date)
        {
            if (date == null)
                return base.GetAll(uow, false);
            else
                date = date.Value.Date;

            return base.GetAll(uow, false)
                .Where(x =>
                    ((x.DateFrom == null || (x.DateFrom != null && x.DateFrom.Value.Date <= date)) &&
                    (x.DateTo == null || (x.DateTo != null && x.DateTo.Value.Date >= date)) )
                    
            );

        }
        public int? GetObjIDByDate(IUnitOfWork unitOfWork, int id, DateTime? date)
        {            
            return id;
        }

        public DateTime? GetMinDate(IUnitOfWork unitOfWork, int id)
        {

            return null;
        }
    }
}
