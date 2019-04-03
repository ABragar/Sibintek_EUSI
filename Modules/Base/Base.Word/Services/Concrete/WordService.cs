using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Base.Service;
using Base.Word.Services.Abstract;
using Newtonsoft.Json.Linq;
using WordTemplates;
using WordTemplates.Core;

namespace Base.Word.Services.Concrete
{
    public class WordService : IWordService
    {

        private readonly IFileSystemService _file_system_service;


        public WordService(IFileSystemService file_system_service)
        {
            _file_system_service = file_system_service;
        }

        public void ProcessDocument(Stream stream, TemplateContent content)
        {

            DocumentProcessor.ProcessContent(stream, content);


        }
        public void ProcessDocument(Stream stream, JObject content)
        {

            DocumentProcessor.ProcessContent(stream, GetContent(content));

        }
        private static TemplateContent GetContent(JObject obj)
        {
            var content = new TemplateContent();

            var values = obj["values"] as JObject;
            if (values != null)
            {

                foreach (var property in values.Properties())
                {
                    var value = property.Value as JValue;
                    if (value != null)
                    {
                        var v = value.Value;
                        content.Values.Add(property.Name, v?.ToString());
                    }

                    var complex_property = property.Value as JObject;
                    if (complex_property != null)
                    {
                        var title = complex_property["title"];
                        var url = complex_property["url"];
                        content.Values.Add(property.Name, new TemplateValue(title?.ToString(), url?.ToString()));
                    }


                }
            }

            var items = obj["items"] as JObject;
            if (items != null)
            {
                foreach (var property in items.Properties())
                {
                    var array = property.Value as JArray;

                    if (array != null)
                    {
                        content.Items.Add(property.Name, array.OfType<JObject>().Select(GetContent).ToList());
                    }
                }
            }

            return content;
        }

        public string GetContent(Guid file_id)
        {
            var path = _file_system_service.GetFilePath(file_id);
            using (var stream = File.OpenRead(path))
            {
                return DocumentConverter.GetContent(stream);
            }
        }

        public bool HasContent(Guid file_id)
        {
            var path = _file_system_service.GetFilePath(file_id);
            using (var stream = File.OpenRead(path))
            {
                return DocumentConverter.HasContent(stream);
            }
        }

        public Task<string> ConvertToHtmlAsync(Guid file_id)
        {
            return ConvertToHtmlAsync(file_id, false);

        }

        public string ConvertToHtml(byte[] document, string path)
        {

            using (var stream = new MemoryStream())
            {
                stream.Write(document,0,document.Length);

                return ConvertToHtml(stream, path, false);
            }
        }


        public Task<string> ConvertToHtmlTemplateAsync(Guid file_id)
        {
            return ConvertToHtmlAsync(file_id, true);
        }




        private async Task<string> ConvertToHtmlAsync(Guid file_id, bool template)
        {
            var path = _file_system_service.GetFilePath(file_id);

            if (!File.Exists(path))
                throw new FileNotFoundException();

            using (var stream = await Helpers.ReadToMemoryAsync(path).ConfigureAwait(false))
            {
                return ConvertToHtml(stream, path, template);
            }
        }


        private string ConvertToHtml(Stream stream, string path, bool template)
        {
            var filename = Path.GetFileName(path);
            var dir = Path.GetDirectoryName(path);
            var search = filename + "_*";

            var image_exists = Directory.EnumerateFiles(dir, search).Any();

            
            int i = 0;
            Func<Bitmap, string> func = (x =>
            {
                string name = $"{filename}_{i}";

                if (!image_exists)
                    x.Save(Path.Combine(dir, name));
                i++;

                return _file_system_service.GetUrlFromFileName(name);

            });

            var result = template ? DocumentConverter.ToHtmlTemplate(stream, func) : DocumentConverter.ToHtml(stream, func);

            return result.SaveToString();


        }


    }
}
