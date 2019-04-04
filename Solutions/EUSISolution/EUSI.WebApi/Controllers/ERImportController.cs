using Base;
using Base.DAL;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.Service.Log;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Service;
using Base.Utils.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Settings;
using EUSI.Entities.Estate;
using EUSI.Services.Estate;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Controllers;
using WebApi.Models.Crud;
using WebHttp = System.Web.Http;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер импорта заявок ЕУСИ.
    /// </summary>
    [CheckSecurityUser]
    [WebHttp.RoutePrefix("eusi/importER/{mnemonic}")]
    internal class ERImportController : BaseApiController
    {
        private readonly IUiFasade _uiFasade;
        private readonly IAutoMapperCloner _cloner;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly IDvSettingService<DvSettingForType> _dvSettingService;
        private readonly IDvSettingManager _dvSettingManager;
        private readonly INotificationService _notificationService;
        private readonly IFileSystemService _fileSystemService;
        private readonly ISecurityService _security_service;
        private ISecurityUser _securityUser;
        private ISibEmailService _emailService;
        private readonly IEstateRegistrationService _estateRegistrationService;
        private readonly ILogService _logger;

        public ISecurityUser SecurityUser => _securityUser ?? (_securityUser = Base.Ambient.AppContext.SecurityUser);

        public ERImportController(
            IViewModelConfigService viewModelConfigService
            , IUnitOfWorkFactory unitOfWorkFactory
            , IUiFasade uiFasade
            , Base.IAutoMapperCloner cloner
            , IDvSettingService<DvSettingForType> dvSettingService
            , IDvSettingManager dvSettingManager
            , INotificationService notificationService
            , IFileSystemService fileSystemService
            , ISecurityService security_service
            , ISibEmailService emailService,
            IEstateRegistrationService estateRegistrationService
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
            _security_service = security_service;
            _emailService = emailService;
            _estateRegistrationService = estateRegistrationService;
        }
        
      
        [HttpPost]
        [Route("")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Import<T>(string mnemonic, [FromBody]SaveModel<T> save_model) where T : BaseObject
        {
            var sm = save_model as SaveModel<ERImportWizard>;
            if (sm?.model?.FileCard == null)
                return Ok(new
                {
                    error = 1,
                    message = "Выберите файл импорта.",
                });
            int fail = 0;
            ERImportWizard wizard = sm?.model;
            var itemId = wizard.FileCard?.ID;
            const string _errText = "Завершено с ошибками.";
            const string _goodText = "Импорт завершен.";
            string report = "";
            List<string> exportData = new List<string>();

            try
            {                
                List<Guid> historyOids = new List<Guid>();
                var historySession = CreateUnitOfWork();
                int count = 0;
                              
                using (ITransactionUnitOfWork uofw = CreateTransactionUnitOfWork())
                {
                    var fileCard = historySession.GetRepository<FileCardOne>().Find(f => f.ID == itemId);

                    if (fileCard == null)
                        throw new Exception("Не удалось получить данные о выбранном файле.");

                    FileData fileData = fileCard.FileData;
                    string filePath = _fileSystemService.GetFilePath(fileData.FileID);
                    if (fileData.Extension != "XLS" && fileData.Extension != "XLSX")
                        return Ok(new
                        {
                            error = 1,
                            message = @"Разрешенные форматы файлов импорта: *.XLS, *.XLSX"
                        });
                    
                    report += $"Имя файла: { fileData.FileName}. {System.Environment.NewLine}";
                    using (StreamReader stream = new StreamReader(filePath))
                    {
                        IExcelDataReader reader = null;
                        var impHistory = ImportHelper.CreateImportHistory(historySession, fileCard.FileData.FileName, this.SecurityUser?.ID);
                        historyOids.Add(impHistory.Oid);
                        impHistory.FileCard = fileCard;

                        if (fileData.Extension == "XLSX")
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream.BaseStream);
                        else
                            reader = ExcelReaderFactory.CreateBinaryReader(stream.BaseStream);
                        var tables = reader.GetVisbleTables();
                        DataTable entryTable = tables[0];
                        impHistory.Mnemonic = ImportHelper.FindTypeName(reader);
                        
                        if (ImportHelper.CheckRepeatImport(uofw, stream.BaseStream))
                        {
                            impHistory.ImportErrorLogs.AddError("Идентичный файл импортировался ранее.");
                            //контрольные процедуры импорта не пройдены, отправляем уведомление ЦДС 
                            _emailService.SendNotice(uofw, new EstateRegistration(), null, "", "ER_BadImportZDS");                            
                        }                            
                        else
                        {
                            Dictionary<string, string> colsMapping = ImportHelper.ColumnsNameMapping(entryTable);
                            ((EstateRegistrationService)_estateRegistrationService)
                                .CustomImport(uofw, historySession, entryTable, colsMapping, wizard, ref count, ref impHistory);
                        }

                        if (impHistory.ImportErrorLogs.Count > 0)
                        {
                            impHistory.ResultText += $"{_errText} Всего обработано объектов: {count}. {System.Environment.NewLine}";
                            impHistory.IsSuccess = false;
                            fail = 1;
                            uofw.Rollback();
                        }
                        else
                        {                            
                            impHistory.ResultText += $"{_goodText} Всего обработано объектов: {count}. {System.Environment.NewLine}";
                            impHistory.IsSuccess = true;
                            uofw.Commit();
                        }

                        if (impHistory.FileCard != null)
                        {
                            impHistory.FileCard.Description += System.Environment.NewLine;
                            impHistory.FileCard.Description += $"Импорт файла от {impHistory.ImportDateTime.ToString()}:";
                            impHistory.FileCard.Description += $" {impHistory.ResultText}";
                        }
                        report += impHistory.ResultText;
                        historySession.GetRepository<ImportHistory>().Create(impHistory);
                    }

                    historySession.SaveChanges();
                    

                }

            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
            return Ok(new
            {
                error = fail,
                message = report
            });

        }


    }
}
