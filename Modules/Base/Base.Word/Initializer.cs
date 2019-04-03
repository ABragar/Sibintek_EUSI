using System.Linq;
using Base.UI;
using Base.Word.Entities;
using Base.Word.Services.Abstract;

namespace Base.Word
{

    public class Initializer : IModuleInitializer
    {


        public void Init(IInitializerContext context)
        {

            context.ProcessConfigs(x =>
            {
                foreach (var config in x.GetAllVmConfigs())
                {
                    config.AddPrintToolbar();
                }
            });


            context.CreateVmConfig<PrintingSettings>()
                .Title("Шаблоны для печати")
                .Service<IPrintingSettingsService>()
                .DetailView(x => x.Toolbar(t => t.Add("DataSourceToolBar", "Print")));



            context.ModifyVmConfig<PrintingSettings>()
                .TemplateConfig(t => t
                    .AddValue("name", x => x.Item.TemplateName)
                    .AddValue("entity", x => x.Item.Mnemonic)
                    .AddGroup("template", x => x.Item.Template, template => template
                        .AddValue("file_id", x => x.Item.FileID.ToString())
                        .AddValue("name", x => x.Item.FileName))
                    .AddCollection("test", x=> Enumerable.Range(0,10).ToArray(),x=>x.AddValue("test_value",i=>i.Item.ToString()))
                );

        }
    }
}