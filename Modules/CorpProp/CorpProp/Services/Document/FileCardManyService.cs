using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Extensions;
using Base.Security.Service;
using Base.Service;
using Base.UI;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.ManyToMany;
using CorpProp.Helpers;
using ExcelDataReader;
using Kendo.Mvc.Extensions;
using CorpProp.Entities.Security;
using CorpProp.Services.Security;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Services.Document
{
    public interface IFileCardManyService : IBaseCategorizedItemService<FileCardMany>
    {
        void MergeFileCardOnesToMany(IUnitOfWork unitOfWork, IList<int> fileCardIds, int fileCardManyDestId);
        void MergeFileCardOnesToMany(IUnitOfWork unitOfWork, IList<int> fileCardIds);
        IQueryable<FileData> ExtractFileDatas(IUnitOfWork uow, List<int> oidsObjectIds);
    }

    public class FileCardManyService : SibAccessableObjectCategorizedItemService<FileCardMany>, IFileCardManyService
    {
        private readonly IViewModelConfigService _configService;

        public FileCardManyService(IBaseObjectServiceFacade facade,
             IPathHelper pathHelper, IViewModelConfigService configService
            ) : base(facade)
        {
            _configService = configService;
        }

        public void OnActionExecuting(ActionExecuteArgs args)
        {

        }

        public void MergeFileCardOnesToMany(IUnitOfWork unitOfWork, IList<int> fileCardIds, int fileCardManyDestId)
        {
            CreateFromMany(unitOfWork, fileCardIds, fileCardManyDestId);
        }

        public void MergeFileCardOnesToMany(IUnitOfWork unitOfWork, IList<int> fileCardIds)
        {
            CreateFromMany(unitOfWork, fileCardIds, null);
        }

        private static string MakePacketName(FileCardOne firstDoc)
        {
            return $"Пакет документов – {firstDoc.Name},..";
        }

        private void CreateFromMany(IUnitOfWork unitOfWork, IList<int> fileCardIds, int? fileCardManyDest)
        {
            var fileCardOnes = from fileCard in unitOfWork.GetRepository<FileCardOne>().All()
                               join fileCardId in fileCardIds on fileCard.ID equals fileCardId
                               select fileCard;
            var firstDoc = fileCardOnes.First();

            FileCardMany newFileCardMany;
            if (fileCardManyDest != null)
                newFileCardMany = unitOfWork.GetRepository<FileCardMany>().Find(fileCardManyDest);
            else
                newFileCardMany = new FileCardMany()
                {
                    Category_ = firstDoc.Category_,
                    CategoryID = firstDoc.Category_.ID,
                    Name = MakePacketName(firstDoc)
                    
                };

            
            foreach (var fileCardOne in fileCardOnes)
            {
                unitOfWork.GetRepository<FileCardOneAndFileCardMany>().Create(new FileCardOneAndFileCardMany(fileCardOne, newFileCardMany));
            }

            if (fileCardManyDest != null)
                Update(unitOfWork, newFileCardMany);
            else
                unitOfWork.SaveChanges();
                //Create(unitOfWork, newFileCardMany);
        }

        public void BeforeInvoke(BaseObject obj)
        {
        }

        public IQueryable<FileData> ExtractFileDatas(IUnitOfWork uow, List<int> oidsObjectIds)
        {
            var fileCardOnes = uow.GetRepository<FileCardOne>().All();
            var fileDataCardOnes = from fileCardOne in fileCardOnes
                                   where oidsObjectIds.Contains(fileCardOne.ID)
                                   select fileCardOne.FileData;

            var fileCardManyes = uow.GetRepository<FileCardMany>().All();
            var fileCardOneAndFileCardManyes = uow.GetRepository<FileCardOneAndFileCardMany>().All();
            var fileDataCardManys = from fileCardMany in fileCardManyes
                                    where oidsObjectIds.Contains(fileCardMany.ID)
                                    join fileCardOneAndFileCardMany in fileCardOneAndFileCardManyes on fileCardMany.ID
                                    equals fileCardOneAndFileCardMany.ObjRigthId
                                    where !fileCardOneAndFileCardMany.ObjLeft.Hidden
                                    let fileData = fileCardOneAndFileCardMany.ObjLeft.FileData 
                                    select fileData;

           var fileDatas = fileDataCardOnes.Concat(fileDataCardManys);

           return fileDatas;
        }

        public override FileCardMany CreateDefault(IUnitOfWork unitOfWork)
        {
            var uid = AppContext.SecurityUser.ID;
            var obj = base.CreateDefault(unitOfWork);
            obj.PersonFullName = unitOfWork.GetRepository<SibUser>()
                .Filter(f=> !f.Hidden && f.UserID == uid).FirstOrDefault();
            return obj;
        }
    }
}
