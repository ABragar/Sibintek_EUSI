using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Base.Extensions;
using Base.Service.Crud;
using Base.Social.Entities.Components;
using Base.Social.Service.Abstract.Components;
using Base.UI.ViewModal;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebUI.Extensions;
using WebUI.Helpers;
using WebUI.Models.BusinessProcess;
using WebUI.Models.Social;

namespace WebUI.Controllers
{
    public class SocialController : BaseController
    {
        public readonly IVoitingService _voitingService;
        public readonly ICommentsService _commentsService;

        public SocialController(IBaseControllerServiceFacade serviceFacade, IVoitingService voitingService, ICommentsService commentsService)
            : base(serviceFacade)
        {
            _voitingService = voitingService;
            _commentsService = commentsService;

        }

        public ActionResult GetVoitingToolbar(string mnemonic, int objectID)
        {
            var config = GetViewModelConfig(mnemonic);

            using (var uof = CreateUnitOfWork())
            {
                if (objectID != 0)
                {
                    var allCount = _voitingService.GetAll(uof).Where(x => x.Type == config.TypeName && x.TypeId == objectID);
                    var countThumpUp = allCount.Count(x => x.Thumb == true);
                    var countThumpDown = allCount.Count(x => x.Thumb == false);
                    return PartialView("~/Views/Social/_VoitingToolbar.cshtml", new VoitingModelVm()
                    {
                        Mnemonic = mnemonic,
                        ObjectId = objectID,
                        ThumpUp = countThumpUp,
                        ThumpDown = countThumpDown
                    });
                }
            }
            return null;
        }

        public ActionResult GetCommentCount(string mnemonic, int objectID)
        {
            var config = GetViewModelConfig(mnemonic);

            using (var uof = CreateUnitOfWork())
            {
                if (objectID != 0)
                {
                    var count = _commentsService.GetAll(uof).Count(x => x.Type == config.TypeName && x.TypeId == objectID);
                    return new JsonNetResult(new { count });
                }
            }
            return null;
        }


        public ActionResult GetCommentToolbar(string mnemonic, int objectID)
        {
            var config = GetViewModelConfig(mnemonic);

            using (var uof = CreateUnitOfWork())
            {
                if (objectID != 0)
                {
                    var count = _commentsService.GetAll(uof).Count(x => x.Type == config.TypeName && x.TypeId == objectID);
                    return PartialView("~/Views/Social/_CommentToolbar.cshtml", new CommentToolbarModelVm()
                    {
                        Mnemonic = mnemonic,
                        ObjectId = objectID,
                        Count = count
                    });
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveComments(string mnemonic, int objectID, string message)
        {
            var config = GetViewModelConfig(mnemonic);

            using (var uof = CreateUnitOfWork())
            {
                var obj = _commentsService.Create(uof, new Сomments()
                {
                    Message = message,
                    Type = config.TypeName,
                    TypeId = objectID,
                    DateTime = DateTime.Now,
                    UserId = SecurityUser.ID,
                });

                var count = _commentsService.GetAll(uof).Count(x => x.Type == config.TypeName && x.TypeId == objectID);
                return new JsonNetResult(new { count });
            }
        }

        public ActionResult GetGrid(string mnemonic, int objectID)
        {
            return PartialView("~/Views/Social/_Grid.cshtml", new CommentToolbarModelVm() { ObjectId = objectID, Mnemonic = mnemonic });
        }

        public async Task<JsonNetResult> GetCollection([DataSourceRequest] DataSourceRequest request, string mnemonic, int objectID)
        {
            var config = GetViewModelConfig(mnemonic);

            using (var uof = CreateUnitOfWork())
            {
                var q = _commentsService.GetAll(uof).Where(x => x.Type == config.TypeName && x.TypeId == objectID).OrderBy(x => x.DateTime);

                return new JsonNetResult(await q.Select(x => new CommentsModelVm()
                {
                    DateTime = x.DateTime,
                    Message = x.Message,
                    Image = x.User.Image,
                    UserName = x.User.FullName,
                    UserID = x.User.ID
                }).ToDataSourceResultAsync(request));
            }
        }

        [HttpPost]
        public JsonNetResult ThumpToggle(VoitingModelVm model)
        {
            var config = GetViewModelConfig(model.Mnemonic);

            using (var uof = CreateUnitOfWork())
            {
                _voitingService.CreateOrUpdate(uof, new Voiting()
                {
                    Thumb = model.Toggle,
                    TypeId = model.ObjectId,
                    UserId = SecurityUser.ID,
                    Type = config.TypeName
                });

                var allCount = _voitingService.GetAll(uof).Where(x => x.Type == config.TypeName && x.TypeId == model.ObjectId);

                int countThumpUp = allCount.Count(x => x.Thumb == true);
                int countThumpDown = allCount.Count(x => x.Thumb == false);

                return new JsonNetResult(new
                {
                    thumpUp = countThumpUp,
                    thumpDown = countThumpDown
                });
            }
        }
    }
}