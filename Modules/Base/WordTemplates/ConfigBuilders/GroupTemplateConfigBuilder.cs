using System;

namespace WordTemplates.ConfigBuilders
{
    internal class GroupTemplateConfigBuilder<TContext, TParent, TItem> :
        TemplateConfigBuilder<TContext, TItem>,
        ITemplateConfigBuilderItem<TContext, TParent>
    {
        private readonly string _prefix;
        private readonly Func<BuilderContext<TContext, TParent>, TItem> _group_func;

        public GroupTemplateConfigBuilder(string prefix, Template template, Func<BuilderContext<TContext, TParent>, TItem> group_func)
            : base(template)
        {
            if (group_func == null)
                throw new ArgumentNullException(nameof(group_func));

            _prefix = prefix;
            _group_func = group_func;
        }


        protected override string GetFullName(string name)
        {
            if (string.IsNullOrEmpty(_prefix))
                return base.GetFullName(name);

            if (string.IsNullOrEmpty(name))
                return _prefix;

            return $"{_prefix}.{name}";
        }


        public Action<TemplateContent, BuilderContext<TContext, TParent>> GetFillAction()
        {
            var action = GetItemsAction();

            return (content, context) =>
            {
                var item = _group_func(context);
                if (item != null)
                    action(content, context.Context, item);
            };
        }
    }
}