using Base.DAL;
using Base.Service;
using System;
using System.IO;
using System.Linq;
using static System.String;

namespace Base.FileStorage
{
    public class FileStorageItemService : BaseCategorizedItemService<FileStorageItem>, IFileStorageItemService
    {
        public FileStorageItemService(IBaseObjectServiceFacade facade) : base(facade) { }

        public override IQueryable<FileStorageItem> GetAllCategorizedItems(IUnitOfWork unitOfWork, int categoryID, bool? hidden = false)
        {
            string strID = HCategory.IdToString(categoryID);

            return GetAll(unitOfWork, hidden).Cast<FileStorageItem>().Where(a => (a.Category_.sys_all_parents != null && a.Category_.sys_all_parents.Contains(strID)) || a.Category_.ID == categoryID);
        }

        protected override IObjectSaver<FileStorageItem> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<FileStorageItem> objectSaver)
        {
            if (IsNullOrEmpty(objectSaver.Src.Title) && objectSaver.Src.File != null)
            {
                objectSaver.Dest.Title = Path.GetFileNameWithoutExtension(objectSaver.Src.File.FileName);
            }

            return base.GetForSave(unitOfWork, objectSaver).SaveOneObject(x => x.File);
        }

        public FileStorageItem GetFileStorageItem(IUnitOfWork unitOfWork, int id)
        {
            return this.GetAll(unitOfWork).FirstOrDefault(x => x.File != null && x.FileID == id);
        }

        public FileStorageItem GetFileStorageItem(IUnitOfWork unitOfWork, Guid fileID)
        {
            return this.GetAll(unitOfWork).FirstOrDefault(x => x.File != null && x.File.FileID == fileID);
        }

    }
}
