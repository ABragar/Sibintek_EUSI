using Base.DAL;
using Base.Extensions;
using Base.Service;
using CorpProp.Common;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using EUSI.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Services.Monitor
{
    
    public class MonitorResultCustomService : BaseObjectService<ReportMonitoringResult>, ICustomDataSource<ReportMonitoringResult>
    {
        public MonitorResultCustomService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public IQueryable<ReportMonitoringResult> GetAllCustom(IUnitOfWork uow, params object[] objs)
        {
            if (objs == null || objs.Length < 1) return GetAll(uow);

            var monitorID = Convert.ToInt32(objs[0]);

            var eventCode = uow.GetRepository<ReportMonitoring>()
                .Filter(f => f.ID == monitorID)
                .Include(inc => inc.ReportMonitoringEventType)
                .FirstOrDefault()?.ReportMonitoringEventType?.Code;

            return
                uow.GetRepository<MonitorEventTypeAndResult>()
                .Filter(f => !f.Hidden
                    && f.ObjLeft != null
                    && f.ObjLeft.Code == eventCode
                    && f.IsManualPick)
                 .Select(s => s.ObjRigth)                 
              ;
           
        }
    }
}
