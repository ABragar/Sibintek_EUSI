using Base;
using Base.DAL;
using Base.Service;
using Base.UI;
using Base.Utils.Common;
using System;
using System.Linq;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Controllers;
using EUSI.Export;
using WebApi.Models.Crud;
using EUSI.Entities.Accounting;
using EUSI.Entities.Estate;
using CorpProp.Entities.Import;
using System.Collections.Generic;
using CorpProp.Common;
using Base.Service.Log;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер выгрузки движений.
    /// </summary>
    [CheckSecurityUser]
    [RoutePrefix("eusi/rentalOSExportZip/{mnemonic}")]
    internal class RentalOSExportZipController : BaseApiController
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly ILogService _logger;

        public RentalOSExportZipController(
            IViewModelConfigService viewModelConfigService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IFileSystemService fileSystemService,
            ILogService logger
            )
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _fileSystemService = fileSystemService;
        }

        [HttpPost]
        [Route("")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Export<T>(string mnemonic, string ids) where T : BaseObject
        {
            try
            {
                if (String.IsNullOrEmpty(ids))
                    return Ok(new
                    {
                        err = 1,
                        message = "Выберите объект для экспорта",
                    });

                string message = "";
                var exportData = new List<string>();
                using (ITransactionUnitOfWork uow = CreateTransactionUnitOfWork())
                {
                    List<int> srcIdsList = ids.Split(',').Select(x => int.Parse(x)).ToList<int>();
                    var iddss = uow.GetRepository<ImportHistory>()
                        .FilterAsNoTracking(f => srcIdsList.Contains(f.ID) && f.IsSuccess && f.FileCardID != null)
                        .Select(s => s.FileCardID.Value);

                    if (!iddss.Any())
                    {
                        return Ok(new
                        {
                            err = 1,
                            message = "Экспортировать можно только файлы, прошедшие успешный импорт.",
                        });
                    }

                    var config = ViewModelConfigService.Get(nameof(RentalOS));
                    if (config != null && config.ServiceType != null &&
                        config.ServiceType.GetInterfaces().Contains(typeof(IExportToZip)))
                    {
                        var service = config.GetService<EUSI.Services.Accounting.RentalOSService>();
                        if (service != null)
                        {
                            var val = service.CustomExportToZip(uow, srcIdsList.ToArray());
                            if (!String.IsNullOrEmpty(val))
                                exportData.Add(val);
                        }
                    }
                }
                if (exportData != null && exportData.Count > 0)
                {
                    return Ok(new
                    {
                        err = 0,
                        message = message,
                        mimetype = "application/zip",
                        filename = "export.zip",
                        datas = exportData
                    });
                }

                return Ok(new
                {
                    err = 0,
                    message = message
                });

            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    err = 1,
                    message = ex.ToStringWithInner(),
                });
            }
        }
    }
}
