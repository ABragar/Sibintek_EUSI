using Base.UI;

namespace Base.FileStorage
{

    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<FileStorageCategory>()
                .Service<IFileStorageCategoryService>()
                .Title("Файловое хранилище - Каталоги")
                .ListView(x => x.Title("Каталоги"))
                .DetailView(x => x.Title("Каталог"));

            context.CreateVmConfig<FileStorageItem>()
                .Service<IFileStorageItemService>()
                .Title("Файловое хранилище - Файлы")
                .ListView(x => x.Title("Файлы"))
                .DetailView(x => x.Title("Файл"));

            context.CreateVmConfig<FileStorageItem>("FileStorageItemImage")
                .Service<IFileStorageItemService>()
                .Title("Файловое хранилище - Изображения")
                .ListView(
                    x =>
                        x.Title("Изображения")
                            .DataSource(
                                d =>
                                    d.Filter(f => f.File.FileName.EndsWith(".JPG")
                                                   || f.File.FileName.EndsWith(".JPEG")
                                                   || f.File.FileName.EndsWith(".PNG")
                                                   || f.File.FileName.EndsWith(".GIF")
                                                   || f.File.FileName.EndsWith(".TIF")
                                                   || f.File.FileName.EndsWith(".TIFF"))))
                .DetailView(x => x.Title("Изображение"));

        }
    }
}
