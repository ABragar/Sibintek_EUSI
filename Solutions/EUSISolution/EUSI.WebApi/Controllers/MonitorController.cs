using Base;
using Base.DAL;
using Base.Service.Log;
using Base.UI;
using CorpProp.Entities.NSI;
using CorpProp.Extentions;
using EUSI.Entities.Accounting;
using EUSI.Entities.NonPersistent;
using EUSI.Entities.NSI;
using EUSI.Entities.Report;
using EUSI.Services.Accounting;
using EUSI.Services.Monitor;
using System;
using System.Linq;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Controllers;
using WebApi.Models.Crud;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер монитора.
    /// </summary>
    [CheckSecurityUser]
    [RoutePrefix("eusi/createMonitor/{mnemonic}")]
    internal class MonitorController : BaseApiController
    {
        IDraftOSService _draftOSService;
        private readonly ILogService _logger;

        public MonitorController(
            IDraftOSService draftOSService,
            IViewModelConfigService viewModelConfigService,
            IUnitOfWorkFactory unitOfWorkFactory,
            ILogService logger)
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _draftOSService = draftOSService;
        }


        [HttpPost]
        [Route("")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Create<T>(string mnemonic, [FromBody]SaveModel<T> save_model) where T : BaseObject
        {
            var sm = save_model as SaveModel<BEAndMonthPeriod>;
            var obj = sm.model;
            MonitorReportingImportService monitor = new MonitorReportingImportService();
            var eventCode = "Report_Screen_DraftOS";
            var monitorService = new MonitorReportingService();

            var beCode = obj.Consolidation?.Code;
            var start = new DateTime(obj.MonthPeriod.Value.Year, obj.MonthPeriod.Value.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);


            using (var uow = CreateUnitOfWork())
            {               
                var eventType = uow.GetRepository<ReportMonitoringEventType>()
                    .Filter(f => !f.Hidden && f.Code == eventCode).FirstOrDefault();

                var susses = !_draftOSService.GetReportDS(uow, beCode, end).Any();

                var rm = new ReportMonitoring();
                rm.ImportDateTime = DateTime.Now;
                rm.IsValid = susses;
                rm.ReportName = eventType?.Name;
                rm.Mnemonic = nameof(DraftOS);
                rm.ReportMonitoringEventType = eventType;
                rm.SibUser = Base.Ambient.AppContext.SecurityUser.GetSibUser(uow);
                rm.StartDate = start;
                rm.EndDate = end;
                var resultCode = (susses) ? MonitorReportingImportService.SuccessImportCode : MonitorReportingImportService.FailedImportCode;
                rm.ReportMonitoringResult = uow.GetRepository<ReportMonitoringResult>()
                    .Filter(f => !f.Hidden && !f.IsHistory && f.Code == resultCode)
                    .FirstOrDefault();
                rm.Consolidation = uow.GetRepository<Consolidation>()
                    .Filter(f => !f.Hidden && f.Code == beCode)
                    .FirstOrDefault();

                uow.GetRepository<ReportMonitoring>().Create(rm);
                uow.SaveChanges();
                monitorService.CreateMonitorReporting(rm, uow);
            }


            return Ok(new
            {
                error = 0,
                message = "",
            });
        }
                
    }
}
