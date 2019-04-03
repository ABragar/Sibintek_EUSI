namespace WordTemplates.Core
{
    public class HtmlTemplate
    {
        public string TemplateId { get; } = Generator.Generate();

        public HtmlModel Model { get; }

        public ContentControlInfo Info { get; }

        public HtmlTemplate(HtmlModel model, ContentControlInfo info)
        {
            Model = model;
            Info = info;
        }
    }
}