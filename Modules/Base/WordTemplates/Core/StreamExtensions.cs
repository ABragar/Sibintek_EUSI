using System.IO;
using DocumentFormat.OpenXml.Packaging;

namespace WordTemplates.Core
{
    public static class StreamExtensions
    {


        public static WordprocessingDocument OpenWordDocument(this Stream stream,bool can_write)
        {

            var settings = new OpenSettings
            {
                AutoSave = false
            };

            return WordprocessingDocument.Open(stream,can_write, settings);

        }

    }


    
}