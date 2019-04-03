using System.Xml.Linq;

namespace WordTemplates.Core
{
    internal static class Word
    {
        private static readonly XNamespace _main =
            "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        private static readonly XNamespace _wordml =
            "http://schemas.microsoft.com/office/word/2012/wordml";

        private static readonly XNamespace _relationships =
            "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        public static readonly XName FootnoteReference = _main + "footnoteReference";

        public static readonly XName Id = _main + "id";

        public static readonly XName RelationId = _relationships + "id";


        public static readonly XName Sdt = _main + "sdt";

        public static readonly XName SdtPr = _main + "sdtPr";

        public static readonly XName Tag = _main + "tag";

        public static readonly XName Val = _main + "val";

        public static readonly XName Hyperlink = _main + "hyperlink";

        public static readonly XName Anchor = _main + "anchor";

        public static readonly XName History = _main + "history";

        public static readonly XName SdtContent = _main + "sdtContent";

        public static readonly XName Alias = _main + "alias";

        public static readonly XName Text = _main + "text";

        public static readonly XName T = _main + "t";

        public static readonly XName R = _main + "r";

        public static readonly XName P = _main + "p";

        public static readonly XName RepeatingSection = _wordml + "repeatingSection";


    }
}