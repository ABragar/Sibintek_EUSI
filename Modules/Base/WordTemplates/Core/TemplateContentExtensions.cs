using System.Collections.Generic;
using System.Linq;

namespace WordTemplates.Core
{
    public static class TemplateContentExtensions
    {
        public static IEnumerable<TemplateContent> GetContent(this TemplateContent template_content, string name)
        {
            return template_content.Items.GetOrDefault(name) ?? Enumerable.Empty<TemplateContent>();
        }
    }
}