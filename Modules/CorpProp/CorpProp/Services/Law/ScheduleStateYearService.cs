using Base.Service;
using CorpProp.Entities.Law;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service.Log;

namespace CorpProp.Services.Law
{
    public interface IScheduleStateYearService : ITypeObjectService<ScheduleStateYear>
    {

    }
    public class ScheduleStateYearService : TypeObjectService<ScheduleStateYear>, IScheduleStateYearService
    {
        private readonly ILogService _logger;
        public ScheduleStateYearService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        public override ScheduleStateYear Create(IUnitOfWork unitOfWork, ScheduleStateYear obj)
        {
            return base.Create(unitOfWork, obj);
        }

        protected override IObjectSaver<ScheduleStateYear> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ScheduleStateYear> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                ;
        }

    }
}
