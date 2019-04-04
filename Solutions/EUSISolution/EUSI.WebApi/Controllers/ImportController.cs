using Base;
using Base.DAL;
using Base.Security;
using Base.Service;
using Base.Service.Log;
using Base.UI;
using Base.UI.Service;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Helpers;
using EUSI.WebApi.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Controllers;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер проверки версий файлов.
    /// </summary>
    [CheckSecurityUser]
    [RoutePrefix("eusi/import")]
    internal class ImportController : BaseApiController
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IAccessService _accessService;
        private readonly IUiFasade _uiFasade;
        private ISecurityUser _securityUser;
        private readonly IExcelImportChecker _checker;
        

        public ISecurityUser SecurityUser => _securityUser ?? (_securityUser = Base.Ambient.AppContext.SecurityUser);
        private readonly ILogService _logger;

        public ImportController(
            IViewModelConfigService viewModelConfigService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IFileSystemService fileSystemService,
            IAccessService accessService,
            IUiFasade uiFasade,
            IExcelImportChecker checker,
            ILogService logger
            ) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _fileSystemService = fileSystemService;
            _accessService = accessService;
            _uiFasade = uiFasade;
            _checker = checker;
           
        }

        [HttpPost]
        [Route("checkFileVersions/{fileCardIds}")]
        public IHttpActionResult CheckDocumentVersions(string fileCardIds)
        {
            var result = new List<CheckFileVersionResult>();
            var checkResult = new Dictionary<string, ProcessingCheckImportFile>();
            try
            {
                if (String.IsNullOrEmpty(fileCardIds))
                    throw new Exception("Не выбраны файлы для загрузки.");

                List<int> srcIdsList = fileCardIds.Split(',').Select(x => int.Parse(x)).ToList<int>();



                using (ITransactionUnitOfWork uofw = CreateTransactionUnitOfWork())
                {
                    foreach (int itemId in srcIdsList)
                    {
                        var historySession = CreateUnitOfWork();

                        var fileCard = historySession.GetRepository<FileCardOne>().Find(f => f.ID == itemId);

                        if (fileCard == null)
                            throw new Exception("Не удалось получить данные о выбранном файле.");

                        FileData fileData = fileCard.FileData;
                        if (fileData.Extension != "XLS" && fileData.Extension != "XLSX")
                            continue;

                        string filePath = _fileSystemService.GetFilePath(fileData.FileID);

                        using (StreamReader stream = new StreamReader(filePath))
                        {
                            var impHistory = ImportHelper.CreateImportHistory(historySession, fileCard.FileData.FileName, this.SecurityUser?.ID);

                            var reader = fileData.Extension == "XLSX" ?
                                ExcelReaderFactory.CreateOpenXmlReader(stream.BaseStream) :
                                ExcelReaderFactory.CreateBinaryReader(stream.BaseStream);

                            var fileMnemonic = ImportHelper.FindTypeName(reader);

                            var checkImportResult = _checker.CheckVersionImport(_uiFasade, reader, uofw, stream, impHistory.FileName);

                            if (checkImportResult == null || (checkImportResult != null && !checkImportResult.IsConfirmationRequired && !checkImportResult.IsError))
                            {
                                continue;
                            }

                            var fileDescription = checkImportResult.ConfirmationItemDescription;

                            if (checkResult.ContainsKey(fileMnemonic))
                            {
                                checkResult[fileMnemonic].FileCardIds.Add(itemId);
                                checkResult[fileMnemonic].FileDescriptions.Add(fileDescription);
                            }
                            else
                            {
                                checkResult.Add(fileMnemonic, new ProcessingCheckImportFile
                                {
                                    FileCardIds = new List<int> { itemId },
                                    FileDescriptions = new List<string> { fileDescription },
                                    Checker = checkImportResult.Checker
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                checkResult.Add("", new ProcessingCheckImportFile
                {
                    FileCardIds = new List<int> {},
                    FileDescriptions = new List<string> {}
                });
                foreach (var processingItem in checkResult)
                {
                    result.Add(new CheckFileVersionResult
                    {
                        FileCardIds = processingItem.Value.FileCardIds,
                        ErrorMessage = ex.Message,
                        IsError = true
                    });
                    break;
                }
                return Ok(new
                {

                    confirmationItems = result
                });
            }


                foreach (var processingItem in checkResult)
                {
                    result.Add(new CheckFileVersionResult
                    {
                        FileCardIds = processingItem.Value.FileCardIds,
                        ConfirmMessage = ((processingItem.Value.Checker != null) ? processingItem.Value.Checker.FormatConfirmImportMessage(processingItem.Value.FileDescriptions) :
                        _checker.FormatConfirmImportMessage(processingItem.Value.FileDescriptions))
                    });

                }
            

            return Ok(new
            {
                confirmationItems = result
            });
        }
    }
}