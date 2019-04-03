using Base.ExportImport.Services.Abstract;
using Base.Utils.Common.Wrappers;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Helpers;
using WebUI.Models.CorpProp.ImportExport;

namespace WebUI.Controllers
{
    public class ImportExportExcelController : BaseController
    {
        private readonly IPackageService _packageService;
        private readonly IExportImportManager _exportImportManager;
        private readonly IBaseControllerServiceFacade _bcFacade;

       
        public ImportExportExcelController(IBaseControllerServiceFacade facade, IPackageService packageService, IExportImportManager exportImportManager)
            : base(facade)
        {
            _bcFacade = facade;
            _packageService = packageService;
            _exportImportManager = exportImportManager;
        }

        public ActionResult GetImportView(string mnemonic)
        {
            ImportExcelVm vm = new ImportExcelVm { Mnemonic = mnemonic };
            return PartialView("~/Views/CorpProp/ImportExport/ImportExcel.cshtml", vm);
        }

        /*[HttpPost]
        public ActionResult ImportExcel(IEnumerable<HttpPostedFileBase> files, string mnemonic)
        {
            string report = "";
            int fail = 0;
            using (var uow = CreateUnitOfWork())
            {
                HttpPostedFileBase file = null;
                
                if (Request.Files.Count > 0)
                {
                    file = Request.Files[0];

                    if (file == null || file.ContentLength == 0)
                        throw new FileLoadException("Файл пуст или не указан.");

                    var historySession = CreateUnitOfWork();
                    var impHistory = ImportHelper.CreateImportHistory(historySession, file.FileName, this.SecurityUser?.ID);

                    var wrapp = DependencyResolver.Current.GetService<IPostedFileWrapper>();
                    wrapp.SetItem(file);

                    Base.FileData fd = FileSystemService.SaveFile(wrapp);

                    FileCardOne doc =  historySession.GetRepository<FileCardOne>().Create( new FileCardOne()
                    {
                        //TODO: установить папку ПХД по умолчанию
                        CategoryID = 1,
                        CreateDate = DateTime.Now,
                        ImportDate = DateTime.Now,
                        Name = file.FileName,
                        DateCard = DateTime.Now,
                        FileData = fd
                    });

                    impHistory.FileCard = doc;

                    int count = 0;
                    string error = "";

                    IExcelDataReader reader = null;

                    if (file.FileName.Contains(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(file.InputStream);
                    }
                    else
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(file.InputStream);
                    }

                    new ImportStarter().Import(_bcFacade.UiFasade, uow, historySession, reader, ref error, ref count, ref impHistory);

                    if (impHistory.ImportErrorLogs.Count > 0)
                    {
                        impHistory.ResultText += $"{_errText}{System.Environment.NewLine}Всего обработано объектов: {count}. {System.Environment.NewLine}";
                        fail = 1;
                    }
                    else
                    {
                        impHistory.ResultText += $"{_goodText}{System.Environment.NewLine}Всего обработано объектов: {count}. {System.Environment.NewLine}";
                    }

                    report = impHistory.ResultText;
                    historySession.GetRepository<ImportHistory>().Create(impHistory);
                    historySession.SaveChanges();
                }
                var res = new {
                    error = fail,
                    importReport = report
                };
                return new JsonNetResult(res);
            }

        }*/


        public ActionResult GetImportXmlView(string mnemonic)
        {
            ImportExcelVm vm = new ImportExcelVm { Mnemonic = mnemonic };
            return PartialView("~/Views/CorpProp/ImportExport/ImportXML.cshtml", vm);
        }

        /*[HttpPost]
        public ActionResult ImportXml(IEnumerable<HttpPostedFileBase> files, string mnemonic)
        {
            int fail = 0;
            string report = _goodText; 
            try
            {               
                using (var uow = CreateUnitOfWork())
                {
                    HttpPostedFileBase file = null;

                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {                           
                            file = Request.Files[i];
                            if (file == null)
                                throw new Exception("Файл пуст!");
                            var uowHistory = CreateUnitOfWork();
                            string tx = "";

                            var wrap = DependencyResolver.Current.GetService<IPostedFileWrapper>();
                            wrap.SetItem(file);
                            if (CorpProp.RosReestr.Helpers.ImportLoader.Import(wrap, FileSystemService, uow, uowHistory, file.InputStream, file.FileName, this.SecurityUser?.ID))
                                tx = _goodText;
                            else
                            {
                                tx = _errText;
                                fail = 1;
                            }                                
                            if (Request.Files.Count == 1)
                                report = tx;
                            else
                                report += $"{file.FileName} - {tx}{System.Environment.NewLine}";                            
                        }                       
                    }                  
                }
                var res = new
                {
                    error = fail,
                    importReport = report
                };
                return new JsonNetResult(res);
            }
            catch (Exception ex)
            {
                var res = new
                {
                    error = 1,
                    importReport = report
                };
                return new JsonNetResult(res);
            }

        }*/
    }
}