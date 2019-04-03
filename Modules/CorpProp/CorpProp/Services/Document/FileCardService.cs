using System.Linq;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.Document;
using CorpProp.Entities.ManyToMany;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Entities.Security;
using CorpProp.Services.Security;

namespace CorpProp.Services.Document
{

    public interface IFileCardService : IBaseCategorizedItemService<FileCard>
    {

    }
    public class FileCardService : SibAccessableObjectCategorizedItemService<FileCard>, IFileCardService
    {
        public FileCardService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override IQueryable<FileCard> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var fileCards = base.GetAll(unitOfWork, hidden);
            var fileCardOneAndFileCardManyes = unitOfWork.GetRepository<FileCardOneAndFileCardMany>().All();
            var cards = from fileCard in fileCards
                        join fileCardOneAndFileCardMany in fileCardOneAndFileCardManyes
                        on fileCard.ID equals fileCardOneAndFileCardMany.ObjLeftId
                        into fileCardsInPackets
                        from fileCardsInPacket in fileCardsInPackets.DefaultIfEmpty()
                        where fileCardsInPacket == null
                        select fileCard;
            return cards;
        }

        public override FileCard CreateDefault(IUnitOfWork unitOfWork)
        {
            var uid = AppContext.SecurityUser.ID;
            var obj = base.CreateDefault(unitOfWork);
            obj.PersonFullName = unitOfWork.GetRepository<SibUser>()
                .Filter(f => !f.Hidden && f.UserID == uid).FirstOrDefault();
            return obj;
        }
    }




    public interface IFileCardNonCategoryService : IBaseCategorizedItemService<FileCard>
    {

    }
    public class FileCardNonCategoryService : BaseCategorizedItemService<FileCard>, IFileCardNonCategoryService
    {
        public FileCardNonCategoryService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override IQueryable<FileCard> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var fileCards = base.GetAll(unitOfWork, hidden);
            var fileCardOneAndFileCardManyes = unitOfWork.GetRepository<FileCardOneAndFileCardMany>().All();
            var cards = from fileCard in fileCards
                        join fileCardOneAndFileCardMany in fileCardOneAndFileCardManyes
                        on fileCard.ID equals fileCardOneAndFileCardMany.ObjLeftId
                        into fileCardsInPackets
                        from fileCardsInPacket in fileCardsInPackets.DefaultIfEmpty()
                        where fileCardsInPacket == null
                        select fileCard;
            return cards;
        }

        public override FileCard CreateDefault(IUnitOfWork unitOfWork)
        {
            var uid = AppContext.SecurityUser.ID;
            var obj = base.CreateDefault(unitOfWork);
            obj.PersonFullName = unitOfWork.GetRepository<SibUser>()
                .Filter(f => !f.Hidden && f.UserID == uid).FirstOrDefault();
            return obj;
        }
    }


    public class FileCardForFileCardManyService : BaseCategorizedItemService<FileCard>, IFileCardNonCategoryService
    {
        public FileCardForFileCardManyService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override IQueryable<FileCard> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {           
            return base.GetAll(unitOfWork, hidden);
        }
        public override FileCard CreateDefault(IUnitOfWork unitOfWork)
        {
            var uid = AppContext.SecurityUser.ID;
            var obj = base.CreateDefault(unitOfWork);
            obj.PersonFullName = unitOfWork.GetRepository<SibUser>()
                .Filter(f => !f.Hidden && f.UserID == uid).FirstOrDefault();
            return obj;
        }
    }
}
