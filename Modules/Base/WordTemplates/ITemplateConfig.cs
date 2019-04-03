namespace WordTemplates
{
    public interface ITemplateConfig<in TContext>
    {
        Template Template { get; }

        TemplateContent GetContent(TContext context, object item);
    }
}