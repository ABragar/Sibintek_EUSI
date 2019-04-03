using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;
using WordTemplates.Core;
using Xhtml = WordTemplates.Core.Xhtml;

namespace WordTemplates
{
    public class DocumentConverter : IDisposable
    {

        protected DocumentConverter(Stream stream, Func<Bitmap, string> image_handler)
        {

            _doc = stream.OpenWordDocument(true);
            _image_handler = image_handler;
        }

        public void Dispose()
        {
            _doc.Dispose();
            _image_handler = null;
        }

        public static XElement ToHtml(Stream stream, Func<Bitmap, string> image_handler = null)
        {
            using (var converter = new DocumentConverter(stream, image_handler))
            {
                return converter.ToHtml();
            }
        }

        public XElement ToHtml()
        {

            _doc.MainDocumentPart.GetXDocument().Root.RemoveContentControls();

            return Convert();

        }

        public static bool HasContent(Stream stream)
        {
            using (var doc = stream.OpenWordDocument(false))
            {
                using (var reader = new OpenXmlPartReader(doc.MainDocumentPart))
                {
                    while (reader.Read())
                    {
                        if (reader.ElementType == typeof(Text) && !string.IsNullOrWhiteSpace(reader.GetText()))
                            return true;
                    }
                    return false;
                }
            }
        }

        public static string GetContent(Stream stream)
        {
            using (var doc = stream.OpenWordDocument(false))
            {
                return string.Join(" ", doc.MainDocumentPart.Document.Body.Descendants<Text>().Select(x => x.InnerText));
            }
        }


        private XElement Convert()
        {
            var settings = new HtmlConverterSettings();

            if (_image_handler != null)
            {
                settings.ImageHandler = info =>
                {
                    var img = _image_handler(info.Bitmap);

                    if (img != null)
                    {
                        return new XElement(Xhtml.Img,
                            new XAttribute(Xhtml.Src, img),
                            info.ImgStyleAttribute);
                    }
                    return null;
                };
            }


            var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();

            var ci2 = (CultureInfo)ci.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            try
            {
                Thread.CurrentThread.CurrentCulture = ci;


                var html = HtmlConverter.ConvertToHtml(_doc, settings);
                return html;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = ci2;
            }
        }




        public static XElement ToHtmlTemplate(Stream stream, Func<Bitmap, string> image_handler = null)
        {
            using (var converter = new DocumentConverter(stream, image_handler))
            {
                return converter.ToHtmlTemplate();
            }
        }

        private Dictionary<string, HtmlTemplate> _templates;

        private XElement ToHtmlTemplate()
        {

            var xdoc = _doc.MainDocumentPart.GetXDocument();

            _templates = new Dictionary<string, HtmlTemplate>();

            var model = new HtmlModel(null);

            ReplaceContentControlsAndAddMarks(xdoc.Root, model);

            var html = Convert();

            var body = html.Element(Xhtml.Body);

            RemoveMarks(body);

            _templates = null;

            GenerateScript(body, "jquery.min.js");
            GenerateScript(body, "kendo.web.min.js");

            GenerateScript(body, model);


            return html;

        }

        private void GenerateScript(XElement body, string url)
        {
            body.Add(new XElement(Xhtml.Script, new XAttribute(Xhtml.Src, url), ""));

        }


        private void GenerateScript(XElement body, HtmlModel model)
        {
            var nodes = body.Nodes().ToArray();

            var root = new XElement(Xhtml.Div, BindUid(new XElement(Xhtml.Div, nodes, new XAttribute(Xhtml.Class, "template-form"))));


            root.Add(new XAttribute(Xhtml.Id, model.Id));

            root.Add(new XElement(Xhtml.Ul, new XAttribute(Xhtml.Class, "template-menu"), ""));

            var builder = new StringBuilder();

            var generator = new HtmlGenerator(builder);

            generator.GenerateTemplate(model);


            root.Add(new XElement(Xhtml.Script, new RawText(builder.ToString())));

            body.ReplaceNodes(root);
        }

        private class RawText : XText
        {

            public RawText(string val) : base(val)
            {

            }

            public override void WriteTo(XmlWriter writer)
            {
                writer.WriteRaw(Value);
            }


        }

        private void RemoveMarks(XElement body)
        {

            //TODO Если data-bind будет повторяться


            var marks = GetMarks(body, (x, t) => new { Element = x, Template = t }).ToArray();

            var prev = marks.ElementAtOrDefault(-1);

            var stack = CreateStack(prev);

            foreach (var mark in marks)
            {
                if (mark.Template.Info.Type == ContentControlType.Text)
                {
                    mark.Element.ReplaceWith(GetEditor(mark.Template));

                }
                else if (mark.Template.Info.Type == ContentControlType.Repeat)
                {

                    if (prev != null && prev.Template == mark.Template)
                    {

                        var script = ExtractScript(mark.Template, prev.Element.Parent, mark.Element.Parent);

                        body.Add(script);

                        prev = stack.Pop();
                    }
                    else
                    {
                        stack.Push(prev);
                        prev = mark;
                    }
                }

            }

            if (stack.Count != 0)
                throw new InvalidOperationException();



        }

