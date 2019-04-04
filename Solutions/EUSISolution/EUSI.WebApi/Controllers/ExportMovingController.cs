using Base;
using Base.DAL;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Service;
using Base.Utils.Common;
using CorpProp.Entities.Document;
using EUSI.Import;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Http;
using EUSI.Helpers;
using WebApi.Attributes;
using WebApi.Controllers;
using WebHttp = System.Web.Http;
using CorpProp.Entities.NSI;
using EUSI.Services.Accounting;
using EUSI.Export;
using CorpProp.Extentions;
using WebApi.Models.Crud;
using EUSI.Entities.Accounting;
using CorpProp.Services.Document;
using Base.Service.Log;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер выгрузки движений.
    /// </summary>
    [CheckSecurityUser]  
    [WebHttp.RoutePrefix("eusi/exportMoving/{mnemonic}")]
    internal class ExportMovingController : BaseApiController
    {
        private readonly IUiFasade _uiFasade;
        private readonly IAutoMapperCloner _cloner;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly IDvSettingService<DvSettingForType> _dvSettingService;
        private readonly IDvSettingManager _dvSettingManager;
        private readonly INotificationService _notificationService;
        private readonly IFileSystemService _fileSystemService;
        private readonly IFileDBService _fileDBService;
        private readonly ISecurityService _security_service;
        private ISecurityUser _securityUser;
        private IEmailService _emailService;
        private readonly IExportMovingService _exportMovingService;
        private readonly ILogService _logger;

        public ISecurityUser SecurityUser => _securityUser ?? (_securityUser = Base.Ambient.AppContext.SecurityUser);

        public ExportMovingController(
            IViewModelConfigService viewModelConfigService
            , IUnitOfWorkFactory unitOfWorkFactory
            , IUiFasade uiFasade
            , Base.IAutoMapperCloner cloner
            , IDvSettingService<DvSettingForType> dvSettingService
            , IDvSettingManager dvSettingManager
            , INotificationService notificationService
            , IFileSystemService fileSystemService
            , IFileDBService fileDBService
            , ISecurityService security_service
            , IEmailService emailService
            , IExportMovingService exportMovingService
            , ILogService logger)
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _viewModelConfigService = viewModelConfigService;
            _uiFasade = uiFasade;
            _cloner = cloner;
            _dvSettingService = dvSettingService;
            _dvSettingManager = dvSettingManager;
            _notificationService = notificationService;
            _fileSystemService = fileSystemService;
            _fileDBService = fileDBService;
            _security_service = security_service;
            _emailService = emailService;
            _exportMovingService = exportMovingService;
        }


        [HttpPost]
        [WebHttp.Route("")]
        [GenericAction("mnemonic")]
        public WebHttp.IHttpActionResult Export<T>(string mnemonic, [FromBody]SaveModel<T> save_model) where T : BaseObject
        {
            var sm = save_model as SaveModel<ExportMoving>;
            if (sm == null || sm.model == null 
                || sm.model.Consolidations == null
                || sm.model.StartDate == null
                || sm.model.EndDate == null)
                return Ok(new
                {
                    error = 1,
                    message = "Неверный набор параметров.",
                });
            try
            {
                var obj = sm.model;
                using (IUnitOfWork uow = CreateUnitOfWork())
                {                   
                    var export = new AccMovingsExport(_fileDBService, _fileSystemService, uow, obj.Consolidations, obj.StartDate, obj.EndDate);
                    var fileContent = export.Export();
                    if (String.IsNullOrEmpty(fileContent))
                        return Ok(new
                        {
                            error = 1,
                            message = "Нет данных для выгрузки. Проверьте параметры запроса.",
                        });

                    return Ok(new
                    {
                        mimetype = "application/zip",
                        filename = "export.zip",
                        data = fileContent
                    });

                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner(),
                });
            }
        }

    }
}
