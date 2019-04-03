namespace WordTemplates.ConfigBuilders
{
    public class BuilderContext<TContext, TItem>
    {
        internal BuilderContext(TContext context, TItem item)
        {
            Context = context;
            Item = item;
        }

        public TContext Context
        { get; }
        public TItem Item
        { get; }
    }
}