        private static Stack<T> CreateStack<T>(T obj)
        {
            return new Stack<T>();
        }

        private static XElement GetEditor(HtmlTemplate template)
        {
            var dataeditor = template.Info.Tag != null ? new XAttribute(Xhtml.DataEditor, template.Info.Tag) : null;

            return new XElement(Xhtml.Input,
                new XAttribute(Xhtml.Type, "text"),
                new XAttribute(Xhtml.DataBind, $"value: {template.Model.Id}"),
                new XAttribute(Xhtml.Class, "template-editor"),
                dataeditor);
        }

        private static XElement GetTemplateRoot(HtmlTemplate template, XElement mark)
        {
            var root_name = (mark.Parent?.Name == Xhtml.Table) ? Xhtml.Tbody : Xhtml.Div;

            return new XElement(root_name,
                new XAttribute(Xhtml.DataTemplate, template.TemplateId),
                new XAttribute(Xhtml.DataBind, $"source: {template.Model.Id}"),
                "");

        }
        private static XElement GetScript(HtmlTemplate template, IEnumerable<XElement> elements)
        {

            return new XElement(Xhtml.Script,
                new XAttribute(Xhtml.Id, template.TemplateId),
                new XAttribute(Xhtml.Type, "text/x-kendo-template"),
                elements,
                "");


        }

        private static XElement BindUid(XElement element)
        {
            if (element.Attribute(Xhtml.DataBind) == null)
            {
                element.Add(new XAttribute(Xhtml.DataBind, "attr: {data-uid: uid }"));

            }
            return element;
        }

        private static XElement ExtractScript(HtmlTemplate template, XElement begin_mark, XElement end_mark)
        {
            if (begin_mark.Parent != end_mark.Parent)
                throw new InvalidOperationException();

            var group = begin_mark.ElementsAfterSelf().TakeWhile(x => x != end_mark).ToArray();

            end_mark.AddAfterSelf(GetTemplateRoot(template, end_mark));
            group.Remove();
            begin_mark.Remove();
            end_mark.Remove();

            return GetScript(template, group.Select(BindUid));
        }


        private static readonly Regex _regex = new Regex("^#xxx(.+)yyy$");

        private Func<Bitmap, string> _image_handler;
        private readonly WordprocessingDocument _doc;

        private static string GetTemplateId(string str)
        {
            if (str == null)
                return null;
            var match = _regex.Match(str);
            return match.Success ? match.Groups[1].Value : null;
        }

        private static XElement GetMarkText(HtmlTemplate template)
        {
            var run =
                new XElement(new XElement(Word.R, new XElement(Word.T, template.Info.Name + " " + template.Info.Type)));

            return run.CreateHyperlink(null, $"xxx{template.TemplateId}yyy");
        }
        private IEnumerable<TResult> GetMarks<TResult>(XElement body, Func<XElement, HtmlTemplate, TResult> selector)
        {

            foreach (var element in body.Descendants(Xhtml.A))
            {

                var template_id = GetTemplateId(element.Attribute(Xhtml.Href)?.Value);

                if (template_id != null)
                {
                    var t = _templates.GetOrDefault(template_id);
                    if (t != null)
                        yield return selector(element, t);

                }


            }

        }


        private HtmlTemplate AddTemplate(HtmlModel model, ContentControlInfo info)
        {
            var template = new HtmlTemplate(model,info);

            _templates.Add(template.TemplateId,template);

            return template;
        }

        private XElement ReplaceContentControlsAndAddMarks(XElement element, HtmlModel model)
        {

            element.ReplaceContentControls((x, e) =>
            {
                switch (e.Type)
                {
                    case ContentControlType.Text:

                        var text = model.Add(e);
                        var text_mark = GetMarkText(AddTemplate(text, e));
                        x.CreateSingleRun().ReplaceWith(text_mark);


                        return x.Elements();
                    case ContentControlType.Repeat:
                        var repeat = model.Add(e);
                        var repeat_mark = new XElement(Word.P, GetMarkText(AddTemplate(repeat, e)));

                        return new object[] { repeat_mark, ReplaceContentControlsAndAddMarks(x, repeat).Elements(), repeat_mark };

                    default:
                        return ReplaceContentControlsAndAddMarks(x, model).Elements();
                }

            });

            return element;
        }

    }
}