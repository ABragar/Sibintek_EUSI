using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.ExportImport.Entities;
using Base.ExportImport.Services.Abstract;
using Base.UI;

namespace Base.ExportImport
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Package>()                
                .DetailView(x => x.Title("Пакет выгрузки"))
                .ListView(x => x.Title("Пакеты выгрузки"))
                .Service<IPackageService>();
        }
    }
}
