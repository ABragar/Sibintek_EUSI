using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WordTemplates.Core
{



    public class TemplateBuilder
    {
        private string Name { get; }

        private ContentControlType Type { get; }

        private readonly Dictionary<object, TemplateBuilder> _builders = new Dictionary<object, TemplateBuilder>();

        public Template ToTemplate()
        {
            var template = new Template { };

            
            foreach (var builder in _builders.Values)
            {
                if (builder.Type == ContentControlType.Text)
                    template.Values.Add(builder.Name);
                else if (builder.Type == ContentControlType.Repeat)
                    template.Items.Add(builder.Name,builder.ToTemplate());

            }
            
            return template;
        }


        public TemplateBuilder(): this(null)
        {

        }

        private TemplateBuilder(ContentControlInfo info)
        {
            Name = info?.Name;
            Type = info?.Type ?? ContentControlType.Unknown;
            
        }

        private TemplateBuilder AddTemplate(ContentControlInfo info)
        {
            return _builders.GetOrAdd(new { info.Name, info.Type}, () => new TemplateBuilder(info));
        }

        public void Fill(XElement element)
        {

            element.ProcessContentControls((x, e) =>
            {
                if (e.Type == ContentControlType.Text)
                {
                    AddTemplate(e);
                }
                else if (e.Type == ContentControlType.Repeat)
                {
                    AddTemplate(e).Fill(x);
                }
                else
                {
                    Fill(x);

                }
            });
        }
    }
}