using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.DAL;
using Base.Links.Entities;

namespace CorpProp.Entities.Document
{
    static class FileDataHelper
    {
        public static IQueryable<FileData> VersionsFilter(IUnitOfWork unitOfWork, IQueryable<FileData> query, int fileCardOneId, Guid oid)
        {
            var linkItems = unitOfWork.GetRepository<LinkItem>().All();
            var name = typeof(FileCardOne).GetTypeName();
            var currentFileCardOneLinkeItems = from item in linkItems
                                               where item.SourceType == name
                                               where (item.SourceObject.Link.ID == 0 && item.SourceObject.ID == fileCardOneId) || item.SourceObject.Link.ID == fileCardOneId
                                               select item;
            var currentFileId = unitOfWork.GetRepository<FileCardOne>().Find(fileCardOneId).FileDataID;
            var linkedFileDatas = from fileData in query
                                  join linkItem in currentFileCardOneLinkeItems
                                  on fileData.ID equals linkItem.DestObject.Link.ID
                                  where linkItem.DestObject.Link.ID != currentFileId
                                  select fileData;
            return linkedFileDatas.OrderByDescending(fileData => fileData.SortOrder);
        }
    }
}
