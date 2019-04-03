using System.Collections.Generic;
using Base.UI;
using Base.UI.ViewModal;
using WordTemplates;

namespace Base.Word
{
    public static class ViewModelConfigExtensions
    {
        public static ITemplateConfig<TemplateConfigContext> GetPrintConfig(this ViewModelConfig config)
        {
            return config.GetAdditional<ITemplateConfig<TemplateConfigContext>>();
        }

        public static void SetPrintConfig(this ViewModelConfig config, ITemplateConfig<TemplateConfigContext> template_config)
        {
            config.SetAdditional(template_config);
        }


        internal static void AddPrintToolbar(this ViewModelConfig config)
        {

            if (config.GetPrintConfig() != null)
            {
                config.DetailView.Toolbars.Add(new Toolbar()
                {
                    AjaxAction =
                        new AjaxAction
                        {
                            Name = "Toolbar",
                            Controller = "Print",
                            Params =
                                new Dictionary<string, string>() { { "mnemonic", config.Mnemonic }, { "id", "[ID]" } }
                        },
                    Title = "Печать",
                    IsAjax = true,
                });
            }
        }
    }
}