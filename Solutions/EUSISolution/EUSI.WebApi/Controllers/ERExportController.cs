using System;
using System.Linq;
using WebHttp = System.Web.Http;
using Base.DAL;
using Base.Service;
using Base.UI;
using Base.Utils.Common;
using WebApi.Attributes;
using WebApi.Controllers;
using Base.BusinessProcesses.Entities;
using EUSI.Export;
using Base.Service.Log;

namespace EUSI.WebApi.Controllers
{
    [CheckSecurityUser]
    [WebHttp.RoutePrefix("eusi/estateRegistrationExport")]
    internal class ERExportController : BaseApiController
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILogService _logger;

        public ERExportController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IFileSystemService fileSystemService, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _fileSystemService = fileSystemService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Экспорт ОС в Excel.
        /// </summary>
        /// <param name="elementsIds">ID элементов.</param>
        /// <param name="isAccountingObject">Признак определяющий, будут ли передаваемые ID являться ID ОСов.</param>
        /// <returns>Архив с Excel-файлами.</returns>
        [WebHttp.HttpGet]
        [WebHttp.Route("export/{elementsIds}/{isAccountingObject}")]
        public WebHttp.IHttpActionResult Export(string elementsIds, bool isAccountingObject = false)
        {
            try
            {
                if (String.IsNullOrEmpty(elementsIds))
                    throw new Exception("Не указаны элементы.");

                int[] idList = elementsIds.Split(',').Select(x => int.Parse(x)).ToArray<int>();
                using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
                {
                    AccObjectExport export = new AccObjectExport(unitOfWork, _fileSystemService, isAccountingObject);
                    int emptyEusiNumbers = 0;
                    var fileContent = export.Export(idList, ref emptyEusiNumbers);

                    return Ok(new
                    {
                        mimetype = "application/zip",
                        filename = "export.zip",
                        data = fileContent,
                        emptyEusiNumbers
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

        /// <summary>
        /// Получение информации о шаге БП.
        /// </summary>
        /// <param name="actionId">ID шага.</param>
        /// <returns>Информация о шаге.</returns>
        [WebHttp.HttpGet]
        [WebHttp.Route("getWfAction/{actionID}/")]
        public WebHttp.IHttpActionResult GetWorkflowAction(int actionId)
        {
            var intError = 0;
            var strMsg = "";
            try
            {
                using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
                {
                    StageAction action = unitOfWork.GetRepository<StageAction>().All().FirstOrDefault(x => x.ID == actionId);
                    if (action == null)
                        throw new Exception("Не удалось найти действие");

                    return Ok(new
                    {
                        error = intError,
                        actionInfo = new
                        {
                            name = action.Title,
                            sysName = action.SystemName,
                            id = action.ID
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                intError = 1;
                strMsg = ex.ToStringWithInner();
            }

            return Ok(new { error = intError, message = strMsg });
        }

    }
}
