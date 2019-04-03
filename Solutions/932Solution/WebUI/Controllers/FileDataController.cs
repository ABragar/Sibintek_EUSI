using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Base;
using WebUI.Helpers;
using Base.Service;
using Base.FileStorage;
using System.IO;
using System.Threading.Tasks;
using Base.Utils.Common.Caching;
using Base.Utils.Common.Wrappers;
using Base.Word.Services.Abstract;
using WebUI.Extensions;

namespace WebUI.Controllers
{
    public class FileDataController : BaseController
    {
        private readonly IFileStorageItemService _fileStorageItemService;
        private readonly IWordService _wordService;
        private readonly IServiceFactory<IPostedFileWrapper> _posted_file_wrapper_service_factory;
        private readonly IServiceFactory<IPathHelper> _path_helper_service_factory;

        public FileDataController(IBaseControllerServiceFacade baseServiceFacade,
            IFileStorageItemService fileStorageItemService,
            IWordService wordService,
            IServiceFactory<IPostedFileWrapper> posted_file_wrapper_service_factory,
            IServiceFactory<IPathHelper> path_helper_service_factory)
            : base(baseServiceFacade)
        {
            _fileStorageItemService = fileStorageItemService;
            _wordService = wordService;
            _posted_file_wrapper_service_factory = posted_file_wrapper_service_factory;
            _path_helper_service_factory = path_helper_service_factory;
        }

        [HttpPost]
        public void SaveImage(HttpPostedFileBase file)
        {
            var test = Request.Files;

        }

        public JsonNetResult SaveFile()
        {
            HttpPostedFileBase file = null;

            if (Request.Files.Count > 0)
            {
                file = Request.Files[0];
            }

            var wrapp = _posted_file_wrapper_service_factory.GetService();
            wrapp.SetItem(file);

            return new JsonNetResult(FileSystemService.SaveFile(wrapp));
        }

        public JsonNetResult SaveFiles()
        {
            var res = new List<FileData>();

            if (Request.Files.Count == 0) return new JsonNetResult(res);

            var wrapp = _posted_file_wrapper_service_factory.GetService();

            foreach (string key in Request.Files)
            {
                wrapp.SetItem(Request.Files[key]);

                var fd = FileSystemService.SaveFile(wrapp);

                if (fd != null)
                {
                    res.Add(fd);
                }
            }

            return new JsonNetResult(res);
        }

        public JsonNetResult DeleteFiles(string[] fileNames, bool isNewObject)
        {
            return new JsonNetResult(null);
        }

        [DeleteTempFileFilter]
        public ActionResult GetTempFile(string id, string fileName)
        {
            var pathHelper = _path_helper_service_factory.GetService();

            string filePath = Path.Combine(pathHelper.GetTempDirectory(), id);

            return File(filePath, MimeMapping.GetMimeMapping(filePath), fileName);
        }

        public JsonNetResult GetWidget(int id)
        {
            FileStorageItem fileStorageItem;

            using (var uow = CreateUnitOfWork())
            {
                fileStorageItem = _fileStorageItemService.GetFileStorageItem(uow, id);
            }

            string extension = "";

            if (fileStorageItem != null && fileStorageItem.File != null)
            {
                string ext = Path.GetExtension(fileStorageItem.File.FileName);

                if (ext != null)
                    extension = ext.Replace(".", "").ToLower();
            }

            string view = "Unknown";

            var vd = new ViewDataDictionary();

            switch (extension)
            {
                case "gif":
                case "jpeg":
                case "jpg":
                case "png":
                case "tif":
                case "tiff":
                    view = "Image";
                    break;
                //case "pdf":
                //    view = "Book";
                //    break;

                case "wmv":
                case "mp4":
                case "avi":
                    view = "Video";
                    break;

                    //case "wma":
                    //case "mp3":
                    //    view = "Audio";
                    //    break;
            }

            return new JsonNetResult(new
            {
                html = this.RenderPartialViewToString(view, fileStorageItem, vd)
            });
        }

        
        private static readonly CacheAccessor<string> ShowDocGroup = new CacheAccessor<string>();
        public ActionResult ShowDoc(Guid id)
        {
            var result = CacheWrapper.GetOrAdd(ShowDocGroup, id.ToString(), () =>
            {
                ViewBag.FileID = id;
                return this.RenderPartialViewToString("_Word",null,ViewData);
            });
        
            return Content(result);
        }

        private static readonly CacheAccessor<string> ContentGroup = new CacheAccessor<string>(); 

        public async Task<ContentResult> GetDocContent(Guid id)
        {

            var context = await CacheWrapper.GetOrAddAsync(ContentGroup,id.ToString(), async () =>
                {
                    try
                    {
                        return await _wordService.ConvertToHtmlAsync(id);
                    }
                    catch (FileNotFoundException)
                    {
                        return FormatError("Файл не найден!");
                    }
                    catch (Exception)
                    {
                        return FormatError("Некорректный формат файла");

                    }

                });

            return Content(context);
        }

        private static string FormatError(string message)
        {
            return String.Format("<div class='error'>{0}</div>", message);
        }
    }
}