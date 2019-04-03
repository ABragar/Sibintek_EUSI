using System;
using Base.UI;
using WordTemplates;
using WordTemplates.ConfigBuilders;

namespace Base.Word
{
    
    public static class ViewModelConfigBuilderExtensions
    {
        
        public static ViewModelConfigBuilder<T> TemplateConfig<T>(this ViewModelConfigBuilder<T> builder,Action<TemplateConfigBuilder<TemplateConfigContext,T>> builder_func)
            where T: class
        {
            var config = TemplateConfig<TemplateConfigContext, T>.Build(builder_func);
            
            builder.Config.SetPrintConfig(config);

            return builder;
        }

    }
}