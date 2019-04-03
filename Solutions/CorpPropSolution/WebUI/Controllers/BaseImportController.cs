using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Data.BaseImport.Exceptions;
using Common.Data.Services.Abstract;
using WebUI.Helpers;
using WebUI.Models.ExportImport;


namespace WebUI.Controllers
{
    public class BaseImportController : BaseController
    {
        private readonly Dictionary<FormImport, IBaseImportService> _registryImportServices = new Dictionary<FormImport, IBaseImportService>();
        private readonly List<ImportItemModel> _listImport;
        public BaseImportController(IRolesBaseImportService rolesBaseImportService, IPresetMenuBaseImportService presetMenuBaseImportService,
            IBaseControllerServiceFacade serviceFacade) : base(serviceFacade)
        {
            _registryImportServices.Add(FormImport.Roles, rolesBaseImportService);
            _registryImportServices.Add(FormImport.PresetMenu, presetMenuBaseImportService);

            _listImport = new List<ImportItemModel>()
            {
                new ImportItemModel() {Name="Роли", FileName = "RolesImport.xlsx", FormImport = FormImport.Roles},
                new ImportItemModel() {Name="Пресеты меню",FileName = "PresetMenuImport.xlsx", FormImport = FormImport.PresetMenu}
            };
            foreach (ImportItemModel item in _listImport)
            {
                string path;
                item.Enabled = GetTryPath(item.FileName, out path);
                item.Path = path;
            }
        }

        public ActionResult Index()
        {
            return View(_listImport);
        }

        public JsonNetResult StartImport(FormImport form)
        {
            try
            {
                if (!_registryImportServices.ContainsKey(form))
                    throw new Exception("Данный импорт не доступен!");
                _registryImportServices[form].Import(_listImport
                    .SingleOrDefault(x => x.FormImport == form)?.Path);
                return new JsonNetResult(new {message = "Импорт успешно завершен!"});
            }
            catch (FailImportException e)
            {
                return new JsonNetResult(new { error = $"{e.Message}. Дополнительные сведенья доступны в лог-файле."});
            }
            catch (Exception e)
            {
                return new JsonNetResult(new { error = e });
            }
        }

        private bool GetTryPath(string fileName, out string pathFile)
        {
            pathFile = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", fileName);
            return System.IO.File.Exists(pathFile);
        }
    }
}