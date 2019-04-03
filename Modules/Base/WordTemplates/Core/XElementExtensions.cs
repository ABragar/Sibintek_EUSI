using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WordTemplates.Core
{
    public static class XElementExtensions
    {
        public static IEnumerable<XElement> FirstDescendants(this XElement element, XName name)
        {
            XContainer parent = element;
            var current_node = parent.FirstNode;


            while (current_node != null)
            {

                var current_element = current_node as XElement;
                var current_container = current_node as XContainer;
                if (current_element != null && current_element.Name == name)
                {

                    yield return current_element;
                    current_node = current_node.NextNode;


                }
                else if (current_container != null)
                {
                    current_node = current_container.FirstNode;
                    parent = current_container;

                }
                else
                {
                    current_node = current_node.NextNode;
                }

                while (current_node == null)
                {

                    if (parent == null || parent == element)
                        yield break;

                    current_node = parent.NextNode;
                    parent = parent.Parent;


                }


            }
        }

        public static XElement CreateSingleRun(this XElement element)
        {
            var runs = element.Descendants(Word.R).ToArray();


            if (runs.Length == 0)
            {
                var run = new XElement(Word.R);

                var p = element.Descendants(Word.P).FirstOrDefault() ?? element;
                p.ReplaceNodes(run);

                return run;
            }

            for (var i = 1; i < runs.Length; i++)
            {
                runs[i].Remove();
            }
            runs[0].Elements(Word.T).Remove();
            return runs[0];
        }


        public static void ProcessContentControls(this XElement element,
            Action<XElement, ContentControlInfo> action)
        {
            var controls = element
                .FirstDescendants(Word.Sdt);

            foreach (var x in controls)
            {
                action(x.GetSdtContent(), new ContentControlInfo(x));
            }
        }

        public static XElement ReplaceFirstDescendants(this XElement element, XName name,
            Func<XElement, object> replace_func)
        {
            var replaces = element
                .FirstDescendants(name)
                .Select(x => new { Old = x, New = replace_func(x) })
                .ToArray();

            foreach (var replace in replaces)
            {
                replace.Old.ReplaceWith(replace.New);
            }

            return element;
        }


        public static XElement ReplaceContentControls(this XElement element,
            Func<XElement, ContentControlInfo, object> func)
        {

            return element.ReplaceFirstDescendants(Word.Sdt, x => func(x.GetSdtContent(), new ContentControlInfo(x)));

        }

        public static string SaveToString(this XElement element)
        {
            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder, new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            }))
            {
                element.Save(writer);
            }

            return builder.ToString();

        }

        
        public static XElement RemoveContentControls(this XElement element)
        {

            return element.ReplaceContentControls((x, e) => x.RemoveContentControls().Elements());

        }

        public static XElement CreateHyperlink(this XElement element, string id, string anchor)
        {
            return new XElement(Word.Hyperlink,
                element,
                id == null ? null : new XAttribute(Word.RelationId, id),
                anchor == null ? null : new XAttribute(Word.Anchor, anchor),
                new XAttribute(Word.History, 1));
        }


        private static XElement GetSdtContent(this XElement sdt)
        {
            return sdt.Element(Word.SdtContent);
        }
    }
}