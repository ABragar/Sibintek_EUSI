using System;

namespace WordTemplates.ConfigBuilders
{
    internal class SimpleTemplateConfigBuilderItem<TContext, TItem> : ITemplateConfigBuilderItem<TContext, TItem>
    {
        private readonly string _name;
        private readonly Func<BuilderContext<TContext, TItem>, TemplateValue> _func;

        public SimpleTemplateConfigBuilderItem(string name, Func<BuilderContext<TContext, TItem>, TemplateValue> func)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _name = name;
            _func = func;
        }

        public Action<TemplateContent, BuilderContext<TContext, TItem>> GetFillAction()
        {
            return (content, context) => content.Values[_name] = _func(context);
        }
    }
}