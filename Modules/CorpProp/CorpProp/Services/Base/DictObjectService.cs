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
   
    public interface IDictObjectService<T> : IBaseObjectService<T> where T : DictObject
    {

    }

   
    [EnableFullTextSearch]
    public class DictObjectService<T> : BaseObjectService<T>, IDictObjectService<T> where T : DictObject
    {

        
        public DictObjectService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        
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

        ///TODO: историчность
        /// <summary>
        /// Возвращает коллекцию записей, актуальных на текущую дату.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="hidden"></param>
        /// <returns></returns>
        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            DictObjectState dActualState = unitOfWork.GetRepository<DictObjectState>().FilterAsNoTracking(f => f.Code == "NotOld").FirstOrDefault();

            return base.GetAll(unitOfWork, hidden)
                .Where(x =>
                    (
                        (x.DateFrom == null || (x.DateFrom != null && x.DateFrom.Value.Date <= DateTime.Now.Date)) &&
                        (x.DateTo == null || (x.DateTo != null && x.DateTo.Value.Date >= DateTime.Now.Date)) 
                        && (x.DictObjectStateID == null || x.DictObjectStateID == dActualState.ID)
                    )
            );
        }
    }
}
