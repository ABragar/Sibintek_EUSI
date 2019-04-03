using System;

namespace WordTemplates.ConfigBuilders
{
    internal interface ITemplateConfigBuilderItem<TContext, TItem>
    {
        Action<TemplateContent, BuilderContext<TContext, TItem>> GetFillAction();
    }
}