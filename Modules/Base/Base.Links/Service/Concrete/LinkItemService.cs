using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Entities.Complex;
using Base.Events;
using Base.Links.Entities;
using Base.Links.Service.Abstract;
using Base.Service;
using Base.Service.Crud;
using Base.UI;

namespace Base.Links.Service.Concrete
{
    public class LinkItemService : BaseObjectService<LinkItem>, ILinkItemService
    {
        //макс. уровень глубины
        private const int MaxTreeDeep = 3;
        private readonly ILinksConfigurationManager _linksConfigurationManager;
        private readonly IViewModelConfigService _viewModelConfigService;

        public LinkItemService(IBaseObjectServiceFacade facade, ILinksConfigurationManager linksConfigurationManager, IViewModelConfigService viewModelConfigService) : base(facade)
        {
            _linksConfigurationManager = linksConfigurationManager;
            _viewModelConfigService = viewModelConfigService;
        }

        private ICollection<LinkItem> GetLinks(IUnitOfWork uow, LinkItemBaseObject linkedObj, LinkItemBaseObject objIgnore, ref int depthLevel, bool showHidden = false)
        {
            depthLevel++;
            if (depthLevel >= MaxTreeDeep)
            {
                return new List<LinkItem>();
            }

            var left = GetAll(uow, showHidden).Where(x => x.DestObject.Link.ID == linkedObj.Link.ID && x.DestObject.Link.TypeName == linkedObj.Link.TypeName
                         && !(x.SourceObject.Link.ID == objIgnore.Link.ID && x.SourceObject.Link.TypeName == objIgnore.Link.TypeName)).ToList();
            foreach (var linkItem in left)
            {
                left = left.Union(GetLinks(uow, linkItem.SourceObject, linkItem.DestObject, ref depthLevel, showHidden)).ToList();
                depthLevel--;
            }

            var right = GetAll(uow, showHidden).Where(x => x.SourceObject.Link.ID == linkedObj.Link.ID && x.SourceObject.Link.TypeName == linkedObj.Link.TypeName
               && !(x.DestObject.Link.ID == objIgnore.Link.ID && x.DestObject.Link.TypeName == objIgnore.Link.TypeName)).ToList();
            foreach (var linkItem in right)
            {
                right = right.Union(GetLinks(uow, linkItem.DestObject, linkItem.SourceObject, ref depthLevel, showHidden)).ToList();
                depthLevel--;
            }

            return left.Union(right).ToList();
        }

        public ICollection<LinkItem> GetLinks(IUnitOfWork uow, string mnemonic, int objectID, bool showHidden = false)
        {
            var left = GetAll(uow, showHidden).Where(x =>
                (x.DestObject.Link.TypeName == mnemonic || x.DestObject.Link.Mnemonic == mnemonic) &&
                x.DestObject.Link.ID == objectID).ToList();

            var right = GetAll(uow, showHidden).Where(x =>
                    (x.SourceObject.Link.TypeName == mnemonic || x.SourceObject.Link.Mnemonic == mnemonic) &&
                    x.SourceObject.Link.ID == objectID).ToList();

            //переменная для фиксации уровня вложенности
            int depthLevel = 0;
            foreach (var linkItem in left)
            {
                depthLevel = 0;
                left = left.Union(GetLinks(uow, linkItem.SourceObject, linkItem.DestObject, ref depthLevel, showHidden)).ToList();

            }
            foreach (var linkItem in right)
            {
                depthLevel = 0;
                right = right.Union(GetLinks(uow, linkItem.DestObject, linkItem.SourceObject, ref depthLevel, showHidden)).ToList();
            }

            var links = left.Union(right).ToList();            

            return links;
        }

        public BaseObject CreateLinkObject(IUnitOfWork uow, IBaseObjectCrudService destService, IBaseObjectCrudService sourceService, int sourceID)
        {
            if (destService == null || sourceService == null)
                throw new ArgumentNullException("Service is null");

            var destObject = destService.CreateDefault(uow);

            var sourceObject = sourceService.Get(uow, sourceID);

            var config = _linksConfigurationManager.Links.FirstOrDefault(x =>
                            x.DestType == destObject.GetType().GetBaseObjectType() &&
                            x.SourceType == sourceObject.GetType().GetBaseObjectType());

            if (config == null)
                throw new Exception("Config is null");

            config.Init(sourceObject, destObject);

            return destObject;
        }

        public BaseObject InitLinkObject(IUnitOfWork uow, BaseObject src, BaseObject dest)
        {
            var config = _linksConfigurationManager.Links.FirstOrDefault(x =>
                            x.DestType == dest.GetType().GetBaseObjectType() &&
                            x.SourceType == src.GetType().GetBaseObjectType());

            config?.Init(src, dest);

            return dest;
        }

        public void SaveLink(IUnitOfWork uow, BaseObject dest, BaseObject source, string mnemonicDest, string mnemonicSource)
        {
            LinkItem li = new LinkItem
            {
                SourceObject = GetLinkedRegister(uow, source),
                DestObject = GetLinkedRegister(uow, dest)
            };

            var rep = uow.GetRepository<LinkItem>();
            rep.Create(li);
            uow.SaveChanges();
        }

