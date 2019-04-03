using System.Collections.Generic;
using WordTemplates.Core;

namespace WordTemplates
{
    public static class TemplateExtensions
    {


        public static TemplateContent GenerateTestContent(this Template template, string name = null)
        {
            var result = new TemplateContent();
            //foreach (var val in template.Values)
            //{


            //    result.Values.Add(val.Key, new TemplateValue($"{name} {val.Key} testitem",$"http://google.ru/{Generator.Generate()}"));
            //}

            //foreach (var tt in template.Items)
            //{
            //    var ttt = tt.Value.GenerateTestContent($"{name} {tt.Key}");
            //    result.Items.Add(tt.Key, new List<TemplateContent>(new[] { ttt, ttt }));
            //}
            return result;
        }
    }
}