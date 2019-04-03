using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.DAL;
using Base.Links.Service.Concrete;
using Base.Service;
using CorpProp.Entities.Document;

namespace CorpProp.Services.Document
{
    public class VersionsServiceDecorator<T, TL>
        where T: BaseObject
        where TL : BaseObject
    {
        private readonly LinkItemService _linkItemService;
        private readonly Func<T, TL> _getLinkedObject;

        public VersionsServiceDecorator(LinkItemService linkItemService, Func<T, TL> getLinkedObject )
        {
            _linkItemService = linkItemService;
            _getLinkedObject = getLinkedObject;
        }

        private void Link(IUnitOfWork unitOfWork, T src, BaseObject dest)
        {
            _linkItemService.SaveLink(unitOfWork, dest, src);
        }

        public void Link(IUnitOfWork unitOfWork, T obj)
        {
            var fileData = _getLinkedObject(obj);
            if (fileData == null)
                return;
            Link(unitOfWork, obj, fileData);
        }

        public void LinkCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection)
        {
            foreach (var item in collection)
            {
                Link(unitOfWork, item);
            }
        }

        private void Unlink(IUnitOfWork unitOfWork, T src, TL dest)
        {
            var linkItems = _linkItemService.GetAll(unitOfWork)
                                            .Where(_ => _.SourceObject.ID == src.ID 
                                                        && _.SourceType == typeof(T).Name);
            foreach (var item in linkItems.ToArray())
            {
                _linkItemService.Delete(unitOfWork, item);
            }
        }

        public void Unlink(IUnitOfWork unitOfWork, T obj)
        {
            var fileData = _getLinkedObject(obj);
            if (fileData == null)
                return;
            Unlink(unitOfWork, obj, fileData);
        }

        public void UnlinkCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection)
        {
            foreach (var obj in collection)
            {
                Unlink(unitOfWork, obj);
            }
        }



    }

}
