using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.Document;
using CorpProp.Entities.ManyToMany;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Entities.Security;
using CorpProp.Services.Security;

namespace CorpProp.Services.Document
{

    public interface ICardFolderService : IBaseCategoryService<CardFolder>
    {

    }

    public class CardFolderService : BaseCategoryService<CardFolder>, ICardFolderService
    {
        public CardFolderService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public static void SysAllParentsIndexRebuild(IRepository<CardFolder> fileCardRepository)
        {
            var cardFolders = fileCardRepository.Filter(card => card.ID != 0);

            void MakeActionOnAllParents(HCategory cardFolder, Action<HCategory> parentAction)
            {
                if (cardFolder.ParentID != null)
                {
                    parentAction(cardFolder.Parent);
                    if (cardFolder.Parent.Parent != null)
                        MakeActionOnAllParents(cardFolder.Parent.Parent, parentAction);
                }
            }

            foreach (var cardFolder in cardFolders)
            {
                cardFolder.sys_all_parents = null;
                MakeActionOnAllParents(cardFolder, cardFolder.SetParent);
            }
        }
    }
}
