using Base.ExportImport.Services.Abstract;
using Base.ExportImport.Services.Concrete;
using Common.Data.Services.Abstract;
using Common.Data.Services.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class ExportImportBinding
    {
        public static void Bind(Container container)
        {
            container.Register<Base.ExportImport.Initializer>();
            container.Register<IPackageService, PackageService>();
            container.Register<IExportImportManager, ExportImportManager>();
            container.Register<IRolesBaseImportService, RolesBaseImportService>();
            container.Register<IPresetMenuBaseImportService, PresetMenuBaseImportService>();
        }
    }
}