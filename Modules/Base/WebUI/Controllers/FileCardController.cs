using System;
using CorpProp.Services.Document;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Base;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class FileCardController : BaseController
    {
        private readonly IFileCardManyService _fileCardService;
        private readonly IFileCardOneService _fileCardOneService;

        public FileCardController(IBaseControllerServiceFacade serviceFacade, IFileCardManyService fileCardService)
            : base(serviceFacade)
        {
            _fileCardService = fileCardService;
        }

        public ActionResult GetMergeToolbar()
        {
            return PartialView("FileCardMergeToolbar");
        }

        public class Oids
        {
            public List<int> ObjectIds { get; set; }
        }

        //public class OidsTo: Oids
        //{
        //    public string Dest;
        //}

        [HttpPost]
        public ActionResult SetMergeDocuments(Oids oids)
        {
            using (var uow = CreateUnitOfWork())
            {
                _fileCardService.MergeFileCardOnesToMany(uow, oids.ObjectIds);
            }
            //TODO Проверить корректность эскалации ошибки в интерфейс
            return new JsonNetResult(null);
        }

        [HttpPost]
        public ActionResult SetMergeDocumentsTo(Oids oids, int dest)
        {
            using (var uow = CreateUnitOfWork())
            {
                _fileCardService.MergeFileCardOnesToMany(uow, oids.ObjectIds, dest);
            }
            //TODO Проверить корректность эскалации ошибки в интерфейс
            return new JsonNetResult(null);
        }

        public ActionResult GetMassDownloadToolbar()
        {
            return PartialView("FileCardMassDownload");
        }

        [HttpPost]
        public ActionResult GetFileCardsFileDatas(Oids oids)
        {
            using (var uow = CreateUnitOfWork())
            {
                IQueryable<FileData> files = _fileCardService.ExtractFileDatas(uow, oids.ObjectIds);
                var fileDataIDs = files.Select(data => data.FileID).ToList();
                return new JsonNetResult(fileDataIDs);
            }
        }
    }
}