        public void SaveLink(IUnitOfWork uow, BaseObject dest, BaseObject source)
        {

            var li = new LinkItem
            {
                SourceObject = GetLinkedRegister(uow, source),
                DestObject = GetLinkedRegister(uow, dest)
            };

            var rep = uow.GetRepository<LinkItem>();
            rep.Create(li);
            uow.SaveChanges();
        }




        public int GetLinksCount(IUnitOfWork uow, string typeName, int objectID)
        {
            int linksCount = GetAll(uow).Count(x => (x.SourceObject.Link.ID == objectID && x.SourceObject.Link.TypeName == typeName) || (x.DestObject.Link.ID == objectID && x.DestObject.Link.TypeName == typeName));
            return linksCount;
        }


        public void OnEvent(IOnDelete<BaseObject> evnt)
        {
            string typeName = evnt.Modified.GetType().GetBaseObjectType().GetTypeName();
            var unitOfWork = evnt.UnitOfWork;

            var register =
               unitOfWork.GetRepository<LinkItemBaseObject>()
                   .All()
                   .SingleOrDefault(x => x.Link.ID == evnt.Modified.ID && x.Link.TypeName == typeName);

            if (register == null)
            {
                return;                
            }

            var links = unitOfWork.GetRepository<LinkItem>().All().Where(x => (x.DestObject.Link.ID == evnt.Modified.ID && x.DestObject.Link.TypeName == typeName) ||
                               (x.SourceObject.Link.ID == evnt.Modified.ID && x.SourceObject.Link.TypeName == typeName)).ToList();

            if (links.Any())
            {
                foreach (var linkItem in links)
                {
                    linkItem.Hidden = true;
                    Update(unitOfWork, linkItem);
                }
            }
        }

        public void OnEvent(IOnUpdate<BaseObject> evnt)
        {
            string typeName = evnt.Modified.GetType().GetBaseObjectType().GetTypeName();

            var unitOfWork = evnt.UnitOfWork;

            var register =
                unitOfWork.GetRepository<LinkItemBaseObject>()
                    .All()
                    .SingleOrDefault(x => x.Link.ID == evnt.Modified.ID && x.Link.TypeName == typeName);

            if (register != null)
            {
                var config = _viewModelConfigService.Get(typeName);

                var property = evnt.Modified.GetType().GetProperty(config.LookupProperty.Text);

                register.Link.Title = property.GetValue(evnt.Modified).ToString();

                unitOfWork.GetRepository<LinkItemBaseObject>().Update(register);
                unitOfWork.SaveChanges();
            }
        }

        private string GetTitle(BaseObject obj)
        {
            var type = obj.GetType().GetBaseObjectType();
            var config = _viewModelConfigService.Get(type);
            var property = type.GetProperty(config.LookupProperty.Text);
            return property.GetValue(obj)?.ToString();
        }

        private string GetTitle(IUnitOfWork uow, string typeName, int id)
        {
            var config = _viewModelConfigService.Get(typeName);
            var service = config.GetService<IBaseObjectCrudService>();
            var obj = service.Get(uow, id);
            return GetTitle(obj);
        }

        private LinkItemBaseObject GetLinkedRegister(IUnitOfWork uow, BaseObject obj)
        {
            string typeName = obj.GetType().GetBaseObjectType().GetTypeName();

            var register = uow.GetRepository<LinkItemBaseObject>().All().SingleOrDefault(x => x.Link.ID == obj.ID && x.Link.TypeName == typeName);

            if (register == null)
            {
                register = new LinkItemBaseObject
                {
                    Link = new LinkItemComplexBaseObject(obj)
                    {
                        Title = GetTitle(obj)
                    }
                };

                uow.GetRepository<LinkItemBaseObject>().Create(register);
                uow.SaveChanges();
            }
            

            return register;
        }

        private LinkItemBaseObject GetLinkedRegister(IUnitOfWork uow, int id, string typeName)
        {
            var register =
                uow.GetRepository<LinkItemBaseObject>()
                    .All()
                    .SingleOrDefault(x => x.Link.ID == id && x.Link.TypeName == typeName);

            if (register == null)
            {
                register = new LinkItemBaseObject
                {
                    Link = new LinkItemComplexBaseObject()
                    {
                        ID = id,
                        TypeName = typeName,
                        Title = GetTitle(uow, typeName, id)
                    }
                };

                uow.GetRepository<LinkItemBaseObject>().Create(register);
                uow.SaveChanges();
            }

            return register;

        }

        protected override IObjectSaver<LinkItem> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<LinkItem> objectSaver)
        {
            if (objectSaver.Dest.DestObject == null || objectSaver.Dest.SourceObject == null)
            {
                var srcObject = objectSaver.Src;

                var destlink = GetLinkedRegister(unitOfWork, srcObject.DestObject.Link.ID, srcObject.DestObject.Link.TypeName);
                objectSaver.Src.DestObject = destlink;

                var srcLink = GetLinkedRegister(unitOfWork, srcObject.SourceObject.Link.ID, srcObject.SourceObject.Link.TypeName);
                objectSaver.Src.SourceObject = srcLink;
            }

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.DestObject)
                .SaveOneObject(x => x.SourceObject);
        }

        private enum SearchDirection
        {
            Left = 0,
            Rigth = 1,
        }
    }

}
