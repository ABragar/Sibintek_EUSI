using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Base;
using Base.Utils.Common.Wrappers;
using Base.ExportImport.Services.Abstract;
using Base.Service;
using Base.Service.Crud;
using Base.UI.Extensions;
using Base.Utils.Common;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebUI.Extensions;
using WebUI.Helpers;
using WebUI.Models.ExportImport;


namespace WebUI.Controllers
{

    [Obsolete]
    // => web api
    public class ExportImportController : BaseController
    {
        private readonly IPackageService _packageService;
        private readonly IExportImportManager _exportImportManager;        

        public ExportImportController(IBaseControllerServiceFacade facade, IPackageService packageService, IExportImportManager exportImportManager)
            : base(facade)
        {
            _packageService = packageService;
            _exportImportManager = exportImportManager;           
        }

        public FileResult ExportJSON(string mnemonic, int objectID)
        {
            using (var uow = CreateUnitOfWork())
            {
                var service = GetService<IExportImportObject>(mnemonic);
                string result = service.Export(uow, objectID);

                FileContentResult cresult = new FileContentResult(Encoding.UTF8.GetBytes(result), "text/json")
                {
                    FileDownloadName = $"{mnemonic}-{objectID}.json"
                };
                return cresult;
            }
        }

        public ActionResult GetImportWindow(string mnemonic)
        {
            ImportVm vm = new ImportVm { Mnemonic = mnemonic };
            return PartialView("_Import", vm);
        }

        [HttpPost]
        public ActionResult ImportJSON(IEnumerable<HttpPostedFileBase> files, string mnemonic)
        {
            using (var uow = CreateUnitOfWork())
            {
                HttpPostedFileBase file = null;

                if (Request.Files.Count > 0)
                {
                    file = Request.Files[0];

                    if (file == null)
                        throw new Exception("File is null");

                    var service = GetService<IExportImportObject>(mnemonic);

                    using (StreamReader reader = new StreamReader(file.InputStream))
                    {
                        service.Import(uow, reader.ReadToEnd());
                    }
                }

                var wrapp = DependencyResolver.Current.GetService<IPostedFileWrapper>();
                wrapp.SetItem(file);

                return new JsonNetResult(FileSystemService.SaveFile(wrapp));
            }

        }

        public JsonNetResult GetPackages(string mnemonic)
        {
            using (var uow = CreateUnitOfWork())
            {
                var config = GetViewModelConfig(mnemonic);

                var packages = _packageService.GetPackags(uow, config.TypeEntity.GetTypeName());

                return new JsonNetResult(packages.Select(x => new { x.Title, x.ID }));
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonNetResult ExportXML([DataSourceRequest] DataSourceRequest request, string mnemonic, int? categoryID, bool? allItems, string searchStr, string extrafilter, int? parentID, int packageID)
        {
            var serv = this.GetService<IQueryService<object>>(mnemonic);
            var config = this.GetViewModelConfig(mnemonic);

            using (var uow = CreateUnitOfWork())
            {
                IQueryable q = null;

                var service = serv as ICategorizedItemCrudService;
                if (service != null)
                {
                    if (allItems ?? false)
                    {
                        q = service.GetAllCategorizedItems(uow, categoryID ?? 0);
                    }
                    else
                    {
                        q = service.GetCategorizedItems(uow, categoryID ?? 0);
                    }
                }
                else
                {
                    q = serv.GetAll(uow);
                }

                q = q.Filter(this, uow, config, extrafilter);

                q = q.FullTextSearch(searchStr, CacheWrapper);

                var package = _packageService.Get(uow, packageID);

                var t = q.Select(config.ListView).ToDataSourceResult(request);

                var stream = _exportImportManager.GetExportStream(t.Data, package);

                return new JsonNetResult(new
                {
                    mimetype = "text/xml",
                    filename = "doc.xml",
                    data = Convert.ToBase64String(stream.ToArray())
                });
            }
        }

        [HttpPost]
        public ActionResult ExportSave(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}