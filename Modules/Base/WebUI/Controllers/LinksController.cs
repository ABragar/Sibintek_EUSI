using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Base;
using Base.Links.Entities;
using Base.Links.Service.Abstract;
using Base.Service.Crud;
using WebUI.Models.Links;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class LinksController : BaseController
    {
        private readonly ILinksConfigurationManager _linksConfigurationManager;
        private readonly ILinkItemService _linkItemService;

        public LinksController(IBaseControllerServiceFacade facade, ILinksConfigurationManager linksConfigurationManager,
            ILinkItemService linkItemService)
            : base(facade)
        {
            _linksConfigurationManager = linksConfigurationManager;
            _linkItemService = linkItemService;
        }

        public ActionResult GetToolbar(string mnemonic, int objectID)
        {
            var entityConfig = GetViewModelConfig(mnemonic);
            var entityType = entityConfig.TypeEntity;

            var links = _linksConfigurationManager.Links
                .Where(x => x.SourceType == entityType)
                .Select(x => GetViewModelConfig(x.DestType));

            var model = new LinksToolbarVm
            {
                CanCreate = links.Select(x => new LinksToolbarItem(x.DetailView.Title ?? x.Title, x.Mnemonic)).ToList(),
                CurrentItemID = objectID + entityType.FullName
            };

            return PartialView("_Toolbar", model);
        }

        [HttpPost]
        public JsonNetResult SaveLink(string sourceMnemonic, int sourceID, string destMnemonic, int destID)
        {
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var destService = GetService<IBaseObjectCrudService>(destMnemonic);
                    var destObject = destService.Get(uow, destID);

                    var sourceService = GetService<IBaseObjectCrudService>(sourceMnemonic);
                    var sourceObject = sourceService.Get(uow, sourceID);

                    _linkItemService.SaveLink(uow, destObject, sourceObject);

                    return new JsonNetResult(new { res = 0 });
                }
            }
            catch (Exception error)
            {
                return new JsonNetResult(error);
            }
        }

        public JsonNetResult CreateLinkObject(string destMnemonic, string sourceMnemonic, int sourceID)
        {
            using (var uof = CreateUnitOfWork())
            {
                var destService = GetService<IBaseObjectCrudService>(destMnemonic);

                var sourceService = GetService<IBaseObjectCrudService>(sourceMnemonic);

                try
                {
                    var destObject = _linkItemService.CreateLinkObject(uof, destService, sourceService, sourceID);

                    return new JsonNetResult(new
                    {
                        Model = GetViewModelConfig(destMnemonic).DetailView.SelectObj(destObject)
                    });
                }
                catch (Exception error)
                {
                    return new JsonNetResult(new
                    {
                        error = error.Message
                    });
                }
            }
        }

        public ActionResult GetMapItems(string mnemonic, int objectID, bool showHidden = false)
        {
            using (var uow = CreateUnitOfWork())
            {
                string typeName = GetViewModelConfig(mnemonic).TypeName;
                var links = _linkItemService.GetLinks(uow, typeName, objectID);

                var points = links.Select(x => new LinkMapItemModel()
                {
                    ID = x.DestObject.Link.ID.ToString() + x.DestObject.Link.TypeName,
                    Mnemonic = x.DestObject.Link.TypeName,
                    Title = x.DestObject.Link.Title,
                    RealID = x.DestObject.Link.ID,
                    Hidden = false
                }).ToList();

                points = points.Union(links.Select(x => new LinkMapItemModel()
                {
                    ID = x.SourceObject.Link.ID.ToString() + x.SourceObject.Link.TypeName,
                    Mnemonic = x.SourceObject.Link.TypeName,
                    Title = x.SourceObject.Link.Title,
                    RealID = x.SourceObject.Link.ID,
                    Hidden = false
                }))
                .GroupBy(x => x.ID)
                .Select(group => group.First()).ToList();

                if (!points.Any())
                {
                    var config = GetViewModelConfig(mnemonic);
                    var obj = config.GetService<IBaseObjectCrudService>().Get(uow, objectID);
                    var type = obj.GetType().GetBaseObjectType();
                    string title = type.GetProperty(config.LookupProperty.Text).GetValue(obj).ToString();


                    points.Add(new LinkMapItemModel()
                    {
                        ID = objectID.ToString() + objectID.ToString(),
                        Mnemonic = type.GetTypeName(),
                        RealID = objectID,
                        Title = title
                    });
                }


                return new JsonNetResult(points);
            }
        }

        public ActionResult GetConnections(string mnemonic, int objectID, bool showHidden = false)
        {
            using (var uow = CreateUnitOfWork())
            {
                string typeName = GetViewModelConfig(mnemonic).TypeName;
                var links = _linkItemService.GetLinks(uow, typeName, objectID).ToList();

                var connections = links.Select(x => new LinkConnection()
                {
                    ID = x.ID,
                    ToObject = x.DestObject.Link.ID.ToString() + x.DestObject.Link.TypeName,
                    FromObject = x.SourceObject.Link.ID.ToString() + x.SourceObject.Link.TypeName
                }).ToList();
                return new JsonNetResult(connections);
            }
        }

        public JsonNetResult GetLinksCount(string mnemonic, int objectID)
        {
            using (var uow = CreateUnitOfWork())
            {
                try
                {
                    string typeName = GetViewModelConfig(mnemonic).TypeName;
                    int linksCount = _linkItemService.GetLinksCount(uow, typeName, objectID);

                    return new JsonNetResult(linksCount);
                }
                catch
                {
                    return new JsonNetResult(0);
                }
            }
        }

        public JsonNetResult GetLinkGroups()
        {
            using (var uow = CreateUnitOfWork())
            {
                var result = uow.GetRepository<LinkGroupConfig>().All().Where(x => !x.Hidden).Select(x => new
                {
                    name = x.Mnemonic,
                    size = x.Size,
                    icon = new
                    {
                        face = "FontAwesome",
                        code = x.Icon.Code,
                        size = 24,
                        color = x.Icon.Color,
                        value = x.Icon.Value
                    },
                    font = new
                    {
                        color = x.FontColor,
                        size = x.FontSize,
                        face = x.FontFace,
                        align = x.FontAlign,
                        value = x.Icon.Value
                    }

                }).ToList();


                return new JsonNetResult(result);
            }
        }

        public JsonNetResult GetLinkedTypes(string typeName)
        {
            var destTypes = _linksConfigurationManager.Links.Where(x => x.SourceType.GetTypeName() == typeName).Select(x => x.DestType);

            var configs = GetViewModelConfigs().Where(x => destTypes.Contains(x.TypeEntity));

            var result = configs.Select(x => new
            {
                Title = x.Title,
                TypeName = x.TypeEntity.GetTypeName()
            });

            return new JsonNetResult(result);
        }

        [HttpPost]
        public JsonNetResult RemoveLink(int id)
        {
            using (var uow = CreateUnitOfWork())
            {
                var link = _linkItemService.Get(uow, id);
                _linkItemService.Delete(uow, link);
                return new JsonNetResult(new { ok = "ok" });
            }            
        }

    }
}


