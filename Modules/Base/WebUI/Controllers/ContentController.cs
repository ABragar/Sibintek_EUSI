using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Base.Content.Entities;
using Base.Content.Service.Abstract;
using Base.Extensions;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class ContentController: BaseController
    {
        private readonly IContentItemService _contentItemService;

        public ContentController(IBaseControllerServiceFacade serviceFacade, IContentItemService contentItemService) : base(serviceFacade)
        {
            _contentItemService = contentItemService;
        }

        public async Task<ActionResult> GetNews()
        {
            using (var uofw = this.CreateUnitOfWork())
            {
                var data = await _contentItemService.GetAll(uofw)
                    .Where(
                        x =>
                            x.Top &&
                            x.ContentItemStatus == ContentItemStatus.Published)
                    .OrderByDescending(x => x.Date)
                    .Take(10)
                    .Select(x => new
                    {
                        x.ID,
                        x.Title,
                        x.Description,
                        x.Date,
                        x.ImagePreview,
                    }).ToListAsync();


                return new JsonNetResult(new
                {
                    Data = data
                });
            }
        }
    }
}