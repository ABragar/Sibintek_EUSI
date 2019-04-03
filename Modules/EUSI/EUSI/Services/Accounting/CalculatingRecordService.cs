using Base.DAL;
using Base.Service;
using EUSI.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Services.Accounting
{
    public interface ICalculatingRecordService : IBaseObjectService<CalculatingRecord>
    {

    }

    public class CalculatingRecordService : BaseObjectService<CalculatingRecord>,
        ICalculatingRecordService
    {
        public CalculatingRecordService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<CalculatingRecord> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<CalculatingRecord> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(s => s.Consolidation)
                .SaveOneObject(s => s.PositionConsolidation)
                .SaveOneObject(s => s.Initiator)
                .SaveOneObject(s => s.TaxRateType)
                ;
        }
    }
}
