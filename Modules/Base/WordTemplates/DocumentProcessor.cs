using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using WordTemplates.Core;

namespace WordTemplates
{

    public class DocumentProcessor: IDisposable
    {
        
        private readonly WordprocessingDocument _word_document;
        

        protected DocumentProcessor(Stream stream)
        {

            _word_document = stream.OpenWordDocument(true);
           
            
        }


        public static Template GetTemplate(Stream stream)
        {
            using (var doc = stream.OpenWordDocument(false))
            {
                var builder = new TemplateBuilder();

                builder.Fill(doc.MainDocumentPart.GetXDocument().Root);

                return builder.ToTemplate();
            }
        }

        private void ProcessValue(XElement element, TemplateValue value)
        {

            var run = element.CreateSingleRun();

            run.Add(new XElement(Word.T, value.Title));


            if (value.Url != null)
            {

                run.ReplaceWith(run.CreateHyperlink(AddHyperlinkReference(value.Url), null));
            }

        }

        private XElement ProcessContent(XElement element, TemplateContent content)
        {
            element.ReplaceContentControls((x, e) =>
            {

                switch (e.Type)
                {
                    case ContentControlType.Text:
                        TemplateValue value;
                        if (content.Values.TryGetValue(e.Name, out value))
                        {
                            ProcessValue(x,value);
                        }

                        return x.Elements();
                    case ContentControlType.Repeat:
                        return content.GetContent(e.Name).SelectMany(c =>
                        {
                            var new_element = new XElement(x);

                            return ProcessContent(new_element,c).Elements();
                        });

                    default:
                        return ProcessContent(x,content).Elements();
                }

            });

            return element;

        }


        public static void ProcessContent(Stream stream, TemplateContent content)
        {
            using (var processor = new DocumentProcessor(stream))
            {
                processor.ProcessContent(content);
            }
            stream.Seek(0, SeekOrigin.Begin);
        }

        private void ProcessContent(TemplateContent content)
        {
            
            var root = ProcessContent(_word_document.MainDocumentPart.GetXDocument().Root, content);

            root.Descendants(Word.FootnoteReference)
                .Attributes(Word.Id)
                .GroupBy(x => x.Value, x => x.Parent)
                .SelectMany(x => x.InDocumentOrder().Skip(1))
                .Remove();

            _word_document.MainDocumentPart.PutXDocument();
        }

        private readonly Dictionary<string, string> _urls = new Dictionary<string, string>();

        private string AddHyperlinkReference(string url)
        {
            return _urls.GetOrAdd(url, x =>
            {
                var id = Generator.Generate();

                _word_document.MainDocumentPart.AddHyperlinkRelationship(new Uri(x, UriKind.RelativeOrAbsolute), true, id);

                return id;
            });
        }


        public void Dispose()
        {
            _word_document.Dispose();
        }
    }
}