using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Events;
using Base.Links.Entities;
using Base.Service;
using Base.Service.Crud;

namespace Base.Links.Service.Abstract
{
    public interface ILinkItemService : IBaseObjectService<LinkItem>, 
        IEventBusHandler<IOnDelete<BaseObject>>,
        IEventBusHandler<IOnUpdate<BaseObject>>
    {
        ICollection<LinkItem> GetLinks(IUnitOfWork uow, string mnemonic, int objectID, bool showHidden = false);

        BaseObject CreateLinkObject(IUnitOfWork uow, IBaseObjectCrudService destService, IBaseObjectCrudService sourceService, int sourceID);

        void SaveLink(IUnitOfWork uow, BaseObject dest, BaseObject source);
        void SaveLink(IUnitOfWork uow, BaseObject dest, BaseObject source, string mnemonicDest, string mnemonicSource);
        int GetLinksCount(IUnitOfWork uow, string entityType, int objectID);
        BaseObject InitLinkObject(IUnitOfWork uow, BaseObject src, BaseObject dest);
    }
}
