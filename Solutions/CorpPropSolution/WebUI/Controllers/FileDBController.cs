using Base.ExportImport.Services.Abstract;
using Base.Utils.Common.Wrappers;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Services.Document;
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
    //SIB
    /// <summary>
    /// Контроллер для сохранения файлов в БД.
    /// </summary>
    public class FileDBController : BaseController
    {
        
        private readonly IFileDBService _fileDBService;
        private readonly IBaseControllerServiceFacade _bcFacade;


        public FileDBController(
            IBaseControllerServiceFacade facade
            , IFileDBService fileDBService
           )
            : base(facade)
        {
            _bcFacade = facade;
            _fileDBService = fileDBService;
           
        }

        [HttpPost]
        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {            
            int fail = 0;
            string errtext = "";
            FileDB fd = null;
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    HttpPostedFileBase file = null;

                    if (Request.Files.Count > 0)
                    {
                        file = Request.Files[0];

                        if (file == null || file.ContentLength == 0)
                            throw new FileLoadException("Файл пуст или не указан.");

                        //var historySession = CreateUnitOfWork();
                        //var impHistory = ImportHelper.CreateImportHistory(historySession, file.FileName, this.SecurityUser?.ID);

                        var wrapp = DependencyResolver.Current.GetService<IPostedFileWrapper>();
                        wrapp.SetItem(file);

                        fd = _fileDBService.SaveFile(uow, wrapp);
                        errtext = fd.Name;

                    }
                    
                }
            }
            catch (Exception ex)
            {
                fail = 1;
                errtext = ex.Message;
            }

            var res = new
            {
                err = fail,
                text = errtext,
                file = fd
            };
            return new JsonNetResult(res);
        }
        

    }
}