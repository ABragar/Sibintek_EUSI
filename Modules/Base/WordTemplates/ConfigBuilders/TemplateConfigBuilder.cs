using System;
using System.Collections.Generic;
using System.Linq;

namespace WordTemplates.ConfigBuilders
{
    [Serializable]
    public class TemplateConfigBuilder<TContext, TItem>
    {
        private readonly Template _template;

        private readonly IList<ITemplateConfigBuilderItem<TContext, TItem>> _config_items = new List<ITemplateConfigBuilderItem<TContext, TItem>>();

        internal TemplateConfigBuilder(Template template)
        {
            _template = template;
        }

        protected virtual string GetFullName(string name)
        {
            return name ?? "";
        }


        internal Action<TemplateContent, TContext, TItem> GetItemsAction()
        {
            var actions = _config_items.Select(x => x.GetFillAction()).ToArray();


            return (content, context, item) =>
            {
                var builder_context = new BuilderContext<TContext, TItem>(context, item);
                foreach (var action in actions)
                {
                    action(content, builder_context);
                }
            };
        }

        public TemplateConfigBuilder<TContext, TItem> AddValue(string name,
            Func<BuilderContext<TContext, TItem>, TemplateValue> property_func)
        {
            if (property_func != null)
            {
                var full_name = GetFullName(name);
                if (!_template.Values.Add(full_name))
                    throw new InvalidOperationException("name not unique");

                _config_items.Add(new SimpleTemplateConfigBuilderItem<TContext, TItem>(full_name, property_func));
            }
            return this;
        }

        public TemplateConfigBuilder<TContext, TItem> AddGroup<TGroup>(string prefix,
            Func<BuilderContext<TContext, TItem>, TGroup> group_func,
            Action<TemplateConfigBuilder<TContext, TGroup>> builder_func)
        {

            var full_name = GetFullName(prefix);

            var group_builder = new GroupTemplateConfigBuilder<TContext, TItem, TGroup>(full_name, _template, group_func);

            _config_items.Add(group_builder);

            builder_func?.Invoke(group_builder);

            return this;
        }

        public TemplateConfigBuilder<TContext, TItem> AddCollection<TCollectionItem>(string name,
            Func<BuilderContext<TContext, TItem>, ICollection<TCollectionItem>> collection_func,
            Action<TemplateConfigBuilder<TContext, TCollectionItem>> builder_func)
        {

            var full_name = GetFullName(name);

            var template = new Template();

            _template.Items.Add(full_name, template);

            var collection_builder = new CollectionTemplateConfigBuilder<TContext, TItem, TCollectionItem>(full_name, template, collection_func);

            _config_items.Add(collection_builder);

            builder_func?.Invoke(collection_builder);

            return this;
        }





    }
}