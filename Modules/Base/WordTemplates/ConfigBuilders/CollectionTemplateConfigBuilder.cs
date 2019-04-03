using System;
using System.Collections.Generic;

namespace WordTemplates.ConfigBuilders
{
    internal class CollectionTemplateConfigBuilder<TContext, TParent, TItem> :
        TemplateConfigBuilder<TContext, TItem>,
        ITemplateConfigBuilderItem<TContext, TParent>

    {
        private readonly string _full_name;
        private readonly Func<BuilderContext<TContext, TParent>, ICollection<TItem>> _collection_func;

        public CollectionTemplateConfigBuilder(string full_name,
            Template template,
            Func<BuilderContext<TContext, TParent>, ICollection<TItem>> collection_func)
            : base(template)
        {
            if (full_name == null)
                throw new ArgumentNullException(nameof(full_name));

            if (collection_func == null)
                throw new ArgumentNullException(nameof(collection_func));

            _full_name = full_name;
            _collection_func = collection_func;
        }

        public Action<TemplateContent, BuilderContext<TContext, TParent>> GetFillAction()
        {
            var action = GetItemsAction();

            return (content, context) =>
            {

                var items = _collection_func(context);

                if (items != null)
                {
                    var contents = new List<TemplateContent>();
                    content.Items.Add(_full_name, contents);

                    foreach (var item in items)
                    {
                        var item_content = new TemplateContent();
                        contents.Add(item_content);
                        action(item_content, context.Context, item);
                    }
                }
            };
        }
    }
}