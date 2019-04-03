using System;
using WordTemplates.ConfigBuilders;

namespace WordTemplates
{
    [Serializable]
    public class TemplateConfig<TContext,TItem>: ITemplateConfig<TContext>
    {
        private readonly Action<TemplateContent, TContext, TItem> _content_action;


        public Template Template { get; }

        public TemplateContent GetContent(TContext context, object item)
        {
            return GetContent(context, (TItem) item);
        }

        public TemplateContent GetContent(TContext context,TItem item)
        {
            var content = new TemplateContent();
            _content_action?.Invoke(content,context,item);

            return content;
        }



        public static TemplateConfig<TContext,TItem> Build(Action<TemplateConfigBuilder<TContext, TItem>> builder_func)
        {
            var template = new Template();
            var builder = new TemplateConfigBuilder<TContext, TItem>(template);

            builder_func?.Invoke(builder);

            var content_action = builder.GetItemsAction();

            return new TemplateConfig<TContext, TItem>(template, content_action);
        }



        internal TemplateConfig(Template template, Action<TemplateContent, TContext, TItem> content_action)
        {
            Template = template;
            _content_action = content_action;
        }
    }
}