using Base.Service;

namespace Base.FileStorage
{
    public class FileStorageCategoryService : BaseCategoryService<FileStorageCategory>, IFileStorageCategoryService
    {
        public FileStorageCategoryService(IBaseObjectServiceFacade facade) : base(facade) { }
    }
}
