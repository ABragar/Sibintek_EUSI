using System.Xml.Linq;

namespace WordTemplates.Core
{
    public class Xhtml
    {
        private static readonly XNamespace _xhtml = "http://www.w3.org/1999/xhtml";
        public static readonly XName Script = _xhtml + "script";
        public static readonly XName Id = "id";
        public static readonly XName Ul = _xhtml + "ul";
        public static readonly XName Src = "src";
        public static readonly XName DataBind = "data-bind";
        public static readonly XName DataTemplate = "data-template";
        public static readonly XName Table = _xhtml + "table";
        public static readonly XName Type = "type";
        public static readonly XName DataEditor = "data-editor";
        public static readonly XName Div = _xhtml + "div";
        public static readonly XName P = _xhtml + "p";
        public static readonly XName Span = _xhtml + "span";
        public static readonly XName Class = "class";
        public static readonly XName Input = _xhtml + "input";
        public static readonly XName Tbody = _xhtml + "tbody";
        public static readonly XName Img  = _xhtml + "img";
        public static readonly XName Body = _xhtml + "body";
        public static readonly XName A = _xhtml + "a";
        public static readonly XName Href = "href";

    }
}