using Base.DAL;
using Base.Service;
using System;

namespace Base.FileStorage
{
    public interface IFileStorageItemService : IBaseCategorizedItemService<FileStorageItem>
    {
        FileStorageItem GetFileStorageItem(IUnitOfWork unitOfWork, int id);
        FileStorageItem GetFileStorageItem(IUnitOfWork unitOfWork, Guid fileID);
    }
}
