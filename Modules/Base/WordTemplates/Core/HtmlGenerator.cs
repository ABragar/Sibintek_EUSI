using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WordTemplates.Properties;

namespace WordTemplates.Core
{
    public class HtmlGenerator
    {

        private readonly StringBuilder _builder;

        public HtmlGenerator(StringBuilder builder)
        {

            _builder = builder;
        }

        public void GenerateTemplate(HtmlModel model)
        {

            WriteLine("$(function () {");

            GenerateViewModelDefinition();

            WriteLine("var viewModel = new ViewModeldefinition();");

            WriteLine("viewModel.set(\"modelTrigger\",modelTrigger);");


            var writer = new JsonTextWriter(new StringWriter(_builder));

            Write("model = ");

            GenerateModel(model).WriteTo(writer);
            
            WriteLine(";");


            WriteLine("viewModel.setModel(model)");
            WriteLine("viewModel.createDefault()");



            WriteLine($"kendo.bind($(\"#{model.Id} .template-form\"), viewModel);");

            WriteLine($"menuDefinition(\"#{model.Id} .template-menu\", \"#{model.Id}\");");

            
            WriteLine("});");
        }




        private void GenerateViewModelDefinition()
        {
            WriteLine(Resources.viewModeldefinition);
        }

        public JObject GenerateModel(HtmlModel model, bool root = true)
        {

            
            var obj = new JObject();

            
            if (model.Type != ContentControlType.Text)
            {

                obj.Add("values",new JArray(model.Models.Where(x => x.Type == ContentControlType.Text).Select(x=>GenerateModel(x,false))));

                obj.Add("items", new JArray(model.Models.Where(x => x.Type == ContentControlType.Repeat).Select(x => GenerateModel(x, false))));
              
            }
            obj.Add("id",model.Id);
            obj.Add("name",model.Name);

            return obj;
        }




        private void WriteLine(string str)
        {
            _builder.AppendLine(str);
        }

        private void Write(string str)
        {
            _builder.Append(str);
        }

        private void WriteLines(IEnumerable<string> str)
        {
            foreach (var s in str)
            {
                _builder.AppendLine(s);
            }
        }

        private void WriteBlock(string header, string separator, string footer, IEnumerable<Action> writers)
        {
            _builder.Append(header);
            var is_first = true;
            foreach (var writer in writers)
            {
                if (is_first)
                {
                    is_first = false;
                }
                else
                {
                    _builder.Append(separator);

                }
                writer.Invoke();
            }

            _builder.Append(footer);
        }
    }
}