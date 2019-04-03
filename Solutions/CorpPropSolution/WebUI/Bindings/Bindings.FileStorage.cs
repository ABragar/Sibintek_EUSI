using Base.FileStorage;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class FileStorageBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.FileStorage.Initializer>();
            container.Register<IFileStorageCategoryService, FileStorageCategoryService>();
            container.Register<IFileStorageItemService, FileStorageItemService>();
        }
    }
}