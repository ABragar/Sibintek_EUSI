using Base;
using Base.DAL;
using Base.Service;
using Base.UI;
using Base.Utils.Common;
using EUSI.Entities.Accounting;
using EUSI.Entities.Estate;
using EUSI.Export;
using System;
using System.Linq;
using System.Web.Http;
using Base.Enums;
using WebApi.Attributes;
using WebApi.Controllers;
using WebApi.Models.Crud;
using Base.Service.Log;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер выгрузки движений.
    /// </summary>
    [CheckSecurityUser]
    internal class ExportZipController : BaseApiController
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IAccessService _accessService;
        private readonly ILogService _logger;

        public ExportZipController(
            IViewModelConfigService viewModelConfigService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IFileSystemService fileSystemService,
            IAccessService accessService,
            ILogService logger
            )
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _fileSystemService = fileSystemService;
            _accessService = accessService;
        }

        [HttpPost]
        [Route("eusi/exportZip/{mnemonic}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Export<T>(string mnemonic, [FromBody]SaveModel<T> save_model) where T : BaseObject
        {
            var sm = save_model as SaveModel<ExportZip>;
            if (sm?.model?.Consolidations == null || sm.model.StartDate == null || sm.model.EndDate == null)
                return Ok(new
                {
                    error = 1,
                    message = "Неверный набор параметров.",
                });
            try
            {
                var obj = sm.model;
                obj.StartDate = obj.StartDate?.Date;
                obj.EndDate = obj.EndDate?.Date;
                using (IUnitOfWork uow = CreateUnitOfWork())
                {
                    var export = new AccObjectExport(uow, _fileSystemService);
                    var consalidationCodes = sm.model.Consolidations.Select(c => c.Object.Code);
                    var consalidationINNs = sm.model.Consolidations.Select(c => c.Object.INN);

                    //включать ранее выгруженные? Да- т.е. все
                    var ers = (obj.NotIncludeTransferBus) ?
                        uow.GetRepository<EstateRegistration>()
                        .FilterAsNoTracking(er =>
                            er.State != null
                            && er.State.Code == "COMPLETED"
                            && er.ERControlDateAttributes != null
                            && er.ERControlDateAttributes.DateVerification.Value.Date >= obj.StartDate.Value.Date
                            && er.ERControlDateAttributes.DateVerification.Value.Date <= obj.EndDate.Value.Date
                            && ((er.ERType.Code == "OSVGP" && er.ERReceiptReason != null && er.ERReceiptReason.Code == "RentOut" && er.Contragent != null && consalidationINNs.Contains(er.Contragent.INN)) //это вгп аренда
                            || (!(er.ERType.Code == "OSVGP" && er.ERReceiptReason != null && er.ERReceiptReason.Code == "RentOut") && er.Consolidation != null && consalidationCodes.Contains(er.Consolidation.Code))) //все остальное

                            )
                        .Select(s => s.ID)

                        //нет - только не выгруженные
                        : uow.GetRepository<EstateRegistration>()
                        .FilterAsNoTracking(er =>
                            er.State != null
                            && er.State.Code == "COMPLETED"
                            && er.ERControlDateAttributes != null
                            && er.ERControlDateAttributes.DateVerification.Value.Date >= obj.StartDate.Value.Date
                            && er.ERControlDateAttributes.DateVerification.Value.Date <= obj.EndDate.Value.Date
                            && ((er.ERType.Code == "OSVGP" && er.ERReceiptReason != null && er.ERReceiptReason.Code == "RentOut" && er.Contragent != null && consalidationINNs.Contains(er.Contragent.INN)) //это вгп аренда
                            || (!(er.ERType.Code == "OSVGP" && er.ERReceiptReason != null && er.ERReceiptReason.Code == "RentOut") && er.Consolidation != null && consalidationCodes.Contains(er.Consolidation.Code))) //все остальное

                            && !er.TransferBUS)
                        .Select(s => s.ID)
                        ;

                    if (ers == null || (ers != null && ers.Count() == 0))
                        return Ok(new
                        {
                            error = 1,
                            message = "Нет данных для выгрузки. Проверьте параметры запроса.",
                        });
                    
                    var emptyeusinumbers = 0;
                    var fileContent = export.Export(ers.ToArray(), ref emptyeusinumbers);
                    if (string.IsNullOrEmpty(fileContent))
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

        [HttpPost]
        [Route("eusi/exportZipOS/{mnemonic}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult ExportOS<T>(string mnemonic, [FromBody] SaveModel<T> save_model) where T : BaseObject
        {
            var sm = save_model as SaveModel<ExportZip>;

            if (sm?.model?.Consolidations == null)
            {
                return Ok(new
                {
                    error = 1,
                    message = "Неправильный набор вводимых параметров.",
                });
            }
            
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    _accessService.ThrowIfAccessDenied(uow, typeof(MigrateOS), TypePermission.Read);

                    var export = new AccObjectExport(uow, _fileSystemService, true);
                    string fileContent = export.GetContentForExportOs(sm.model);

                    if (string.IsNullOrEmpty(fileContent))
                    {
                        return Ok(new
                        {
                            error = 1,
                            message = "Нет данных для выгрузки. Проверьте параметры запроса.",
                        });
                    }

                    return Ok(new
                    {
                        mimetype = "application/zip",
                        filename = $"{DateTime.Now.Date:ddMMyyyy}_ExportOS.zip",
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
