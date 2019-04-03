using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WordTemplates;

namespace Base.Word.Services.Abstract
{
    public interface IWordService
    {

        void ProcessDocument(Stream stream, TemplateContent content);
        void ProcessDocument(Stream stream, JObject content);

        string GetContent(Guid file_id);

        bool HasContent(Guid file_id);


        string ConvertToHtml(byte[] document, string original_file_path);

        Task<string> ConvertToHtmlAsync(Guid file_id);

        Task<string> ConvertToHtmlTemplateAsync(Guid file_id);
    }



}