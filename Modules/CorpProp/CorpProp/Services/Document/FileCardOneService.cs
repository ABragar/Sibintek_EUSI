using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Extensions;
using Base.Links.Service.Concrete;
using Base.Security.Service;
using Base.Service;
using Base.UI;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Helpers;
using ExcelDataReader;
using Kendo.Mvc.Extensions;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Entities.Security;
using CorpProp.Services.Security;
using System.IO;
using CorpProp.Extentions;

namespace CorpProp.Services.Document
{
    public interface IFileCardOneService : IBaseObjectService<FileCardOne>
    {
    }

    public class FileCardOneService : SibAccessableObjectCategorizedItemService<FileCardOne>, IFileCardOneService
    {
        private readonly VersionsServiceDecorator<FileCardOne, FileData> _fileVersionsServiceDecorator;
        private IFileSystemService _fileService;

        public FileCardOneService(IBaseObjectServiceFacade facade,
            IFileSystemService fileService,
             IPathHelper pathHelper, LinkItemService linkItemService
            ) : base(facade)
        {
            _fileVersionsServiceDecorator = new VersionsServiceDecorator<FileCardOne, FileData>(linkItemService, one => one.FileData);
            _fileService = fileService;
        }

        public override FileCardOne Create(IUnitOfWork unitOfWork, FileCardOne obj)
        {
            var fileCardOne = base.Create(unitOfWork, obj);
            SetCurrentUser(unitOfWork, fileCardOne);
            SetHash(unitOfWork, fileCardOne);
            _fileVersionsServiceDecorator.Link(unitOfWork, fileCardOne);
            return fileCardOne;
        }

        public override IReadOnlyCollection<FileCardOne> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<FileCardOne> collection)
        {
            var fileCardOnes = base.CreateCollection(unitOfWork, collection);
            _fileVersionsServiceDecorator.LinkCollection(unitOfWork, fileCardOnes);
            return fileCardOnes;
        }

        public override FileCardOne Update(IUnitOfWork unitOfWork, FileCardOne obj)
        {
            var original = unitOfWork.GetRepository<FileCardOne>().GetOriginal(obj.ID);
            if (original.FileDataID != obj.FileDataID)
                SetHash(unitOfWork, obj);
            var fileCardOne = base.Update(unitOfWork, obj);
            _fileVersionsServiceDecorator.Link(unitOfWork, obj);
            return fileCardOne;
        }

        public override IReadOnlyCollection<FileCardOne> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<FileCardOne> collection)
        {
            var readOnlyCollection = base.UpdateCollection(unitOfWork, collection);
            _fileVersionsServiceDecorator.LinkCollection(unitOfWork, readOnlyCollection);
            return readOnlyCollection;
        }

        public override void Delete(IUnitOfWork unitOfWork, FileCardOne obj)
        {
            base.Delete(unitOfWork, obj);
            _fileVersionsServiceDecorator.Unlink(unitOfWork, obj);
        }

        public override void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<FileCardOne> collection)
        {
            base.DeleteCollection(unitOfWork, collection);
            _fileVersionsServiceDecorator.UnlinkCollection(unitOfWork, collection);
        }

        public override FileCardOne CreateDefault(IUnitOfWork unitOfWork)
        {
            
            var obj = base.CreateDefault(unitOfWork);
            SetCurrentUser(unitOfWork, obj);
            return obj;
        }

        private void SetCurrentUser(IUnitOfWork uow, FileCardOne obj)
        {
            
            var uid = AppContext.SecurityUser.ID;
            obj.SetCreateUser(uow, uid);
        }

        private void SetHash(IUnitOfWork uow, FileCardOne obj)
        {
            FileData fd = null;
            if (obj.FileData != null)
            {
                fd = obj.FileData;

            }
            else if (obj.FileDataID != null)
            {
                fd = uow.GetRepository<FileData>().Find(obj.FileDataID);
            }
            if (fd == null)
                return;
            var path =  _fileService.GetFilePath(fd.FileID);
            using (StreamReader stream = new StreamReader(path))
            {
                obj.Hash = FileCard.GetHash(stream.BaseStream.ToByteArray());
            }
        }

        
    }
}
